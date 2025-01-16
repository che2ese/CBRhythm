using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField]
    Transform noteAppear = null;

    TimingManager tm;
    EffectManager ef;

    private void Awake()
    {
        tm = GetComponent<TimingManager>();
        ef = FindAnyObjectByType<EffectManager>();
    }
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)
        {
            GameObject notePre = ObjectPool.instance.noteQueue.Dequeue();
            notePre.transform.position = noteAppear.position;
            notePre.SetActive(true);
            tm.boxNoteList.Add(notePre);
            currentTime -= 60d / bpm;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteFlag())
                ef.JudgeEffect(4);

            tm.boxNoteList.Remove(collision.gameObject);

            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}
