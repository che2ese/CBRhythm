using System.Collections;
using UnityEngine;

public class CubeTrail : MonoBehaviour
{
    [SerializeField]
    GameObject trailPrefab; // 잔상으로 사용할 프리팹
    [SerializeField]
    float trailDuration = 0.3f; // 잔상이 남아있는 시간
    [SerializeField]
    float spawnInterval = 0.1f; // 잔상이 생성되는 간격

    public bool isTrailing = false;

    void Start()
    {
        // 잔상 생성 시작
        StartCoroutine(SpawnTrail());
    }

    public IEnumerator SpawnTrail()
    {
        while (true)
        {
            if (isTrailing)
            {
                // 잔상 생성
                GameObject trail = Instantiate(trailPrefab, transform.position, transform.rotation);
                Destroy(trail, trailDuration); // 일정 시간 후 잔상 제거
            }
            yield return new WaitForSeconds(spawnInterval); // 잔상 생성 간격
        }
    }

    void Update()
    {
        // 움직임 감지 및 잔상 활성화
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isTrailing = true;
        }
        else
        {
            isTrailing = false;
        }
    }
}
