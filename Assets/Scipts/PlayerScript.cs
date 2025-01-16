using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    TimingManager tm;

    private void Awake()
    {
        tm = FindAnyObjectByType<TimingManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tm.CheckTiming();
        }
    }
}
