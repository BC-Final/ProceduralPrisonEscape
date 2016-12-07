using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateFramework;
using DG.Tweening;

namespace StateFramework {
	public class AbstractTurretState : AbstractState {
		protected StateMachine<AbstractTurretState> _fsm = null;
		protected Turret _turret;

		public AbstractTurretState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) {
			_turret = pTurret;
			_fsm = pFsm;
		}

		public override void Enter () { }
		public override void Step () { }
		public override void Exit () { }

		public virtual void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) { }

		protected float ClampAngle (float angle, float min, float max) {

			angle = NormalizeAngle(angle);
			if (angle > 180) {
				angle -= 360;
			} else if (angle < -180) {
				angle += 360;
			}

			min = NormalizeAngle(min);
			if (min > 180) {
				min -= 360;
			} else if (min < -180) {
				min += 360;
			}

			max = NormalizeAngle(max);
			if (max > 180) {
				max -= 360;
			} else if (max < -180) {
				max += 360;
			}

			// Aim is, convert angles to -180 until 180.
			return Mathf.Clamp(angle, min, max);
		}

		/** If angles over 360 or under 360 degree, then normalize them.
		 */
		protected float NormalizeAngle (float angle) {
			while (angle > 360)
				angle -= 360;
			while (angle < 0)
				angle += 360;
			return angle;
		}


		protected void rotateTowards (Transform pTarget) {
			Vector3 direction = pTarget.position - _turret.RotaryBase.position;

			Vector3 baseDirection = _turret.RotaryBase.InverseTransformDirection(direction);
			baseDirection.y = 0;
			baseDirection.Normalize();
			baseDirection = _turret.RotaryBase.TransformDirection(baseDirection);
			Quaternion baseLookRotation = Quaternion.LookRotation(baseDirection, _turret.transform.up);
			_turret.RotaryBase.rotation = Quaternion.Slerp(_turret.RotaryBase.rotation, baseLookRotation, Time.deltaTime * _turret.HorizontalRotationSpeed);

			Vector3 gunDirection = _turret.Gun.InverseTransformDirection(direction);
			gunDirection.x = 0;
			gunDirection.Normalize();
			gunDirection = _turret.Gun.TransformDirection(gunDirection);
			Quaternion gunLookDirection = Quaternion.LookRotation(gunDirection, Vector3.Cross(gunDirection, _turret.RotaryBase.right));
			_turret.Gun.rotation = Quaternion.Slerp(_turret.Gun.rotation, gunLookDirection, Time.deltaTime * _turret.VerticalRotationSpeed);

			_turret.Gun.localRotation = Quaternion.Euler(new Vector3(ClampAngle(_turret.Gun.localRotation.eulerAngles.x, -_turret.MaxGunRotation, _turret.MaxGunRotation), 0f, 0f));
		}

		protected bool canSeeObject (GameObject pObject, Transform pOrigin, float pRange, float pAngle) {
			if (Vector3.Distance(pOrigin.position, pObject.transform.position) < pRange) {
				if (pAngle < 360.0f) {
					float angle = Vector3.Angle(pObject.transform.position - pOrigin.position, pOrigin.forward);
					float sign = Mathf.Sign(Vector3.Dot(pObject.transform.position - pOrigin.position, pOrigin.right));
					float finalAngle = sign * angle;

					if (finalAngle <= pAngle / 2f && finalAngle >= -pAngle) {
						RaycastHit hit;
						//TODO This can see trough wall apparently ????
						if (Physics.Raycast(pOrigin.position, (pObject.transform.position - pOrigin.position).normalized, out hit, _turret.SeeRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
							if (hit.collider.GetComponent<PlayerMotor>() != null) {
								return true;
							}
						}
					}
				} else {
					return true;
				}
			}

			return false;
		}
	}
}