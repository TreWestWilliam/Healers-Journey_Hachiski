using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Burst.CompilerServices;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Variables")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float sprintMultiplier = 2f;
	[SerializeField] private float walkMultiplier = 0.5f;
	[SerializeField] private float interactRange = 2f;
	[SerializeField] private CharacterController cc;
	private Vector3 movement;
	private Vector3 gravity = new Vector3(0, -1f, 0);
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private LayerMask NPCMask;

	private IInteractable currentInteraction;
	private Transform interactionTransform;
	//The original distance to the interacted object.
	private float interactionDist;
    [SerializeField] private EscapeMenu escapeMenu;
    [SerializeField] private GameObject inventory;
	[SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private NotebookHandler notebookHandler;
	public Reputation reputation;
    public PuzzleHandler puzzleHandler;

	[Space]
	[Header("Animation")]
	public Animator animator;

    private bool canMove;
    private bool escMenu;
    private bool invOpen;
    private bool interacting;
    private bool reading;

    private void Awake()
	{
        canMove = true;
		invOpen = false;
        interacting = false;
        reading = false;
    }

	// Update is called once per frame
	void Update()
	{
		// Check for input every frame
		movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		movement.Normalize();
		if (movement.x + movement.z > 0.1) 
		{
			
		}
		movement *= Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f;
        movement *= Input.GetKey(KeyCode.LeftControl) ? walkMultiplier : 1f;
		float MoveSpeed = movement.magnitude;
		bool IsWalking = (MoveSpeed > 0.1);
		animator.SetFloat("MoveSpeed", MoveSpeed);
		animator.SetBool("IsWalking", IsWalking);
		animator.SetBool("IsSprinting", Input.GetKey(KeyCode.LeftShift)); // If we ever change the controls above please change this with it.

		if (canMove && Input.GetKeyDown(KeyCode.E)) Interact();

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (canMove)
			{
				openEscapeMenu();
            }
            else if(escMenu)
            {
				closeEscapeMenu();
            }
            else if(invOpen)
			{
				invOpen = false;
				canMove = true;
				inventory.SetActive(false);
            }
            else if(reading)
            {
                reading = false;
                canMove = true;
                notebookHandler.closeNotebook();
            }
            else if(interacting)
            {
                interacting = false;
                canMove = true;
                currentInteraction.Disengage(this);
            }
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            if(canMove)
            {
                canMove = false;
                invOpen = true;
                inventory.SetActive(true);
				inventoryManager.UpdateInventory();
            }
            else if(invOpen)
            {
                invOpen = false;
                canMove = true;
                inventory.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            if(canMove)
            {
                canMove = false;
                reading = true;
                notebookHandler.openNotebook();
            }
            else if(reading)
            {
                reading = false;
                canMove = true;
                notebookHandler.closeNotebook();
            }
        }

        //HandleInteractNotifs();
	}

	private void FixedUpdate()
	{
		// Check if need to move this update
		if (canMove && movement != Vector3.zero)
		{
			// Rotate and move
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);

			cc.Move(movement * moveSpeed * Time.fixedDeltaTime);
		}

		if(cc.isGrounded) velocity = Vector3.zero;
		velocity += gravity * Time.fixedDeltaTime;
		cc.Move(velocity);
		ControlInteractions();
	}

	private void ControlInteractions()
	{
		if (!interactionTransform) return;

		if ((interactionTransform.position - transform.position).magnitude >= interactionDist + 3f)
		{
			currentInteraction.Disengage(this);
			interactionTransform = null;
			currentInteraction = null;
		}
	}

	public void beginInteracting()
	{
		canMove = false;
		interacting = true;
	}

	public void endInteracting()
	{
		if(interacting)
		{
			interacting = false;
			canMove = true;
			interactionTransform = null;
            currentInteraction = null;
        }
	}

	private void openEscapeMenu()
    {
        canMove = false;
        escMenu = true;
		escapeMenu.openMenu();
    }

	public void closeEscapeMenu()
    {
        escMenu = false;
        canMove = true;
		escapeMenu.closeMenu();
    }

	private void Interact()
	{
		//Debug.Log("Interact Invoked");

		Collider[] ineractionColliders = Physics.OverlapSphere(transform.position, interactRange, 1 << 6);

		IInteractable target = null;
		Transform targetTransform = null;
		float shortestDist = Mathf.Infinity;

		foreach (Collider c in ineractionColliders)
		{
			//Debug.Log("Collider Found " + c.name);
			float dist = (c.transform.position - transform.position).magnitude;
			IInteractable interactable = c.transform.GetComponent<IInteractable>();//transform.root.GetComponentInChildren<IInteractable>();
			if (interactable != null && dist < shortestDist)
			{
				//Debug.Log("Interactable found");
				shortestDist = dist;
				target = interactable;
				targetTransform = c.transform;
			}
		}

		if(target != null)
		{
			currentInteraction = target;
			currentInteraction.Engage(this);
			interactionTransform = targetTransform;
			interactionDist = shortestDist;
		}
	}

	/*private void HandleInteractNotifs()
	{
        //Needs to be rewritten.
        
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactDist, NPCMask))
		{
			if (hit.transform.CompareTag("Ingredient") || hit.transform.CompareTag("NPC"))
			{
				//Refactor collect notifs to be contained within their own script. This way, they can also be applied to NPCs
				hit.transform.GetComponent<InteractNotif>().SetCollectNotifVisible(true);
			}
		}
    }*/
}
