using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

[SelectionBase]
public class ShooterPipe : MonoBehaviour, IShooterNetworked {
	[SerializeField]
	private GameObject _normalModel;

	[SerializeField]
	private GameObject _brokenModel;

	[SerializeField]
	private float _normalExplosionRadius;

	[SerializeField]
	private float _chargedExplosionRadius;

	[SerializeField]
	private float _normalExplosionDamage;

	[SerializeField]
	private float _chargedExplosionDamage;

	[SerializeField]
	private float _chargedExplodeDelay;

	[SerializeField]
	private bool _visualize;

	private bool _broken = false;

	public static void ProcessPacket (NetworkPacket.Update.Pipe pPacket) {
		ShooterPipe pipe = ShooterPackageSender.GetNetworkedObject<ShooterPipe>(pPacket.Id);

		if (pipe != null) {
			if (pPacket.ChargedExplosion) {
				Timers.CreateTimer("Pipe explosion").SetTime(pipe._chargedExplodeDelay).SetCallback(() => pipe.explode(pPacket.ChargedExplosion)).Start();
			} else {
				pipe.explode(pPacket.ChargedExplosion);
			}
		} else {
			Debug.LogError("Trying to access non existent networked object with id " + pPacket.Id);
		}
	}

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	/// <summary>
	/// Registers Network Door reference and add listener to state change
	/// </summary>
	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}



	/// <summary>
	/// Removes Network Door Reference
	/// </summary>
	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	private void explode (bool pCharged) {
		_normalModel.SetActive(false);
		_brokenModel.SetActive(true);
		_broken = true;

		//FIX If this causes lag, replace with a collider or try using layers
		Collider[] coll = Physics.OverlapSphere(transform.position, pCharged ? _chargedExplosionRadius : _normalExplosionRadius);

		foreach (Collider c in coll) {
			IDamageable d = c.gameObject.GetComponent<IDamageable>();
			if (d != null) {
				RaycastHit hit;
				if (Physics.Raycast(transform.position, c.transform.position - transform.position, out hit, pCharged ? _chargedExplosionRadius : _normalExplosionRadius)) {
					//TODO Create explosion effect
					d.ReceiveDamage(transform.position - c.transform.position, hit.point, pCharged ? _chargedExplosionDamage : _normalExplosionDamage);
				}
			}
		}
	}


	public void Initialize() {
		sendUpdate();
	}

	private void sendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Pipe(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _broken));
	}

	private void OnDrawGizmosSelected () {
		if (_visualize) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, _normalExplosionRadius);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _chargedExplosionRadius);
			Gizmos.color = Color.white;
		}
	}
}
