using UnityEngine;
using System.Collections;

public class MininglaserAmmoPack : AmmoPack {
	public override void Interact() {
		WeaponHolder.Instance.AddAmmo<Mininglaser>(_amount);
		base.Interact();
	}

    public override void Initialize()
    {
        ShooterPackageSender.SendPackage(new NetworkPacket.Create.ShotgunAmmoIcon(Id, this.transform.position));
        base.Initialize();
    }
}
