using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    GameObject stage = null;
    Transform[] stageplates;

    [SerializeField]
    float offsetY = -5f;
    [SerializeField]
    float plateSpeed = 10f;

    int stepCount = 0;
    int totalPlateCount = 0;

    void Start()
    {
        // Stage의 plates 배열 초기화가 완료될 때까지 대기
        Stage stageComponent = stage.GetComponent<Stage>();
        if (stageComponent.plates == null || stageComponent.plates.Length == 0)
        {
            Debug.LogWarning("Stage 초기화가 완료되지 않았습니다.");
            return;
        }

        stageplates = stageComponent.plates;
        totalPlateCount = stageplates.Length;

        for(int i =0; i<totalPlateCount; i++)
        {
            stageplates[i].position = new Vector3(stageplates[i].position.x,
                                                    stageplates[i].position.y + offsetY,
                                                    stageplates[i].position.z);
        }
    }

    public void ShowNextplate()
    {
        if (stepCount < totalPlateCount)
            StartCoroutine(MovePlateCo(stepCount++));
    }

    IEnumerator MovePlateCo(int num)
    {
        stageplates[num].gameObject.SetActive(true);
        Vector3 desPos = new Vector3(stageplates[num].position.x,
                                        stageplates[num].position.y - offsetY,
                                        stageplates[num].position.z);

        while(Vector3.SqrMagnitude(stageplates[num].position - desPos) >= 0.001f)
        {
            stageplates[num].position = Vector3.Lerp(stageplates[num].position, desPos, plateSpeed * Time.deltaTime);
            yield return null;
        }

        stageplates[num].position = desPos;
    }
}
