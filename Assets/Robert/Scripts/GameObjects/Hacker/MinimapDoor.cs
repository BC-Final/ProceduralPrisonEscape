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
					_renderer.sprite = closedSprite;
					break;
				}
			case Door.DoorStatus.Locked:
				{
					_renderer.sprite = lockedSprite;
					break;
				}
		}
	}
	public override void OnMouseClick()
	{
		if(_mainDoor.GetDoorState() == Door.DoorStatus.Closed)
		{
			Debug.Log("opening door");
			_mainDoor.ChangeState(Door.DoorStatus.Open);
			_manager.SendDoorUpdate(_mainDoor);
			
		}
		else if(_mainDoor.GetDoorState() == Door.DoorStatus.Open)
		{
			Debug.Log("closing door");
			_mainDoor.ChangeState(Door.DoorStatus.Closed);
			_manager.SendDoorUpdate(_mainDoor);
		}
		base.OnMouseClick();
	}

	public void SetManger( MinimapManager manager)
	{
		_manager = manager;
	}

	private void GetRenderer()
	{
		Debug.Log("Getting Renderer");
		if(_renderer == null)
		{
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}
	}
}
