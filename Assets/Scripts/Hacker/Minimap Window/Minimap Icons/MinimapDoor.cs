using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class MinimapDoor : MonoBehaviour {

	#region Sprites
	[Header("Sprites")]
	[SerializeField]
	private Sprite openSprite;
	[SerializeField]
	private Sprite closedSprite;
	          
    #endregion

    #region DoorColors
    [SerializeField]
    Color neutralColor;
    [SerializeField]
    Color lockedColor;
    [SerializeField]
    Color hackedColor;
    #endregion

    #region References
    private SpriteRenderer _renderer;
	private HackerDoor _associatedDoor;
	#endregion



	#region Properties
	/// <summary>
	/// Sets the associated door and subscribes to is state update
	/// </summary>
	public HackerDoor AssociatedDoor {
		set {
			_associatedDoor = value;
			_associatedDoor.State.OnValueChange += () => changedState();
			_associatedDoor.Accessible.OnValueChange += () => changedState();
			_associatedDoor.Hacked.OnValueChange += () => changedState();
			changedState();
		}
	}



	/// <summary>
	/// Lazily initializes renderer. This is necessary because sometimes its not initialized on start
	/// </summary>
	private SpriteRenderer spriteRenderer {
		get {
			if (_renderer == null) {
				_renderer = GetComponentInChildren<SpriteRenderer>();
			}

			return _renderer;
		}
	}
    #endregion

    private bool _keyProtected = false;

	/// <summary>
	/// Gets called during this objects initialization
	/// </summary>
	public void Awake () {
		_renderer = GetComponentInChildren<SpriteRenderer>();
	}

    public void SetKeyColor(Color color)
    {
        spriteRenderer.color = color;
        _keyProtected = true;
    }

    private void SetColor(Color color)
    {
        if (!_keyProtected)
        {
            spriteRenderer.color = color;
        }
    }

	/// <summary>
	/// Should get called when the associated door changes its state
	/// </summary>
	private void changedState () {
		switch (_associatedDoor.State.Value) {
			case DoorState.Open: {
                    spriteRenderer.sprite = openSprite;
                    if (!_associatedDoor.Accessible.Value) {
                        SetColor(lockedColor);
					} else if(_associatedDoor.Hacked.Value){
                        SetColor(hackedColor);
					}else {
                        SetColor(neutralColor);
                    }
					break;
				}
			case DoorState.Closed: {
                    spriteRenderer.sprite = closedSprite;
                    if (!_associatedDoor.Accessible.Value) {
                        SetColor(lockedColor);
					} else if (_associatedDoor.Hacked.Value) {
                        SetColor(hackedColor);
                    }
                    else {
                        SetColor(neutralColor);
					}
					break;
				}
		}
	}

	void OnMouseOver () {
		if (Input.GetMouseButtonDown(1)) {
			FindObjectOfType<CameraMove>().MoveTo(_associatedDoor.DoorNode.transform.position.x, _associatedDoor.DoorNode.transform.position.y);
			FindObjectOfType<NetworkSelectionHighlight>().IndicateSelection(_associatedDoor.DoorNode.GetComponent<RectTransform>().anchoredPosition);
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
