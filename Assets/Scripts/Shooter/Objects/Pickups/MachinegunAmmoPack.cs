using UnityEngine;
using System.Collections;

public class MachinegunAmmoPack : AmmoPack {
	public override void Interact() {
		WeaponHolder.Instance.AddAmmo<Machinegun>(_amount);
		base.Interact();
	}

    public override void Initialize()
    {
        ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.PickUpIcon.MachineGunAmmoCreation(Id, this.transform.position));
        base.Initialize();
    }
}
