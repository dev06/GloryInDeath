using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventID
{
	NONE,
	ENEMY_KILLED,
	WAVE_START,
	WAVE_END,
	PLAYER_HURT,
}
public class EventManager : MonoBehaviour {

	public delegate void GameEvent(EventID id);
	public static GameEvent OnGameEvent;

	public delegate void ButtonClick(ButtonID id);
	public static ButtonClick OnButtonClick;
	public static ButtonClick OnButtonEnter;

	public delegate void StateChange(State s);
	public static StateChange OnStateChange;

	public delegate void CharacterModelEvent(CharacterModel c);
	public static CharacterModelEvent OnCharacterModelHover;
}
