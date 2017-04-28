using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Gamelogic.Extensions;

public class PlayerHealth : Singleton<PlayerHealth>, IDamageable, ITargetable {
	[SerializeField]
	[Tooltip("The maximum amount of health")]
	private float _maxHealth;

	public GameObject GameObject { get { return gameObject; } }
	public Faction Faction { get { return Faction.Player; } }

	private System.Action<GameObject> _destroyEvent;

	public void AddToDestroyEvent (System.Action<GameObject> pObject) {
		_destroyEvent += pObject;
	}

	public void RemoveFromDestroyEvent (System.Action<GameObject> pObject) {
		_destroyEvent -= pObject;
	}



	/// <summary>
	/// Accessor for max health
	/// </summary>
	public float MaxHealth {
		get { return _maxHealth; }
	}


	/// <summary>
	/// The current health of the shooter
	/// </summary>
	private ObservedValue<float> _currentHealth;



	/// <summary>
	/// Accessor for current health and its events
	/// </summary>
	public ObservedValue<float> CurrentHealth {
		get { return _currentHealth; }
	}



	/// <summary>
	/// Sets health to maxhealth
	/// </summary>
	private void Awake() {
		_currentHealth = new ObservedValue<float>(_maxHealth);
	}



	public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
		_currentHealth.Value = Mathf.Max(0.0f, _currentHealth.Value - pDamage);

		FindObjectOfType<DamageIndicatorControllerShooterUI>().ShowHitMarker(pSource);
		
		if (_currentHealth.Value == 0.0f) {
			_destroyEvent.Invoke(gameObject);
			//TODO Add death event
		}
	}



	/// <summary>
	/// Removes damage from shooter health and handles feedback 
	/// </summary>
	/// <param name="pAmount"></param>
	public void HealDamage(float pAmount) {
		_currentHealth.Value = Mathf.Min(_maxHealth, _currentHealth.Value + pAmount);
	}
}
