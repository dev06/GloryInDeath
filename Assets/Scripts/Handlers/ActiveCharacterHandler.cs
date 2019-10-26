using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActiveCharacterHandler : MonoBehaviour
{
	public CharacterStats stats;
	public HealthHandler healthHandler;
	public GameObject shield, sword;
	public Transform activeParentTransform, unactiveParentTranform;

	public Transform swordActiveTransform; 

	private PlayerController _player;
	private bool _shieldActive;
	void OnEnable ()
	{
		EventManager.OnButtonClick += OnButtonClick;
	}
	void OnDisable ()
	{
		EventManager.OnButtonClick -= OnButtonClick;
	}
	void Start ()
	{
		_player = PlayerController.Instance;
	}
	void Update ()
	{
		if (_player.IsDead)
		{
			healthHandler.Toggle (false);
		}
		healthHandler.health = PlayerController.Instance.HealthRatio;
		healthHandler.healthString = PlayerController.Instance.HealthText;
		healthHandler.stamina = PlayerController.Instance.StaminaRatio;
		healthHandler.levelProgress = PlayerController.Instance.LevelProgress;
		healthHandler.levelTextString = PlayerController.Instance.LevelString;
	}
	void OnButtonClick (ButtonID id, SimpleButtonHandler handler)
	{
		switch (id)
		{
			case ButtonID.DEFENSE:
				{
					if (stats.type == CharacterType.AURA_BLACKSWORD)
					{
						_shieldActive = !_shieldActive;
						if (_shieldActive)
						{
							shield.transform.SetParent (activeParentTransform);
							shield.transform.localPosition = Vector3.zero;
							shield.transform.localRotation = Quaternion.Euler (Vector3.zero);

							sword.transform.SetParent (unactiveParentTranform);
							sword.transform.localPosition = Vector3.zero;
							sword.transform.localRotation = Quaternion.Euler (Vector3.zero);
						}
						else
						{
							shield.transform.SetParent (unactiveParentTranform);
							shield.transform.localPosition = Vector3.zero;
							shield.transform.localRotation = Quaternion.Euler (Vector3.zero);

							sword.transform.SetParent (swordActiveTransform);
							sword.transform.localPosition = Vector3.zero;
							sword.transform.localRotation = Quaternion.Euler (Vector3.zero);
						}

					}
					break;
				}
		}
	}

	public bool ShieldActive
	{
		get { return _shieldActive; }
	}

	public CharacterType Type
	{
		get { return stats.type; }
	}
}