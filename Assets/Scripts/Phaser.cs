using UnityEngine;
using System.Collections;
using StateFramework;

public class Phaser : Weapon {
	private void Start() {
		_fsm = new StateMachine<AbstractWeaponState>();

		_fsm.AddState(new PhaserIdleState(this, _fsm));
		_fsm.AddState(new PhaserShootState(this, _fsm));
		_fsm.AddState(new PhaserReloadState(this, _fsm));
		_fsm.AddState(new PhaserMeleeState(this, _fsm));

		_fsm.SetState<PhaserIdleState>();
	}
}
