using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class DroneAlarmReturnState : AbstractDroneState {
		private float _startStoppingDistance;
		private float _seeTimer;

		public DroneAlarmReturnState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_startStoppingDistance = _drone.Agent.stoppingDistance;

			_drone.Agent.stoppingDistance = 0.0f;
			_drone.Agent.SetDestination(DroneSpawnManager.Instance.GetClosestSpawner(_drone.gameObject).transform.position);
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
				_seeTimer = 0.0f;
			}

			if (_drone.Agent.velocity.magnitude == 0.0f) {
				_drone.DestroyEvent();
				GameObject.Destroy(_drone.gameObject);
			}
		}

		public override void Exit() {
			_drone.Agent.stoppingDistance = _startStoppingDistance;
		}

		public override void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_fsm.SetState<DroneAlarmFollowState>();
			} else if (src == null) {
				_fsm.SetState<DroneAlarmSearchState>();
			}
		}
	}
}
