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
		currentDoorStatus = status;
	}

	[SerializeField]
	protected DoorStatus currentDoorStatus;
	public int Id;

	public DoorStatus GetDoorState()
	{
		return currentDoorStatus;
	}

	public static T ParseEnum<T>(string value)
	{
		return (T)Door.DoorStatus.Parse(typeof(T), value, true);
	}

}
