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
	public void ChangeState(Door.DoorState state)
	{
		GetRenderer();
		switch (state)
		{
		case Door.DoorState.Open:
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
		case Door.DoorState.Closed:
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
		if(_mainDoor.GetDoorState() == Door.DoorState.Open)
		{
			_mainDoor.ChangeState(Door.DoorState.Closed);
        }
		else if (_mainDoor.GetDoorState() == Door.DoorState.Closed)
		{
			_mainDoor.ChangeState(Door.DoorState.Open);
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
