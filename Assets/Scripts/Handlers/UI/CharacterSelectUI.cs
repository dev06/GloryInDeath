using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterSelectUI : MonoBehaviour {




	public State activeState;
	private PlayerController player;

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

	private void Toggle(bool b)
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.alpha = b ? 1 : 0;
		cg.blocksRaycasts = b;
	}
}
