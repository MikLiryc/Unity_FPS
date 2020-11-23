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
    }

    private void CharacterMove()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(dirX, .0f, dirZ);

        //Vector3 moveHor = transform.right * dirX;
        //Vector3 moveVer = transform.forward * dirZ;
        //
        //Vector3 velocity = (moveHor + moveVer).normalized * currentSpeed;

        playerRigidbody.MovePosition(transform.position + dir * currentSpeed * Time.deltaTime);
        //playerRigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }
}
