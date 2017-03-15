using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneDeadState : AbstractDroneState {
		private UnityEngine.AI.NavMeshAgent _agent;


		public DroneDeadState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		public override void Enter() {

			_agent.enabled = false;

			//GameObject.Instantiate(_drone.DroneExplode, _drone.transform.position, _drone.transform.rotation);

			//GameObject.Destroy(_drone.gameObject);
		}

		public override void Step() {
			//TODO Despawn after some time
		}

		public override void Exit() {

		}

		public override void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
			GameObject go = GameObject.Instantiate(_drone.DroneExplode, _drone.transform.position, _drone.transform.rotation);
			go.GetComponent<ExplodeDrone>().SetImpact(pPoint, pDirection, pDamage);
			GameObject.Destroy(_drone.gameObject);
		}
	}
}