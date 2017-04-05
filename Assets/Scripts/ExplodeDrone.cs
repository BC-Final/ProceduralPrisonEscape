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
	private Transform _backParts;

	[SerializeField]
	private Transform _outerParts;

	[SerializeField]
	private float _lifeTime;

	private void Start () {
		GameObject.Destroy(this.gameObject, _lifeTime);
	}

	public void SetAnimationPercentage (float pPercent) {
		_backParts.Rotate(new Vector3(0, 0, 1), Utilities.Math.Remap(pPercent, 0, 1, 0, 360));
		_outerParts.Rotate(new Vector3(0, 0, 1), Utilities.Math.Remap(pPercent, 0, 1, 0, -360));
	}

	public void SetImpact (Vector3 pImpactPoint, Vector3 pDirection, float pForce) {
		foreach (Rigidbody r in transform.GetComponentsInChildren<Rigidbody>()) {
			r.AddForceAtPosition(pDirection * pForce, pImpactPoint);
			r.AddExplosionForce(_explosionForce, _explosionPoint.position, _explosionRadius);
		}
	}
}
