using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretHideState : AbstractTurretState {
		private GameObject _player;
		private Sequence _sequence;

		public TurretHideState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			_turret.GetComponentInChildren<Light>().intensity = 0.0f;

			_sequence = DOTween.Sequence();
			_sequence.Append(_turret.RotaryBase.DOLocalMove(new Vector3(0.0f, 0.1f, 0.0f), _turret.DeployTime).SetEase(Ease.Linear));
			_sequence.Join(_turret.Gun.DOLocalRotate(new Vector3(-90.0f, 0.0f, 0.0f), _turret.DeployTime).SetEase(Ease.Linear));
		}

		public override void Step () {
			if (Vector3.Distance(_turret.transform.position, _player.transform.position) < _turret.SeeRange) {
				_sequence.Kill();
				_fsm.SetState<TurretDeployState>();
			}

			if (!_sequence.IsPlaying()) {
				_fsm.SetState<TurretGuardState>();
			}
		}

		public override void Exit () {

		}

		public override void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
			_fsm.SetState<TurretDeployState>();
		}
	}
}