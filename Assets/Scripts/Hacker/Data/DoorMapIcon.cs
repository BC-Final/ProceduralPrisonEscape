using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMapIcon : AbstractMapIcon {
	public static void CreateInstance (Vector2 pPos, float rot, DoorState pState) {
        GameObject go = (GameObject)Instantiate(HackerReferenceManager.Instance.DoorIcon, new Vector3(pPos.x / MinimapManager.scale, pPos.y / MinimapManager.scale, 0), Quaternion.Euler(0, rot, 0));
        go.GetComponent<DoorMapIcon>()._currentDoorState = pState;
        go.GetComponent<DoorMapIcon>().StateChanged();
    }

	public enum DoorState {
		Open,
		Closed,
		Locked
	}

    #region Sprites
    [Header("Sprites")]
    [SerializeField]
    private Sprite openSprite;
    [SerializeField]
    private Sprite closedSprite;
    [SerializeField]
    private Sprite lockedSprite;
    #endregion

    private DoorState _currentDoorState;

	public void Toggle () {
		if(_currentDoorState == DoorState.Open)
        {
            _currentDoorState = DoorState.Closed;
        }
        else
        {
            _currentDoorState = DoorState.Open;
        }
        StateChanged();
    }

	public void Lock () {
        _currentDoorState = DoorState.Locked;
        StateChanged();
    }

   	private void StateChanged()
    {
   	
   	   switch (_currentDoorState)
        { 
   		    case DoorState.Open:
                {
                    spriteRenderer.sprite = openSprite;
                    break;
   		        }
   		    case DoorState.Closed:
                {
                    spriteRenderer.sprite = closedSprite;
                    break;
                }
            case DoorState.Locked:
                {
                    spriteRenderer.sprite = lockedSprite;
                    break;
                }
        }
    }
}
