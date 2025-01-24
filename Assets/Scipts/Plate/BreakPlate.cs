using System.Collections;
using UnityEngine;

public class BreakPlate : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody 컴포넌트 참조
    private Vector3 initialPosition; // 초기 위치 저장
    private Quaternion initialRotation; // 초기 회전 저장

    int bpm;

    private void Start()
    {
        // GameManager에서 bpm 값을 가져옴
        bpm = GameManager.instance.nm.bpm;
        Debug.Log(bpm);

        // Rigidbody를 미리 가져오기
        rb = GetComponent<Rigidbody>();

        // Rigidbody가 없으면 자동으로 추가
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // 초기에는 중력 비활성화
        rb.isKinematic = true;

        // 초기 위치와 회전값 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX("Break");
            StartCoroutine(DropPlate());
        }
    }

    IEnumerator DropPlate()
    {

        Debug.Log((float)((24f / bpm) * 1.5f));
        yield return new WaitForSeconds((float)((24f / bpm) * 1.5f)); // 2초 대기

        rb.isKinematic = false; // 중력 활성화

        StartCoroutine(ResetPlate());
    }

    IEnumerator ResetPlate()
    {
        yield return new WaitForSeconds(3f); // 2초 대기
        // Rigidbody 초기화
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 위치를 초기값 + Y축으로 5만큼 상승
        transform.position = new Vector3(initialPosition.x, -0.6f, initialPosition.z);
        transform.rotation = initialRotation;
    }
}
