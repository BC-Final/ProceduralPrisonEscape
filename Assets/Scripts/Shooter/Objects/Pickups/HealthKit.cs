using UnityEngine;
using System.Collections;

public class HealthKit : MonoBehaviour, IInteractable, INetworked {
	[SerializeField]
	private int _amount;

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public void Initialize () {
		ShooterPackageSender.SendPackage(new CustomCommands.Creation.Items.HealthKitCreation(Id, transform.position.x, transform.position.z, false));
	}

	public void Interact() {
		FindObjectOfType<PlayerHealth>().HealDamage(_amount);

		FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_shooter/PE_shooter_pickhealth");
		FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
		ins.start();

		Destroy(gameObject);
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(_id, true, this.transform.position));
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}
}
