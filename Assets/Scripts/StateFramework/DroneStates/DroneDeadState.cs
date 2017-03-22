using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneDeadState : AbstractDroneState {
		public DroneDeadState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_drone.SeesTarget = false;
			_drone.Agent.enabled = false;
			_drone.LastTarget = null;
			_drone.gameObject.SetActive(false);

			//Collider[] coll = _drone.GetComponentsInChildren<Collider>();

			//foreach (Collider c in coll) {
			//	c.enabled = false;
			//}
		}

		public override void Step() { }

		public override void Exit() { }

		public override void ReceiveDamage (IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage) {
			GameObject go = GameObject.Instantiate(ShooterReferenceManager.Instance.ExplodingDrone, _drone.transform.position, _drone.transform.rotation);
			go.GetComponent<ExplodeDrone>().SetImpact(pPoint, pDirection, pDamage);
			GameObject.Destroy(_drone.gameObject, 0.25f);
		}
	}
}