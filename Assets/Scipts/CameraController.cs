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

    [SerializeField]
    Vector3 offset = new Vector3(2f, 2f, 0f); // 플레이어와 카메라의 초기 상대 위치

    Vector3 playerDistance; // 플레이어와 카메라의 거리
    float hitDistance = 0f; // 카메라 줌 거리

    [SerializeField]
    float zoomDistance = -1.5f; // 줌 거리 설정

    public bool isGravityReversed = false; // 중력 반전 상태를 추적
    public bool isCameraReversed = false; // 카메라 전환 상태
    private bool isTransitioning = false; // 애니메이션 진행 여부

    private void Start()
    {
        // 초기 위치 설정 (플레이어와의 상대적 오프셋 적용)
        playerDistance = offset;
        ResetGravity();
    }

    private void Update()
    {
        // 목표 위치 계산 (오프셋 포함)
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
        // y 값을 음수로 반전
        offset.y = -Mathf.Abs(offset.y);
        TriggerCameraControl();
    }

    public void ResetGravity()
    {
        // 중력 반전 상태 해제
        isGravityReversed = false;
        // y 값을 양수로 변경
        offset.y = Mathf.Abs(offset.y);
        TriggerCameraControl();
    }

    public void CameraReverse()
    {
        isCameraReversed = true;
        TriggerCameraControl();
    }

    public void ResetCamera()
    {
        isCameraReversed = false;
        TriggerCameraControl();
    }

    private void TriggerCameraControl()
    {
        // 애니메이션이 진행 중이 아니면 실행
        if (!isTransitioning)
        {
            StartCoroutine(CameraControl());
        }
    }

    private IEnumerator CameraControl()
    {
        isTransitioning = true;

        Quaternion targetRotation = Quaternion.identity; // 기본값 설정
        Vector3 targetOffset = offset; // 기본 오프셋

        if (isCameraReversed && isGravityReversed)
        {
            // 중력이 반전된 상태에서의 카메라 회전
            targetRotation = Quaternion.Euler(45f, -90f, 180f);
        }
        else if (isCameraReversed && !isGravityReversed)
        {
            targetRotation = Quaternion.Euler(45f, 90f, 0f);
        }
        else if (!isCameraReversed && isGravityReversed)
        {
            targetRotation = Quaternion.Euler(150f, 45f, 0f);
        }
        else if (!isCameraReversed && !isGravityReversed)
        {
            targetRotation = Quaternion.Euler(30f, 45f, 0f);
        }

        // 애니메이션 실행
        yield return SmoothTransition(targetOffset, targetRotation);

        isTransitioning = false;
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
