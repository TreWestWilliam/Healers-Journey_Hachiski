using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	public GameObject DialogueBoxPrefab;

	private GameObject _dialogueBoxInstance;
	private DialogueBox _dialogueBox;

	private void Awake()
	{
		CreateDialogueBox();
	}

	public void CreateDialogueBox()
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

		if (!_dialogueBoxInstance && DialogueBoxPrefab)
		{
			_dialogueBoxInstance = Instantiate(DialogueBoxPrefab.transform.root.gameObject);
			_dialogueBox = _dialogueBoxInstance.GetComponentInChildren<DialogueBox>();

			_dialogueBox.transform.position = screenPosition;
		}
	}
	public void DestroyDialogueBox()
	{
		DestroyImmediate(_dialogueBoxInstance);
	}
}
