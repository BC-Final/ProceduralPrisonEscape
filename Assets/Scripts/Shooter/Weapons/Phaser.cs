using UnityEngine;
using System.Collections;
using StateFramework;
using System;

public class Phaser : Weapon {
	protected override void Start() {
		base.Start();
	}

	public void Update() {
		if (_active) {
			if (Input.GetMouseButtonDown(0) && _magazineContent != 0 && _canShoot) {
				StartCoroutine(shoot());
			} else if (_magazineContent == 0) {

			}

			if (Input.GetKeyDown(KeyCode.R) && !_reloading) {
				StartCoroutine(reload());
			}
		}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		GameObject laser = Instantiate(ShooterReferenceManager.Instance.LaserShot, pHitPoint, Quaternion.identity) as GameObject;
		laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_muzzlePosition.position));
		//GameObject.Destroy(laser, 0.05f);

	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		GameObject decal = Instantiate(ShooterReferenceManager.Instance.BulletHole, pHitPoint + pHitNormal * 0.001f, Quaternion.LookRotation(pHitNormal)) as GameObject;
		decal.transform.parent = pHitTransform;
		Destroy(decal, 10);
	}
}
