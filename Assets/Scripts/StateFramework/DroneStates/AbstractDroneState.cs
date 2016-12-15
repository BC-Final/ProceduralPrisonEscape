using UnityEngine;
using System;

namespace StateFramework {
	public class AbstractDroneState : AbstractState {
		protected StateMachine<AbstractDroneState> _fsm = null;
		protected DroneEnemy _drone;

		public AbstractDroneState (DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) {
			_drone = pDrone;
			_fsm = pFsm;
		}

		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }

		public virtual void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) { }

		protected void rotateTowards (GameObject pDroneModel, Transform pTarget) {
			Vector3 mdlDirection = (new Vector3(pTarget.position.x, pTarget.position.y, pTarget.position.z) - pDroneModel.transform.position).normalized;
			Quaternion mdlLookRotation = Quaternion.LookRotation(mdlDirection);
			pDroneModel.transform.rotation = Quaternion.Slerp(pDroneModel.transform.rotation, mdlLookRotation, Time.deltaTime * _drone.RotationSpeed);

			Vector3 direction = (new Vector3(pTarget.position.x, _drone.transform.position.y, pTarget.position.z) - _drone.transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			_drone.transform.rotation = Quaternion.Slerp(_drone.transform.rotation, lookRotation, Time.deltaTime * _drone.RotationSpeed);
		}
	}
}