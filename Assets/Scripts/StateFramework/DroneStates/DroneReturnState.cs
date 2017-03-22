using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace StateFramework {
	public class DroneReturnState : AbstractDroneState {
		private Vector3 _startPosition;
		private Vector3 _startRotation;
		private float _startStoppingDistance;
		private float _seeTimer;

		public DroneReturnState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_startPosition = _drone.transform.position;
			_startRotation = _drone.transform.rotation.eulerAngles;
		}

		public override void Enter() {
			_startStoppingDistance = _drone.Agent.stoppingDistance;

			_drone.Agent.stoppingDistance = 0.0f;
			_drone.Agent.SetDestination(_startPosition);
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


			//TODO Setting the rotation like this might be buggy
			if (_drone.Agent.velocity.magnitude == 0.0f) {
				_drone.transform.DORotate(_startRotation, 0.25f);
				_fsm.SetState<DroneGuardState>();
			}
		}

		public override void Exit() {
			_drone.Agent.stoppingDistance = _startStoppingDistance;
		}

		public override void ReceiveDamage (IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage) {
			if (pSender != null && Utilities.AI.FactionIsEnemy(_drone.Faction, pSender.Faction)) {
				_drone.LastTarget = pSender.GameObject;
				_fsm.SetState<DroneFollowState>();
			} else if (pSender == null) {
				_fsm.SetState<DroneSearchState>();
			}
		}
	}
}