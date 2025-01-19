using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlate : MonoBehaviour
{
    CameraController cc;
    private bool isActivated = false; // 중력 판 활성화 여부 확인

    private void Awake()
    {
        cc = FindAnyObjectByType<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player")) // 중력 판이 활성화되지 않았을 때만 실행
        {
            isActivated = true; // 중력 판을 활성화된 상태로 변경

            if (cc.isGravityReversed)
            {
                cc.ResetGravity();
            }
            else
            {
                cc.GravityReverse();
            }
        }
    }
}
