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
	LEVEL_UP,
	CHARACTER_UPG,
	DEATH,
	CHR_UPG_DAMAGE,
	CHR_UPG_CRITHIT,

}
public class EventManager : MonoBehaviour {

	public delegate void GameEvent(EventID id);
	public static GameEvent OnGameEvent;

	public delegate void ButtonClick(ButtonID id, SimpleButtonHandler handler);
	public static ButtonClick OnButtonClick;
	public static ButtonClick OnButtonEnter;
	public static ButtonClick OnButtonDown;
	public static ButtonClick OnButtonUp; 

	public delegate void StateChange(State s);
	public static StateChange OnStateChange;

	public delegate void CharacterModelEvent(CharacterModel c);
	public static CharacterModelEvent OnCharacterModelHover;

	//refers to old upgrade system, haven't remove yet. 
	public delegate void UpgradeCharacterStat(UpgradeStat.Type type);
	public static UpgradeCharacterStat OnUpgradeStat;


	public delegate void WeaponHitDetector(GameObject go);
	public static WeaponHitDetector OnWeaponHit;

	public delegate void SpecialAttack(string _id);
	public static SpecialAttack OnSpecialAttackHit;

	public delegate void RegularAttack(string _id);
	public static RegularAttack OnRegularAttack;
	public static RegularAttack OnRegularAttackStart;
	public static RegularAttack OnRegularAttackEnd;


}
