using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class DroneAmbushState : AbstractDroneState {
		private float _seeTimer;

		public DroneAmbushState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
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
		}

		public override void Exit() { }

		public override void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_drone.Agent.SetDestination(src.GameObject.transform.position);
			}
		}
	}
}