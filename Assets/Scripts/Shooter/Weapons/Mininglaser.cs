﻿using UnityEngine;
using System.Collections;

public class Mininglaser : Weapon {
	[Header("Weapon Specific")]
	[SerializeField]
	private float _chargeTime;
	//TODO Scale damage with charge time??
	//TODO Add more lasers after charge
	//TODO Maybe stun enemies?

	protected override void Start() {
		base.Start();
	}

	public void Update() {
		if (_active) {
			if (Input.GetMouseButtonDown(0) && _magazineContent != 0 && _canShoot && !_reloading && !_moving) {
				shoot();
			} else if (_magazineContent == 0) {

			}

			if (Input.GetKeyDown(KeyCode.R) && !_reloading && _reserveAmmo != 0 && _magazineContent != _magazineCapacity && !_moving && !_aiming) {
				reload();
			}
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
