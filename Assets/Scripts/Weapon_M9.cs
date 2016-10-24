using UnityEngine;
using System.Collections;
using System;

public class Weapon_M9 : MonoBehaviour, IWeapon {
	[SerializeField]
	private string _ammoName;

	[SerializeField]
	private int _magazineContent;

	[SerializeField]
	private int _magazineCapacity;

	[SerializeField]
	private int _currentReserveAmmo;

	[SerializeField]
	private int _maxReserveAmmo;

	[SerializeField]
	private float _shotDamage;

	[SerializeField]
	private float _shotRange;

	[SerializeField]
	private float _meleeDamage;

	[SerializeField]
	private float _meleeRange;

	private Animator _animator;
	private AudioSource _audio;

	private void Start() {
		_animator = GetComponent<Animator>();
		_audio = GetComponent<AudioSource>();
		_animator.SetInteger("Ammo", _magazineContent);
	}

	public void Shoot() {
		if (_magazineContent == 0) {
			_audio.PlayOneShot(Resources.Load<AudioClip>("Sounds/sfx_weapon_pistol_dry"));
		}

		_animator.SetTrigger("Shoot");
		_magazineContent = Math.Max(_magazineContent - 1, 0);
		_animator.SetInteger("Ammo", _magazineContent);
	}

	public bool Reload() {
		if (_currentReserveAmmo > 0) {
			_audio.PlayOneShot(Resources.Load<AudioClip>("Sounds/sfx_weapon_pistol_reload"));
			_animator.SetTrigger("Reload");
			_magazineContent = (_currentReserveAmmo >= _magazineCapacity) ? _magazineCapacity : _currentReserveAmmo;
			_currentReserveAmmo -= _magazineContent;
			_animator.SetInteger("Ammo", _magazineContent);
			return true;
		}

		return false;
	}

	public void Melee() {
		_animator.SetTrigger("Melee");
	}

	public void SpawnShell() {
		//TODO Spawn a shell
	}

	public void SpawnMag() {
		//TODO Spawn a magazine
	}

	public void SpawnBullet() {
		_audio.PlayOneShot(Resources.Load<AudioClip>("Sounds/sfx_weapon_pistol_shoot"));
		GetComponentInChildren<ParticleSystem>().Play();

		Transform cam = Camera.main.transform;
		RaycastHit hit;

		if (Physics.Raycast(cam.position, cam.forward, out hit, _shotRange)) {
			GameObject decal = Instantiate(Resources.Load("Prefabs/pfb_bullethole"), hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal)) as GameObject;
			decal.transform.parent = hit.collider.transform;
			Destroy(decal, 10);

			if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null) {
				hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage(cam.forward, hit.point, _shotDamage);
			}
		}
	}

	public void MakeMeleeDamage() {
		Transform cam = Camera.main.transform;
		RaycastHit hit;

		if (Physics.Raycast(cam.position, cam.forward, out hit, _meleeRange)) {
			//GameObject decal = Instantiate(Resources.Load("pfb_bullethole"), hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal)) as GameObject;
			//decal.transform.parent = hit.collider.transform;
			//Destroy(decal, 10);

			if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null) {
				hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage(cam.forward, hit.point, _meleeDamage);
			}
		}
	}
}
