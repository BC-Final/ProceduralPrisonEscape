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
	private void GetRenderer()
	{
		Debug.Log("Getting Renderer");
		if(_renderer == null)
		{
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}
	}
}
