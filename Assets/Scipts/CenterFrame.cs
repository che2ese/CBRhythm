using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    AudioSource backGround;
    bool musicStart = false;
    private void Awake()
    {
        backGround = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                backGround.Play();
                musicStart = true;
            }
        }
    }
}
