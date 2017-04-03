using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class AbstractModule : MonoBehaviour, IShooterNetworked {
	[SerializeField]
	private Objective[] _objectives;

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

	public static void ProcessPacket (NetworkPacket.Update.Drone pPacket) {
		//TODO Implement
	}


	public void Initialize () {
		foreach (Objective o in _objectives) {
			o.SetVisible(true);
		}

		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Module(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _solved.Value));
	}

	private void sendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Module(Id, _solved.Value));
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	private ObservedValue<bool> _solved;
	public ObservedValue<bool> Solved { get { return _solved; } }

	//TODO Implement discovery
	private bool _discovered;

	protected void solve () {
		foreach (Objective o in _objectives) {
			o.SetVisible(false);
		}

		_solved.Value = true;
		sendUpdate();
	}
}
