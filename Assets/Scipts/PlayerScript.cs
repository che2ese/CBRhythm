using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static bool canPressKey = true;

    // 이동
    [Header("Movement")]
    [SerializeField]
    float moveSpeed = 3f;
    Vector3 posDir = new Vector3();
    public Vector3 pos = new Vector3();
    public Vector3 originPos = new Vector3();

    // 회전
    [Header("Rotation")]
    [SerializeField]
    float spinSpeed = 270f;

    Vector3 rotDir = new Vector3();
    Quaternion rot = new Quaternion();

    // 반동
    [Header("Rebound")]
    [SerializeField]
    float recoilPosY = 0.25f;
    [SerializeField]
    float recoilSpeed = 1.5f;

    [Header("State")]
    bool canMove = true;
    bool isFalling = false;
    public bool isBig = false;

    [SerializeField]
    Transform fakeCube = null;

    public Transform realCube = null;

    // 기타 
    TimingManager tm;
    CameraController cc;
    Rigidbody rb;
    StatusManager sm;
    CubeTrail ct;

    private void Awake()
    {
        tm = FindAnyObjectByType<TimingManager>();
        cc = FindAnyObjectByType<CameraController>();
        sm = FindAnyObjectByType<StatusManager>();
        rb = GetComponentInChildren<Rigidbody>();
        ct = FindAnyObjectByType<CubeTrail>();
    }
    private void Start()
    {
        canPressKey = true;
        originPos = transform.position;
    }

    public void Initialized()
    {
        transform.position = Vector3.zero;
        pos = Vector3.zero;
        realCube.localPosition = Vector3.zero;
        canMove = true;
        canPressKey = true;
        isFalling = false;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                if (canMove && canPressKey && !isFalling)
                {
                    Calc();

                    if (tm.CheckTiming())
                    {
                        StartAction();
                    }
                }
            }
        }
    }
    public void MoveUp()
    {
        if (GameManager.instance.isStartGame && !TeleportPlate.isTeleporting)
        {
            CheckFalling();
            if (canMove && canPressKey && !isFalling)
            {
                posDir = Vector3.forward; // 위쪽
                ExecuteMovement();
            }
        }
    }

    public void MoveDown()
    {
        if (GameManager.instance.isStartGame && !TeleportPlate.isTeleporting)
        {
            CheckFalling();
            if (canMove && canPressKey && !isFalling)
            {
                posDir = Vector3.back; // 아래쪽
                ExecuteMovement();
            }
        }
    }

    public void MoveLeft()
    {
        if (GameManager.instance.isStartGame && !TeleportPlate.isTeleporting)
        {
            CheckFalling();
            if (canMove && canPressKey && !isFalling)
            {
                posDir = Vector3.left; // 왼쪽
                ExecuteMovement();
            }
        }
    }

    public void MoveRight()
    {
        if (GameManager.instance.isStartGame && !TeleportPlate.isTeleporting)
        {
            CheckFalling();
            if (canMove && canPressKey && !isFalling)
            {
                posDir = Vector3.right; // 오른쪽
                ExecuteMovement();
            }
        }
    }


    private void ExecuteMovement()
    {
        // 이동 목표 계산
        pos = transform.position + new Vector3(posDir.z, 0, -posDir.x);

        // 회전 목표 계산
        rotDir = new Vector3(posDir.x, 0f, posDir.z);
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        rot = fakeCube.rotation;

        // 타이밍 체크 후 동작
        if (tm.CheckTiming())
        {
            StartAction();
            ct.isTrailing = true;
            StartCoroutine(ct.SpawnTrail());
            StartCoroutine(EndTrail());
        }
    }
    IEnumerator EndTrail()
    {
        yield return new WaitForSeconds(0.1f);
        ct.isTrailing = false;
    }

    void Calc()
    {
        // 방향 계산
        posDir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        // 이동 목표 계산
        pos = transform.position + new Vector3(posDir.x, 0, -posDir.z);

        // 회전 목표 계산
        rotDir = new Vector3(posDir.z, 0f, posDir.x);
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        rot = fakeCube.rotation;

    }

    void StartAction()
    {
        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(cc.ZoomCam());
    }

    IEnumerator MoveCo()
    {
        canMove = false;

        while (Vector3.SqrMagnitude(transform.position - pos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = pos;

        canMove = true;
    }
    IEnumerator SpinCo()
    {
        while (Quaternion.Angle(realCube.rotation, rot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, rot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = rot;
    }

    IEnumerator RecoilCo()
    {
        while (realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while (realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        realCube.localPosition = new Vector3(0, 0, 0);
    }
    void CheckFalling()
    {
        if (!isFalling && canMove)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                Falling();
            }
        }
    }
    void Falling()
    {
        isFalling = true;
        rb.useGravity = true;
        rb.isKinematic = false; // 물리 효과 끄기
    }
    public void ResetFalling()
    {
        sm.DecreaseHp(1);
        AudioManager.instance.PlaySFX("Falling");

        if (!sm.IsDead())
        {
            isFalling = false;
            rb.useGravity = false;
            rb.isKinematic = true;

            transform.position = originPos;
            realCube.localPosition = new Vector3(0, 0, 0);
        }
    }
    // 강제 이동 메서드
    public void TeleportTo(Vector3 targetPosition)
    {
        // 이동 동작 중지
        StopAllCoroutines();

        // 즉시 위치 변경
        transform.position = targetPosition;

        // 강제 이동 후 상태 초기화
        realCube.localPosition = Vector3.zero;
        canMove = true;
        canPressKey = true;
        Debug.Log($"플레이어가 강제로 {targetPosition}로 이동했습니다.");
    }

}