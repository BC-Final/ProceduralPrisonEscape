using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

[SelectionBase]
public class AbstractModule : MonoBehaviour, IShooterNetworked, IInteractable {
	[SerializeField]
	protected Objective[] _objectives;

	private bool _activated = false;

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
			o.Solved.OnValueChange += checkSolved;
		}

		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Module(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _solved.Value));
	}

	private void checkSolved () {
		foreach (Objective o in _objectives) {
			if (!o.Solved.Value) {
				return;
			}
		}

		_solved.Value = true;
		sendUpdate();
	}

	private void sendUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Module(Id, _solved.Value));
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	private ObservedValue<bool> _solved = new ObservedValue<bool>(false);
	public ObservedValue<bool> Solved { get { return _solved; } }

	public virtual void Interact () {
		if (!_activated) {
			_activated = true;

			foreach (Objective obj in _objectives) {
				if (!obj.Solved.Value) {
					obj.ActivateObjective();
				}
			}
		}
	}
}
