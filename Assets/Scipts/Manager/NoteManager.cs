using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField]
    Transform noteAppear = null;
    [SerializeField]
    GameObject notePrefab = null;

    TimingManager tm;

    private void Awake()
    {
        tm = GetComponent<TimingManager>();
    }
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)
        {
            GameObject notePre = Instantiate(notePrefab, noteAppear.position, Quaternion.identity);
            notePre.transform.SetParent(this.transform);
            tm.boxNoteList.Add(notePre);
            currentTime -= 60d / bpm;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            tm.boxNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
