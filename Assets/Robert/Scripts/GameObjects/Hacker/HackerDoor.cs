using UnityEngine;
using System.Collections;

public class HackerDoor : Door {

	NetworkViewDoor _networkviewDoor;
	MinimapDoor _minimapDoor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ChangeState(DoorStatus status)
	{
		Debug.Log("Changing to : " + status.ToString());
		base.ChangeState(status);
		_minimapDoor.ChangeState(status);
	}

	public void SetMinimapDoor(MinimapDoor door)
	{
		_minimapDoor = door;
	}

	public void GoToMinimapDoor()
	{

	}

	public void GoToNetworkViewDoor()
	{

	}
}
