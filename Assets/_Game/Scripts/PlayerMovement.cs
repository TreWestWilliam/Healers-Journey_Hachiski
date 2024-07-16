using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public CharacterController cc;
    Vector3 movement;
    Vector3 gravity;

    private void Awake()
    {
        gravity = new Vector3 (0, -9.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(movement.magnitude > 1) movement = movement.normalized;
    }

    void FixedUpdate()
    {
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            cc.Move(movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
