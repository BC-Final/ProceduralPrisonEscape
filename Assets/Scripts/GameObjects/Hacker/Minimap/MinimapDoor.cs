﻿using UnityEngine;
using System.Collections;

public class MinimapDoor : MonoBehaviour {

	#region Sprites
	[Header("Sprites")]
	[SerializeField]
	private Sprite openSprite;
	[SerializeField]
	private Sprite openSpriteLocked;
	[SerializeField]
	private Sprite closedSprite;
	[SerializeField]
	private Sprite closedSpriteLocked;
	#endregion

	#region References
	private SpriteRenderer _renderer;
	private HackerDoor _associatedDoor;
	#endregion


	#region Properties
	/// <summary>
	/// Sets the associated door and subscribes to is state update
	/// </summary>
	public HackerDoor AccociatedDoor {
		set {
			_associatedDoor = value;
			_associatedDoor.State.OnValueChange += () => changedState();
		}
	}
	#endregion



	/// <summary>
	/// Gets called after this object is initialized
	/// </summary>
	public void Start () {
		_renderer = GetComponentInChildren<SpriteRenderer>();
	}



	/// <summary>
	/// Should get called when the associated door changes its state
	/// </summary>
	private void changedState () {
		switch (_associatedDoor.State.Value) {
			case DoorState.Open: {
					//TODO When protected render openSpriteLocked
					_renderer.sprite = openSprite;
					break;
				}
			case DoorState.Closed: {
					//TODO When protected render closedSpriteLocked
					_renderer.sprite = closedSprite;
					break;
				}
		}
	}



	/// <summary>
	/// Gets called when the icon on the minimap is clicked
	/// </summary>
	public void OnMouseClick () {
		//TODO Make this jump to the icon on network view
		if (_associatedDoor.State.Value == DoorState.Open) {
			_associatedDoor.SetState(DoorState.Closed);
		} else if (_associatedDoor.State.Value == DoorState.Closed) {
			_associatedDoor.SetState(DoorState.Open);
		}
	}
}
