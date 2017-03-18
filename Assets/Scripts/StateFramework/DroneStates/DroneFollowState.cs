using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneFollowState : AbstractDroneState {
		private GameObject _player;
		private UnityEngine.AI.NavMeshAgent _agent;

		private float _seeTimer;

		public DroneFollowState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		public override void Enter() {
			_agent.SetDestination(_player.transform.position);
			_seeTimer = 0.0f;

			_drone.SeesPlayer = true;
		}

		public override void Step() {
			if (canSeeObject(_player, _drone.LookPos, _drone.SeeRange, _drone.SeeAngle) || Vector3.Distance(_drone.LookPos.position, _player.transform.position) < _drone.AwarenessRadius) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.AlarmedReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}
			} else {
				_seeTimer = 0.0f;
			}



			if (!_drone.Chase && _agent.remainingDistance <= _agent.stoppingDistance) {
				_fsm.SetState<DroneSearchState>();
			}
		}

		public override void Exit() {

		}
	}
}