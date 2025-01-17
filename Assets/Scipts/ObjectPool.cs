using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 큐를 이용한 효율적인 노트 생성

[System.Serializable]
public class ObjectInfo 
{
    public GameObject CreatePrefab;
    public int count;
    public Transform PoolParent;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    ObjectInfo[] objectInfo = null;

    public Queue<GameObject> noteQueue = new Queue<GameObject>();

    public static ObjectPool instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for(int i = 0; i<p_objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.CreatePrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if (p_objectInfo.PoolParent != null)
                t_clone.transform.SetParent(p_objectInfo.PoolParent);
            else
                t_clone.transform.SetParent(this.transform);

            t_queue.Enqueue(t_clone);
        }
        return t_queue;
    }
}