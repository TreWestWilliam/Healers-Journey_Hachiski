using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "ScriptableObjects/DialogueBlock")]
public class DialogueBlock : ScriptableObject
{
	//populate with dialogue lines.
	public string[] dialoguePool;

	public DialogueBranch branch;

	public string GetRandomLine()
	{
		return dialoguePool[Random.Range(0, dialoguePool.Length)];
	}


}

[System.Serializable]
public class DialogueBranch
{
	public string ButtonText;

	public DialogueBlock nextBlock;
}
