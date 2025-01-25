using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource endAudio;
    NoteManager nm;
    Result rs;
    ScoreManager sm;

    private void Awake()
    {
        endAudio = GetComponent<AudioSource>();
        nm = FindAnyObjectByType<NoteManager>();
        rs = FindAnyObjectByType<Result>();
        sm = FindAnyObjectByType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endAudio.Play();
            PlayerScript.canPressKey = false;

            // 노트 제거
            nm.RemoveNote();

            // 결과 표시
            rs.ShowResult();
        }
    }
}
