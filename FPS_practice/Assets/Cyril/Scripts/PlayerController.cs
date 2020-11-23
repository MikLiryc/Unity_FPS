using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5.0f;
    [SerializeField]
    private float jumpForce = 3.0f;

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
    private Rigidbody playerRigidbody;

    private void Start()
    {

        playerRigidbody = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        CameraRotation();
        //CameraVerticalRotation();
        //CameraHorizontalRotation();
    }

    private void CharacterMove()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");

        //플레이어가 바라보고 있는 방향에 상관없이 world space 기준 상하좌우 방향 계산
        //Vector3 dir = new Vector3(dirX, .0f, dirZ).normalized;
        //playerRigidbody.MovePosition(transform.position + dir * currentSpeed * Time.deltaTime);


        //플레이어가 바라보고 있는 방향 기준 상하좌우 방향 계싼
        Vector3 moveHor = transform.right * dirX;
        Vector3 moveVer = transform.forward * dirZ;
        
        Vector3 velocity = (moveHor + moveVer).normalized * currentSpeed;
        
        playerRigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    //화면 상하 회전
    private void CameraVerticalRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        //float xRotation = Input.GetAxisRaw("Vertical");
        float cameraRotationX = -xRotation * lookSensitivity;
        currentCameraRotaitonX += cameraRotationX;
        currentCameraRotaitonX = Mathf.Clamp(currentCameraRotaitonX, -cameraRoationLimit, cameraRoationLimit);
        playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotaitonX, 0f, 0f);
    }

    //화면 좌우 회전
    private void CameraHorizontalRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        //float yRotation = Input.GetAxisRaw("Horizontal");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(characterRotationY));
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
        playerCamera.transform.eulerAngles = new Vector3(-my, mx, 0f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, mx, transform.eulerAngles.z);
    }
}
