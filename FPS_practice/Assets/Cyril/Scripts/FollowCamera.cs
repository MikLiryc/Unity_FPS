using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;                        //카메라가 따라다닐 타겟
    public Transform targetFirst;                   //1인칭 시점
    public Transform focusPoint;
    public float speed = 10.0f;                     //카메라 이동속도

    public bool isFPS = true;
    public bool isOnSetting = false;

    private Vector3 FPSPos;
    private Vector3 TPSPos;
    private Vector3 FPSFocus;
    private Vector3 TPSFocus; 

    void Start()
    {
        FPSPos = new Vector3(0f, 1f, 0f);
        TPSPos = new Vector3(0f, 4f, -4f);
        FPSFocus = new Vector3(0f, 0.8f, 5f);
        TPSFocus = new Vector3(0f, 0.8f, 0f);
    }

    void Update()
    {
        //FollowTarget();

        //FPS to TPS, TPS to FPS
        ChangeView();
        LookAtTarget();
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (isFPS)
        {
            //카메라의 위치를 강제로 타겟에 고정
            if (transform.localPosition != FPSPos)
            {
                isOnSetting = true;
                transform.localPosition = Vector3.Lerp(transform.localPosition, FPSPos, 3 * Time.deltaTime);
                focusPoint.localPosition = Vector3.Lerp(focusPoint.localPosition, FPSFocus, 3 * Time.deltaTime);
                transform.LookAt(focusPoint);
                float distance = (FPSPos - transform.localPosition).sqrMagnitude;
                if (distance < 0.01f)
                {
                    transform.localPosition = FPSPos;
                    focusPoint.localPosition = FPSFocus;
                    isOnSetting = false;
                }
            }
        }
        else
        {
            if (transform.localPosition != TPSPos)
            {
                isOnSetting = true;
                transform.localPosition = Vector3.Lerp(transform.localPosition, TPSPos, 2 * Time.deltaTime);
                focusPoint.localPosition = Vector3.Lerp(focusPoint.localPosition, TPSFocus, 2 * Time.deltaTime);
                float distance = (TPSPos - transform.localPosition).sqrMagnitude;
                if (distance < 0.01f)
                {
                    transform.localPosition = TPSPos;
                    focusPoint.localPosition = TPSFocus;
                    isOnSetting = false;
                }
            }
        }
    }

    private void LookAtTarget()
    {
        if (!isFPS || isOnSetting)
        {
            transform.LookAt(focusPoint);
        }
    }

    private void ChangeView()
    {
        if (Input.GetKeyDown("1"))
        {
            isFPS = true;
        }
        if (Input.GetKeyDown("3"))
        {
            isFPS = false;
        }
    }

    private void FollowTarget()
    {
        //타겟 방향 구하기
        //방향 = 타겟 - 자신
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        
        transform.Translate(dir * speed * Time.deltaTime);

        //일정거리 이상 붙으면 고정시키는 방법
        //1. Distance() 사용
        //if (Vector3.Distance(target.position, transform.position) < 1.0f) transform.position = target.position;
        
        //2. magnitude 사용
        //float distance = (target.position - transform.position).magnitude; //dir.magnitude랑 똑같은듯?
        //if (distance < 1.0f) transform.position = target.position;
        
        //3. sqrMagnitude 사용 (정확한 값은 아니지만, 제곱근 연산을 하지 않기 때문에, 연산량이 적어 크기만 비교할 때 많이 사용한다)
        float distance1 = (target.position - transform.position).sqrMagnitude;
        if (distance1 < 1.0f) transform.position = target.position;

        //위와 같이 했는데도 카메라가 흔들리는 경우
        //FollowTarget 을 LateUpdate()에 넣으면 해결됨...

    }
}
