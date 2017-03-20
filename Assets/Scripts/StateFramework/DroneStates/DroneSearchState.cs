using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneSearchState : AbstractDroneState {

		private int _searchCounter;

		private GameObject _player;
		private UnityEngine.AI.NavMeshAgent _agent;

		private float _seeTimer;

		public DroneSearchState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		public override void Enter() {
			_searchCounter = _drone.SearchCount;
			_seeTimer = 0.0f;

			_drone.SeesPlayer = true;
		}

		//TODO Make the direction to choose more intelligent
		public override void Step() {
			if (canSeeObject(_player, _drone.LookPos, _drone.SeeRange, _drone.SeeAngle) || Vector3.Distance(_drone.LookPos.position, _player.transform.position) < _drone.AwarenessRadius) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.IdleReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = 0.0f;
			}

			if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance) {
				if (_searchCounter > 0) {
					_searchCounter--;
					Vector3 randomDir = Random.insideUnitSphere * _drone.SearchRadius;
					randomDir += _drone.transform.position;
					UnityEngine.AI.NavMeshHit hit;
					UnityEngine.AI.NavMesh.SamplePosition(randomDir, out hit, _drone.SearchRadius, 1);
					_agent.SetDestination(hit.position);
				} else {
					if (_drone.Route == null) {
						_fsm.SetState<DroneReturnState>();
					} else {
						_fsm.SetState<DronePatrolState>();
					}
				}
			}
		}

		public override void Exit() {

		}

		public override void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
			_fsm.SetState<DroneFollowState>();
		}
	}
}