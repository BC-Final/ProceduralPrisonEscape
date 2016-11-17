using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public enum DoorStatus
	{
		Open,
		Closed,
	};

	


	[SerializeField]
	protected Firewall _firewall;
	[SerializeField]
	protected KeyCard keycard;
	[SerializeField]
	protected DoorStatus _currentDoorState;
	protected DoorManager _manager;
	public int Id;

	public virtual void Start()
	{
		if (_firewall != null)
		{
			_firewall.AddDoor(this);
		}
	}

	public virtual void ChangeState(Door.DoorStatus status)
	{
		if (_firewall == null || _firewall.GetPermission())
		{
			_currentDoorState = status;
		}
		else
		{
			Debug.Log("Access Denied");
		}
	}

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
	public Firewall GetFireWall()
	{
		return _firewall;
	}
	public void SetFireWall(Firewall firewall)
	{
		_firewall = firewall;
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
