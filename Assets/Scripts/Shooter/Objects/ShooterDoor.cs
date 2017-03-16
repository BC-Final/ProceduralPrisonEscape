using UnityEngine;
using System.Collections;
using StateFramework;
using System.Collections.Generic;
using System.Net.Sockets;
using Gamelogic.Extensions;
using DG.Tweening;
using UnityEngine.AI;

[SelectionBase]
public class ShooterDoor : MonoBehaviour, INetworked {
	/// <summary>
	/// Netowork Identification
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



	/// <summary>
	/// Sends initialization packet to hacker
	/// </summary>
	public void Initialize () {
		//TODO Only initialze when shooter saw object or database
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Door(_id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _open.Value, _locked));
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
	/// Indicated if door is locked
	/// </summary>
	private bool _locked;



	/// <summary>
	/// Registers Network Door reference and add listener to state change
	/// </summary>
	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
		_open.OnValueChange += stateChanged;
	}



	/// <summary>
	/// Removes Network Door Reference
	/// </summary>
	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}



	/// <summary>
	/// Plays the animation to current state
	/// </summary>
	private void stateChanged () {
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
	private void sendStateUpdate () {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Door(_id, _open.Value));
	}



	/// <summary>
	/// Checks if Drone is trying to pass door and opens it if not locked or already open
	/// </summary>
	/// <param name="pOther">Other Collider</param>
	private void OnTriggerEnter (Collider pOther) {
		//TODO Notify drone when door is locked
		if (pOther.GetComponent<DroneEnemy>() != null && !_locked && !_open.Value) {
			sendStateUpdate();
			_open.Value = true;
		}
	}



	/// <summary>
	/// Checks if drone is has passed door and closes it if not locked or already closed
	/// </summary>
	/// <param name="pOther"></param>
	private void OnTriggerExit (Collider pOther) {
		if (pOther.GetComponent<DroneEnemy>() != null && !_locked && _open.Value) {
			sendStateUpdate();
			_open.Value = false;
		}
	}



	/// <summary>
	/// Computes a Door Update Packet to update a corrresponding door
	/// </summary>
	/// <param name="pPacket">The received packet</param>
	public static void UpdateDoor (NetworkPacket.Update.Door pPacket) {
		ShooterDoor door = ShooterPackageSender.Instance.GetNetworkedObject<ShooterDoor>(pPacket.Id);

		if (door != null) {
			door._open.Value = pPacket.Open;
			door._locked = pPacket.Locked;
		} else {
			Debug.LogError("Could not find Door with Id " + pPacket.Id);
		}
	}
}
