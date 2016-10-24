using UnityEngine;
using System.Collections;

public class ShooterDoor : Door {

	//Prototype
	Collider _boxCollider;
	Renderer _renderer;
	//

	void Start () {

		_boxCollider = GetComponent<BoxCollider>();
		_renderer = GetComponent<Renderer>();
		if(GetDoorState() == DoorStatus.Open)
		{
			_boxCollider.enabled = false;
			_renderer.enabled = false;
		}
	}

	public override void ChangeState(DoorStatus status)
	{
		base.ChangeState(status);
		if(status == DoorStatus.Open)
		{
			_boxCollider.enabled = false;
			_renderer.enabled = false;
		}
		else if(status == DoorStatus.Closed)
		{
			_boxCollider.enabled = true;
			_renderer.enabled = true;
		}
	}

	//Example

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (_currentDoorState == DoorStatus.Open)
			{
				ChangeState(DoorStatus.Closed);
			}
			else if (_currentDoorState == DoorStatus.Closed)
			{
				ChangeState(DoorStatus.Open);
			}
			OnUse();
		}
	}

	public void OnUse()
	{
		SendDoorUpdate();
	}
	//
}
