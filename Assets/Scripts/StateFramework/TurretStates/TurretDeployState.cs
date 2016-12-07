using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StateFramework {
	public class TurretDeployState : AbstractTurretState{
		private GameObject _player;
		private Sequence _sequence;

		public TurretDeployState (Turret pTurret, StateMachine<AbstractTurretState> pFsm) : base(pTurret, pFsm) {
			_player = GameObject.FindGameObjectWithTag("Player");
		}

		public override void Enter () {
			Debug.Log("Deploy");
			_sequence = DOTween.Sequence();
			_sequence.Append(_turret.RotaryBase.DOLocalMove(new Vector3(0.0f, 0.8f, 0.0f), _turret.DeployTime));
			_sequence.Join(_turret.Gun.DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), _turret.DeployTime));
		}

		public override void Step () {
			if (!_sequence.IsPlaying()) {
				if (Vector3.Distance(_turret.transform.position, _player.transform.position) > _turret.SeeRange) {
					_fsm.SetState<TurretEngageState>();
				} else {
					_fsm.SetState<TurretScanState>();
				}
			}
		}

		public override void Exit () {

		}
	}
}