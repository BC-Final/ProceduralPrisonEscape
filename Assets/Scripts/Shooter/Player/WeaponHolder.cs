using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Gamelogic.Extensions;

public class WeaponHolder : Singleton<WeaponHolder> {
	[SerializeField]
	private Transform _aimPosition;

	[SerializeField]
	private float _weaponSwitchSpeed;

	private List<Weapon> _weapons;
	private int _currentWeapon;
	private Text _ammobar;

	private Tweener _zoomTween;

	private void Start() {
		_weapons = new List<Weapon>();

		_weapons.Add((Instantiate(ShooterReferenceManager.Instance.Phaser, transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());
		_weapons.Add((Instantiate(ShooterReferenceManager.Instance.Machinegun, transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());
		_weapons.Add((Instantiate(ShooterReferenceManager.Instance.Mininglaser, transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());

		_currentWeapon = 0;

		foreach (Weapon w in _weapons) {
			w.SetActive(false);
		}

		_weapons[0].SetActive(true);

		//TODO I hate using strings
		_ammobar = GameObject.FindGameObjectWithTag("ammobar").GetComponent<Text>();
	}

	private void Update() {
		//TODO This can be done more clever!
		//int nextWeapon = Mathf.Min(Mathf.Max(0, _currentWeapon + (int)Input.mouseScrollDelta.y), _weapons.Count - 1);
		int nextWeapon = _currentWeapon;

		for (int i = 0; i < (int)Mathf.Abs(Input.mouseScrollDelta.y); ++i) {
			nextWeapon += (int)Mathf.Sign(Input.mouseScrollDelta.y);

			if (nextWeapon == _weapons.Count) {
				nextWeapon = 0;
			} else if (nextWeapon == -1) {
				nextWeapon = _weapons.Count - 1;
			}
		}

		if (nextWeapon != _currentWeapon) {
			_weapons[_currentWeapon].SetActive(false);
			_weapons[nextWeapon].SetActive(true);
			_currentWeapon = nextWeapon;
		}
		//END commentpart

		if (Input.GetMouseButtonDown(1)) {
			FindObjectOfType<CrosshairDistance>().Disable();
			_weapons[_currentWeapon].transform.DOLocalMove(transform.InverseTransformPoint(_aimPosition.position) - _weapons[_currentWeapon].AimPosition.localPosition, _weapons[_currentWeapon].AimTime);
			//TODO Get FOV from options
			//TODO Get substracted FOW from weapon
			Camera.main.DOFieldOfView(30, _weapons[_currentWeapon].AimTime);
			GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<Camera>().DOFieldOfView(30, _weapons[_currentWeapon].AimTime);
		} else if (Input.GetMouseButtonUp(1)) {
			FindObjectOfType<CrosshairDistance>().Enable();
			_weapons[_currentWeapon].transform.DOLocalMove(Vector3.zero, _weapons[_currentWeapon].AimTime);
			//TODO Get FOV from options
			Camera.main.DOFieldOfView(60, _weapons[_currentWeapon].AimTime);
			GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<Camera>().DOFieldOfView(60, _weapons[_currentWeapon].AimTime);
		}
	}

	public void AddAmmo<T>(int pAmount) {
		_weapons.Find(x => x is T).AddAmmo(pAmount);
	}

	private void OnGUI() {
		_ammobar.text = _weapons[_currentWeapon].MagazineContent + "/" + _weapons[_currentWeapon].MagazineCapacity + "  R:" + _weapons[_currentWeapon].ReserveAmmo + "/" + _weapons[_currentWeapon].MaxReserveAmmo;
	}
}
