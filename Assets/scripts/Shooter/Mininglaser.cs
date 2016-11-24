using UnityEngine;
using System.Collections;

public class Mininglaser : Weapon {
	[Header("Weapon Specific")]
	[SerializeField]
	private float _chargeTime;
	//TODO Scale damage with charge time??

	protected override void Start() {
		base.Start();
	}

	public void Update() {
		if (Input.GetMouseButtonDown(0) && _magazineContent != 0 && _canShoot) {
			StartCoroutine(shoot());
		} else if (_magazineContent == 0) {

		}

		if (Input.GetKeyDown(KeyCode.R) && !_reloading) {
			StartCoroutine(reload());
		}
	}

	protected override void spawnBullet(Vector3 pHitPoint) {
		GameObject laser = Instantiate(Resources.Load("prefabs/shooter/pfb_laser"), pHitPoint, Quaternion.identity) as GameObject;
		laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_muzzlePosition.position));
		laser.GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
		GameObject.Destroy(laser, 0.05f);
	}

	protected override void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform) {
		GameObject decal = Instantiate(Resources.Load("prefabs/shooter/pfb_bullethole"), pHitPoint + pHitNormal * 0.001f, Quaternion.LookRotation(pHitNormal)) as GameObject;
		decal.transform.parent = pHitTransform;
		decal.transform.localScale = decal.transform.localScale * 2.0f;
		Destroy(decal, 10);
	}
}
