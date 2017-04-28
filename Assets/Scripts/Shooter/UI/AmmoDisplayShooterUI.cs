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

	public void SetValues (int pMagazine, int pReserve) {
		_magazine.text = pMagazine.ToString();
		_reserve.text = pReserve.ToString();
	}
}
