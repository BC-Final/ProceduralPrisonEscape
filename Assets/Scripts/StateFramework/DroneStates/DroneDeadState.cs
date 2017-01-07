using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneDeadState : AbstractDroneState {
		private UnityEngine.AI.NavMeshAgent _agent;
		private Rigidbody _rigidbody;


		public DroneDeadState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) {
			_agent = _drone.GetComponent<UnityEngine.AI.NavMeshAgent>();
			_rigidbody = _drone.GetComponent<Rigidbody>();
		}

		public override void Enter() {
			DropBeacon db = _drone.GetComponent<DropBeacon>();

			if (db != null) {
				db.Drop();
			}

			_agent.enabled = false;
			_rigidbody.isKinematic = false;
			_rigidbody.useGravity = true;

			/*
			foreach (Transform t in _drone.Model.GetComponentsInChildren<Transform>()) {
				Rigidbody b = t.gameObject.AddComponent<Rigidbody>();
				b.AddExplosionForce(200.0f, _drone.Model.position, 3.0f);
				t.parent = null;
				GameObject.Destroy(t.gameObject, 8.0f);
			}
			*/


			GameObject.Destroy(_drone.gameObject, 8.0f);
		}

		public override void Step() {
			//TODO Despawn after some time
		}

		public override void Exit() {

		}

		public override void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
			//foreach (Rigidbody t in _drone.Model.GetComponentsInChildren<Rigidbody>()) {
				//t.AddForceAtPosition(pDirection * pDamage, pPoint);
			//}
			_rigidbody.AddForceAtPosition(pDirection * pDamage, pPoint);
		}
	}
}