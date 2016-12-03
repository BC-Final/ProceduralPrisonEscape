﻿using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneIdleState : AbstractDroneState {
		private GameObject _player;

		public DroneIdleState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter() {

		}

		public override void Step() {
			if (Vector3.Distance(_drone.transform.position, _player.transform.position) < _drone.QuitIdleRange) {
				//TODO could also swtich to patrol
				_fsm.SetState<DroneGuardState>();
			}
		}

		public override void Exit() {
			
		}
	}
}