using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

[SelectionBase]
public abstract class AbstractModule : MonoBehaviour, IShooterNetworked, IInteractable
{
	[SerializeField]
	protected AbstractObjective[] _objectives;

	private ObservedValue<bool> _solved = new ObservedValue<bool>(false);

	public bool IsSolved {
		get { return _solved.Value; }
	}
	

	public void OnSolved(System.Action pAction) {
		_solved.OnValueChange += pAction;
	}

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

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void Initialize() {
		foreach (AbstractObjective o in _objectives) {
			o.OnSolved(checkSolved);
		}

		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Module(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _solved.Value));
	}



	private void checkSolved() {
		foreach (AbstractObjective o in _objectives) {
			if (!o.IsSolved) {
				return;
			}
		}

		_solved.Value = true;

		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Module(Id, _solved.Value));
	}

	public virtual void Interact() {
		if (!_activated) {
			Debug.Log("Activated: " + gameObject.name);
			_activated = true;

			//TODO Send Activated Package

			foreach (AbstractObjective obj in _objectives) {
				if (!obj.IsSolved) {
					obj.SetActive(true);
				}
			}
		}
	}
}
