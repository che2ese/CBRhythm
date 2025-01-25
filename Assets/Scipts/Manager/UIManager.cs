using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject settingUI = null;

    [SerializeField]
    GameObject gameSettingUI = null;

    [SerializeField]
    GameObject note = null;

    private void Start()
    {
        settingUI.SetActive(false);
        gameSettingUI.SetActive(false);
    }
    public void Initialized()
    {
        gameSettingUI.SetActive(false);
    }
    public void OpenSetting()
    {
        AudioManager.instance.PlaySFX("Clap");
        settingUI.SetActive(true);
    }

    public void CloseSetting()
    {
        AudioManager.instance.PlaySFX("Clap");
        settingUI.SetActive(false);
    }

    public void OpenGameSet()
    {
        AudioManager.instance.PlaySFX("Clap");
        gameSettingUI.SetActive(true);
        note.SetActive(false);
    }

    public void CloseGameSet()
    {
        AudioManager.instance.PlaySFX("Clap");
        gameSettingUI.SetActive(false);
        note.SetActive(true);
    }
    public void MainMenu()
    {
        // 씬 0 재실행
        SceneManager.LoadScene(0);
    }
}
