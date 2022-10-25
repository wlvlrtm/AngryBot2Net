using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    private CharacterController controller;
    private Animator animator;
    private new Transform transform;
    private new Camera camera;
    
    private Plane plane;
    private Ray ray;
    private Vector3 hitPoint;

    public float moveSpeed = 10.0f;


    private void Start() {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        
        camera = Camera.main;
        plane = new Plane(transform.up, transform.position);
    }

    private void Update() {
        Move();
        Turn();
    }

    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    private void Move() {
        Vector3 cameraForward = camera.transform.forward;   // Z
        Vector3 cameraRight = camera.transform.right;       // X

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);

        controller.SimpleMove(moveDir * moveSpeed);

        float forward = Vector3.Dot(moveDir, transform.forward);    // Forward & Back
        float strafe = Vector3.Dot(moveDir, transform.right);       // Left & Right

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    private void Turn() {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        plane.Raycast(ray, out enter);
        hitPoint = ray.GetPoint(enter);
        
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0;
        transform.localRotation = Quaternion.LookRotation(lookDir);
    }
}