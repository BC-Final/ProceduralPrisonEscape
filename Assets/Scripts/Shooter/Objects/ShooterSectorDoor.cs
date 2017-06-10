using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

[SelectionBase]
public class ShooterSectorDoor : MonoBehaviour, IShooterNetworked {
	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	private List<AbstractModule> _modules;

	private bool allModulesSolved () {
		bool value = true;

		_modules.ForEach(x => value = value && x.IsSolved);

		return value;
	}

	private Animator _animator;
	private ObservedValue<bool> _open = new ObservedValue<bool>(false);

	private void Start () {
		_animator = GetComponentInChildren<Animator>();
		_open.OnValueChange += () => _animator.SetTrigger("Toggle");

		_modules = new List<AbstractModule>(FindObjectsOfType<AbstractModule>());
		_modules.ForEach(x => x.OnSolved(sendStateUpdate));
	}

	public void Initialize () {
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.SectorDoor.Creation(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _open.Value, !allModulesSolved()));
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	private void sendStateUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.SectorDoor.sUpdate(Id, _open.Value, !allModulesSolved()));
	}

	public static void ProcessPacket (NetworkPacket.GameObjects.SectorDoor.hUpdate pPacket) {
		ShooterSectorDoor door = ShooterPackageSender.GetNetworkedObject<ShooterSectorDoor>(pPacket.Id);

		if (door != null) {
			if(door.allModulesSolved()) {
				door._open.Value = pPacket.Open;
			}
		} else {
			Debug.LogError("Could not find Sector Door with Id " + pPacket.Id);
		}
	}
}
