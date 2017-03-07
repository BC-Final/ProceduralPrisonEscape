using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeDrone : MonoBehaviour {
	[SerializeField]
	private float _explosionForce;

	[SerializeField]
	private float _explosionRadius;

	[SerializeField]
	private Transform _explosionPoint;

	[SerializeField]
	private float _lifeTime;

	private void Start () {
		GameObject.Destroy(this.gameObject, _lifeTime);
	}

	public void SetImpact (Vector3 impactPoint, Vector3 impactDir, float pDamage) {
		foreach (Rigidbody r in transform.GetComponentsInChildren<Rigidbody>()) {
			r.AddForceAtPosition(impactDir * pDamage * 2, impactPoint);
			r.AddExplosionForce(_explosionForce, _explosionPoint.position, _explosionRadius);
		}
	}
}
