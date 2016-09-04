using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	[SerializeField]
	private float _speed;

	[SerializeField]
	private float _lifeTime;

	private void Start () {
		Destroy(this, _lifeTime);
	}

	private void Update () {
		transform.Translate(Vector3.forward * _speed);
	}
}
