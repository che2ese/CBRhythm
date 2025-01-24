using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Result : MonoBehaviour
{
    [SerializeField]
    GameObject goUI = null;

    [SerializeField]
    TextMeshProUGUI[] txtCount = null;
    [SerializeField]
    TextMeshProUGUI txtCoin = null;
    [SerializeField]
    TextMeshProUGUI txtScore = null;
    [SerializeField]
    TextMeshProUGUI txtMaxCombo = null;

    int currentSong = 0;

    ScoreManager sm;
    TimingManager tm;
    DataBaseManager dm;

    private void Awake()
    {
        sm = FindAnyObjectByType<ScoreManager>();
        tm = FindAnyObjectByType<TimingManager>();
    }
    private void Start()
    {
        dm = FindAnyObjectByType<DataBaseManager>();
    }

    public void ShowResult()
    {
        FindAnyObjectByType<CenterFrame>().ResetMusic();

        AudioManager.instance.StopBGM();

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

        if(t_currentScore > dm.score[currentSong])
        {
            dm.score[currentSong] = t_currentScore;
            dm.SaveScore();
        }
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        sm.ResetCombo();
    }

    public void SetCurrentSong(int p_songNum)
    {
        currentSong = p_songNum;
    }
}
