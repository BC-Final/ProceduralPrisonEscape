﻿using UnityEngine;
using System;

namespace StateFramework {
	public class AbstractDroneState : AbstractState {
		protected StateMachine<AbstractDroneState> _fsm = null;
		protected Enemy_Drone _drone;

		public AbstractDroneState (Enemy_Drone pDrone, StateMachine<AbstractDroneState> pFsm) {
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

		protected bool canSeeObject (GameObject pObject, float pRange, float pAngle) {
			if (Vector3.Distance(_drone.transform.position, pObject.transform.position) < pRange) {
				if (pAngle < 360.0f) {
					float angle = Vector3.Angle(pObject.transform.position - _drone.transform.position, _drone.transform.forward);
					float sign = Mathf.Sign(Vector3.Dot(pObject.transform.position - _drone.transform.position, _drone.transform.right));
					float finalAngle = sign * angle;

					if (finalAngle <= pAngle / 2f && finalAngle >= -pAngle) {
						RaycastHit hit;
						if (Physics.Raycast(_drone.transform.position, (pObject.transform.position - _drone.transform.position).normalized, out hit, _drone.SeeRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
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