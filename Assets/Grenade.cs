﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grenade : MonoBehaviour, IShooterNetworked {
	[SerializeField]
	private float _maxRange;

	[SerializeField]
	private float _maxDamage;

	[SerializeField]
	private float _maxForce;

	[SerializeField]
	private AnimationCurve _damageOverDistance;

	[SerializeField]
	private AnimationCurve _explosionForceOverDistance;

	[SerializeField]
	private float _throwForce;

	[SerializeField]
	private float _throwRotation;

	[SerializeField]
	private float _updateInterval = 0.1f;

	private Timer _updateTimer;

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public void Initialize() {
		_updateTimer = TimerManager.CreateTimer("Player Update", false)
			.SetDuration(_updateInterval)
			.SetLoops(-1)
			.AddCallback(() => ShooterPackageSender.SendPackage(new NetworkPacket.Update.Grenade(Id, transform.position.x, transform.position.z)))
			.Start();
	}

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}


	private void OnDestroy() {
		if (_updateTimer != null) {
			_updateTimer.Stop();
		}

		_updateTimer = null;

		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public static void ProcessPacket(NetworkPacket.Update.Grenade pPacket) {
		Grenade grenade = ShooterPackageSender.GetNetworkedObject<Grenade>(pPacket.Id);

		if (grenade != null) {
			grenade.explode();
		} else {
			Debug.LogError("Trying to access non existent networked object with id " + pPacket.Id);
		}
	}

	private void Start() {
		Light l = GetComponentInChildren<Light>();
		DOTween.Sequence()
		.Append(l.DOIntensity(l.intensity + 2.0f, 0.1f))
		.AppendInterval(0.2f)
		.Append(l.DOIntensity(l.intensity, 0.1f))
		.AppendInterval(0.4f)
		.SetLoops(-1);

		GetComponent<Rigidbody>().AddForce(transform.forward * _throwForce);
		GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * _throwRotation);
	}

	private void explode() {
		Collider[] colls = Physics.OverlapSphere(transform.position, _maxRange, LayerMask.GetMask("RayTrigger", "PhysicsAffected", "NoPlayerCollidePhysicsAffected"));

		List<Rigidbody> rigidbodies = new List<Rigidbody>();

		foreach(Collider coll in colls) {
			IDamageable dam = coll.GetComponentInParent<IDamageable>();

			if (dam != null) {
				RaycastHit hit;

				if (Physics.Linecast(transform.position, coll.transform.position, out hit, LayerMask.GetMask("RayTrigger", "Environment"))) {
					if (hit.collider.gameObject == coll.gameObject) {
						float dist = Utilities.Math.Remap(Vector3.Distance(transform.position, dam.GameObject.transform.position), 0.0f, _maxRange, 0.0f, 1.0f);
						dam.ReceiveDamage(transform, hit.point, _damageOverDistance.Evaluate(dist) * _maxDamage, _explosionForceOverDistance.Evaluate(dist) * _maxForce);
					}
				}
			} else {
				if (coll.attachedRigidbody != null && !rigidbodies.Contains(coll.attachedRigidbody)) {
					rigidbodies.Add(coll.attachedRigidbody);
				}
			}
		}

		foreach (Rigidbody rig in rigidbodies) {
			float dist = Utilities.Math.Remap(Vector3.Distance(transform.position, rig.transform.position), 0.0f, _maxRange, 0.0f, 1.0f);
			rig.AddExplosionForce(_explosionForceOverDistance.Evaluate(dist) * _maxForce, transform.position, 0.0f);
		}

		Instantiate(ShooterReferenceManager.Instance.GrenadeExplosion, transform.position, Quaternion.Euler(0.0f, Random.Range(0.0f, 359.0f), 0.0f));
		Destroy(this.gameObject);
	}

	private void OnDrawGizmos() {
		Gizmos.DrawWireSphere(transform.position, _maxRange);
	}
}
