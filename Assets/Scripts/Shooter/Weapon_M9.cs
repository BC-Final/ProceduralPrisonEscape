using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Weapon_M9 : MonoBehaviour, IDamaging {
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

	[SerializeField]
	private float _spreadConeLength;

	[SerializeField]
	private float _spreadConeRadius;

	[SerializeField]
	private bool _visualizeSpreadCone;

	private Animator _animator;
	private AudioSource _audio;

	private Text _ammobar;

	private void Start() {
		_animator = GetComponent<Animator>();
		_audio = GetComponent<AudioSource>();
		_animator.SetInteger("Ammo", _magazineContent);
		_ammobar = GameObject.FindGameObjectWithTag("ammobar").GetComponent<Text>();
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

	public void AddAmmo(int pAmount) {
		_currentReserveAmmo = Mathf.Min(_maxReserveAmmo, _currentReserveAmmo + pAmount);
	}

	public void Melee() {
		_animator.SetTrigger("Melee");
	}

	public void SpawnShell() {
	}

	public void SpawnMag() {
	}

	public void SpawnBullet() {
		_audio.PlayOneShot(Resources.Load<AudioClip>("sounds/sfx_weapon_pistol_shoot"));
		GetComponentInChildren<ParticleSystem>().Play();

		Transform cam = Camera.main.transform;
		RaycastHit hit;

		float randomRadius = UnityEngine.Random.Range(0, _spreadConeRadius);
		float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

		Vector3 rayDir = new Vector3(
			randomRadius * Mathf.Cos(randomAngle),
			randomRadius * Mathf.Sin(randomAngle),
			_spreadConeLength
			);

		rayDir = cam.transform.TransformDirection(rayDir.normalized);

		if (Physics.Raycast(cam.position, rayDir, out hit, _shotRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
			GameObject laser = Instantiate(Resources.Load("prefabs/shooter/pfb_laser"), hit.point, Quaternion.identity) as GameObject;
			laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(transform.GetComponentInChildren<ParticleSystem>().transform.position));
			GameObject.Destroy(laser, 0.05f);

			GameObject decal = Instantiate(Resources.Load("prefabs/shooter/pfb_bullethole"), hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal)) as GameObject;
			decal.transform.parent = hit.collider.transform;
			Destroy(decal, 10);

			if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null) {
				hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage(cam.forward, hit.point, _shotDamage);
			}
		} else {
			GameObject laser = Instantiate(Resources.Load("prefabs/shooter/pfb_laser"), Camera.main.transform.forward * _shotRange, Quaternion.identity) as GameObject;
			laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(transform.GetComponentInChildren<ParticleSystem>().transform.position));
			GameObject.Destroy(laser, 0.05f);
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

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		#if (UNITY_EDITOR)
		if (_visualizeSpreadCone) {
			UnityEditor.Handles.color = Color.white;
			UnityEditor.Handles.DrawWireDisc(Camera.main.transform.position + Camera.main.transform.forward * _spreadConeLength, Camera.main.transform.forward, _spreadConeRadius);
		}
		#endif
	}
#endif

	private void OnGUI() {
		_ammobar.text = _magazineContent + "/" + _magazineCapacity + "  R:" + _currentReserveAmmo;
	}
}
