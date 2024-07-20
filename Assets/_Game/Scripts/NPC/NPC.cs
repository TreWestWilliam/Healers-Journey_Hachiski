using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
	public DialogueBox DialogueBoxPrefab;

	private GameObject _dialogueBoxInstance;
	private DialogueBox _dialogueBox;

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
				Camera.main.transform.rotation,transform);
			_dialogueBox = _dialogueBoxInstance.GetComponentInChildren<DialogueBox>();
		}
	}
	public void DestroyDialogueBox()
	{
		DestroyImmediate(_dialogueBoxInstance);
	}

	public void Engage()
	{
		CreateDialogueBox();
		_dialogueBox.textField.text = "Hello Player!";
	}

	public void Disengage()
	{
		DestroyDialogueBox();
	}
}
