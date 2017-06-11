﻿using UnityEngine;
using System.Collections;

public class PhaserAmmoPack : AmmoPack {

    public override void Initialize()
    {
        ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.PickUpIcon.PhaserAmmoCreation(Id, this.transform.position));
        base.Initialize();
    }

    public override void Interact() {
		WeaponHolder.Instance.AddAmmo<Phaser>(_amount);
		base.Interact();
	}
}
