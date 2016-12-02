using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneGuardState : AbstractDroneState {
		private GameObject _player;

		private float _seeTimer;

		public DroneGuardState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter() {
			_seeTimer = 0.0f;
		}

		public override void Step() {
			if (canSeeObject(_player, _drone.SeeRange, _drone.SeeAngle) || canSeeObject(_player, _drone.AwarenessRadius, 360.0f)) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.IdleReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
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