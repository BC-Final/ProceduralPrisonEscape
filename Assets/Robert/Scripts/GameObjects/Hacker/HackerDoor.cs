using UnityEngine;
using System.Collections;

public class HackerDoor : Door {

	NetworkViewDoor _networkviewDoor;
	MinimapDoor _minimapDoor;

	public override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public override void ChangeState(DoorStatus status)
	{
		if (firewall.GetPermission())
		{
			base.ChangeState(status);
			_minimapDoor.ChangeState(status);
		}
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
