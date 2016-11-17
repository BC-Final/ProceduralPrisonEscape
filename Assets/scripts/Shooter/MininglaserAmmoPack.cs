using UnityEngine;
using System.Collections;

public class MininglaserAmmoPack : AmmoPack {
	public override void Interact() {
		WeaponHolder.Instance.AddAmmo<Mininglaser>(_amount);
		base.Interact();
	}
}
