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
    StageManager sgm;
    public NoteManager nm;
    Result rs;
    CameraController cc;
    UIManager um;
    GoalPlate gp;

    [SerializeField]
    CenterFrame theMusic = null;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        sm = FindAnyObjectByType<ScoreManager>();
        tm = FindAnyObjectByType<TimingManager>();
        stm = FindAnyObjectByType<StatusManager>();
        ps = FindAnyObjectByType<PlayerScript>();
        sgm = FindAnyObjectByType<StageManager>();
        nm = FindAnyObjectByType<NoteManager>();
        rs = FindAnyObjectByType<Result>();
        cc = FindAnyObjectByType<CameraController>();
        um = FindAnyObjectByType<UIManager>();
        gp = FindAnyObjectByType<GoalPlate>();
    }

    public void GameStart(int p_songNum, int p_bpm)
    {
        for (int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(true);
        }

        sgm.RemoveStage();

        // 모든 TeleportPlate 이펙트를 초기화
        TeleportPlate[] teleportPlates = FindObjectsOfType<TeleportPlate>();
        foreach (TeleportPlate plate in teleportPlates)
        {
            plate.ResetTeleportEffect();
        }

        // Stage 초기화 완료 여부 확인
        if (sgm != null)
        {
            sgm.SettingStage(p_songNum);
        }
        else
        {
            Debug.LogWarning("StageManager가 초기화되지 않았습니다.");
        }

        theMusic.bgmName = "BGM" + p_songNum;
        nm.bpm = p_bpm;
        sm.ResetCombo();
        sm.Initialized();
        tm.Initialized();
        ps.Initialized();
        cc.Initialized();
        um.Initialized();
        gp.Initialized();
        rs.SetCurrentSong(p_songNum);

        if (p_songNum == 10)
        {
            stm.Initialized(1);
        }
        else
        {
            stm.Initialized(3);
        }

        AudioManager.instance.StopBGM();
            
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

    public void Quit()
    {
        Application.Quit();
    }
}
