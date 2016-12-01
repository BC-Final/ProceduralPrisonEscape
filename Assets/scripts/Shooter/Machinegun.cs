using UnityEngine;
using System.Collections;

public class Machinegun : Weapon {
	[Header("Specific")]
	[SerializeField]
	private GameObject _bullettracer;

	protected override void Start() {
		base.Start();
	}

	public void Update() {
		if (_active) {
			if (Input.GetMouseButton(0) && _magazineContent != 0 && _canShoot) {
				StartCoroutine(shoot());
			} else if (_magazineContent == 0) {

			}

			if (Input.GetKeyDown(KeyCode.R) && !_reloading) {
				StartCoroutine(reload());
			}
		}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		GameObject tracer = Instantiate(_bullettracer, _muzzlePosition.position, Quaternion.LookRotation(pHitPoint - _muzzlePosition.position)) as GameObject;
		tracer.GetComponentInChildren<ParticleSystem>().Play();
		Destroy(tracer, 1.0f);
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
