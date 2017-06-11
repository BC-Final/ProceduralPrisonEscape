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
			_drone.SendUpdates(false);
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

		public override void Exit() {
			_drone.SendUpdates(true);
		}

		public override void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_fsm.SetState<DroneFollowState>();
			} else if (src == null) {
				_fsm.SetState<DroneSearchState>();
			}
		}
	}
}