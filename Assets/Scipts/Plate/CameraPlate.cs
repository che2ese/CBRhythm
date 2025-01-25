using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlate : MonoBehaviour
{
    CameraController cc;
    private bool isActivated = false; // 중력 판 활성화 여부 확인

    StageManager stm;

    private void Awake()
    {
        cc = FindAnyObjectByType<CameraController>();
        stm = FindAnyObjectByType<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player")) // 중력 판이 활성화되지 않았을 때만 실행
        {
            stm.ShowNextplate();
            stm.ShowNextplate();
            isActivated = true; // 중력 판을 활성화된 상태로 변경

            if (cc.isCameraReversed)
            {
                AudioManager.instance.PlaySFX("Reset");
                cc.ResetCamera();
            }
            else
            {
                AudioManager.instance.PlaySFX("Reverse");
                cc.CameraReverse();
            }
        }
    }
}
