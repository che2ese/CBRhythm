using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    Animator noteHitAnimator = null;
    string hit = "Hit";

    [SerializeField]
    Animator judgeAnimator = null;
    [SerializeField]
    Image judgeImage = null;
    [SerializeField]
    Sprite[] judgeSprite = null;

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }
    public void JudgeEffect(int num)
    {
        judgeImage.sprite = judgeSprite[num];
        judgeAnimator.SetTrigger(hit);
    }
}
