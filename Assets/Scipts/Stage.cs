using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject prefab; // 일반 타일 Prefab
    public GameObject goalPlate; // 마지막 타일(Goal) Prefab
    public int numberOfTiles = 10; // 추가로 생성할 타일 수
    public GameObject[] initialTileObjects; // 초기 타일 오브젝트 배열
    public Transform[] plates; // 생성된 타일 Transform 배열 (비활성화된 상태로 저장됨)

    private Vector3 currentPosition; // 현재 타일의 위치
    private Vector3 lastDirection; // 이전에 이동한 방향
    private Vector3[] directions = new Vector3[] // 타일이 이동 가능한 방향 배열
    {
        Vector3.right, // x축 방향 (1, 0, 0)
        Vector3.forward // z축 방향 (0, 0, 1)
    };

    void Awake() 
    {
        // plates 배열 초기화: 생성될 타일 개수만큼 배열 크기 설정
        plates = new Transform[numberOfTiles];

        // 초기 타일 오브젝트가 존재하는지 확인
        if (initialTileObjects.Length > 0)
        {
            for (int i = 0; i < initialTileObjects.Length; i++)
            {
                // 초기 타일의 위치 확인 (디버그용 로그 출력)
                Debug.Log($"초기 타일 {i + 1} 위치: {initialTileObjects[i].transform.position}");
            }

            // 마지막 초기 타일의 위치를 현재 위치로 설정
            currentPosition = initialTileObjects[initialTileObjects.Length - 1].transform.position;
        }
        else
        {
            // 초기 타일이 없을 경우 오류 출력
            Debug.LogError("초기 타일 오브젝트 배열이 비어 있습니다!");
            return;
        }

        // 이전 방향 초기화
        lastDirection = Vector3.zero;

        // 추가 타일 생성
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == numberOfTiles - 1)
            {
                // 마지막 타일은 Goal 타일로 생성
                GenerateGoalTile(i);
            }
            else
            {
                // 일반 타일 생성
                GenerateTile(i);
            }
        }
    }

    // 일반 타일을 생성하는 메서드
    void GenerateTile(int index)
    {
        // 다음 방향 결정 (이전 방향의 반대 방향으로 돌아가는 것 방지)
        Vector3 nextDirection;
        do
        {
            nextDirection = directions[Random.Range(0, directions.Length)];
        } while (nextDirection == -lastDirection);

        // 현재 위치 갱신 (y축은 고정)
        currentPosition += nextDirection;
        currentPosition.y = -0.6f;

        // 일반 타일 Prefab 생성
        GameObject newTile = Instantiate(prefab, currentPosition, Quaternion.identity);
        newTile.transform.parent = this.transform; // 이 스크립트가 부착된 오브젝트를 부모로 설정
        newTile.SetActive(false); // 타일 비활성화 상태로 저장
        plates[index] = newTile.transform; // plates 배열에 Transform 저장

        // 이전 방향 갱신
        lastDirection = nextDirection;
    }

    // Goal 타일을 생성하는 메서드
    void GenerateGoalTile(int index)
    {
        // 다음 방향 결정 (이전 방향의 반대 방향으로 돌아가는 것 방지)
        Vector3 nextDirection;
        do
        {
            nextDirection = directions[Random.Range(0, directions.Length)];
        } while (nextDirection == -lastDirection);

        // 현재 위치 갱신 (y축은 고정)
        currentPosition += nextDirection;
        currentPosition.y = -0.6f;

        // Goal 타일 Prefab 생성
        GameObject goalTile = Instantiate(goalPlate, currentPosition, Quaternion.identity);
        goalTile.transform.parent = this.transform; // 이 스크립트가 부착된 오브젝트를 부모로 설정
        goalTile.SetActive(false); // 타일 비활성화 상태로 저장
        plates[index] = goalTile.transform; // plates 배열에 Transform 저장

        // 이전 방향 갱신
        lastDirection = nextDirection;
    }
}
