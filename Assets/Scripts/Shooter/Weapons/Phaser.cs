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
			} else if (_magazineContent == 0 && _reserveAmmo != 0) {
				reload();
			}

			if (Input.GetKeyDown(KeyCode.R) && _magazineContent < _magazineCapacity && _reserveAmmo != 0) {
				reload();
			}
		}

		//if (_active) {
  //          if (Input.GetMouseButtonDown(0) && _magazineContent == 0 && !_reloading && _reserveAmmo != 0 && _magazineContent != _magazineCapacity && !_moving && !_aiming)
  //          {

  //              reload();
  //          }

  //          if (Input.GetMouseButtonDown(0) && _magazineContent != 0 && _canShoot && !_reloading && !_moving) {
		//		shoot();
		//		FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/ep/PE_weapon_ep_shoot");
		//		//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
		//		ins.start();
		//	} else if (_magazineContent == 0) {

		//	}

		//	if (Input.GetKeyDown(KeyCode.R) && !_reloading && _reserveAmmo != 0 && _magazineContent != _magazineCapacity && !_moving && !_aiming) {
		//		FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/ep/PE_weapon_ep_reload");
		//		//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
		//		ins.start();

		//		reload();
		//	}
		//}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		Utilities.Weapons.DisplayLaser(_muzzlePosition.position, pHitPoint);
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		Utilities.Weapons.DisplayDecal(pHitPoint, pHitNormal, pHitTransform);
	}
}
