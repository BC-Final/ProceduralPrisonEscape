using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretScanState : AbstractTurretState {
		private GameObject _player;

		private float _scanTime;

		private Sequence _sequence;

		public TurretScanState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			Debug.Log("Scan");
			_scanTime = 0.0f;

			//_sequence.Append(_turret.Gun.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 0.25f));
			_sequence = DOTween.Sequence();
			_sequence.Append(_turret.RotaryBase.DOBlendableLocalRotateBy(new Vector3(0.0f, 60.0f, 0.0f), 1.0f).SetEase(Ease.Linear));
			_sequence.Append(_turret.RotaryBase.DOBlendableLocalRotateBy(new Vector3(0.0f, -120.0f, 0.0f), 2.0f).SetEase(Ease.Linear));
			_sequence.Append(_turret.RotaryBase.DOBlendableLocalRotateBy(new Vector3(0.0f, 60.0f, 0.0f), 1.0f).SetEase(Ease.Linear));
			_sequence.SetLoops(-1);
		}

		public override void Step () {
			_scanTime += Time.deltaTime;

			if (_scanTime >= _turret.ScanTime) {
				_sequence.Kill();
				_fsm.SetState<TurretHideState>();
			}

			if (canSeeObject(_player, _turret.transform, _turret.SeeRange, 360.0f)) {
				_sequence.Kill();
				_fsm.SetState<TurretEngageState>();
			}
		}

		public override void Exit () {

		}
	}
}