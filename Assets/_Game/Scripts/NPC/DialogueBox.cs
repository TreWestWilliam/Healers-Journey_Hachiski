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

	public int charTillNewLine = 30;


	public void ReadDialogue(string s)
	{
		StartCoroutine(ReaderEnumerator(s));
	}
	private IEnumerator ReaderEnumerator(string s)
	{
		textField.text = "";
		int charCount = 0;
		foreach (char c in s)
		{
			charCount++;
			char ch = c;
			if(charCount >= charTillNewLine && ch == ' ')
			{
				ch = '\n';
			}
			textField.text += ch;
			if(ch == '\n')
			{
				charCount = 0;
			}
			yield return new WaitForSeconds(1f / charactersPerSecond);
		}

	}
}
