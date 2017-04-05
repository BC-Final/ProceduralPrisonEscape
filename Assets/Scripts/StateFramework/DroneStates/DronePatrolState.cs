using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace StateFramework {
	public class DronePatrolState : AbstractDroneState {
		private float _seeTimer;

		private int _currentWaypointIndex;
		private bool _reverse;

		private float _normalStopDist;

		public DronePatrolState (DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter () {
			_seeTimer = 0.0f;

			_currentWaypointIndex = _drone.Route.GetNearestWaypointIndex(_drone.transform.position);
			_drone.Agent.SetDestination(_drone.Route.GetWaypoint(_currentWaypointIndex).transform.position);

			_normalStopDist = _drone.Agent.stoppingDistance;
			_drone.Agent.stoppingDistance = 0.5f;

			_drone.SeesTarget = false;
			_drone.LastTarget = null;
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.Parameters.IdleReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = Mathf.Max(_seeTimer - Time.deltaTime, 0.0f);
			}


			//TODO This waypoint finding might be very buggy
			if (!_drone.Agent.pathPending) {
				if (_drone.Agent.remainingDistance <= _drone.Agent.stoppingDistance) {
					if (!_drone.Agent.hasPath || _drone.Agent.velocity.sqrMagnitude == 0f) {
						if (_reverse) {
							_currentWaypointIndex--;
						} else {
							_currentWaypointIndex++;
						}

						if (_currentWaypointIndex == _drone.Route.WaypointCount()) {
							if (_drone.Route.Looping) {
								_currentWaypointIndex = 0;
							} else {
								_currentWaypointIndex -= 2;
								_reverse = true;
							}
						} else if (_currentWaypointIndex < 0) {
							if (_drone.Route.Looping) {
								_currentWaypointIndex = _drone.Route.WaypointCount() - 1;
							} else {
								_reverse = false;
								_currentWaypointIndex += 2;
							}
						}

						_drone.Agent.SetDestination(_drone.Route.GetWaypoint(_currentWaypointIndex).transform.position);
					}
				}
			}
		}

		public override void Exit () {
			_drone.Agent.stoppingDistance = _normalStopDist;
		}

		public override void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
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