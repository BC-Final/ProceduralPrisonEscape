using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneDeadState : AbstractDroneState {
		public DroneDeadState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_drone.SeesTarget = false;
			_drone.Agent.enabled = false;
			_drone.LastTarget = null;
			//_drone.gameObject.SetActive(false);

			//Collider[] coll = _drone.GetComponentsInChildren<Collider>();

			//foreach (Collider c in coll) {
			//	c.enabled = false;
			//}
		}

		public override void Step() { }

		public override void Exit() { }

		public override void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			

			Animator animator = _drone.transform.GetComponentInChildren<Animator>();
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			_drone.gameObject.SetActive(false);
			GameObject go = GameObject.Instantiate(ShooterReferenceManager.Instance.ExplodingDrone, _drone.transform.position, _drone.transform.rotation);

			go.GetComponent<ExplodeDrone>().SetAnimationPercentage(stateInfo.normalizedTime % 1);
			go.GetComponent<ExplodeDrone>().SetImpact(pHitPoint, -(pSource.position - _drone.transform.position).normalized, pForce);
			GameObject.Destroy(_drone.gameObject, 0.1f);
		}
	}
}