using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
	public DialogueBox DialogueBoxPrefab;
	public AilmentData ailment;

	public string npcName;

	private GameObject _dialogueBoxInstance;
	private DialogueBox _dialogueBox;

	private bool cureMenu = false;

	private void Awake()
	{
	}

	private void Update()
	{
	}

	public void CreateDialogueBox()
	{
		if (!_dialogueBoxInstance && DialogueBoxPrefab)
		{
			_dialogueBoxInstance =
				Instantiate(DialogueBoxPrefab.transform.root.gameObject,
				transform.position + Vector3.up * 1.5f,
				Camera.main.transform.rotation, transform);
			_dialogueBox = _dialogueBoxInstance.GetComponentInChildren<DialogueBox>();
		}
	}
	public void DestroyDialogueBox()
	{
		DestroyImmediate(_dialogueBoxInstance);
	}

	public void Engage(PlayerMovement player)
	{
		if(ailment != null && player.reputation.CurrentTierRep >= ailment.tier)
		{
			player.beginInteracting();
			player.puzzleHandler.openInteraction(this);
			cureMenu = true;
		}
		else
		{
			CreateDialogueBox();
			_dialogueBox.ReadDialogue("Hello Player!");
		}
	}

	public void recieveCure()
	{
		StartCoroutine(delayedAilment(ailment, 30f));
		ailment = null;
    }

    public void developeAilment(AilmentData problem)
    {
        ailment = problem;
    }

    public IEnumerator delayedAilment(AilmentData problem, float delay)
	{
		yield return new WaitForSeconds(delay);

		developeAilment(problem);
	}

    public void Disengage(PlayerMovement player)
	{
		if(cureMenu)
		{
			cureMenu = false;
			player.puzzleHandler.closeInteraction();
			player.endInteracting();
		}
		else
		{
			DestroyDialogueBox();
		}
	}
}
