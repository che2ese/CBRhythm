using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Basic Plate")]
    public GameObject prefab; // 일반 타일 Prefab
    public GameObject goalPlate; // 마지막 타일(Goal) Prefab
    public int numberOfTiles = 100; // 추가로 생성할 타일 수
    public GameObject[] initialTileObjects; // 초기 타일 오브젝트 배열

    [Header("Big Plate")]
    public GameObject bigPrefab; // 크기가 3배 큰 타일 Prefab
    public List<Range> bigTileRanges = new List<Range>(); // 빅 타일 범위 리스트

    [System.Serializable]
    public struct Range
    {
        public int startIndex;
        public int endIndex;
    }

    [System.Serializable]
    public class PlateIndices
    {
        public List<int> indices = new List<int>(); // 반드시 초기화
    }

    [Header("Gravity Plate")]
    public GameObject gravityPlate; // 중력 반전 타일 Prefab
    public PlateIndices gravityPlateIndices = new PlateIndices(); // 중력 반전 타일 위치 리스트

    [Header("Camera Plate")]
    public GameObject cameraPlate; // 카메라 전환 타일 Prefab
    public PlateIndices cameraPlateIndices = new PlateIndices(); // 카메라 전환 타일 위치 리스트

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
    }

    private void Start()
    {
        CreateTiles();
    }

    void CreateTiles()
    {
        if (!Application.isPlaying)
            return;

        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == numberOfTiles - 1)
            {
                GenerateGoalTile(i);
            }
            else if (IsInBigTileRange(i))
            {
                GenerateBigTile(i);
            }
            else if (gravityPlateIndices?.indices != null && gravityPlateIndices.indices.Contains(i))
            {
                GenerateGravityTile(i);
            }
            else if (cameraPlateIndices?.indices != null && cameraPlateIndices.indices.Contains(i))
            {
                GenerateCameraTile(i);
            }
            else
            {
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

    // 카메라 전환 타일을 생성하는 메서드
    void GenerateCameraTile(int index)
    {
        Vector3 nextDirection;
        do
        {
            nextDirection = directions[Random.Range(0, directions.Length)];
        } while (nextDirection == -lastDirection);

        // 카메라 타일이 짝수 번째 배열에 해당하는 경우 이동 거리 2
        int plateIndex = cameraPlateIndices.indices.IndexOf(index);
        if (plateIndex >= 0 && plateIndex % 2 == 1)
        {
            currentPosition += nextDirection * 2f;
        }
        else
        {
            currentPosition += nextDirection;
        }
        currentPosition.y = -0.6f;

        GameObject cameraTile = Instantiate(cameraPlate, currentPosition, Quaternion.Euler(0f, -90f, 0f));
        cameraTile.transform.parent = this.transform;
        cameraTile.SetActive(false);
        plates[index] = cameraTile.transform;

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

    bool IsInBigTileRange(int index)
    {
        foreach (Range range in bigTileRanges)
        {
            if (index >= range.startIndex && index <= range.endIndex)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFirstTileInBigTileRange(int index)
    {
        foreach (Range range in bigTileRanges)
        {
            if (index == range.startIndex)
            {
                return true;
            }
        }
        return false;
    }

    void GenerateBigTile(int index)
    {
        Vector3 nextDirection;
        do
        {
            nextDirection = directions[Random.Range(0, directions.Length)];
        } while (nextDirection == -lastDirection);

        // 현재 타일의 이동 거리 결정
        if (IsFirstTileInBigTileRange(index))
        {
            // 범위의 첫 번째 타일: 이동 거리 2
            currentPosition += nextDirection * 2f;
        }
        else
        {
            // 범위 내 나머지 타일: 이동 거리 3
            currentPosition += nextDirection * 3f;
        }
        currentPosition.y = -0.6f;

        // bigPrefab 생성
        GameObject bigTile = Instantiate(bigPrefab, currentPosition, Quaternion.identity);
        bigTile.transform.parent = this.transform;
        bigTile.transform.localScale = new Vector3(3f, 0.2f, 3f); // 크기를 3배로 설정
        bigTile.SetActive(false);

        plates[index] = bigTile.transform;

        // 다음 타일 위치 조정
        lastDirection = nextDirection;
    }

}