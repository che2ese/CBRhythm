using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public int[] score;

    void Start()
    {
        LoadScore();   
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score1", score[0]);
        PlayerPrefs.SetInt("Score2", score[1]);
        PlayerPrefs.SetInt("Score3", score[2]);
        PlayerPrefs.SetInt("Score4", score[3]);
        PlayerPrefs.SetInt("Score5", score[4]);
        PlayerPrefs.SetInt("Score6", score[5]);
        PlayerPrefs.SetInt("Score7", score[6]);
        PlayerPrefs.SetInt("Score8", score[7]);
        PlayerPrefs.SetInt("Score9", score[8]);
        PlayerPrefs.SetInt("Score10", score[9]);
        PlayerPrefs.SetInt("Score11", score[10]);
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("Score1"))
        {
            score[0] = PlayerPrefs.GetInt("Score1");
            score[1] = PlayerPrefs.GetInt("Score2");
            score[2] = PlayerPrefs.GetInt("Score3");
            score[3] = PlayerPrefs.GetInt("Score4");
            score[4] = PlayerPrefs.GetInt("Score5");
            score[5] = PlayerPrefs.GetInt("Score6");
            score[6] = PlayerPrefs.GetInt("Score7");
            score[7] = PlayerPrefs.GetInt("Score8");
            score[8] = PlayerPrefs.GetInt("Score9");
            score[9] = PlayerPrefs.GetInt("Score10");
            score[10] = PlayerPrefs.GetInt("Score11");
        }
    }
}
