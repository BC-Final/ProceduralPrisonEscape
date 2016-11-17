using UnityEngine;
using System.Collections;

public class PhaserAmmoPack : AmmoPack {
	public override void Interact() {
		WeaponHolder.Instance.AddAmmo<Phaser>(_amount);
		base.Interact();
	}
}
