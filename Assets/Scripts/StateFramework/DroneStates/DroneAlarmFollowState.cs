using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class DroneAlarmFollowState : AbstractDroneState {
		private float _seeTimer;
		private float _looseTimer;
		private float _nextPathTick;

		public DroneAlarmFollowState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_drone.Agent.SetDestination(_drone.LastTarget.transform.position);
			_drone.SeesTarget = true;
			_seeTimer = 0.0f;
			_looseTimer = 0.0f;
			_nextPathTick = 0.0f;
		}

		public override void Step() {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.Parameters.AlertReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else if (_drone.LastTarget != null) {
				_looseTimer += Time.deltaTime;

				if (_nextPathTick - Time.time <= 0.0f) {
					_nextPathTick = Time.time + 1.0f / _drone.Parameters.PathTickRate;
					_drone.Agent.SetDestination(_drone.LastTarget.transform.position);
				}

				if (_looseTimer > _drone.Parameters.FollowTime) {
					_drone.LastTarget = null;
				}
			} else {
				_seeTimer = 0.0f;
			}

			if (_drone.Agent.remainingDistance <= _drone.Agent.stoppingDistance && _drone.LastTarget == null) {
				_drone.SeesTarget = false;
				DroneSpawnManager.Instance.NotifyLostPlayer();
				_fsm.SetState<DroneAlarmSearchState>();
			}
		}

		public override void Exit() {

		}

		public override void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_drone.Agent.SetDestination(src.GameObject.transform.position);
			}
		}
	}
}