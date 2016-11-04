using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneGuardState : AbstractDroneState {
		private GameObject _player;
		private NavMeshAgent _agent;

		public DroneGuardState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_agent = _drone.GetComponent<NavMeshAgent>();
		}

		public override void Enter() {

		}

		public override void Step() {
			if (canSeeObject(_player, _drone.SeeRange)) {
				_fsm.SetState<DroneEngangeState>();
			}

			if (Vector3.Distance(_drone.transform.position, _player.transform.position) > _drone.QuitIdleRange) {
				_fsm.SetState<DroneIdleState>();
			}
		}

		public override void Exit() {

		}

		public override void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
			_fsm.SetState<DroneFollowState>();
		}
	}
}