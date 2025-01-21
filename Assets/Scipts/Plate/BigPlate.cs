using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlate : MonoBehaviour
{
    public GameObject[] basicPlate; // 기본 타일 배열
    public GameObject[] trapPlates; // 트랩 타일 배열 (2개)

    // Start is called before the first frame update
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

        // 첫 번째 트랩 위치 설정
        Vector3 firstPosition = basicPlate[firstIndex].transform.localPosition;
        firstPosition.y += 4f; // Y축 값만 변경
        trapPlates[0].transform.localPosition = firstPosition;

        // 두 번째 트랩 위치 설정
        Vector3 secondPosition = basicPlate[secondIndex].transform.localPosition;
        secondPosition.y += 4f; // Y축 값만 변경
        trapPlates[1].transform.localPosition = secondPosition;

        Debug.Log($"첫 번째 트랩 위치 변경 완료: {firstPosition}");
        Debug.Log($"두 번째 트랩 위치 변경 완료: {secondPosition}");
    }
}
