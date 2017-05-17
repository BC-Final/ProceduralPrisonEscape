using UnityEngine;
using System.Collections;
using StateFramework;
using System.Collections.Generic;
using System.Net.Sockets;
using Gamelogic.Extensions;
using DG.Tweening;
using UnityEngine.AI;

[SelectionBase]
public class ShooterDoor : MonoBehaviour, IShooterNetworked {
	/// <summary>
	/// Network Identification
	/// </summary>
	private int _id;



	/// <summary>
	/// Accessor for Network Id
	/// </summary>
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	//A hack to test keypads when no one is connected
	private void Start() {
		_portal = GetComponent<OcclusionPortal>();
		_open.OnValueChange += OnDoorStateChange;
		if (_keypad) {
			_keypad.Doors.Add(this);
		}
	}
	/// <summary>
	/// Sends initialization packet to hacker
	/// </summary>


	/// <summary>
	/// Sends initialization packet to hacker
	/// </summary>
	public void Initialize() {
		//TODO Only initialze when shooter saw object or database
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Door(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _open.Value, _locked));
		if (_keypad) {
			//_keypad.Doors.Add(this);
			ShooterPackageSender.SendPackage(new NetworkPacket.Create.DecodeAddon(Id, _keypad.keyCode));
		}
		if (IsDisabled) {
			ShooterPackageSender.SendPackage(new NetworkPacket.Update.DisableDoor(Id));
		}
		if (_hasDuoButton) {
			ShooterPackageSender.SendPackage(new NetworkPacket.Create.DuoButtonAddon(Id));
		}
		//Keycard should also be managed in the door class for consistency
	}
	public void DisableDoor() {
		IsDisabled = true;
	}

	public void AddDuoButton() {
		_hasDuoButton = true;
	}

	//TODO Replace with animation
	//--------temp--------
	[SerializeField]
	private Transform _leftDoor;

	[SerializeField]
	private Transform _rightDoor;
	//--------temp end--------



	/// <summary>
	/// Indicates if door is open or closed
	/// </summary>
	private ObservedValue<bool> _open = new ObservedValue<bool>(false);



	/// <summary>
	/// Indicated if door is locked, keylocked or disabled
	/// </summary>#
	private bool _hasDuoButton;
	public bool IsDisabled;
	private bool _locked;
	private bool _lockedForDrones;
	private bool _keyLocked;
	[SerializeField]
	private Keypad _keypad;
	private OcclusionPortal _portal;

	/// <summary>
	/// HACK! To be changed!
	/// </summary>
	public void ForceOpen() {
		_open.Value = true;
		sendStateUpdate();
		_lockedForDrones = true;
	}

	public void ForceClose() {
		_open.Value = false;
		sendStateUpdate();
	}

	public void ForceToggle() {
		_open.Value = !_open.Value;
		sendStateUpdate();
	}

	private void OnDoorStateChange() {
		TimerManager.CreateTimer("Portal Change", true).SetDuration(1.0f).AddCallback(() => _portal.open = _open.Value);
	}

	public void SetRequireKeyCard(Color keyColor) {
		if (_keyLocked) {
			Debug.Log("WARNING!!! This door already has a Keycard");
			return;
		}
		_keyLocked = true;
		Sprite keySprite = ShooterReferenceManager.Instance.KeycardIcon;
		foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>()) {
			sprite.sprite = keySprite;
			sprite.color = keyColor;
		}
	}

	/// <summary>
	/// Registers Network Door reference and add listener to state change
	/// </summary>
	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
		_open.OnValueChange += stateChanged;
	}



	/// <summary>
	/// Removes Network Door Reference
	/// </summary>
	private void OnDestroy() {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}



	/// <summary>
	/// Plays the animation to current state
	/// </summary>
	private void stateChanged() {
		//TODO Also change nav mesh obstacle

		if (_open.Value) {
			//--------temp--------
			_rightDoor.DOLocalMove(new Vector3(1.25f, 1.25f, 0.0f), 1.0f);
			_leftDoor.DOLocalMove(new Vector3(-1.25f, 1.25f, 0.0f), 1.0f);

			_rightDoor.DOScale(new Vector3(0.1f, 2.5f, 0.5f), 1.0f);
			_leftDoor.DOScale(new Vector3(0.1f, 2.5f, 0.5f), 1.0f);
			//--------temp end--------
		} else {
			//--------temp--------
			_rightDoor.DOLocalMove(new Vector3(0.625f, 1.25f, 0.0f), 1.0f);
			_leftDoor.DOLocalMove(new Vector3(-0.625f, 1.25f, 0.0f), 1.0f);

			_rightDoor.DOScale(new Vector3(1.25f, 2.5f, 0.5f), 1.0f);
			_leftDoor.DOScale(new Vector3(1.25f, 2.5f, 0.5f), 1.0f);
			//--------temp end--------
		}
	}



	/// <summary>
	/// Sends the current state to the hacker
	/// </summary>
	private void sendStateUpdate() {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Door(Id, _open.Value));
	}



	/// <summary>
	/// Checks if Drone is trying to pass door and opens it if not locked or already open
	/// </summary>
	/// <param name="pOther">Other Collider</param>
	private void OnTriggerStay(Collider pOther) {
		//TODO Notify drone when door is locked
		if (pOther.GetComponentInParent<Inventory>()) {
			if (pOther.GetComponentInParent<Inventory>().Contains(this)) {
				_open.Value = true;
				sendStateUpdate();
			}
		}

		if (pOther.GetComponentInParent<DroneEnemy>() != null && !_locked && !_open.Value && !_lockedForDrones) {
			_open.Value = true;
			sendStateUpdate();
			//Debug.Log("Drone opened door");
		}
	}



	/// <summary>
	/// Checks if drone is has passed door and closes it if not locked or already closed
	/// </summary>
	/// <param name="pOther"></param>
	private void OnTriggerExit(Collider pOther) {
		if (pOther.GetComponentInParent<Inventory>()) {
			if (pOther.GetComponentInParent<Inventory>().Contains(this)) {
				_open.Value = false;
				sendStateUpdate();
			}
		}
		//Debug.Log("Trigger exit");
		if (pOther.GetComponentInParent<DroneEnemy>() != null && !_locked && _open.Value && !_lockedForDrones) {
			_open.Value = false;
			sendStateUpdate();
			//Debug.Log("Drone closed door");
		}
	}



	/// <summary>
	/// Computes a Door Update Packet to update a corrresponding door
	/// </summary>
	/// <param name="pPacket">The received packet</param>
	public static void ProcessPacket(NetworkPacket.Update.Door pPacket) {
		ShooterDoor door = ShooterPackageSender.GetNetworkedObject<ShooterDoor>(pPacket.Id);

		if (door != null) {
			door._open.Value = pPacket.Open;
			door._locked = pPacket.Locked;
		} else {
			//Debug.LogError("Could not find Door with Id " + pPacket.Id);
		}
	}
}
