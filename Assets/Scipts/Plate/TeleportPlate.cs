using System.Collections;
using UnityEngine;

public class TeleportPlate : MonoBehaviour
{
    public int teleportIndex; // 텔레포트 타일의 고유 인덱스
    public static bool isTeleporting = false; // 순간이동 중 여부 플래그
    private Transform[] teleportTileTransforms; // Stage에서 가져온 텔레포트 타일 Transform 배열

    StageManager stm;

    [SerializeField]
    GameObject teleportEffectPrefab = null; // 생성할 이펙트 프리팹
    private GameObject activeEffect = null; // 생성된 이펙트의 참조

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
        // 텔레포트 타일에 이펙트 생성
        CreateTeleportEffect();
    }

    private void CreateTeleportEffect()
    {
        if (teleportEffectPrefab != null)
        {
            // 이펙트를 생성하면서 위치와 회전을 설정
            Vector3 effectPosition = transform.position;
            effectPosition.y = -0.2f; // y 좌표를 -0.6f로 설정

            activeEffect = Instantiate(teleportEffectPrefab, effectPosition, transform.rotation);
        }
        else
        {
            Debug.LogWarning("TeleportEffectPrefab이 설정되지 않았습니다!");
        }
    }

    public void ResetTeleportEffect()
    {
        // 기존 이펙트를 삭제
        if (activeEffect != null)
        {
            Destroy(activeEffect);
            activeEffect = null;
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