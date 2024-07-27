using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
//Place this script on the root of the dialogue box prefab.
public class DialogueBox : MonoBehaviour
{
	public int charactersPerSecond = 32;

	public GameObject ButtonPrefab;

	public Text textField;


	public void ReadDialogue(string s)
	{
		StartCoroutine(ReaderEnumerator(s));
	}
	private IEnumerator ReaderEnumerator(string s)
	{
		textField.text = "";
		foreach (char c in s)
		{
			textField.text += c;
			yield return new WaitForSeconds(1f / charactersPerSecond);
		}

	}
}
