using UnityEngine;

namespace StateFramework {
	public abstract class AbstractState {
		public AbstractState () { }

		public abstract void Enter ();
		public abstract void Step ();
		public abstract void Exit ();

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

			return Mathf.Clamp(angle, min, max);
		}

		private float NormalizeAngle (float angle) {
			while (angle > 360)
				angle -= 360;
			while (angle < 0)
				angle += 360;
			return angle;
		}

		protected bool canSeeObject (GameObject pTarget, Transform pOrigin, float pRange, float pAngle) {
			if (Vector3.Distance(pOrigin.position, pTarget.transform.position) < pRange) {
				if (pAngle < 360.0f) {
					float angle = Vector3.Angle(pTarget.transform.position - pOrigin.position, pOrigin.forward);
					float sign = Mathf.Sign(Vector3.Dot(pTarget.transform.position - pOrigin.position, pOrigin.right));
					float finalAngle = sign * angle;

					if (!(finalAngle <= pAngle / 2f && finalAngle >= -pAngle / 2f)) {
						return false;
					}
				}

				Collider[] coll = pOrigin.GetComponentsInChildren<Collider>();

				foreach (Collider c in coll) {
					c.enabled = false;
				}

				RaycastHit hit;
				if (Physics.Raycast(pOrigin.position, (pTarget.transform.position - pOrigin.position).normalized, out hit, pRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
					if (hit.collider.gameObject == pTarget) {
						foreach (Collider c in coll) {
							c.enabled = true;
						}

						return true;
					}
				}

				foreach (Collider c in coll) {
					c.enabled = true;
				}
			}

			return false;
		}
	}
}