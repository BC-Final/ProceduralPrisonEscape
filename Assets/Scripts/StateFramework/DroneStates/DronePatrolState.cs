using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace StateFramework {
	public class DronePatrolState : AbstractDroneState {
		private GameObject _player;
		private NavMeshAgent _agent;

		private float _seeTimer;

		private int _currentWaypointIndex;
		private bool _reverse;

		private float _normalStopDist;

		public DronePatrolState (DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		public override void Enter () {
			_seeTimer = 0.0f;

			_currentWaypointIndex = _drone.Route.GetNearestWaypointIndex(_drone.transform.position);
			_agent.SetDestination(_drone.Route.GetWaypoint(_currentWaypointIndex).transform.position);

			_normalStopDist = _agent.stoppingDistance;
			_agent.stoppingDistance = 0.5f;
		}

		public override void Step () {
			//TODO This might be buggy
			if (!_agent.pathPending) {
				if (_agent.remainingDistance <= _agent.stoppingDistance) {
					if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f) {
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
								_currentWaypointIndex = _drone.Route.WaypointCount() -1 ;
							} else {
								_reverse = false;
								_currentWaypointIndex += 2;
							}
						}

						_agent.SetDestination(_drone.Route.GetWaypoint(_currentWaypointIndex).transform.position);
					}
				}
			}


			if (canSeeObject(_player, _drone.LookPos, _drone.SeeRange, _drone.SeeAngle) || Vector3.Distance(_drone.LookPos.position, _player.transform.position) < _drone.AwarenessRadius) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.IdleReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = 0.0f;
			}

			if (Vector3.Distance(_drone.transform.position, _player.transform.position) > _drone.QuitIdleRange) {
				_fsm.SetState<DroneIdleState>();
			}
		}

		public override void Exit () {
			_agent.stoppingDistance = _normalStopDist;
		}

		public override void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
			_fsm.SetState<DroneFollowState>();
		}
	}
}