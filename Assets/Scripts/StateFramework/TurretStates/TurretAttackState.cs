using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class TurretAttackState : AbstractTurretState {
		private GameObject _player;

		private float _nextAttackTime;


		public TurretAttackState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			_nextAttackTime = 0.0f;

			_turret.SeesTarget = true;
		}

		public override void Step () {
			Transform target = _turret.Controlled ? getClosestSeeableObject(_turret.Targets.ToArray(), _turret.ShootPos, _turret.AttackRange, _turret.AttackAngle).transform : _player.transform;

			if (!canSeeObject(_player, _turret.ShootPos, _turret.AttackRange, _turret.AttackAngle) && !_turret.Controlled || _turret.Controlled && target != null) {
				_fsm.SetState<TurretEngageState>();
			}

			rotateTowards(target);

			if (_nextAttackTime - Time.time <= 0.0f) {
				_nextAttackTime = Time.time + _turret.FireRate;

				RaycastHit hit;

				float randomRadius = UnityEngine.Random.Range(0, _turret.SpreadConeRadius);
				float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

				Vector3 rayDir = new Vector3(
					randomRadius * Mathf.Cos(randomAngle),
					randomRadius * Mathf.Sin(randomAngle),
					_turret.SpreadConeLength
					);

				rayDir = _turret.Gun.transform.TransformDirection(rayDir.normalized);

				if (Physics.Raycast(_turret.ShootPos.transform.position, rayDir, out hit, 100.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
					//Shooting laser
					//GameObject laser = GameObject.Instantiate(ShooterReferenceManager.Instance.LaserShot, hit.point, Quaternion.identity) as GameObject;
					//laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_turret.ShootPos.transform.position));
					//GameObject.Destroy(laser, 0.05f);

					//Shoot bullet
					GameObject tracer = GameObject.Instantiate(ShooterReferenceManager.Instance.BulletTracer, _turret.ShootPos.position, Quaternion.LookRotation(hit.point - _turret.ShootPos.position)) as GameObject;
					tracer.GetComponentInChildren<ParticleSystem>().Play();
					GameObject.Destroy(tracer, 1.0f);
					_turret.GetComponentInChildren<ParticleSystem>().Play();


					//Plaxing decal
					GameObject decal = GameObject.Instantiate(ShooterReferenceManager.Instance.BulletHole, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal)) as GameObject;
					decal.transform.parent = hit.collider.transform;
					GameObject.Destroy(decal, 10);

					//Inflict damage
					if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null) {
						hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage((_turret.ShootPos.position - target.transform.position).normalized, hit.point, _turret.Damage);
					}

					//ShooterPackageSender.SendPackage(new CustomCommands.Creation.Shots.LaserShotCreation(_turret.transform.position, hit.point));
					ShooterPackageSender.SendPackage(new NetworkPacket.Create.LaserShot(_turret.transform.position, hit.point));
				} else {
					//Shooting laser
					//GameObject laser = GameObject.Instantiate(ShooterReferenceManager.Instance.LaserShot, _turret.ShootPos.position + _turret.RotaryBase.forward * _turret.AttackRange, Quaternion.identity) as GameObject;
					//laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_turret.ShootPos.position));
					//GameObject.Destroy(laser, 0.05f);

					//Shoot bullet
					GameObject tracer = GameObject.Instantiate(ShooterReferenceManager.Instance.BulletTracer, _turret.ShootPos.position, Quaternion.LookRotation(_turret.ShootPos.position + _turret.ShootPos.forward * _turret.AttackRange)) as GameObject;
					tracer.GetComponentInChildren<ParticleSystem>().Play();
					GameObject.Destroy(tracer, 1.0f);
					_turret.GetComponentInChildren<ParticleSystem>().Play();

					ShooterPackageSender.SendPackage(new NetworkPacket.Create.LaserShot(_turret.transform.position, _turret.ShootPos.transform.position + _turret.RotaryBase.forward * _turret.AttackRange));
					//ShooterPackageSender.SendPackage(new CustomCommands.Creation.Shots.LaserShotCreation(_turret.transform.position, _turret.ShootPos.transform.position + _turret.RotaryBase.forward * _turret.AttackRange));
				}
			}
		}

		public override void Exit () {

		}
	}
}