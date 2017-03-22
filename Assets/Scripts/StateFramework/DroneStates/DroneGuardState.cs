using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneGuardState : AbstractDroneState {
		private float _seeTimer;

		public DroneGuardState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_seeTimer = 0.0f;
			_drone.SeesTarget = false;
			_drone.LastTarget = null;
		}

		public override void Step() {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.Parameters.IdleReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = Mathf.Max(_seeTimer - Time.deltaTime, 0.0f);
			}
		}

		public override void Exit() { }

		public override void ReceiveDamage(IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage) {
			if (pSender != null && Utilities.AI.FactionIsEnemy(_drone.Faction, pSender.Faction)) {
				_drone.LastTarget = pSender.GameObject;
				_fsm.SetState<DroneFollowState>();
			} else if (pSender == null) {
				_fsm.SetState<DroneSearchState>();
			}
		}
	}
}