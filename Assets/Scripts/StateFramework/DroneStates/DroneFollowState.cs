﻿using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneFollowState : AbstractDroneState {
		private float _seeTimer;

		public DroneFollowState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			//TODO This sometimes causes a nullreference exception
			_drone.Agent.SetDestination(_drone.LastTarget.transform.position);
			_drone.SeesTarget = true;
			_seeTimer = 0.0f;
		}

		public override void Step() {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.Parameters.AlertReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = 0.0f;
			}

			//TODO Make the following a bit smarter
			if (_drone.Agent.remainingDistance <= _drone.Agent.stoppingDistance) {
				_fsm.SetState<DroneSearchState>();
			}
		}

		public override void Exit() { }

		public override void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_drone.Agent.SetDestination(src.GameObject.transform.position);
			}
		}
	}
}