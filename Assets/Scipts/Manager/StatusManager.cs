using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [SerializeField]
    float blinkSpeed = 0.1f;
    [SerializeField]
    int blinkCount = 10;
    int currentBlinkCount = 0;
    bool isBlink = false;

    bool isDead = false;

    int maxHp = 3;
    int currentHp = 3;

    [SerializeField]
    Image[] hpImage = null;

    Result rs;
    NoteManager nm;
    [SerializeField]
    MeshRenderer playerMesh = null;

    private void Awake()
    {
        rs = FindAnyObjectByType<Result>();
        nm = FindAnyObjectByType<NoteManager>();
    }

    public void Initialized()
    {
        currentHp = maxHp;
        isDead = false;
        SettingHPImage();
    }

    public void DecreaseHp(int p_num)
    {
        if (!isBlink)
        {
            currentHp -= p_num;
            if (currentHp <= 0)
            {
                isDead = true;
                rs.ShowResult();
                nm.RemoveNote();
            }
            else
                StartCoroutine(BlinkCo());

            SettingHPImage();
        }
    }

    void SettingHPImage()
    {
        for(int i =0; i < hpImage.Length; i++)
        {
            if (i < currentHp)
            {
                hpImage[i].gameObject.SetActive(true);
            }
            else
                hpImage[i].gameObject.SetActive(false);
        }
    }
    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator BlinkCo()
    {
        isBlink = true;

        while(currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            currentBlinkCount++;
        }
        playerMesh.enabled = true;
        currentBlinkCount = 0;
        isBlink = false;
    }
}
