using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace StateFramework {
	public class DroneReturnState : AbstractDroneState {
		private float _startStoppingDistance;
		private float _seeTimer;

		public DroneReturnState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_startStoppingDistance = _drone.Agent.stoppingDistance;

			_drone.Agent.stoppingDistance = 0.0f;
			_drone.Agent.SetDestination(_drone.StartPoint.transform.position);
			_seeTimer = 0.0f;

			_drone.SeesTarget = false;
			_drone.LastTarget = null;
		}

		public override void Step() {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.Parameters.IdleReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = 0.0f;
			}

			if (!_drone.Agent.pathPending) {
				if (_drone.Agent.remainingDistance <= _drone.Agent.stoppingDistance) {
					if (!_drone.Agent.hasPath || _drone.Agent.velocity.sqrMagnitude == 0f) {
						_drone.transform.DORotate(_drone.StartPoint.transform.rotation.eulerAngles, _drone.Parameters.RotationSpeed).SetSpeedBased();

						if (_drone.Route == null) {
							_fsm.SetState<DroneGuardState>();
						} else {
							_fsm.SetState<DronePatrolState>();
						}
					}
				}
			}
		}

		public override void Exit() {
			_drone.Agent.stoppingDistance = _startStoppingDistance;
		}

		public override void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_fsm.SetState<DroneFollowState>();
			} else if (src == null) {
				_fsm.SetState<DroneSearchState>();
			}
		}
	}
}