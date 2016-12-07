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
		}

		//TODO Make the direction to choose more intelligent
		public override void Step() {
			if (canSeeObject(_player, _drone.SeeRange, _drone.SeeAngle) || canSeeObject(_player, _drone.AwarenessRadius, 360.0f)) {
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
					_fsm.SetState<DroneReturnState>();
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