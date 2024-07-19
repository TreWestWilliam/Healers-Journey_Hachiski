using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float interactDist = 2f;
    [SerializeField] private CharacterController cc;
    private Vector3 movement;
    private Vector3 gravity;
    [SerializeField] private LayerMask NPCMask;

    private void Awake()
    {
        gravity = new Vector3 (0, -9.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input every frame
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Normalize movement
        if(movement.magnitude > 1) movement = movement.normalized;

        if(Input.GetKeyDown(KeyCode.E) ) Interact();
    }

    void FixedUpdate()
    {
        // Check if need to move this update
        if (movement != Vector3.zero)
        {
            // Rotate and move
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            cc.Move(movement * moveSpeed * Time.fixedDeltaTime);
        }

        // Apply gravity so slopes work correctly
        cc.Move(gravity * Time.fixedDeltaTime);
    }

    private void Interact()
    {
        // Check if object in Interactable layer in front of player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactDist, NPCMask))
        {
            // Check whether object is NPC or Ingredient using tags
            if (hit.transform.CompareTag("NPC"))
            {
                Debug.Log("NPC: " + hit.transform.name);
            }
            else if (hit.transform.CompareTag("Ingredient"))
            {
                Debug.Log("Ingredient: " + hit.transform.name);
            }
        }
    }
}
