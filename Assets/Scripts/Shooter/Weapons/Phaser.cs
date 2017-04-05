﻿using UnityEngine;
using System.Collections;
using StateFramework;
using System;

public class Phaser : Weapon {
	protected override void Start() {
		base.Start();
	}

	public void Update() {
		if (_active) {
			if (Input.GetMouseButtonDown(0) && _magazineContent != 0 && _canShoot && !_reloading && !_moving) {
				shoot();
				FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/ep/PE_weapon_ep_shoot");
				//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
				ins.start();
			} else if (_magazineContent == 0) {

			}

			if (Input.GetKeyDown(KeyCode.R) && !_reloading && _reserveAmmo != 0 && _magazineContent != _magazineCapacity && !_moving && !_aiming) {
				FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/ep/PE_weapon_ep_reload");
				//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
				ins.start();

				reload();
			}
		}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		Debug.Log("Direct Spawn");
		Utilities.Weapons.DisplayLaser(_muzzlePosition.position, pHitPoint);
		//GameObject laser = Instantiate(ShooterReferenceManager.Instance.LaserShot, pHitPoint, Quaternion.identity) as GameObject;
		//laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_muzzlePosition.position));
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		Utilities.Weapons.DisplayDecal(pHitPoint, pHitNormal, pHitTransform);
		//GameObject decal = Instantiate(ShooterReferenceManager.Instance.BulletHole, pHitPoint + pHitNormal * 0.001f, Quaternion.LookRotation(pHitNormal)) as GameObject;
		//decal.transform.parent = pHitTransform;
		//Destroy(decal, 10);
	}
}
