using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject prefab; // 일반 타일 Prefab
    public GameObject goalPlate; // 마지막 타일(Goal) Prefab
    public int numberOfTiles = 100; // 추가로 생성할 타일 수
    public GameObject[] initialTileObjects; // 초기 타일 오브젝트 배열
    public GameObject gravityPlate; // 중력 반전 타일 Prefab
    public int gravityCount = 2; // 중력 반전 타일 개수
    public Transform[] plates; // 생성된 타일 Transform 배열 (비활성화된 상태로 저장됨)

    private Vector3 currentPosition; // 현재 타일의 위치
    private Vector3 lastDirection; // 이전에 이동한 방향
    private Vector3[] directions = new Vector3[] // 타일이 이동 가능한 방향 배열
    {
        Vector3.right, // x축 방향 (1, 0, 0)
        Vector3.forward // z축 방향 (0, 0, 1)
    };

    private Queue<int> gravityTileIndices; // 중력 반전 타일 인덱스를 순서대로 저장

    void Awake()
    {
        // plates 배열 초기화: 생성될 타일 개수만큼 배열 크기 설정
        plates = new Transform[numberOfTiles];

        // 중력 반전 타일 인덱스 생성
        gravityTileIndices = new Queue<int>(GenerateRandomIndices(gravityCount, numberOfTiles - 1, numberOfTiles / (gravityCount * 2))); // Goal 타일 제외, 간격 20 보장

        // 초기 타일 오브젝트가 존재하는지 확인
        if (initialTileObjects.Length > 0)
        {
            for (int i = 0; i < initialTileObjects.Length; i++)
            {
                Debug.Log($"초기 타일 {i + 1} 위치: {initialTileObjects[i].transform.position}");
            }

            // 마지막 초기 타일의 위치를 현재 위치로 설정
            currentPosition = initialTileObjects[initialTileObjects.Length - 1].transform.position;
        }
        else
        {
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
            else if (gravityTileIndices.Count > 0 && gravityTileIndices.Peek() == i)
            {
                // 중력 반전 타일 생성
                gravityTileIndices.Dequeue(); // 인덱스 제거
                GenerateGravityTile(i);
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
        Vector3 nextDirection;
        do
        {
            nextDirection = directions[Random.Range(0, directions.Length)];
        } while (nextDirection == -lastDirection);

        currentPosition += nextDirection;
        currentPosition.y = -0.6f;

        GameObject newTile = Instantiate(prefab, currentPosition, Quaternion.identity);
        newTile.transform.parent = this.transform;
        newTile.SetActive(false);
        plates[index] = newTile.transform;

        lastDirection = nextDirection;
    }

    // 중력 반전 타일을 생성하는 메서드
    void GenerateGravityTile(int index)
    {
        Vector3 nextDirection;
        do
        {
            nextDirection = directions[Random.Range(0, directions.Length)];
        } while (nextDirection == -lastDirection);

        currentPosition += nextDirection;
        currentPosition.y = -0.6f;

        GameObject gravityTile = Instantiate(gravityPlate, currentPosition, Quaternion.Euler(0f, 90f, 0f));
        gravityTile.transform.parent = this.transform;
        gravityTile.SetActive(false);
        plates[index] = gravityTile.transform;

        lastDirection = nextDirection;
    }

    // Goal 타일을 생성하는 메서드
    void GenerateGoalTile(int index)
    {
        Vector3 nextDirection;
        do
        {
            nextDirection = directions[Random.Range(0, directions.Length)];
        } while (nextDirection == -lastDirection);

        currentPosition += nextDirection;
        currentPosition.y = -0.6f;

        GameObject goalTile = Instantiate(goalPlate, currentPosition, Quaternion.identity);
        goalTile.transform.parent = this.transform;
        goalTile.SetActive(false);
        plates[index] = goalTile.transform;

        lastDirection = nextDirection;
    }

    // 중력 반전 타일 인덱스를 랜덤으로 생성하는 메서드 (최소 간격 포함)
    List<int> GenerateRandomIndices(int count, int maxIndex, int minGap)
    {
        List<int> indices = new List<int>();

        while (indices.Count < count)
        {
            int randomIndex = Random.Range(0, maxIndex);

            // 최소 간격 보장
            if (indices.Count == 0 || randomIndex - indices[indices.Count - 1] >= minGap)
            {
                indices.Add(randomIndex);
            }
        }

        indices.Sort(); // 오름차순 정렬하여 순서대로 배치
        return indices;
    }
}
