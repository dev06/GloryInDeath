using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectUI : MonoBehaviour {


	public TextMeshProUGUI goldText, characterDescText, sessionGoldCollectedText;
	public Image leftArrow, rightArrow;
	public CanvasGroup goldCollectedDialog;
	public State activeState;
	private PlayerController player;
	private DragSnapHandler dragSnap;


	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
		EventManager.OnButtonClick += OnButtonClick;
		EventManager.OnButtonEnter += OnButtonEnter;
		EventManager.OnCharacterModelHover += OnCharacterModelHover;
		EventManager.OnUpgradeStat += OnUpgradeStat;
	}
	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnButtonEnter -= OnButtonEnter;
		EventManager.OnCharacterModelHover -= OnCharacterModelHover;
		EventManager.OnUpgradeStat -= OnUpgradeStat;
	}

	void Start()
	{
		player = FindObjectOfType<PlayerController>();
		dragSnap = FindObjectOfType<DragSnapHandler>();
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
		if (player == null)
		{
			player =  PlayerController.Instance;
		}

		UpdateCharacterDescription(m);
	}

	private void UpdateCharacterDescription(CharacterModel m)
	{

		switch (m.ModelType)
		{
			case CharacterType.AURA_BLACKSWORD:
			{
				characterDescText.text = "Name: Aura Blacksword\nRace: Human\nGender: Female";

				break;
			}
			case CharacterType.HALLFRED_THORALDSON:
			{
				characterDescText.text = "Name: Hallfred Thoraldson\nRace: Dwarf\nGender: Male";

				break;
			}
			case CharacterType.FREYA_SKAAR:
			{
				characterDescText.text = "Name: Freya Skaar\nRace: - \nGender: Female";

				break;
			}
		}
	}



	public void OnButtonEnter(ButtonID id, SimpleButtonHandler handler)
	{
		switch (id)
		{

		}
	}

	public void OnButtonClick(ButtonID id, SimpleButtonHandler handler)
	{
		switch (id)
		{
			case ButtonID.SELECT_CHARACTER:
			{
				PlayerController.Instance.SetCharacter(CharacterModel.SELECTED_MODEL.ModelType);
				GameController.Instance.SetState(State.GAME);
				break;
			}
		}
	}

	public void UpdateUI()
	{
		goldText.text = GameController.Instance.CurrentGold + "";
	}

	public void ToggleArrows(int currentIndex, int childCount)
	{
		leftArrow.enabled = (currentIndex > 0);
		rightArrow.enabled = (currentIndex < childCount - 1);
	}

	private void Toggle(bool b)
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.alpha = b ? 1 : 0;
		cg.blocksRaycasts = b;
		if (b)
		{
			UpdateUI();
		}
	}

	private void OnUpgradeStat(UpgradeStat.Type type)
	{
		UpdateUI();
	}

	public void TriggerGoldCollectedDialog()
	{
		GameController.Instance.AddGold(WaveController.Instance.GoldCollected);
		sessionGoldCollectedText.text = WaveController.Instance.GoldCollected.ToString();
		goldCollectedDialog.alpha = 1f;
		goldCollectedDialog.blocksRaycasts = true;
		UpdateUI();

	}

	public void DisableGoldCollectedDialog()
	{
		goldCollectedDialog.alpha = 0f;
		goldCollectedDialog.blocksRaycasts = false;
	}
}
