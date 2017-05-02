using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class DroneAlarmSearchState : AbstractDroneState {
		private float _seeTimer;

		public DroneAlarmSearchState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_drone.SeesTarget = false;
			_seeTimer = 0.0f;

			if (!ShooterAlarmManager.Instance.AlarmIsOn) {
				_fsm.SetState<DroneAlarmReturnState>();
			}
		}

		public override void Step() {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.Parameters.AlertReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}

			} else {
				_seeTimer = 0.0f;
			}

			if (!_drone.Agent.hasPath || _drone.Agent.remainingDistance <= _drone.Agent.stoppingDistance) {
				Vector3 randomDir = Random.insideUnitSphere * _drone.Parameters.SearchProbeRadius;
				randomDir += _drone.transform.position;
				UnityEngine.AI.NavMeshHit hit;
				UnityEngine.AI.NavMesh.SamplePosition(randomDir, out hit, _drone.Parameters.SearchProbeRadius, 1);
				_drone.Agent.SetDestination(hit.position);
			}
		}

		public override void Exit() {
			
		}

		public override void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_fsm.SetState<DroneAlarmFollowState>();
			}
		}
	}
}