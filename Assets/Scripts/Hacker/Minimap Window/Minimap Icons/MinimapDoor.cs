using UnityEngine;
using System.Collections;

public class MinimapDoor : MonoBehaviour {

	#region Sprites
	[Header("Sprites")]
	[SerializeField]
	private Sprite neutralOpenSprite;
	[SerializeField]
	private Sprite neutralClosedSprite;
	[SerializeField]
	private Sprite lockedOpenSprite;
	[SerializeField]
	private Sprite lockedClosedSprite;
	[SerializeField]
	private Sprite hackedOpenSprite;
	[SerializeField]
	private Sprite hackedClosedSprite;

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
			_associatedDoor.Accessible.OnValueChange += () => changedState();
			changedState();
		}
	}



	/// <summary>
	/// Lazily initializes renderer. This is necessary because sometimes its not initialized on start
	/// </summary>
	new private SpriteRenderer renderer {
		get {
			if (_renderer == null) {
				_renderer = GetComponentInChildren<SpriteRenderer>();
			}

			return _renderer;
		}
	}
	#endregion



	/// <summary>
	/// Gets called during this objects initialization
	/// </summary>
	public void Awake () {
		_renderer = GetComponentInChildren<SpriteRenderer>();
	}



	/// <summary>
	/// Should get called when the associated door changes its state
	/// </summary>
	private void changedState () {
		switch (_associatedDoor.State.Value) {
			case DoorState.Open: {
					if (!_associatedDoor.Accessible.Value) {
						renderer.sprite = lockedOpenSprite;
					} else if(_associatedDoor.Hacked){
						renderer.sprite = hackedOpenSprite;
					}else {
						renderer.sprite = neutralOpenSprite;
                    }
					break;
				}
			case DoorState.Closed: {
					if (!_associatedDoor.Accessible.Value) {
						renderer.sprite = lockedClosedSprite;
					} else if (_associatedDoor.Hacked) {
						renderer.sprite = hackedClosedSprite;
					}else {
						renderer.sprite = neutralClosedSprite;
					}
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
