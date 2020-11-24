using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void CamRotationDel();

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5.0f;
    [SerializeField]
    private float jumpForce = 8.0f;
    private float gravity = 15.0f;

    private float currentSpeed;

    private float mx = 0f;
    private float my = 0f;

    //카메라 민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 한계
    [SerializeField]
    private float cameraRoationLimit;
    private float currentCameraRotaitonX = .0f;

    [SerializeField]
    private Camera playerCamera;
    private CharacterController cCon;
    private CamRotationDel camDel;
    private FollowCamera fCam;
    private bool isGrounded = false;

    private Vector3 moveDir;

    private void Start()
    {
        camDel = new CamRotationDel(CameraVerticalRotation);
        camDel += CameraHorizontalRotation;

        moveDir = Vector3.zero;

        fCam = playerCamera.GetComponent<FollowCamera>();
        cCon = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        
        camDel();
        //CameraRotation();
        //CameraVerticalRotation();
        //CameraHorizontalRotation();

        TryJump();
        CheckGrounded();
    }

    private void CharacterMove()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");

        //플레이어가 바라보고 있는 방향에 상관없이 world space 기준 상하좌우 방향 계산
        //Vector3 dir = new Vector3(dirX, .0f, dirZ).normalized;
        //playerRigidbody.MovePosition(transform.position + dir * currentSpeed * Time.deltaTime);
        
        //플레이어가 바라보고 있는 방향 기준 상하좌우 방향 계산
        Vector3 moveHor = transform.right * dirX;
        Vector3 moveVer = transform.forward * dirZ;

        Vector3 velocity = (moveHor + moveVer).normalized * currentSpeed;

        cCon.Move(velocity * Time.deltaTime);
    }

    //화면 상하 회전
    private void CameraVerticalRotation()
    {
        if (fCam.isFPS && !fCam.isOnSetting)
        {
            float xRotation = Input.GetAxisRaw("Mouse Y");
            //float xRotation = Input.GetAxisRaw("Vertical");
            float cameraRotationX = -xRotation * lookSensitivity;
            currentCameraRotaitonX += cameraRotationX;
            currentCameraRotaitonX = Mathf.Clamp(currentCameraRotaitonX, -cameraRoationLimit, cameraRoationLimit);
            playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotaitonX, 0f, 0f);
        }
    }

    //화면 좌우 회전
    private void CameraHorizontalRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        //float yRotation = Input.GetAxisRaw("Horizontal");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        transform.Rotate(characterRotationY);
    }

    //마우스를 따라 카메라가 회전 - 한번에 상하좌우 모두 처리
    private void CameraRotation()
    {
        float rotSpeed = 200;
        float h = Input.GetAxisRaw("Mouse X");
        float v = Input.GetAxisRaw("Mouse Y");

        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -cameraRoationLimit, cameraRoationLimit);
        playerCamera.transform.eulerAngles = new Vector3(-my, 0f, 0f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, mx, transform.eulerAngles.z);
    }

    private void TryJump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            Jump();
        }
        ApplyGravity();
    }

    private void Jump()
    {
        moveDir.y = jumpForce;
    }

    private void ApplyGravity()
    {
        moveDir.y -= gravity * Time.deltaTime;
        cCon.Move(moveDir * Time.deltaTime);
    }

    private void CheckGrounded()
    {
        var playerHeight = cCon.height / 2;
        var playerRadius = cCon.radius;
        var maxDistance = playerHeight - playerRadius - 0.05f;
        Ray sphRay = new Ray(new Vector3(0, cCon.center.y - playerHeight + playerRadius, 0f), transform.up * -1);
        RaycastHit hit;

        Physics.SphereCast(cCon.center, playerRadius, -transform.up, out hit, maxDistance);

        isGrounded = cCon.isGrounded;
        // Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
        
        //isGrounded = cCon.isGrounded;
        //Debug.Log(isGrounded);
   }
}
