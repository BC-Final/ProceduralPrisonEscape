using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
	public static class Angles {
		/// <summary>
		/// Clamps an angle between a min and max angle.
		/// </summary>
		/// <param name="pAngle">The angle to clamp.</param>
		/// <param name="pMin">The minimum angle.</param>
		/// <param name="pMax">The maximum angle.</param>
		/// <returns>Returns the clamped angle</returns>
		public static float ClampAngle (float pAngle, float pMin, float pMax) {
			pAngle = NormalizeAngle(pAngle);
			if (pAngle > 180) {
				pAngle -= 360;
			} else if (pAngle < -180) {
				pAngle += 360;
			}

			pMin = NormalizeAngle(pMin);
			if (pMin > 180) {
				pMin -= 360;
			} else if (pMin < -180) {
				pMin += 360;
			}

			pMax = NormalizeAngle(pMax);
			if (pMax > 180) {
				pMax -= 360;
			} else if (pMax < -180) {
				pMax += 360;
			}

			return Mathf.Clamp(pAngle, pMin, pMax);
		}



		/// <summary>
		/// Normalizes an angle between 0 and 360 degrees.
		/// </summary>
		/// <param name="pAngle">The angle to normalize</param>
		/// <returns>Returns the normalized angle</returns>
		public static float NormalizeAngle (float pAngle) {
			while (pAngle > 360)
				pAngle -= 360;
			while (pAngle < 0)
				pAngle += 360;
			return pAngle;
		}
	}

	public class Weapons {
		public static void DisplayBulletTracer (Vector3 pStartPoint, Vector3 pDirection, float pRange) {
			DisplayBulletTracer(pStartPoint, pStartPoint + pDirection * pRange);
		}

		public static void DisplayBulletTracer (Vector3 pStartPoint, Vector3 pEndPoint) {
			GameObject tracer = GameObject.Instantiate(ShooterReferenceManager.Instance.BulletTracer, pStartPoint, Quaternion.LookRotation(pEndPoint - pStartPoint)) as GameObject;
			tracer.GetComponentInChildren<ParticleSystem>().Play();
			GameObject.Destroy(tracer, 1.0f);
		}

		public static void DisplayLaser (Vector3 pStartPoint, Vector3 pDirection, float pRange) {
			DisplayLaser(pStartPoint, pStartPoint + pDirection * pRange);
		}

		public static void DisplayLaser (Vector3 pStartPoint, Vector3 pEndPoint) {
			GameObject laser = GameObject.Instantiate(ShooterReferenceManager.Instance.LaserShot) as GameObject;
			laser.GetComponent<LineRenderer>().SetPosition(0, pStartPoint);
			laser.GetComponent<LineRenderer>().SetPosition(1, pEndPoint);
			GameObject.Destroy(laser, 0.05f);

			ShooterPackageSender.SendPackage(new NetworkPacket.Create.LaserShot(pStartPoint, pEndPoint));
		}

		public static void DisplayDecal (Vector3 pPoint, Vector3 pNormal, Transform pParent) {
			GameObject decal = GameObject.Instantiate(ShooterReferenceManager.Instance.BulletHole, pPoint + pNormal * 0.01f, Quaternion.LookRotation(pNormal)) as GameObject;
			decal.transform.parent = pParent;
			GameObject.Destroy(decal, 10);
		}

		public static bool CastShot (float pSpreadConeRadius, float pSpreadConeLength, Transform pShotStartLocation, out RaycastHit pHit) {
			float randomRadius = UnityEngine.Random.Range(0, pSpreadConeRadius);
			float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

			Vector3 rayDir = new Vector3(
				randomRadius * Mathf.Cos(randomAngle),
				randomRadius * Mathf.Sin(randomAngle),
				pSpreadConeLength
			);

			rayDir = pShotStartLocation.TransformDirection(rayDir.normalized);

			return Physics.Raycast(pShotStartLocation.position, rayDir, out pHit, 200.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
		}
	}

	public static class AI {
		/// <summary>
		/// Determines if the other is an enemy of the tester.
		/// </summary>
		/// <param name="pTester">The tester</param>
		/// <param name="pOther">The other</param>
		/// <returns>Return TRUE of other is an enemy, otherwise FALSE.</returns>
		public static bool FactionIsEnemy (Faction pTester, Faction pOther) {
			return  pTester != pOther && pOther != Faction.Neutral;
		}



		/// <summary>
		/// Determines if new target is closer to origin than old target.
		/// </summary>
		/// <param name="pOrigin">The origin of the distance test.</param>
		/// <param name="pOldTarget">The old target.</param>
		/// <param name="pNewTarget">The new target.</param>
		/// <returns>Return TRUE when new target is closer to origin than old target, otherwise FALSE.</returns>
		public static bool IsNewTargetCloser (GameObject pOrigin, GameObject pOldTarget, GameObject pNewTarget) {
			return Vector3.Distance(pOrigin.transform.position, pOldTarget.transform.position) > Vector3.Distance(pOrigin.transform.position, pNewTarget.transform.position);
		}



		/// <summary>
		/// Checks if target is in range of origin.
		/// </summary>
		/// <param name="pOrigin">The origin of the range check.</param>
		/// <param name="pTarget">the target of the range check.</param>
		/// <param name="pRange">The view range if origin.</param>
		/// <returns>Returns TRUE if target is in range of origin, otherwise FALSE.</returns>
		public static bool ObjectInRange (Transform pOrigin, Transform pTarget, float pRange) {
			return Vector3.Distance(pOrigin.position, pTarget.position) <= pRange;
		}



		/// <summary>
		/// Checks if target is in the field of view of origin.
		/// </summary>
		/// <param name="pOrgin">The origin of the FOV check.</param>
		/// <param name="pTarget">The target of the FOV check.</param>
		/// <param name="pAngle">The field of view angle of origin.</param>
		/// <returns>Returns TRUE of target is within the FOV of origin, otherwise FALSE.</returns>
		public static bool ObjectInFOV (Transform pOrgin, Transform pTarget, float pAngle) {
			if (pAngle >= 360.0f)
				return true;

			float angle = Vector3.Angle(pTarget.position - pOrgin.position, pOrgin.forward);
			float sign = Mathf.Sign(Vector3.Dot(pTarget.position - pOrgin.position, pOrgin.right));
			float finalAngle = sign * angle;

			if (finalAngle <= pAngle / 2f && finalAngle >= -pAngle / 2f)
				return true;

			return false;
		}



		/// <summary>
		/// Checks if origin can see target with a raycast. This raycast ignores all objects with "MeshCollider" layer
		/// </summary>
		/// <param name="pOrigin">The origin of the raycast</param>
		/// <param name="pTarget">The target of the raycast</param>
		/// <returns>Returns TRUE if origin can see target</returns>
		public static bool ObjectVisible (Transform pOrigin, Transform pTarget) {
			//TODO Maybe replace this with a sphere cast? Might even be faster

			//Ignore "MeshCollider" und Physics.IgnoreRaycastLayer
			//Allow 

			//int mask = ~( (1 << LayerMask.NameToLayer("MeshCollider")) | Physics.IgnoreRaycastLayer);
			int mask = LayerMask.GetMask("RayTrigger", "Environment");
			RaycastHit hit;

			if (Physics.Linecast(pOrigin.position, pTarget.position, out hit, mask)) {
				if (hit.rigidbody != null && hit.rigidbody.gameObject == pTarget.gameObject) {
					return true;
				}
			}

			return false;
		}



		/// <summary>
		/// Checks if the target is in range of, in the fov of and visible to the origin.
		/// </summary>
		/// <param name="pOrigin">The origin of the view check.</param>
		/// <param name="pTarget">The target of the view check.</param>
		/// <param name="pRange">The view range of origin.</param>
		/// <param name="pAngle">The field of view angle of origin.</param>
		/// <returns>Returns TRUE if target is in view, otherwise FALSE.</returns>
		public static bool ObjectInView (Transform pOrigin, Transform pTarget, float pRange, float pAngle) {
			return ObjectInRange(pOrigin, pTarget, pRange) && ObjectInFOV(pOrigin, pTarget, pAngle) && ObjectVisible(pOrigin, pTarget);
		}



		/// <summary>
		/// Checks if an array of targets for the closest visible one.
		/// </summary>
		/// <param name="pOrigin">The origin of the view check.</param>
		/// <param name="pTargets">A list of all targets. Should be sorted ascending by distance.</param>
		/// <param name="pRange">The view range of origin.</param>
		/// <param name="pAngle">The field of view angle of origin.</param>
		/// <returns>Returns the closest GameObject in view. If there is none Null is returned.</returns>
		public static GameObject GetClosestObjectInView(Transform pOrigin, Transform[] pTargets, float pRange, float pAngle) {
			foreach (Transform t in pTargets) {
				if (ObjectInView(pOrigin, t, pRange, pAngle)) {
					return t.gameObject;
				}
			}

			return null;
		}



		/// <summary>
		/// Checks if an array of targets for the closest visible one.
		/// </summary>
		/// <param name="pOrigin">The origin of the view check.</param>
		/// <param name="pTargets">A list of all targets. Should be sorted ascending by distance.</param>
		/// <param name="pRange">The view range of origin.</param>
		/// <param name="pAngle">The field of view angle of origin.</param>
		/// <param name="pAwarenessRange">The awareness radius of origin.</param>
		/// <returns>Returns the closest GameObject in view. If there is none Null is returned.</returns>
		public static GameObject GetClosestObjectInView (Transform pOrigin, Transform[] pTargets, float pRange, float pAngle, float pAwarenessRange) {
			foreach (Transform t in pTargets) {
				if (ObjectInView(pOrigin, t, pRange, pAngle) || ObjectInRange(pOrigin, t, pAwarenessRange)) {
					return t.gameObject;
				}
			}

			return null;
		}
	}
}
