using UnityEngine;
using System.Collections;

public class MinimapDoor : HackerDoorAsset {

	[SerializeField]
	Sprite openSprite;
	[SerializeField]
	Sprite openSpriteLocked;
	[SerializeField]
	Sprite closedSprite;
	[SerializeField]
	Sprite closedSpriteLocked;

	[SerializeField]
	SpriteRenderer _renderer;

	MinimapManager _manager;

	// Use this for initialization
	public override void Start()
	{
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
				if (_mainDoor.GetFirewall() != null && !_mainDoor.GetFirewall().GetPermission())
				{
					_renderer.sprite = openSpriteLocked;
				}
				else
				{
					_renderer.sprite = openSprite;
				}
				break;
			}
		case Door.DoorStatus.Closed:
			{
				if(_mainDoor.GetFirewall() != null && !_mainDoor.GetFirewall().GetPermission())
				{
					_renderer.sprite = closedSpriteLocked;
				}
				else
				{
					_renderer.sprite = closedSprite;
				}
				break;
			}
		}
	}
	public override void OnMouseClick()
	{
		if(_mainDoor.GetDoorState() == Door.DoorStatus.Open)
		{
			_mainDoor.ChangeState(Door.DoorStatus.Closed);
        }
		else if (_mainDoor.GetDoorState() == Door.DoorStatus.Closed)
		{
			_mainDoor.ChangeState(Door.DoorStatus.Open);
		}
	}

	private void GetRenderer()
	{
		if(_renderer == null)
		{
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}
	}
}
