using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Song
{
    public string level;
    public int bpm;
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    [SerializeField]
    Song[] songList = null;
    [SerializeField]
    TextMeshProUGUI txtLevel = null;
    [SerializeField]
    Image imgDisk = null;

    [SerializeField]
    GameObject titleMenu = null;

    int currentSong = 0;

    private void Start()
    {
        SettingSong();
    }
    public void BtnNext()
    {
        AudioManager.instance.PlaySFX("Clap");
        if (++currentSong > songList.Length - 1)
        {
            currentSong = 0;
        }
        SettingSong();
    }
    public void BtnPrior()
    {
        AudioManager.instance.PlaySFX("Clap");
        if (--currentSong < 0)
        {
            currentSong = songList.Length - 1;
        }
        SettingSong();
    }
    void SettingSong()
    {
        txtLevel.text = songList[currentSong].level;
        imgDisk.sprite = songList[currentSong].sprite;

        AudioManager.instance.PlayBGM("BGM" + currentSong);
    }
    public void BtnBack()
    {
        titleMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        int t_bpm = songList[currentSong].bpm;

        GameManager.instance.GameStart(currentSong, t_bpm);
        this.gameObject.SetActive(false);
    }
}
