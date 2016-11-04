using UnityEngine;
using System.Collections;

public class MinimapDoor : HackerDoorAsset {

	[SerializeField]
	Sprite openSprite;
	[SerializeField]
	Sprite closedSprite;
	[SerializeField]
	Sprite lockedSprite;

	[SerializeField]
	SpriteRenderer _renderer;

	MinimapManager _manager;

	// Use this for initialization
	public override void Start()
	{
		Debug.Log("Constructer Minimapdoor");
		GetRenderer();
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ChangeState(Door.DoorStatus state)
	{
		GetRenderer();
		switch (state)
		{
		case Door.DoorStatus.Open:
			{
				_renderer.sprite = openSprite;
				break;
			}
		case Door.DoorStatus.Closed:
			{
				if(_mainDoor.GetFireWall() == null)
				{
					_renderer.sprite = closedSprite;
				}else
				{
					_renderer.sprite = lockedSprite;
				}
				break;
			}
		}
	}
	public override void OnMouseClick()
	{
		if(_mainDoor.GetDoorState() == Door.DoorStatus.Closed)
		{
			Debug.Log("Main door firewall : " + _mainDoor.GetFireWall());
			Debug.Log("Main door firewallID : " + _mainDoor.GetFireWall().ID);
			if (!_mainDoor.GetFireWall() is  HackerFireWall || _mainDoor.GetFireWall().GetPermission())
			{
				_mainDoor.ChangeState(Door.DoorStatus.Open);
				_manager.SendDoorUpdate(_mainDoor);
			}
		}
		else if(_mainDoor.GetDoorState() == Door.DoorStatus.Open)
		{
			if (!_mainDoor.GetFireWall() is HackerFireWall || _mainDoor.GetFireWall().GetPermission())
			{
				_mainDoor.ChangeState(Door.DoorStatus.Closed);
				_manager.SendDoorUpdate(_mainDoor);
			}
		}
		base.OnMouseClick();
	}

	public void SetManger( MinimapManager manager)
	{
		_manager = manager;
	}

	private void GetRenderer()
	{
		if(_renderer == null)
		{
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}
	}
}
