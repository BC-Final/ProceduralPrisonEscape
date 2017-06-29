using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneExplodeBody : MonoBehaviour {
	[SerializeField]
	private float _thresholdForce;

	[SerializeField]
	private UnityEngine.Events.UnityEvent _onBodyCollide;

	private void OnCollisionEnter(Collision collision) {
		//if (collision.gameObject.layer == LayerMask.NameToLayer("Environment")) {
			if (collision.relativeVelocity.magnitude > _thresholdForce) {
				if (_onBodyCollide != null) {
					_onBodyCollide.Invoke();
				}
			}
		//}
	}
}
