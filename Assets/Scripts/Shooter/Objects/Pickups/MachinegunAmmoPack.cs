using UnityEngine;
using System.Collections;

public class MachinegunAmmoPack : AmmoPack {
	public override void Interact() {
		WeaponHolder.Instance.AddAmmo<Machinegun>(_amount);
		base.Interact();
	}
}
