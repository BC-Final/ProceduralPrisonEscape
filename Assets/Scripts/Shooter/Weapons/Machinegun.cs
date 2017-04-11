using UnityEngine;
using System.Collections;

public class Machinegun : Weapon {
	protected override void Awake () {
		base.Awake();
	}

	protected override void Start() {
		base.Start();
	}

	protected override void Update () {
		base.Update();

		if (_currentState != WeaponState.Hidden) {
			if (Input.GetMouseButton(0) && _magazineContent != 0 && _currentState == WeaponState.Idle) {
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
		//              FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/smg/PE_weapon_smg_reload").start();
		//              //FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
		//              reload();
		//          }

		//          if (Input.GetMouseButton(0) && _magazineContent != 0 && _canShoot && !_reloading && !_moving) {
		//		FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/smg/PE_weapon_smg_shot").start();
		//		//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
		//		shoot();
		//	} else if (_magazineContent == 0) {

		//	}

		//	if (Input.GetKeyDown(KeyCode.R) && !_reloading && _reserveAmmo != 0 && _magazineContent != _magazineCapacity && !_moving && !_aiming) {
		//		FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/smg/PE_weapon_smg_reload").start();
		//		//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
		//		reload();
		//	}
		//}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		//GameObject tracer = Instantiate(ShooterReferenceManager.Instance.BulletTracer, _muzzlePosition.position, Quaternion.LookRotation(pHitPoint - _muzzlePosition.position)) as GameObject;
		//tracer.GetComponentInChildren<ParticleSystem>().Play();
		//Destroy(tracer, 1.0f);
		//GetComponentInChildren<ParticleSystem>().Play();
		Utilities.Weapons.DisplayBulletTracer(_muzzlePosition.position, pHitPoint);
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		Utilities.Weapons.DisplayDecal(pHitPoint, pHitNormal, pHitTransform);
	}
}
