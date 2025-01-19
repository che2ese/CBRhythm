using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource endAudio;
    NoteManager nm;

    private void Awake()
    {
        endAudio = GetComponent<AudioSource>();
        nm = FindAnyObjectByType<NoteManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endAudio.Play();
            PlayerScript.canPressKey = false;
            nm.RemoceNote();
        }
    }
}
