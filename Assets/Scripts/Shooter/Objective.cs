using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

[SelectionBase]
public class Objective : MonoBehaviour, IShooterNetworked {
	[SerializeField]
	private float _networkUpdateRate;

	Timer _networkUpdateTimer;

	private bool _shown;

	private ObservedValue<bool> _solved = new ObservedValue<bool>(false);
	public ObservedValue<bool> Solved { get { return _solved; } }

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
		Solved.OnValueChange += solveChanged;
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public static void ProcessPacket (NetworkPacket.Update.Drone pPacket) {
		//TODO Implement
	}

	public void Initialize () {

	}

	private void solveChanged () {
		SetVisible(!_solved.Value);

		if (_networkUpdateTimer != null) {
			//TODO This is a weird edge case
			ShooterPackageSender.SendPackage(new NetworkPacket.Update.Objective(Id, Solved.Value));
		}
	}

	private void sendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Objective(Id, transform.position.x, transform.position.z));
	}

	public void SetVisible (bool pVisible) {
		if (pVisible) {
			_networkUpdateTimer = TimerManager.CreateTimer("Objective Update", false).SetDuration(_networkUpdateRate).SetLoops(-1).AddCallback(() => sendUpdate()).Start();
		} else {
			if (_networkUpdateTimer != null) {
				_networkUpdateTimer.Stop();
			}
		}
	}
}
