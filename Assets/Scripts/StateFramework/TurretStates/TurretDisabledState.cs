using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretDisabledState : AbstractTurretState {
		public TurretDisabledState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_disableTimer = Timers.CreateTimer("Turret disable").SetTime(pTurret.DisabledTime).SetCallback(() => _fsm.SetState<TurretGuardState>()).ResetOnFinish();
		}

		private Sequence _sequence;
		Timers.Timer _disableTimer;

		public override void Enter () {
			_turret.GetComponentInChildren<Light>().intensity = 0.0f;

			_sequence = DOTween.Sequence();
			_sequence.Append(_turret.RotaryBase.DOLocalMove(new Vector3(0.0f, 0.1f, 0.0f), _turret.DeployTime).SetEase(Ease.Linear));
			_sequence.Join(_turret.Gun.DOLocalRotate(new Vector3(-90.0f, 0.0f, 0.0f), _turret.DeployTime).SetEase(Ease.Linear));

			_turret.SeesPlayer = false;
		}

		public override void Step () {
			if (!_sequence.IsPlaying()) {
				_disableTimer.Start();
			}
		}

		public override void Exit () {
			//ShooterPackageSender.SendPackage(new CustomCommands.Update.EnableTurret(_turret.Id));
		}
	}
}