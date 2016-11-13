using UnityEngine;
using System.Collections;

public class HackerDoorAsset : MonoBehaviour {

	protected HackerDoor _mainDoor;
	private Door.DoorStatus _doorStatus;

	public virtual void Start()
	{

	}

	public void SetMainDoor(HackerDoor door)
	{
		_mainDoor = door;
	}
	public void SetState(Door.DoorStatus state)
	{
		_doorStatus = state;
		OnStateChange();
	}
	protected void OnStateChange()
	{

	}
	public virtual void OnMouseClick()
	{

	}
}
