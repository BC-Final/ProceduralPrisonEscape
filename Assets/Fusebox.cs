using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusebox : MonoBehaviour, IShooterNetworked {
	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public void Initialize() {
		ShooterPackageSender.SendPackage(new NetworkPacket.Create.Fusebox(Id, transform.position.x, transform.position.z, _used, _energy > 0));
	}

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public static void ProcessPacket(NetworkPacket.Update.Fusebox pPacket) {
		Fusebox fuse = ShooterPackageSender.GetNetworkedObject<Fusebox>(pPacket.Id);

		if (fuse != null) {
			if (!fuse._used) {
				if (pPacket.Charged) {
					fuse._energy = fuse._chargedEnergy;
				} else {
					fuse._energy = 1;
				}
			}
		} else {
			//Debug.LogError("Could not find Door with Id " + pPacket.Id);
		}
	}

	private void sendUpdate() {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Fusebox(Id, _used));
	}


	[SerializeField]
	private float _radius;

	[SerializeField]
	private float _damage;

	[SerializeField]
	private float _force;

	[SerializeField]
	private int _chargedEnergy = 3;

	[SerializeField]
	private SphereCollider _rangeTrigger;

	private int _energy;
	private bool _used;

	private void Start() {
		_rangeTrigger.radius = _radius;
	}

	private void OnTriggerStay(Collider other) {
		if (_energy > 0) {
			IDamageable d = other.GetComponentInParent<IDamageable>();

			if (d != null && d is DroneEnemy && other.gameObject.layer == LayerMask.NameToLayer("RayTrigger")) {
				_energy--;

				if (_energy == 0) {
					_used = true;
				}

				sendUpdate();

				if (Utilities.AI.ObjectVisible(transform, d.GameObject.transform)) {
					GameObject.Instantiate(ShooterReferenceManager.Instance.Lightning, transform).GetComponent<LightningEffect>().Display(d.GameObject.transform);
					d.ReceiveDamage(transform, d.GameObject.transform.position, _damage, _force);
				}
			} 
		}
	}
}
