using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneEngangeState : AbstractDroneState {
		private GameObject _player;
		private GameObject _droneModel;
		private UnityEngine.AI.NavMeshAgent _agent;

		private float _nextPathTick;

		public DroneEngangeState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_droneModel = _drone.transform.GetChild(0).gameObject;
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		public override void Enter() {
			_nextPathTick = 0.0f;
		}

		public override void Step() {
			rotateTowards(_droneModel, _player.transform);

			if (_nextPathTick - Time.time <= 0.0f) {
				_nextPathTick = Time.time + _drone.PathTickRate;
				_agent.SetDestination(_player.transform.position);
			}

			if (canSeeObject(_player, _drone.LookPos, _drone.AttackRange, _drone.SeeAngle)) {
				_fsm.SetState<DroneAttackState>();
			}

			if(!canSeeObject(_player, _drone.LookPos, _drone.SeeRange, _drone.SeeAngle) && !(Vector3.Distance(_drone.LookPos.position, _player.transform.position) < _drone.AwarenessRadius)) {
				_fsm.SetState<DroneFollowState>();
			}
		}

		public override void Exit() {

		}
	}
}