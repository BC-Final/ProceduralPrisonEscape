using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

[SelectionBase]
public abstract class AbstractModule : MonoBehaviour, IShooterNetworked, IInteractable
{
	[SerializeField]
	protected AbstractObjective _objective;

	private ObservedValue<bool> _solved = new ObservedValue<bool>(false);

	public bool IsSolved {
		get { return _solved.Value; }
	}
	

	public void OnSolved(System.Action pAction) {
		_solved.OnValueChange += pAction;
	}

	private bool _activated = false;

	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void Initialize() {
		_objective.OnSolved(checkSolved);

		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Module.Creation(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _solved.Value));
	}



	private void checkSolved() {
		if (!_objective.IsSolved) {
			return;
		}

		_solved.Value = true;

		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Module.sUpdate(Id, _solved.Value));
	}

	public virtual void Interact() {
		if (!_activated) {
			_activated = true;

			//TODO Send Activated Package

			if (!_objective.IsSolved) {
				_objective.SetActive(true);
			}
		}
	}
}
