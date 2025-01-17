using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static bool canPressKey = true;

    // 이동
    [SerializeField]
    float moveSpeed = 3f;

    Vector3 posDir = new Vector3();
    public Vector3 pos = new Vector3();
    Vector3 originPos = new Vector3();

    // 회전
    [SerializeField]
    float spinSpeed = 270f;

    Vector3 rotDir = new Vector3();
    Quaternion rot = new Quaternion();

    // 반동
    [SerializeField]
    float recoilPosY = 0.25f;
    [SerializeField]
    float recoilSpeed = 1.5f;

    bool canMove = true;
    bool isFalling = false;

    [SerializeField]
    Transform fakeCube = null;
    [SerializeField]
    Transform realCube = null;

    // 기타 
    TimingManager tm;
    CameraController cc;
    Rigidbody rb;

    private void Awake()
    {
        tm = FindAnyObjectByType<TimingManager>();
        cc = FindAnyObjectByType<CameraController>();
        rb = GetComponentInChildren<Rigidbody>();
    }
    private void Start()
    {
        canPressKey = true;
        originPos = transform.position;
    }
    // Update is called once per frame
    void Update()
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
        while(realCube.position.y < recoilPosY)
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
        isFalling = false;
        rb.useGravity = false;
        rb.isKinematic = true;

        transform.position = originPos;
        realCube.localPosition = new Vector3(0, 0, 0);
    }
}
