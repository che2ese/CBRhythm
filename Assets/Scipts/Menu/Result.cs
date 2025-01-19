using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField]
    GameObject goUI = null;

    [SerializeField]
    Text[] txtCount = null;
    [SerializeField]
    Text txtCoin = null;
    [SerializeField]
    Text txtScore = null;
    [SerializeField]
    Text txtMaxCombo = null;

    ScoreManager sm;
    TimingManager tm;

    private void Start()
    {
        sm = FindAnyObjectByType<ScoreManager>();
        tm = FindAnyObjectByType<TimingManager>();
    }

    public void ShowResult()
    {
        goUI.SetActive(true);

        for(int i =0; i<txtCount.Length; i++)
        {
            txtCount[i].text = "0";
        }

        txtCoin.text = "0";
        txtMaxCombo.text = "0";
        txtScore.text = "0";

        int[] t_judge = tm.GetJudgeRecord();
        int t_currentScore = sm.GetCurrentScore();
        int t_maxCombo = sm.GetMaxCombo();
        int t_coin = t_currentScore / 100;

        for (int i =0; i<txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judge[i]);
        }

        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", t_coin);
    }
}
