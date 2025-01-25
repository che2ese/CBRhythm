using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource endAudio;
    NoteManager nm;
    Result rs;
    ScoreManager sm;
    StatusManager stm;

    public int bonus = 0;

    private void Awake()
    {
        endAudio = GetComponent<AudioSource>();
        nm = FindAnyObjectByType<NoteManager>();
        rs = FindAnyObjectByType<Result>();
        sm = FindAnyObjectByType<ScoreManager>();
        stm = FindAnyObjectByType<StatusManager>();
    }

    private void Start()
    {
        bonus = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endAudio.Play();
            PlayerScript.canPressKey = false;

            // 노트 제거
            nm.RemoveNote();

            bonus += stm.currentHp * 500;

            Debug.Log(bonus);

            // 결과 표시
            rs.ShowResult();
        }
    }
}
