using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sfx = null;

    [SerializeField]
    Sound[] bgm = null;

    [SerializeField]
    AudioSource bgmPlayer = null;
    [SerializeField]
    AudioSource[] sfxPlayer = null;

    [SerializeField]
    Slider[] bgmVolumeSlider; // BGM 볼륨 슬라이더
    [SerializeField]
    Slider[] sfxVolumeSlider; // SFX 볼륨 슬라이더

    bool isPlay = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < bgmVolumeSlider.Length; i++)
        {
            // 슬라이더 값 초기화
            bgmVolumeSlider[i].value = bgmPlayer.volume;
            sfxVolumeSlider[i].value = sfxPlayer[0].volume;

            // 슬라이더 값 변경 시 실행될 메서드 연결
            bgmVolumeSlider[i].onValueChanged.AddListener(value => SyncBGMVolume(value));
            sfxVolumeSlider[i].onValueChanged.AddListener(value => SyncSFXVolume(value));
        }

        PlaySFX("Start");
    }

    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = sfx[i].clip;
                        sfxPlayer[x].Play();
                        return;
                    }
                }
                return;
            }
        }
        Debug.Log(p_sfxName + "이름의 효과음이 없습니다.");
    }

    // BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    {
        bgmPlayer.volume = volume;
    }

    // SFX 볼륨 설정
    public void SetSFXVolume(float volume)
    {
        foreach (AudioSource player in sfxPlayer)
        {
            player.volume = volume;
        }
    }

    // 모든 BGM 슬라이더 값 동기화
    private void SyncBGMVolume(float volume)
    {
        SetBGMVolume(volume);
        foreach (Slider slider in bgmVolumeSlider)
        {
            slider.value = volume;
        }
    }

    // 모든 SFX 슬라이더 값 동기화
    private void SyncSFXVolume(float volume)
    {
        SetSFXVolume(volume);
        foreach (Slider slider in sfxVolumeSlider)
        {
            slider.value = volume;
        }
        if (isPlay)
        {
            PlaySFX("Clap");
            StartCoroutine(PlayOne());
        }
    }

    IEnumerator PlayOne()
    {
        isPlay = false;
        yield return new WaitForSecondsRealtime(0.5f);
        isPlay = true;
    }
}
