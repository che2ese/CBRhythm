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

    // Start is called before the first frame update
    void Start()
    {
        ef = FindAnyObjectByType<EffectManager>();

        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i <  timingRect.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                              Center.localPosition.x + timingRect[i].rect.width / 2);
        }

    }

    public void CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i ++) 
        {
            float notePosX = boxNoteList[i].transform.localPosition.x;

            for(int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= notePosX && notePosX <= timingBoxs[x].y)
                {
                    // 노트 제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);

                    // 이펙트 연출
                    if (x < timingBoxs.Length - 1)
                        ef.NoteHitEffect();
                    ef.JudgeEffect(x);
                    return;
                }
            }
        }
        ef.JudgeEffect(timingBoxs.Length);
    }
}
