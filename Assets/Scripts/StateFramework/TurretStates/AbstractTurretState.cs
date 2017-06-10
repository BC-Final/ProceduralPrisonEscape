using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateFramework;
using DG.Tweening;

namespace StateFramework {
	public class AbstractTurretState : AbstractState {
		protected StateMachine<AbstractTurretState> _fsm = null;
		protected ShooterTurret _turret;

		public AbstractTurretState (ShooterTurret pTurret, StateMachine<AbstractTurretState> pFsm) {
			_turret = pTurret;
			_fsm = pFsm;
		}

		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }

		public virtual void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) { }

		//TODO Mopve to AI Utils
		protected void rotateTowards (Transform pTarget) {
			Vector3 direction = pTarget.position - _turret.RotaryBase.position;

			Vector3 baseDirection = _turret.RotaryBase.InverseTransformDirection(direction);
			baseDirection.y = 0;
			baseDirection.Normalize();
			baseDirection = _turret.RotaryBase.TransformDirection(baseDirection);
			Quaternion baseLookRotation = Quaternion.LookRotation(baseDirection, _turret.transform.up);
			_turret.RotaryBase.rotation = Quaternion.Slerp(_turret.RotaryBase.rotation, baseLookRotation, Time.deltaTime * _turret.Parameters.HorizontalRotationSpeed);

			Vector3 gunDirection = _turret.Gun.InverseTransformDirection(direction);
			gunDirection.x = 0;
			gunDirection.Normalize();
			gunDirection = _turret.Gun.TransformDirection(gunDirection);
			Quaternion gunLookDirection = Quaternion.LookRotation(gunDirection, Vector3.Cross(gunDirection, _turret.RotaryBase.right));
			_turret.Gun.rotation = Quaternion.Slerp(_turret.Gun.rotation, gunLookDirection, Time.deltaTime * _turret.Parameters.VerticalRotationSpeed);

			_turret.Gun.localRotation = Quaternion.Euler(new Vector3(Utilities.Angles.ClampAngle(_turret.Gun.localRotation.eulerAngles.x, -_turret.Parameters.MaxHorizontalRotation, _turret.Parameters.MaxHorizontalRotation), 0f, 0f));
		}
	}
}