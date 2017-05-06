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

	public void SetAmmo (int pMagazine, int pReserve) {
		_magazine.text = pMagazine.ToString();
		_reserve.text = pReserve.ToString();
	}

	public void SetGrenades(int pGrenades) {
		_grenades.text = pGrenades.ToString();
	}
}
