using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlate : MonoBehaviour
{
    StatusManager sm;

    private void Awake()
    {
        sm = FindAnyObjectByType<StatusManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX("Crash");
            sm.DecreaseHp(1);
        }
    }
}
