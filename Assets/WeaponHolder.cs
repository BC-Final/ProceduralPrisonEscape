using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponHolder : MonoBehaviour {
	private List<Weapon> _weapons;

	private int _currentWeapon;

	private void Start() {
		_weapons = new List<Weapon>();

		_weapons.Add((Instantiate(Resources.Load("prefabs/shooter/weapons/pfb_weapon_phaser"), transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());
		_weapons.Add((Instantiate(Resources.Load("prefabs/shooter/weapons/pfb_weapon_machinegun"), transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());
		_weapons.Add((Instantiate(Resources.Load("prefabs/shooter/weapons/pfb_weapon_mininglaser"), transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());

		_currentWeapon = 0;

		foreach (Weapon w in _weapons) {
			w.gameObject.SetActive(false);
		}

		_weapons[0].gameObject.SetActive(true);
	}

	private void Update() {
		//TODO This can be done more clever!
		int nextWeapon = Mathf.Min(Mathf.Max(0, _currentWeapon + (int)Input.mouseScrollDelta.y), _weapons.Count - 1);

		if (nextWeapon != _currentWeapon) {
			_weapons[_currentWeapon].gameObject.SetActive(false);
			_weapons[nextWeapon].gameObject.SetActive(true);
			_currentWeapon = nextWeapon;
		}
	}
}
