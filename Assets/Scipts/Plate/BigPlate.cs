using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlate : MonoBehaviour
{
    public GameObject[] basicPlate; // 기본 타일 배열
    public GameObject[] trapPlates; // 트랩 타일 배열 (2개)

    PlayerScript ps;
    StageManager stm;

    private void Awake()
    {
        ps = FindAnyObjectByType<PlayerScript>();
        stm = FindAnyObjectByType<StageManager>();
    }
    void Start()
    {
        if (basicPlate.Length < 1 || trapPlates == null || trapPlates.Length < 2)
        {
            Debug.LogError("basicPlate 배열에 최소 1개의 오브젝트가 필요하며, trapPlates 배열에 최소 2개의 트랩 타일이 설정되어야 합니다.");
            return;
        }

        // 트랩 위치 변경
        MoveTrapPlates();
    }

    void MoveTrapPlates()
    {
        if (basicPlate.Length < 2)
        {
            Debug.LogError("basicPlate 배열에 최소 2개의 오브젝트가 필요합니다.");
            return;
        }

        // basicPlate 배열 중 서로 다른 2개의 랜덤 인덱스 선택
        int firstIndex = Random.Range(0, basicPlate.Length);
        int secondIndex;

        do
        {
            secondIndex = Random.Range(0, basicPlate.Length);
        } while (secondIndex == firstIndex);

        // 첫 번째 트랩 시작
        StartCoroutine(DropTrapPlate(trapPlates[0], basicPlate[firstIndex].transform.localPosition));

        // 두 번째 트랩 시작
        StartCoroutine(DropTrapPlate(trapPlates[1], basicPlate[secondIndex].transform.localPosition));
    }

    IEnumerator DropTrapPlate(GameObject trapPlate, Vector3 targetPosition)
    {
        // 트랩 타일을 초기 위치(+7)로 설정
        Vector3 startPosition = targetPosition;
        startPosition.y += 20f;
        trapPlate.transform.localPosition = startPosition;

        // 트랩 타일 활성화
        trapPlate.SetActive(true);

        // 목표 위치 설정(+4)
        targetPosition.y += 4f;

        // 타일이 목표 위치에 도달할 때까지 빠르게 이동
        float dropSpeed = 20f; // 떨어지는 속도 설정 (값을 높게 설정)

        while (trapPlate.transform.localPosition.y > targetPosition.y)
        {
            trapPlate.transform.localPosition = Vector3.MoveTowards(
                trapPlate.transform.localPosition,
                targetPosition,
                dropSpeed * Time.deltaTime // 빠르게 이동
            );
            yield return null;
        }

        // 정확히 목표 위치에 위치하도록 설정
        trapPlate.transform.localPosition = targetPosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ps.isBig = true;
            stm.ShowNextplate();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ps.isBig = false;
        }
    }
}
