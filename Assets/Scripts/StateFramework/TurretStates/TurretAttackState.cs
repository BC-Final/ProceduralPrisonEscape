using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFramework {
	public class TurretAttackState : AbstractTurretState {
		private float _nextAttackTime;


		public TurretAttackState (ShooterTurret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) { }

		public override void Enter () {
			_nextAttackTime = 0.0f;
			_turret.SeesTarget = true;
		}

		public override void Step () {
			GameObject target = Utilities.AI.GetClosestObjectInView(_turret.ShootPos, _turret.PossibleTargets, _turret.Parameters.AttackRange, _turret.Parameters.AttackAngle);

			if (target != null) {
				rotateTowards(target.transform);

				if (_nextAttackTime - Time.time <= 0.0f) {
					_nextAttackTime = Time.time + _turret.Parameters.AttackRate;

					RaycastHit hit;

					_turret.GetComponentInChildren<ParticleSystem>().Play();

					if (Utilities.Weapons.CastShot(_turret.Parameters.SpreadConeRadius, _turret.Parameters.SpreadConeLength, _turret.ShootPos, out hit)) {
						Utilities.Weapons.DisplayBulletTracer(_turret.ShootPos.position, hit.point);

						IDamageable dam = hit.collider.GetComponentInParent<IDamageable>();

						if (dam != null) {
							//dam.ReceiveDamage(_turret, (_turret.ShootPos.position - dam.GameObject.transform.position).normalized, hit.point, _turret.Parameters.AttackDamage);
							dam.ReceiveDamage(_turret.transform, hit.point, _turret.Parameters.AttackDamage, _turret.Parameters.AttackForce);
						} else {
							Utilities.Weapons.DisplayDecal(hit.point, hit.normal, hit.collider.transform);
						}
					} else {
						Utilities.Weapons.DisplayBulletTracer(_turret.ShootPos.position, _turret.ShootPos.forward, 200.0f);
					}
				}
			} else {
				_fsm.SetState<TurretEngageState>();
			}
		}

		public override void Exit () { }
	}
}