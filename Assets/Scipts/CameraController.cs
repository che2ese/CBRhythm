using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player = null; // 플레이어의 Transform
    [SerializeField]
    float speed = 15f; // 카메라 이동 속도
    [SerializeField]
    float transitionDuration = 0.5f; // 전환 애니메이션 지속 시간

    Vector3 playerDistance = new Vector3(); // 플레이어와 카메라의 거리
    float hitDistance = 0f; // 카메라 줌 거리

    [SerializeField]
    float zoomDistance = -1.5f; // 줌 거리 설정

    public bool isGravityReversed = false; // 중력 반전 상태를 추적

    private void Start()
    {
        // 초기 위치와 회전 설정
        ResetGravity();

        // 플레이어와 카메라의 초기 거리 계산
        playerDistance = transform.position - player.position;
    }

    private void Update()
    {
        // 목표 위치 계산
        Vector3 desPos = player.position + playerDistance + (transform.forward * hitDistance);

        // 카메라를 부드럽게 플레이어를 따라가도록 설정
        transform.position = Vector3.Lerp(transform.position, desPos, speed * Time.deltaTime);
    }

    public IEnumerator ZoomCam()
    {
        // 줌 인
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        // 줌 아웃
        hitDistance = 0f;
    }

    public void GravityReverse()
    {
        // 중력 반전 상태로 전환
        isGravityReversed = true;

        // 중력 반전 전환 애니메이션 시작
        StartCoroutine(SmoothTransition(
            new Vector3(playerDistance.x, -Mathf.Abs(playerDistance.y), playerDistance.z),
            Quaternion.Euler(150f, 45f, 0f)
        ));
    }

    public void ResetGravity()
    {
        // 중력 반전 상태 해제
        isGravityReversed = false;

        // 중력 복원 전환 애니메이션 시작
        StartCoroutine(SmoothTransition(
            new Vector3(playerDistance.x, Mathf.Abs(playerDistance.y), playerDistance.z),
            Quaternion.Euler(30f, 45f, 0f)
        ));
    }

    private IEnumerator SmoothTransition(Vector3 targetPlayerDistance, Quaternion targetRotation)
    {
        Vector3 initialPlayerDistance = playerDistance;
        Quaternion initialRotation = transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            // 시간에 따른 부드러운 전환
            elapsedTime += Time.deltaTime;

            playerDistance = Vector3.Lerp(initialPlayerDistance, targetPlayerDistance, elapsedTime / transitionDuration);
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / transitionDuration);

            yield return null;
        }

        // 최종 값 설정 (안정적인 종료)
        playerDistance = targetPlayerDistance;
        transform.rotation = targetRotation;
    }
}
