using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    Animator noteHitAnimator = null;

    string hit = "Hit";

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }
}
