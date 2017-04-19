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
	private float _explosionForce;

	[SerializeField]
	private bool _visualize;

	private ObservedValue<bool> _broken = new ObservedValue<bool>(false);

	public static void ProcessPacket (NetworkPacket.Update.Pipe pPacket) {
		ShooterPipe pipe = ShooterPackageSender.GetNetworkedObject<ShooterPipe>(pPacket.Id);

		if (pipe != null) {
			if (pPacket.ChargedExplosion) {
				TimerManager.CreateTimer("Pipe explosion", true).SetDuration(pipe._chargedExplodeDelay).AddCallback(() => pipe.explode(pPacket.ChargedExplosion)).Start();
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
		_broken.OnValueChange += sendUpdate;
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
		_broken.Value = true;

		//FIX If this causes lag, replace with a collider or try using layers
		Collider[] coll = Physics.OverlapSphere(transform.position, pCharged ? _chargedExplosionRadius : _normalExplosionRadius, LayerMask.GetMask("RayTrigger"), QueryTriggerInteraction.Collide);


		foreach (Collider c in coll) {
			IDamageable d = c.gameObject.GetComponentInParent<IDamageable>();

			if (d != null) {
				RaycastHit hit;
				if(Utilities.AI.ObjectVisible(transform, d.GameObject.transform)) {
					//if (Physics.Linecast(transform.position, c.transform.position, out hit, LayerMask.GetMask("RayTrigger"), QueryTriggerInteraction.Collide)) {
					//d.ReceiveDamage(null, c.transform.position - transform.position, hit.point, pCharged ? _chargedExplosionDamage : _normalExplosionDamage);
					d.ReceiveDamage(transform, c.transform.position, pCharged ? _chargedExplosionDamage : _normalExplosionDamage, _explosionForce);
				}
			}
		}
	}


	public void Initialize() {
		sendUpdate();
	}

	private void sendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Pipe(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _broken.Value));
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
