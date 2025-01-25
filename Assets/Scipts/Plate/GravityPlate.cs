using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlate : MonoBehaviour
{
    CameraController cc;
    private bool isActivated = false; // 중력 판 활성화 여부 확인

    [SerializeField]
    GameObject effect = null; // 생성할 이펙트 프리팹
    [SerializeField]
    float effectDuration = 2f; // 이펙트 유지 시간

    private void Awake()
    {
        cc = FindAnyObjectByType<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player")) // 중력 판이 활성화되지 않았을 때만 실행
        {
            AudioManager.instance.PlaySFX("Gravity");

            isActivated = true; // 중력 판을 활성화된 상태로 변경

            // 이펙트 생성
            SpawnEffect();

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

    private void SpawnEffect()
    {
        if (effect != null)
        {
            // 이펙트 생성
            GameObject spawnedEffect = Instantiate(effect, transform.position, Quaternion.identity);

            // 일정 시간 후 이펙트 제거
            Destroy(spawnedEffect, effectDuration);
        }
    }
}
