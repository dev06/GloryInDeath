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

	private CSStatHandler csstatHandler;

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
		if (csstatHandler == null)
		{
			csstatHandler = FindObjectOfType<CSStatHandler>();
		}

		if (player == null)
		{
			player =  PlayerController.Instance;
		}

		characterInfoText.health.text = player.attributes.health.ToString();
		characterInfoText.speed.text = player.attributes.speed.ToString();
		characterInfoText.damage.text = player.attributes.damage.ToString();
		characterInfoText.armor.text = player.attributes.armor.ToString();

		switch (m.modelType)
		{
			case CharacterType.AURA_BLACKSWORD:
			{
				//characterInfoText.name.text = "Name: " + CharacterLibrary.CHAR_AURA_BLACKSWORD.info.name;

				break;
			}

				// case CharacterType.HALLFRED_THORALDSON:
				// {

				// 	//characterInfoText.name.text = "Name: " + CharacterLibrary.HALLFRED_THORALDSON.info.name;
				// 	characterInfoText.health.text = player.attributes.health.ToString();
				// 	characterInfoText.speed.text = player.attributes.speed.ToString();
				// 	characterInfoText.damage.text = player.attributes.damage.ToString();
				// 	characterInfoText.armor.text = player.attributes.armor.ToString();

				// 	break;
				// }

				// case CharacterType.FREYA_SKAAR:
				// {
				// 	//	characterInfoText.name.text = "Name: " + CharacterLibrary.FREYA_SKAAR.info.name;
				// 	characterInfoText.health.text =  player.attributes.health.ToString();
				// 	characterInfoText.speed.text =  player.attributes.speed.ToString();
				// 	characterInfoText.damage.text =  player.attributes.damage.ToString();
				// 	characterInfoText.armor.text =  player.attributes.armor.ToString();


				// 	break;
				// }
		}
		csstatHandler.UpdateValues(PlayerController.Instance.attributes);

	}

	public void UpdateCharacterSelectUI()
	{
		characterInfoText.health.text = PlayerController.Instance.attributes.health.ToString();
		characterInfoText.speed.text = PlayerController.Instance.attributes.speed.ToString();
		characterInfoText.damage.text = PlayerController.Instance.attributes.damage.ToString();
		characterInfoText.armor.text = PlayerController.Instance.attributes.armor.ToString();
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
		}
	}

	private void Toggle(bool b)
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.alpha = b ? 1 : 0;
		cg.blocksRaycasts = b;
	}
}
