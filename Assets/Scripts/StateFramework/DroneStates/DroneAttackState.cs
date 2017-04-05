using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneAttackState : AbstractDroneState {
		private float _nextAttackTime;

		public DroneAttackState (DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter () {
			_drone.Agent.Stop();
			_drone.Agent.ResetPath();
			_nextAttackTime = 0.0f;
			_drone.SeesTarget = true;
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.AttackRange, _drone.Parameters.AttackAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				rotateTowards(_drone.Model.gameObject, target.transform);

				if (_nextAttackTime - Time.time <= 0.0f) {
					_nextAttackTime = Time.time + _drone.Parameters.AttackRate;

					if (_drone.Parameters.AttackType == DroneParameters.DroneAttackType.Melee) {
						//target.GetComponent<IDamageable>().ReceiveDamage(_drone, (_drone.transform.position - target.transform.position).normalized, target.transform.position, _drone.Parameters.AttackDamage);
						target.GetComponent<IDamageable>().ReceiveDamage(_drone.transform, target.transform.position, _drone.Parameters.AttackDamage, _drone.Parameters.AttackForce);
					} else if (_drone.Parameters.AttackType == DroneParameters.DroneAttackType.Ranged) {
						RaycastHit hit;

						if (Utilities.Weapons.CastShot(_drone.Parameters.SpreadConeRadius, _drone.Parameters.SpreadConeLength, _drone.ShotTransform, out hit)) {
							Utilities.Weapons.DisplayLaser(_drone.ShotTransform.position, hit.point);

							IDamageable dam = hit.collider.GetComponentInParent<IDamageable>();

							if (dam != null) {
								//dam.ReceiveDamage(_drone, (_drone.ShotTransform.position - dam.GameObject.transform.position).normalized, hit.point, _drone.Parameters.AttackDamage);
								dam.ReceiveDamage(_drone.transform, hit.point, _drone.Parameters.AttackDamage, _drone.Parameters.AttackForce);
							} else {
								Utilities.Weapons.DisplayDecal(hit.point, hit.normal, hit.collider.transform);
							}
						} else {
							Utilities.Weapons.DisplayLaser(_drone.ShotTransform.position, _drone.ShotTransform.forward, 200.0f);
						}
					}
				}
			} else {
				_fsm.SetState<DroneEngangeState>();
			}
		}

		public override void Exit () {
			_drone.Agent.Resume();
		}

		public override void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && src.GameObject != _drone.LastTarget && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction) && Utilities.AI.IsNewTargetCloser(_drone.gameObject, _drone.LastTarget, src.GameObject)) {
				_drone.LastTarget = src.GameObject;
				_fsm.SetState<DroneFollowState>();
			}
		}
	}
}