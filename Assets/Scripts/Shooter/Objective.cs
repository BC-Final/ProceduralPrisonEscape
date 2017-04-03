using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, IShooterNetworked {
	[SerializeField]
	private float _networkUpdateRate;
	Timer _networkUpdateTimer;

	private bool _shown;

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
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public static void ProcessPacket (NetworkPacket.Update.Drone pPacket) {
		//TODO Implement
	}

	public void Initialize () {
		
	}

	private void sendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Objective(Id, transform.position.x, transform.position.z));
	}

	public void SetVisible (bool pVisible) {
		if (pVisible) {
			_networkUpdateTimer = TimerManager.CreateTimer("Objective Update", false).SetDuration(_networkUpdateRate).SetLoops(-1).AddCallback(() => sendUpdate()).Start();
		} else {
			_networkUpdateTimer.Stop();
			ShooterPackageSender.SendPackage(new NetworkPacket.Update.Objective(Id, true));
		}
	}
}
