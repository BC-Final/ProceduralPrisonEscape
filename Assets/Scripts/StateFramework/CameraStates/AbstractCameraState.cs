using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class AbstractCameraState : AbstractState {
		protected StateMachine<AbstractCameraState> _fsm;
		protected ShooterCamera _camera;

		public AbstractCameraState (ShooterCamera pCamera, StateMachine<AbstractCameraState> pFsm) {
			_camera = pCamera;
			_fsm = pFsm;
		}

		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }

		public virtual void ReceiveDamage (IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage) { }

		//TODO Move to AI Utils class
		protected void rotateTowards (Transform pTarget) {
			Vector3 direction = (pTarget.position - _camera.Base.position).normalized;
			Quaternion lookDirection = Quaternion.LookRotation(direction, _camera.transform.up);
			_camera.Base.rotation = Quaternion.Slerp(_camera.Base.rotation, lookDirection, Time.deltaTime * _camera.Parameters.FollowRotationSpeed);
			//_camera.Base.localRotation = Quaternion.Euler(new Vector3(_camera.Base.localRotation.x, _camera.Base.localRotation.y, 0f));
		}
	}
}