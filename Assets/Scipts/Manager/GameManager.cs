using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isStartGame = false;

    [SerializeField]
    GameObject[] goGameUI = null;

    [SerializeField]
    GameObject goTitleUI = null;

    ScoreManager sm;
    TimingManager tm;
    StatusManager stm;
    PlayerScript ps;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        sm = FindAnyObjectByType<ScoreManager>();
        tm = FindAnyObjectByType<TimingManager>();
        stm = FindAnyObjectByType<StatusManager>();
        ps = FindAnyObjectByType<PlayerScript>();
    }

    public void GameStart()
    {
        for(int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(true);
        }
        sm.ResetCombo();
        sm.Initialized();
        tm.Initialized();
        stm.Initialized();
        ps.Initialized();

        isStartGame = true;
    }
    public void MainMenu()
    {
        for (int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(false);
        }
        goTitleUI.SetActive(true);
    }
}
