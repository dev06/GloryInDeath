using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[System.Serializable]
public struct InfoText
{
	public Text name;
	public Text health;
	public Text speed;
	public Text damage;
	public Text armor;
}

public class CharacterSelectUI : MonoBehaviour {




	public State activeState;
	private PlayerController player;

	public InfoText characterInfoText;
	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
		EventManager.OnButtonClick += OnButtonClick;
		EventManager.OnButtonEnter += OnButtonEnter;
		EventManager.OnCharacterModelHover += OnCharacterModelHover;
	}
	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnButtonEnter -= OnButtonEnter;
		EventManager.OnCharacterModelHover -= OnCharacterModelHover;
	}

	void Start()
	{
		player = FindObjectOfType<PlayerController>();
		Toggle(GameController.state == activeState);
	}

	public void OnStateChange(State s)
	{
		if (s != activeState)
		{
			Toggle(false);
		}

	}

	private void OnCharacterModelHover(CharacterModel m)
	{
		switch (m.modelType)
		{
			case Character.CharacterType.AURA_BLACKSWORD:
			{
				//characterInfoText.name.text = "Name: " + CharacterLibrary.CHAR_AURA_BLACKSWORD.info.name;
				characterInfoText.health.text = "Health: " + CharacterLibrary.CHAR_AURA_BLACKSWORD.health.ToString();
				characterInfoText.speed.text = "Speed: " + CharacterLibrary.CHAR_AURA_BLACKSWORD.speed.ToString();
				characterInfoText.damage.text = "Damage: " + CharacterLibrary.CHAR_AURA_BLACKSWORD.damage.ToString();
				characterInfoText.armor.text = "Armor: " + CharacterLibrary.CHAR_AURA_BLACKSWORD.armor.ToString();

				break;
			}

			case Character.CharacterType.HALLFRED_THORALDSON:
			{

				//characterInfoText.name.text = "Name: " + CharacterLibrary.HALLFRED_THORALDSON.info.name;
				characterInfoText.health.text = "Health: " + CharacterLibrary.CHAR_HALLFRED_THORALDSON.health.ToString();
				characterInfoText.speed.text = "Speed: " + CharacterLibrary.CHAR_HALLFRED_THORALDSON.speed.ToString();
				characterInfoText.damage.text = "Damage: " + CharacterLibrary.CHAR_HALLFRED_THORALDSON.damage.ToString();
				characterInfoText.armor.text = "Armor: " + CharacterLibrary.CHAR_HALLFRED_THORALDSON.armor.ToString();

				break;
			}

			case Character.CharacterType.FREYA_SKAAR:
			{
				//	characterInfoText.name.text = "Name: " + CharacterLibrary.FREYA_SKAAR.info.name;
				characterInfoText.health.text = "Health: " + CharacterLibrary.CHAR_FREYA_SKAAR.health.ToString();
				characterInfoText.speed.text = "Speed: " + CharacterLibrary.CHAR_FREYA_SKAAR.speed.ToString();
				characterInfoText.damage.text = "Damage: " + CharacterLibrary.CHAR_FREYA_SKAAR.damage.ToString();
				characterInfoText.armor.text = "Armor: " + CharacterLibrary.CHAR_FREYA_SKAAR.armor.ToString();

				break;
			}
		}
	}

	public void OnButtonEnter(ButtonID id)
	{
		switch (id)
		{

		}
	}

	public void OnButtonClick(ButtonID id)
	{
		switch (id)
		{


			case ButtonID.SELECT_CHARACTER:
			{
				GameController.Instance.SetState(State.GAME);
				break;
			}
			case ButtonID.CHAR_AURA:
			{
				player.SetCharacter(Character.CharacterType.AURA_BLACKSWORD);
				GameController.Instance.SetState(State.GAME);
				break;
			}

			case ButtonID.CHAR_HALFRED:
			{

				player.SetCharacter(Character.CharacterType.HALLFRED_THORALDSON);
				GameController.Instance.SetState(State.GAME);
				break;
			}

			case ButtonID.CHAR_FREYA:
			{
				player.SetCharacter(Character.CharacterType.FREYA_SKAAR);
				GameController.Instance.SetState(State.GAME);
				break;
			}
		}
	}

	private void Toggle(bool b)
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.alpha = b ? 1 : 0;
		cg.blocksRaycasts = b;
	}
}
