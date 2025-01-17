using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField]
    Transform Center = null;
    [SerializeField]
    RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    EffectManager ef;
    ScoreManager sm;

    // Start is called before the first frame update
    void Start()
    {
        ef = FindAnyObjectByType<EffectManager>();
        sm = FindAnyObjectByType<ScoreManager>();

        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i <  timingRect.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                              Center.localPosition.x + timingRect[i].rect.width / 2);
        }

    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i ++) 
        {
            float notePosX = boxNoteList[i].transform.localPosition.x;

            for(int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= notePosX && notePosX <= timingBoxs[x].y)
                {
                    // ?? ??
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);

                    // ??? ??
                    if (x < timingBoxs.Length - 1)
                        ef.NoteHitEffect();
                    ef.JudgeEffect(x);

                    // ?? ??
                    sm.IncreaseScore(x);

                    return true;
                }
            }
        }
        sm.ResetCombo();
        ef.JudgeEffect(timingBoxs.Length);
        return false;
    }
}
