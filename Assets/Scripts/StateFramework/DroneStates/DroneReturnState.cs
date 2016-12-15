using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace StateFramework {
	public class DroneReturnState : AbstractDroneState {
		private GameObject _player;
		private UnityEngine.AI.NavMeshAgent _agent;

		private Vector3 _startPosition;
		private Vector3 _startRotation;
		private float _startStoppingDistance;
		private float _seeTimer;

		public DroneReturnState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();

			_startPosition = _drone.transform.position;
			_startRotation = _drone.transform.rotation.eulerAngles;

			_startStoppingDistance = _agent.stoppingDistance;
		}

		public override void Enter() {
			_agent.stoppingDistance = 0.0f;
			_agent.SetDestination(_startPosition);
			_seeTimer = 0.0f;
		}

		public override void Step() {
			if (canSeeObject(_player, _drone.LookPos, _drone.SeeRange, _drone.SeeAngle) || Vector3.Distance(_drone.LookPos.position, _player.transform.position) < _drone.AwarenessRadius) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.IdleReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = 0.0f;
			}

			//TODO This is buggy
			if (_agent.velocity.magnitude == 0.0f) {
				//This might be buggy
				_drone.transform.DORotate(_startRotation, 0.25f);
				_fsm.SetState<DroneGuardState>();
			}
		}

		public override void Exit() {
			_agent.stoppingDistance = _startStoppingDistance;
		}

		public override void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
			_fsm.SetState<DroneFollowState>();
		}
	}
}