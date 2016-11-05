using UnityEngine;
using System.Collections;
using StateFramework;

public class Weapon : MonoBehaviour {
	[SerializeField]
	private float _shootDamage;

	[SerializeField]
	private float _shootRange;

	[SerializeField]
	private float _shootRate;
	public float ShootRate { get { return _shootRate; } }

	[SerializeField]
	private float _spreadConeRadius;

	[SerializeField]
	private float _spreadConeLength;

	[SerializeField]
	private int _magazineCapacity;

	[SerializeField]
	private int _maxReserveAmmo;

	private int _magazineContent;
	private int _reserveAmmo;

	protected StateMachine<AbstractWeaponState> _fsm;
}
