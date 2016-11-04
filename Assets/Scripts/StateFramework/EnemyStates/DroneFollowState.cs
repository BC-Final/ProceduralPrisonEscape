using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneFollowState : AbstractDroneState {
		private GameObject _player;
		private NavMeshAgent _agent;

		public DroneFollowState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_agent = _drone.GetComponent<NavMeshAgent>();
		}

		public override void Enter() {
			_agent.SetDestination(_player.transform.position);
		}

		public override void Step() {
			if (canSeeObject(_player, _drone.SeeRange)) {
				_fsm.SetState<DroneEngangeState>();
			}

			if (_agent.remainingDistance <= _agent.stoppingDistance) {
				_fsm.SetState<DroneSearchState>();
			}
		}

		public override void Exit() {

		}
	}
}