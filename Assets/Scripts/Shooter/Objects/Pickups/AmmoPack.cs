using UnityEngine;
using System.Collections;

public abstract class AmmoPack : MonoBehaviour, IInteractable, IShooterNetworked {
	[SerializeField]
	protected int _amount;

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public virtual void Initialize () {
		//ShooterPackageSender.SendPackage(new CustomCommands.Creation.Items.AmmoPackCreation(Id, transform.position.x, transform.position.z, false));
	}

	public virtual void Interact() {
        ShooterPackageSender.SendPackage(new NetworkPacket.Update.Icon(Id, true));
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
