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

	protected override void Start() {
		base.Start();

		_chargeTimer = TimerManager.CreateTimer("Mininglaser Chargetimer", false).SetDuration(_chargeTime);
	}

	//private void consumeAmmo () {
	//	_magazineContent -= 1;

	//	if (_magazineContent <= 0) {
	//		//TODO Stop Timer;
	//		//TODO Stop animation
	//		_magazineContent = 0;
	//		_chargeTimer.Pause().FlagFinished();
	//	}
	//}

	public void Update() {
		if (_active) {
			if (_chargeTimer.IsPlaying) {
				int currentAmmoUse = Mathf.RoundToInt(Utilities.Math.Remap(_chargeTimer.FinishedPercentage, 0, 1, _minAmmoUse, _maxAmmoUse));

				Debug.Log(currentAmmoUse - _lastAmmoUse + " = " + _magazineContent);

				_magazineContent -=  (currentAmmoUse - _lastAmmoUse);

				if (_magazineContent <= 0) {
					_magazineContent = 0;
					_chargeTimer.Pause(true);
				}

				_lastAmmoUse = currentAmmoUse;
			}

			if (Input.GetMouseButtonDown(0) && _magazineContent >= _minAmmoUse && _canShoot && !_reloading && !_moving && !_chargeTimer.IsPlaying && !_chargeTimer.IsFinished) {
				//TODO Start charge animation
				_lastAmmoUse = 0;
				_chargeTimer.Start();
			}
			
			if (!Input.GetMouseButton(0) && _canShoot && !_reloading && !_moving && (_chargeTimer.IsPlaying || _chargeTimer.IsFinished)) {
				_chargeTimer.Pause();
				int noOfBeams = Mathf.RoundToInt(Utilities.Math.Remap(_chargeTimer.FinishedPercentage, 0, 1, _minBeams, _maxBeams));
				SetDamage(Utilities.Math.Remap(_chargeTimer.FinishedPercentage, 0, 1, _minDamage, _maxDamage) / noOfBeams);
				shoot(noOfBeams, false);
				_chargeTimer.Reset();
			}

			/*
			if (Input.GetMouseButtonDown(0) && _magazineContent != 0 && _canShoot && !_reloading && !_moving) {
				shoot();
			} else if (_magazineContent == 0) {

			}
			*/
			

			//if (Input.GetKeyDown(KeyCode.R) && !_reloading && _reserveAmmo != 0 && _magazineContent != _magazineCapacity && !_moving && !_aiming) {
			//	reload();
			//}
		}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		Utilities.Weapons.DisplayLaser(_muzzlePosition.position, pHitPoint);
		//GameObject laser = Instantiate(ShooterReferenceManager.Instance.LaserShot, pHitPoint, Quaternion.identity) as GameObject;
		//laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_muzzlePosition.position));
		//laser.GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
		//GameObject.Destroy(laser, 0.05f);
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		Utilities.Weapons.DisplayDecal(pHitPoint, pHitNormal, pHitTransform);
		//GameObject decal = Instantiate(ShooterReferenceManager.Instance.BulletHole, pHitPoint + pHitNormal * 0.001f, Quaternion.LookRotation(pHitNormal)) as GameObject;
		//decal.transform.parent = pHitTransform;
		//decal.transform.localScale = decal.transform.localScale * 2.0f;
		//Destroy(decal, 10);
	}
}
