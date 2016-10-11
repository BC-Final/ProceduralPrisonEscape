using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class WeaponController : MonoBehaviour, IMecanimNotifiable {
	[SerializeField]
	private Transform _aimTransform;

	[SerializeField]
	private float _aimTime;

	private State _state;
	private enum State {
		Idle,
		Shooting,
		Reloading,
		Melee
	}

	private bool _aiming;

	private Transform _equipedWeaponTransform;
	private IWeapon _equipedWeapon;

	private void Start() {
		GameObject weapon = Instantiate(Resources.Load("pfb_weapon_m9"), transform.position, transform.rotation) as GameObject;
		_equipedWeaponTransform = weapon.transform;
		weapon.transform.parent = this.transform;
		_equipedWeapon = weapon.GetComponentInChildren<IWeapon>();

		TransitionNotifier[] notifier = weapon.GetComponentInChildren<Animator>().GetBehaviours<TransitionNotifier>();

		foreach (TransitionNotifier t in notifier) {
			t.SetBehavior(this);
		}
	}

	private void Update() {
		//Automatic or Semi-automatic??
		if (Input.GetButtonDown("Fire") && _state == State.Idle) {
			_state = State.Shooting;
			_equipedWeapon.Shoot();
		}

		if (Input.GetButtonDown("Reload") && _state == State.Idle) {
			if (_equipedWeapon.Reload()) {
				_state = State.Reloading;
			}
		}

		//TODO Move to center of screen
		if (Input.GetButtonDown("Aim")) {
			_equipedWeaponTransform.DOLocalMove(transform.InverseTransformPoint(_aimTransform.position), _aimTime);
			_aiming = true;
		}

		if(Input.GetButtonUp("Aim")) {
			_equipedWeaponTransform.DOLocalMove(Vector3.zero, _aimTime);
			_aiming = false;
		}

		if (!_aiming && Input.GetButtonDown("Melee")) {
			_equipedWeapon.Melee();
		}
	}

	public void Notify(string pMessage) {
		switch (pMessage) {
			case "Shoot":
			case "Reload":
			case "Melee":
				_state = State.Idle;
				break;
		}
	}
}
