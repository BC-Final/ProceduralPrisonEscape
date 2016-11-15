using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneEngangeState : AbstractDroneState {
		private GameObject _player;
		private GameObject _droneModel;
		private NavMeshAgent _agent;

		private float _nextPathTick;

		public DroneEngangeState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
			_droneModel = _drone.transform.GetChild(0).gameObject;
			_agent = _drone.GetComponent<NavMeshAgent>();
		}

		public override void Enter() {
			_nextPathTick = 0.0f;
		}

		public override void Step() {
			//TODO Also rotate _drone without y axis (so he player is in attack range) (maybe only in attack state??)
			rotateTowards(_droneModel, _player.transform);

			if (_nextPathTick - Time.time <= 0.0f) {
				_nextPathTick = Time.time + _drone.PathTickRate;
				_agent.SetDestination(_player.transform.position);
			}

			if (canSeeObject(_player, _drone.AttackRange)) {
				_fsm.SetState<DroneAttackState>();
			}

			if(!canSeeObject(_player, _drone.SeeRange)) {
			//if (Vector3.Distance(_player.transform.position, _drone.transform.position) > _drone.SeeRange) {
				_fsm.SetState<DroneFollowState>();
			}
		}

		public override void Exit() {

		}
	}
}