using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {
	[SerializeField]
	private float _shootCooldown;
	private float _cooldownEnd;

	[SerializeField]
	private Transform _bulletSpawn;

	[SerializeField]
	private GameObject _bulletPrefab;

	private Animator _anim;

	private void Start () {
		_anim = GetComponent<Animator>();
	}

	private void Update () {
		if (Input.GetButtonDown("Fire1") && _cooldownEnd - Time.time <= 0) {
			_anim.SetTrigger("Fire");
			_cooldownEnd = Time.time + _shootCooldown;
		}
	}

	public void SpawnBullet () {
		GameObject.Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
	}

	public void SpawnShell () {

	}
}
