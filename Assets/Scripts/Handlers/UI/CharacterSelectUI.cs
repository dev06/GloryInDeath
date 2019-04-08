using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectUI : MonoBehaviour {


	public TextMeshProUGUI goldText;
	public Image leftArrow, rightArrow;
	public State activeState;
	private PlayerController player;
	private CSStatHandler csstatHandler;
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
		if (csstatHandler == null)
		{
			csstatHandler = FindObjectOfType<CSStatHandler>();
		}

		if (player == null)
		{
			player =  PlayerController.Instance;
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
				PlayerController.Instance.SetCharacter(CharacterModel.SELECTED_MODEL.modelType);
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
}
