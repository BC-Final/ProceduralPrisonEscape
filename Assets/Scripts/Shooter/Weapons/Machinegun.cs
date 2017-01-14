using UnityEngine;
using System.Collections;

public class Machinegun : Weapon {
	protected override void Start() {
		base.Start();
	}

	public void Update() {
		if (_active) {
			if (Input.GetMouseButton(0) && _magazineContent != 0 && _canShoot && !_reloading && !_moving) {
				FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/smg/PE_weapon_smg_shot").start();
				//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
				shoot();
			} else if (_magazineContent == 0) {

			}

			if (Input.GetKeyDown(KeyCode.R) && !_reloading && _reserveAmmo != 0 && _magazineContent != _magazineCapacity && !_moving && !_aiming) {
				FMODUnity.RuntimeManager.CreateInstance("event:/PE_weapon/smg/PE_weapon_smg_reload").start();
				//FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
				reload();
			}
		}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		
		GameObject tracer = Instantiate(ShooterReferenceManager.Instance.BulletTracer, _muzzlePosition.position, Quaternion.LookRotation(pHitPoint - _muzzlePosition.position)) as GameObject;
		tracer.GetComponentInChildren<ParticleSystem>().Play();
		Destroy(tracer, 1.0f);
		GetComponentInChildren<ParticleSystem>().Play();
		

		//GameObject laser = Instantiate(ShooterReferenceManager.Instance.LaserShot, pHitPoint, Quaternion.identity) as GameObject;
		//laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_muzzlePosition.position));
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		GameObject decal = Instantiate(ShooterReferenceManager.Instance.BulletHole, pHitPoint + pHitNormal * 0.001f, Quaternion.LookRotation(pHitNormal)) as GameObject;
		decal.transform.parent = pHitTransform;
		Destroy(decal, 10);
	}
}
