using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using UnityEngine.UI;

public class AmmoDisplayShooterUI : MonoBehaviour {
	[SerializeField]
	private Text _magazine;

	[SerializeField]
	private Text _reserve;

	[SerializeField]
	private Text _grenades;

	private WeaponHolder _weaponHolder;
	private GrenadeThrow _grenadeThrow;

	private void Start() {
		_weaponHolder = FindObjectOfType<WeaponHolder>();
		_grenadeThrow = FindObjectOfType<GrenadeThrow>();
	}

	private void LateUpdate() {
		_magazine.text = _weaponHolder.CurrentWeapon.MagazineContent.ToString();
		_reserve.text = _weaponHolder.CurrentWeapon.ReserveAmmo.ToString();
		_grenades.text = _grenadeThrow.NoOfGrenades.ToString();
	}
}
