using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float damp = 0.3f;

    private Animator anim;
    private Rigidbody controller;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Tick(Vector3 cameraForward, Vector2 rawInputs, bool sprinting, bool crouching, float delta)
    {
        Vector3 forwardVector = transform.InverseTransformDirection(cameraForward);
        float turnAngle = Mathf.Atan2(forwardVector.x, forwardVector.z);

        AnimatorInputs(rawInputs, sprinting, crouching, turnAngle, delta);
        Turn(turnAngle, delta);
    }

    private void AnimatorInputs(Vector2 rawInputs, bool sprinting, bool crouching, float turnAngle, float delta)
    {
        anim.SetBool("Crouching", crouching);
        anim.SetBool("Sprinting", sprinting);
        anim.SetFloat("Turn", turnAngle);
        anim.SetFloat("Vertical", rawInputs.magnitude, damp, delta);
    }

    private void Turn(float turnAngle, float delta)
    {
        transform.Rotate(0, turnAngle * 360 * delta, 0);
    }

    private void OnAnimatorMove()
    {
        Vector3 v = anim.deltaPosition / Time.deltaTime;
        v.y = controller.velocity.y;
        controller.velocity = v;
    }
}
