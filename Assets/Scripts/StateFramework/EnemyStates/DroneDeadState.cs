using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneDeadState : AbstractDroneState {
		private NavMeshAgent _agent;
		private Rigidbody _rigidbody;


		public DroneDeadState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_agent = _drone.GetComponent<NavMeshAgent>();
			_rigidbody = _drone.GetComponent<Rigidbody>();
		}

		public override void Enter() {

		}

		public override void Step() {
			
		}

		public override void Exit() {

		}

		public override void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
			_agent.enabled = false;

			_rigidbody.useGravity = true;
			_rigidbody.isKinematic = false;
			_rigidbody.AddForceAtPosition(pDirection * pDamage, pPoint);
		}
	}
}