using UnityEngine;
using System.Collections;

namespace StateFramework {
	public class DroneSearchState : AbstractDroneState {

		private int _searchCounter;
		private float _seeTimer;

		public DroneSearchState(DroneEnemy pDrone, StateMachine<AbstractDroneState> pFsm) : base(pDrone, pFsm) { }

		public override void Enter() {
			_searchCounter = _drone.Parameters.SearchProbeCount;
			_drone.SeesTarget = false;
			_seeTimer = 0.0f;
		}

		//TODO Make the drone searching state a little bit smarter
		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_drone.transform, _drone.PossibleTargets, _drone.Parameters.ViewRange, _drone.Parameters.ViewAngle, _drone.Parameters.AwarenessRange);

			if (target != null) {
				_seeTimer += Time.deltaTime;

				if (_seeTimer > _drone.Parameters.AlertReactionTime) {
					_fsm.SetState<DroneEngangeState>();
				}

			} else {
				_seeTimer = 0.0f;
			}


			if (!_drone.Agent.hasPath || _drone.Agent.remainingDistance <= _drone.Agent.stoppingDistance) {
				if (_searchCounter > 0) {
					_searchCounter--;
					Vector3 randomDir = Random.insideUnitSphere * _drone.Parameters.SearchProbeRadius;
					randomDir += _drone.transform.position;
					UnityEngine.AI.NavMeshHit hit;
					UnityEngine.AI.NavMesh.SamplePosition(randomDir, out hit, _drone.Parameters.SearchProbeRadius, 1);
					_drone.Agent.SetDestination(hit.position);
				} else {
					if (_drone.Route == null) {
						_fsm.SetState<DroneReturnState>();
					} else {
						_fsm.SetState<DronePatrolState>();
					}
				}
			}
		}

		public override void Exit() { }

		public override void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
			IDamageable src = pSource.GetComponent<IDamageable>();

			if (src != null && Utilities.AI.FactionIsEnemy(_drone.Faction, src.Faction)) {
				_drone.LastTarget = src.GameObject;
				_fsm.SetState<DroneFollowState>();
			} else if (src == null) {
				_fsm.SetState<DroneSearchState>();
			}
		}
	}
}