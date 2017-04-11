using UnityEngine;
using System.Collections;

public class Mininglaser : Weapon {
	[Header("Weapon Specific")]
	[SerializeField]
	private float _chargeTime;

	[SerializeField]
	private float _minDamage;

	[SerializeField]
	private float _maxDamage;

	[SerializeField]
	private int _minBeams;

	[SerializeField]
	private int _maxBeams;

	[SerializeField]
	private int _minAmmoUse;

	[SerializeField]
	private int _maxAmmoUse;

	private int _lastAmmoUse;
	//TODO Scale damage with charge time??
	//TODO Add more lasers after charge
	//TODO Maybe stun enemies?

	private Timer _chargeTimer;

	protected override void Awake () {
		base.Awake();
	}

	protected override void Start () {
		base.Start();

		_chargeTimer = TimerManager.CreateTimer("Mininglaser Chargetimer", false).SetDuration(_chargeTime);
	}

	protected override void Update () {
		base.Update();

		if (_currentState != WeaponState.Hidden) {
			if (_chargeTimer.IsPlaying) {
				int currentAmmoUse = Mathf.RoundToInt(Utilities.Math.Remap(_chargeTimer.FinishedPercentage, 0, 1, _minAmmoUse, _maxAmmoUse));

				_magazineContent -= (currentAmmoUse - _lastAmmoUse);

				if (_magazineContent <= 0) {
					_magazineContent = 0;
					_chargeTimer.Pause(true);
				}

				_lastAmmoUse = currentAmmoUse;
			}

			if (Input.GetMouseButton(0) && _magazineContent != 0 && _currentState == WeaponState.Idle && !_chargeTimer.IsPlaying && !_chargeTimer.IsFinished) {
				_lastAmmoUse = 0;
				_chargeTimer.Start();
			} else if (_magazineContent < _minAmmoUse && _reserveAmmo != 0 && !_chargeTimer.IsFinished && !_chargeTimer.IsPlaying) {
				reload();
			}

			if (!Input.GetMouseButton(0) && _currentState == WeaponState.Idle && (_chargeTimer.IsPlaying || _chargeTimer.IsFinished)) {
				_chargeTimer.Pause();
				int noOfBeams = Mathf.RoundToInt(Utilities.Math.Remap(_chargeTimer.FinishedPercentage, 0, 1, _minBeams, _maxBeams));
				shoot(Utilities.Math.Remap(_chargeTimer.FinishedPercentage, 0, 1, _minDamage, _maxDamage), noOfBeams, false);
				_chargeTimer.Reset();
			}

			if (Input.GetKeyDown(KeyCode.R) && _magazineContent < _magazineCapacity && _reserveAmmo != 0) {
				reload();
			}
		}
	}

	protected override void spawnBullet (Vector3 pHitPoint) {
		Utilities.Weapons.DisplayLaser(_muzzlePosition.position, pHitPoint);
	}

	protected override void spawnDecal (Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		Utilities.Weapons.DisplayDecal(pHitPoint, pHitNormal, pHitTransform);
	}
}
