using UnityEngine;
using System.Collections;
using StateFramework;
using System;

public class Phaser : Weapon {
	protected override void Awake () {
		base.Awake();
	}

	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update();

		if (_currentState != WeaponState.Hidden) {
			if (Input.GetMouseButtonDown(0) && _magazineContent != 0 && _currentState == WeaponState.Idle) {
				shoot(_shootDamage);
			} else if (Input.GetMouseButtonDown(0) && _magazineContent == 0 && _reserveAmmo != 0) {
				reload();
			}

			if (Input.GetKeyDown(KeyCode.R) && _magazineContent < _magazineCapacity && _reserveAmmo != 0) {
				reload();
			}
		}
	}

	protected override void abortShot() {
		base.abortShot();
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		Utilities.Weapons.DisplayLaser(_muzzlePosition.position, pHitPoint);
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		Utilities.Weapons.DisplayDecal(pHitPoint, pHitNormal, pHitTransform);
	}
}
