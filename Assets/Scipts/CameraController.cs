using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player = null;
    [SerializeField]
    float speed = 15f;

    Vector3 playerDistance = new Vector3();

    float hitDistance = 0f;

    [SerializeField]
    float zoomDistance = -1.5f;

    // Start is called before the first frame update
    void Start()
    {
        playerDistance = transform.position - player.position;    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desPos = player.position + playerDistance + (transform.forward * hitDistance);
        transform.position = Vector3.Lerp(transform.position, desPos, speed * Time.deltaTime);
    }
    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0f;
    }
}
