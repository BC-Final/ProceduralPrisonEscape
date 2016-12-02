using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneFollowState : AbstractDroneState {
		private GameObject _player;
		private NavMeshAgent _agent;

		private float _seeTime;

		public DroneFollowState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_agent = _drone.GetComponent<NavMeshAgent>();
		}

		public override void Enter() {
			_agent.SetDestination(_player.transform.position);
			_seeTime = 0.0f;
		}

		public override void Step() {
			if (canSeeObject(_player, _drone.SeeRange, _drone.SeeAngle) || canSeeObject(_player, _drone.AwarenessRadius, 360.0f)) {
				_seeTime += Time.deltaTime;

				if (_seeTime > _drone.AlarmedReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			}



			if (_agent.remainingDistance <= _agent.stoppingDistance) {
				_fsm.SetState<DroneSearchState>();
			}
		}

		public override void Exit() {

		}
	}
}