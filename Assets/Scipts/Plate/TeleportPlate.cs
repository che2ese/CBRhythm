using System.Collections;
using UnityEngine;

public class TeleportPlate : MonoBehaviour
{
    public int teleportIndex; // 텔레포트 타일의 고유 인덱스
    public static bool isTeleporting = false; // 순간이동 중 여부 플래그
    private Transform[] teleportTileTransforms; // Stage에서 가져온 텔레포트 타일 Transform 배열

    StageManager stm;

    private void Awake()
    {
        stm = FindAnyObjectByType<StageManager>();
    }

    private void Start()
    {
        // Stage 스크립트에서 텔레포트 타일 Transform 배열 가져오기
        Stage stage = FindObjectOfType<Stage>();
        if (stage != null)
        {
            teleportTileTransforms = stage.teleportTileTransforms;
        }
        else
        {
            Debug.LogError("Stage 스크립트를 찾을 수 없습니다!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return; // 이미 순간이동 중이면 중단

        if (other.CompareTag("Player")) // 플레이어와 충돌했는지 확인
        {
            PlayerScript player = other.GetComponentInParent<PlayerScript>();
            if (player == null)
            {
                Debug.LogWarning("PlayerScript가 존재하지 않습니다!");
                return;
            }

            if (teleportIndex % 2 == 0) // 짝수 타일인지 확인
            {
                isTeleporting = true; // 순간이동 시작 플래그 설정
                TeleportPlayerToNextOddTile(player);
                stm.ShowNextplate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTeleporting = false; // 충돌 범위에서 벗어나면 순간이동 플래그 초기화
        }
    }

    private void TeleportPlayerToNextOddTile(PlayerScript player)
    {
        if (teleportTileTransforms == null || teleportTileTransforms.Length == 0)
        {
            Debug.LogWarning("TeleportTileTransforms 배열이 비어 있습니다!");
            return;
        }

        // 다음 홀수 타일을 찾기
        for (int i = teleportIndex + 1; i < teleportTileTransforms.Length; i++)
        {
            TeleportPlate nextTeleportPlate = teleportTileTransforms[i].GetComponent<TeleportPlate>();

            if (nextTeleportPlate != null && nextTeleportPlate.teleportIndex % 2 != 0) // 홀수 타일인지 확인
            {
                Vector3 targetPosition = teleportTileTransforms[i].position;

                targetPosition.y = 0; // y 좌표를 항상 0으로 설정

                // 강제 이동 (즉시 이동)
                player.TeleportTo(targetPosition);

                Debug.Log($"플레이어가 타일 {teleportIndex}에서 홀수 타일 {nextTeleportPlate.teleportIndex}로 이동했습니다.");
                return;
            }
        }

        Debug.Log("다음 홀수 텔레포트 타일이 없습니다.");
    }
}