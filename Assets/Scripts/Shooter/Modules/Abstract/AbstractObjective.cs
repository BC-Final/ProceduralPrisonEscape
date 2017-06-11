using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

[SelectionBase]
public abstract class AbstractObjective : MonoBehaviour, IShooterNetworked {
	[SerializeField]
	private float _networkUpdateRate;

	Timer _networkUpdateTimer;

	private bool _active;

	private ObservedValue<bool> _solved = new ObservedValue<bool>(false);

	public bool IsSolved {
		get { return _solved.Value; }
	}

	public void OnSolved (System.Action pAction) {
		_solved.OnValueChange += pAction;
	}



	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void Initialize () {
		_networkUpdateTimer = TimerManager.CreateTimer("Objective Update", false).SetDuration(_networkUpdateRate).SetLoops(-1).AddCallback(() => sendUpdate());
	}

	private void sendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Objective.Creation(Id, transform.position.x, transform.position.z));
	}



	public void SetSolved () {
		SetActive(false);
		_solved.Value = true;

		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Objective.sUpdate(Id, true));
	}

	public void SetActive (bool pActive) {
		if (pActive) {
			_active = pActive;
			SetVisible(pActive);
		} else {
			SetVisible(pActive);
			_active = pActive;
		}
		
	}

	public void SetVisible (bool pVisible) {
		if (_active && _networkUpdateTimer != null) {
			if (pVisible) {
				_networkUpdateTimer.Start();
			} else {
				_networkUpdateTimer.Stop();
			}
		}
	}
}
