using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlate : MonoBehaviour
{
    public static List<TeleportPlate> teleportPlates = new List<TeleportPlate>(); // 텔레포트 타일을 저장하는 리스트
    public int teleportIndex; // 텔레포트 타일의 고유 인덱스

    private static bool isTeleporting = false; // 순간이동 중 상태 플래그

    private void Start()
    {
        // TeleportPlate를 리스트에 추가
        teleportPlates.Add(this);
    }

    private void OnDestroy()
    {
        // 삭제 시 리스트에서 제거
        teleportPlates.Remove(this);
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
            }
            else
            {
                Debug.Log("홀수 텔레포트 타일: 도착지입니다. 아무 동작도 하지 않음.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 충돌 범위에서 벗어나면 순간이동 플래그 초기화
            isTeleporting = false;
        }
    }

    void TeleportPlayerToNextOddTile(PlayerScript player)
    {
        // 현재 타일의 다음 홀수 타일을 찾기
        for (int i = teleportIndex + 1; i < teleportPlates.Count; i++)
        {
            if (teleportPlates[i].teleportIndex % 2 != 0) // 홀수 타일인지 확인
            {
                Vector3 targetPosition = teleportPlates[i].transform.position;
                Vector3 realCubeLocalPosition = player.realCube.localPosition;

                // 플레이어의 위치 변경
                player.transform.position = new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z);
                player.realCube.localPosition = Vector3.zero; // realCube 초기화

                Debug.Log($"플레이어가 타일 {teleportIndex}에서 홀수 타일 {teleportPlates[i].teleportIndex}로 이동했습니다.");
                return;
            }
        }

        Debug.Log("다음 홀수 텔레포트 타일이 없습니다.");
    }
}
