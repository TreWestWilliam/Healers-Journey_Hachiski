using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float sprintMultiplier = 2f;
	[SerializeField] private float walkMultiplier = 0.5f;
	[SerializeField] private float interactRange = 2f;
	[SerializeField] private CharacterController cc;
	private Vector3 movement;
	private Vector3 gravity;
	[SerializeField] private LayerMask NPCMask;

	private IInteractable currentInteraction;
	private Transform interactionTransform;
	//The original distance to the interacted object.
	private float interactionDist;

	private void Awake()
	{
		gravity = new Vector3(0, -9.8f, 0);
	}

	// Update is called once per frame
	void Update()
	{
		// Check for input every frame
		movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		movement.Normalize();

		movement *= Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f;

		if (Input.GetKeyDown(KeyCode.E)) Interact();
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
		ControlInteractions();
	}

	private void ControlInteractions()
	{
		if (!interactionTransform) return;

		if ((interactionTransform.position - transform.position).magnitude >= interactionDist + 3f)
		{
			currentInteraction.Disengage();
			interactionTransform = null;
			currentInteraction = null;
		}
	}

	private void Interact()
	{
		Debug.Log("Interact Invoked");

		Collider[] ineractionColliders = Physics.OverlapSphere(transform.position, interactRange);

		IInteractable target = null;
		Transform targetTransform = null;
		float shortestDist = Mathf.Infinity;

		foreach (Collider c in ineractionColliders)
		{
			Debug.Log("Collider Found");
			float dist = (c.transform.position - transform.position).magnitude;
			IInteractable interactable = c.transform.root.GetComponentInChildren<IInteractable>();
			if (interactable != null && dist < shortestDist)
			{
				Debug.Log("Interactable found");
				shortestDist = dist;
				target = interactable;
				targetTransform = c.transform;
			}
		}

		if (target != null)
		{
			currentInteraction = target;
			currentInteraction.Engage();
			interactionTransform = targetTransform;
			interactionDist = shortestDist;
		}

		/*
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
		}*/
	}
}
