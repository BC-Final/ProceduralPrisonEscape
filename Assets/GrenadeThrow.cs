using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour {
	[SerializeField]
	private int _maxGrenades;
	public int MaxGrenades { get { return _maxGrenades; } }

	[SerializeField]
	private float _cooldown;
	private bool _canThrow = true;

	[SerializeField]
	private float _cameraDistance;

	private int _noOfGrenades;
	public int NoOfGrenades { get { return _noOfGrenades; } set { _noOfGrenades = value; } }

	AmmoDisplayShooterUI _ui;

	private void Start() {
		_noOfGrenades = 0;
		_ui = FindObjectOfType<AmmoDisplayShooterUI>();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.G) && _canThrow && _noOfGrenades != 0) {
			throwGrenade();
		}
	}

	private void throwGrenade() {
		Instantiate(ShooterReferenceManager.Instance.Grenade, transform.position + transform.forward * _cameraDistance, transform.rotation);
		_noOfGrenades--;
		_canThrow = false;
		TimerManager.CreateTimer("Grenade Cooldown", true).SetDuration(_cooldown).AddCallback(() => _canThrow = true).Start();
	}

	private void OnGUI() {
		_ui.SetGrenades(_noOfGrenades);
	}
}
