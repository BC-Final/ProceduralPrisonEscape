using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneEngangeState : AbstractDroneState {
		private float _nextPathTick;

		public DroneEngangeState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_nextPathTick = 0.0f;
			_drone.SeesTarget = true;

			//TODO This creates a loop of getting in this state
			DroneSpawnManager.Instance.NotifySeesPlayer();
		}

		public override void Step() {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_drone.LastTarget = target;

				rotateTowards(_drone.Model.gameObject, target.transform);

				if (Utilities.AI.ObjectInView(_drone.transform, target.transform, _drone.Parameters.AttackRange, _drone.Parameters.AttackAngle)) {
					_fsm.SetState<DroneAttackState>();
				}

				rotateTowards(_drone.Model.gameObject, target.transform);

				if (_nextPathTick - Time.time <= 0.0f) {
					_nextPathTick = Time.time + 1.0f / _drone.Parameters.PathTickRate;
					_drone.Agent.SetDestination(target.transform.position);
				}
			} else {
				if (_drone.AlarmResponder) {
					_fsm.SetState<DroneAlarmFollowState>();
				} else {
					_fsm.SetState<DroneFollowState>();
				}
			}
		}

		public override void Exit() { }

		public override void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && src.GameObject != _drone.LastTarget && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction) && Utilities.AI.IsNewTargetCloser(_drone.gameObject, _drone.LastTarget, src.GameObject)) {
				_drone.LastTarget = src.GameObject;
				if (_drone.AlarmResponder) {
					_fsm.SetState<DroneAlarmFollowState>();
				} else {
					_fsm.SetState<DroneFollowState>();
				}
			}
		}
	}
}