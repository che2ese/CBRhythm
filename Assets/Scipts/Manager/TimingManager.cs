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
    StageManager stm;
    PlayerScript ps;

    // Start is called before the first frame update
    void Start()
    {
        ef = FindAnyObjectByType<EffectManager>();
        sm = FindAnyObjectByType<ScoreManager>();
        stm = FindAnyObjectByType<StageManager>();
        ps = FindAnyObjectByType<PlayerScript>();

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
                    // 노트 제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);

                    // 이펙트 연출
                    if (x < timingBoxs.Length - 1)
                        ef.NoteHitEffect();


                    if (CheckCanNextPlate())
                    {
                        // 점수 증가
                        sm.IncreaseScore(x);
                        // 판떄기 등장
                        stm.ShowNextplate();
                        
                        ef.JudgeEffect(x);
                    }
                    else
                    {
                        ef.JudgeEffect(5);
                    }
                    return true;
                }
            }
        }
        sm.ResetCombo();
        ef.JudgeEffect(timingBoxs.Length);
        return false;
    }

    bool CheckCanNextPlate()
    {
        if(Physics.Raycast(ps.pos, Vector3.down, out RaycastHit hitInfo, 1.1f))
        {
            if (hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate plate = hitInfo.transform.GetComponent<BasicPlate>();
                if (plate.flag)
                {
                    plate.flag = false;
                    return true;
                }
            }
        }
        return false;
    }
}
