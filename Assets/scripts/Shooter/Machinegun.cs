﻿using UnityEngine;
using System.Collections;

public class Machinegun : Weapon {
	public void Update() {
		if (Input.GetMouseButton(0) && _magazineContent != 0 && _canShoot) {
			StartCoroutine(shoot());
		} else if (_magazineContent == 0) {

		}

		if (Input.GetKeyDown(KeyCode.R) && !_reloading) {
			StartCoroutine(reload());
		}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		//GameObject laser = Instantiate(Resources.Load("prefabs/shooter/pfb_laser"), pHitPoint, Quaternion.identity) as GameObject;
		//laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_muzzlePosition.position));
		//GameObject.Destroy(laser, 0.05f);
		GetComponentInChildren<ParticleSystem>().Play();
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		GameObject decal = Instantiate(Resources.Load("prefabs/shooter/pfb_bullethole"), pHitPoint + pHitNormal * 0.001f, Quaternion.LookRotation(pHitNormal)) as GameObject;
		decal.transform.parent = pHitTransform;
		Destroy(decal, 10);
	}
}
