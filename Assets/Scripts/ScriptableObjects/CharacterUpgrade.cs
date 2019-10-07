using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "characterUpgrade", menuName = "ScriptableObjects/CharacterUpgrade")]
public class CharacterUpgrade : ScriptableObject
{
	public string title;
	public Sprite icon;
	public ButtonID buttonID;
}
