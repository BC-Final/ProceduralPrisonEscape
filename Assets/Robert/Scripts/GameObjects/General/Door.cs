using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public enum DoorStatus
	{
		Open,
		Closed,
		Locked
	};

	public virtual void ChangeState(Door.DoorStatus status)
	{
		//if(firewall != null && firewall.GetPermission())
		_currentDoorState = status;

	}


	[SerializeField]
	protected FireWall firewall;
	[SerializeField]
	protected KeyCard keycard;
	[SerializeField]
	protected DoorStatus _currentDoorState;
	protected DoorManager _manager;
	public int Id;



	public DoorStatus GetDoorState()
	{
		return _currentDoorState;
	}

	public void SetDoorState(Door.DoorStatus pStatus) {
		_currentDoorState = pStatus;
	}

	public KeyCard GetKeyCard()
	{
		return keycard;
	}
	public FireWall GetFireWall()
	{
		return firewall;
	}

	public void SendDoorUpdate()
	{
		_manager.SendDoorStateUpdate(this);
	}

	public void SetManager(DoorManager manager)
	{
		_manager = manager;
	}

	public static T ParseEnum<T>(string value)
	{
		return (T)Door.DoorStatus.Parse(typeof(T), value, true);
	}

}
