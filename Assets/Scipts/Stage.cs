using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject prefab; // 생성할 Prefab
    public int numberOfTiles = 10; // 추가로 생성할 타일 수
    public GameObject[] initialTileObjects; // 초기 타일 오브젝트 배열
    public Transform[] plates; // 생성된 타일 Transform 배열

    private Vector3 currentPosition;
    private Vector3 lastDirection; // 이전에 이동한 방향
    private Vector3[] directions = new Vector3[]
    {
        Vector3.right, // x축 방향 (1, 0, 0)
        Vector3.forward // z축 방향 (0, 0, 1)
    };

    void Awake() // Start 대신 Awake로 변경
    {
        // plates 배열 초기화
        plates = new Transform[numberOfTiles];

        // 초기 타일 오브젝트가 있는지 확인
        if (initialTileObjects.Length > 0)
        {
            for (int i = 0; i < initialTileObjects.Length; i++)
            {
                Debug.Log($"초기 타일 {i + 1} 위치: {initialTileObjects[i].transform.position}");
            }

            currentPosition = initialTileObjects[initialTileObjects.Length - 1].transform.position;
        }
        else
        {
            Debug.LogError("초기 타일 오브젝트 배열이 비어 있습니다!");
            return;
        }

        lastDirection = Vector3.zero;

        for (int i = 0; i < numberOfTiles; i++)
        {
            GenerateTile(i);
        }
    }

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
}
