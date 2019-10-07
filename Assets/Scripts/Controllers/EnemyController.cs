using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public static EnemyController Instance;
	public EnemyStats goblin;
	public EnemyStats orc;

	[HideInInspector]
	public  EnemyAttributes goblinAttributes, orcAttributes;
	void OnEnable()
	{
		EventManager.OnGameEvent += OnGameEvent;
	}
	void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start()
	{
		goblinAttributes.SetAttributes(goblin);
		orcAttributes.SetAttributes(orc);
	}

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.LEVEL_UP:
			{
				goblinAttributes.SetAttributes(updateAttributes(goblinAttributes, goblin));
				orcAttributes.SetAttributes(updateAttributes(orcAttributes, orc));
				break;
			}
		}
	}

	private EnemyAttributes updateAttributes(EnemyAttributes _ref, EnemyStats _stats)
	{
		EnemyAttributes _a = new EnemyAttributes();
		_a.SetAttributes(_ref);
		_a.health = _a.health + (_a.health * _stats.healthMult);
		_a.damage = _a.damage + (_a.damage * _stats.damageMult);
		return _a;
	}
}
