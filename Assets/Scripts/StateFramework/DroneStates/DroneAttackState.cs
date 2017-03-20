using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneAttackState : AbstractDroneState {
		private GameObject _player;
		private GameObject _droneModel;
		private UnityEngine.AI.NavMeshAgent _agent;

		private float _nextAttackTime;

		public DroneAttackState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_droneModel = _drone.transform.GetChild(0).gameObject;
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		public override void Enter() {
			_agent.Stop();
			_agent.ResetPath();
			_nextAttackTime = 0.0f;

			_drone.SeesPlayer = true;
		}

		public override void Step() {
			rotateTowards(_droneModel, _player.transform);

			if (_nextAttackTime - Time.time <= 0.0f) {
				_nextAttackTime = Time.time + _drone.AttackRate;

				if (_drone.AttackType == DroneEnemy.eAttackType.Ranged) {
					RaycastHit hit;

					float randomRadius = UnityEngine.Random.Range(0, _drone.SpreadConeRadius);
					float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

					Vector3 rayDir = new Vector3(
						randomRadius * Mathf.Cos(randomAngle),
						randomRadius * Mathf.Sin(randomAngle),
						_drone.SpreadConeLength
						);

					rayDir = _drone.transform.GetChild(0).TransformDirection(rayDir.normalized);

					if (Physics.Raycast(_drone.transform.GetChild(0).GetChild(1).position, rayDir, out hit, 200.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
						GameObject laser = GameObject.Instantiate(ShooterReferenceManager.Instance.LaserShot, hit.point, Quaternion.identity) as GameObject;
						laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_drone.transform.GetChild(0).GetChild(1).position));
						GameObject.Destroy(laser, 0.05f);

						GameObject decal = GameObject.Instantiate(ShooterReferenceManager.Instance.BulletHole, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal)) as GameObject;
						decal.transform.parent = hit.collider.transform;
						GameObject.Destroy(decal, 10);

						if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null && hit.rigidbody.GetComponent<IDamageable>() is PlayerHealth) {
							hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage((_drone.transform.position - _player.transform.position).normalized, hit.point, _drone.AttackDamage);
						}

						//ShooterPackageSender.SendPackage(new CustomCommands.Creation.Shots.LaserShotCreation(_drone.transform.position, hit.point));
						ShooterPackageSender.SendPackage(new NetworkPacket.Create.LaserShot(_drone.transform.position, hit.point));
					} else {
						GameObject laser = GameObject.Instantiate(ShooterReferenceManager.Instance.LaserShot, _drone.transform.GetChild(0).GetChild(1).position + _drone.transform.GetChild(0).forward * _drone.AttackRange, Quaternion.identity) as GameObject;
						laser.GetComponent<LineRenderer>().SetPosition(0, laser.transform.InverseTransformPoint(_drone.transform.GetChild(0).GetChild(1).position));
						GameObject.Destroy(laser, 0.05f);
						//ShooterPackageSender.SendPackage(new CustomCommands.Creation.Shots.LaserShotCreation(_drone.transform.position, _drone.transform.GetChild(0).GetChild(1).position + _drone.transform.GetChild(0).forward * _drone.AttackRange));
						ShooterPackageSender.SendPackage(new NetworkPacket.Create.LaserShot(_drone.transform.position, _drone.transform.GetChild(0).GetChild(1).position + _drone.transform.GetChild(0).forward * _drone.AttackRange));
					}
				} else if (_drone.AttackType == DroneEnemy.eAttackType.Melee) {
					_player.GetComponent<IDamageable>().ReceiveDamage((_drone.transform.position - _player.transform.position).normalized, _player.transform.position, _drone.AttackDamage);
					//TODO play attack animation
				}
			}


			if(!canSeeObject(_player, _drone.LookPos, _drone.AttackRange, _drone.SeeAngle)) { 
				_fsm.SetState<DroneEngangeState>();
			}
		}

		public override void Exit() {
			_agent.Resume();
		}
	}
}