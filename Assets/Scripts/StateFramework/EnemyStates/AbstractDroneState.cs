using UnityEngine;
using System;

namespace StateFramework {
	public class AbstractDroneState : AbstractState {
		protected StateMachine<AbstractDroneState> _fsm = null;
		protected Enemy_Drone _drone;

		public AbstractDroneState(Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) {
			_drone = pDrone;
			_fsm = pFsm;
		}

		public override void Enter() { }
		public override void Step() { }
		public override void Exit() { }

		public virtual void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) { }

		protected void rotateTowards(GameObject pDroneModel, Transform pTarget) {
			Vector3 xzDirection = (new Vector3(pTarget.position.x, pTarget.position.y, pTarget.position.z) - pDroneModel.transform.position).normalized;
			Quaternion xzLookRotation = Quaternion.LookRotation(xzDirection);
			pDroneModel.transform.rotation = Quaternion.Slerp(pDroneModel.transform.rotation, xzLookRotation, Time.deltaTime * _drone.RotationSpeed);
		}

		protected bool canSeeObject(GameObject pObject, float pRange) {
			if (Vector3.Distance(_drone.transform.position, pObject.transform.position) < pRange) {
				float angle = Vector3.Angle(pObject.transform.position - _drone.transform.position, _drone.transform.forward);
				float sign = Mathf.Sign(Vector3.Dot(pObject.transform.position - _drone.transform.position, _drone.transform.right));
				float finalAngle = sign * angle;

				if (finalAngle <= _drone.SeeAngle / 2f && finalAngle >= -_drone.SeeAngle) {
					return true;
				}
			}

			return false;
		}
	}
}