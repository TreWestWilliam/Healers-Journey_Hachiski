using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIllness", menuName = "ScriptableObjects/Illness", order = 1)]
public class Illness : ScriptableObject
{
	public string[] Symptoms;
}
