﻿using UnityEngine;
using System.Collections;

public abstract class AmmoPack : MonoBehaviour, IInteractable, IShooterNetworked {
	[SerializeField]
	protected int _amount;

	[SerializeField]
	private UnityEngine.Events.UnityEvent _onPickup;

	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public virtual void Initialize () {
		//ShooterPackageSender.SendPackage(new CustomCommands.Creation.Items.AmmoPackCreation(Id, transform.position.x, transform.position.z, false));
	}

	public virtual void Interact() {
        ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.PickUpIcon.sIconUpdate(Id, true));

		if (_onPickup != null) {
			_onPickup.Invoke();
		}

		Destroy(gameObject);
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		//ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(_id, true, this.transform.position));
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}
}
