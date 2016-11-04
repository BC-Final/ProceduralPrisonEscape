using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneSearchState : AbstractDroneState {

		private int _searchCounter;
		private NavMeshAgent _agent;

		public DroneSearchState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_agent = _drone.GetComponent<NavMeshAgent>();
		}

		public override void Enter() {
			_searchCounter = _drone.SearchCount;
		}

		//TODO Make the direction to choose more intelligent
		public override void Step() {
			if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance) {
				if (_searchCounter > 0) {
					Debug.Log("Search!");
					_searchCounter--;
					Vector3 randomDir = Random.insideUnitSphere * _drone.SearchRadius;
					randomDir += _drone.transform.position;
					NavMeshHit hit;
					NavMesh.SamplePosition(randomDir, out hit, _drone.SearchRadius, 1);
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