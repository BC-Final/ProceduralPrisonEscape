﻿//using UnityEngine;
//using System.Collections;
//using Gamelogic.Extensions;

//public class MinimapDoor : MonoBehaviour {

//	#region Sprites
//	[Header("Sprites")]
//	[SerializeField]
//	private Sprite openSprite;
//	[SerializeField]
//	private Sprite closedSprite;
	          
//    #endregion

//    #region DoorColors
//    [SerializeField]
//    Color neutralColor;
//    [SerializeField]
//    Color lockedColor;
//    [SerializeField]
//    Color hackedColor;
//    #endregion

//    //#region References
//    private SpriteRenderer _renderer;
//	#endregion



//	#region Properties



//	/// <summary>
//	/// Lazily initializes renderer. This is necessary because sometimes its not initialized on start
//	/// </summary>
//	private SpriteRenderer spriteRenderer {
//		get {
//			if (_renderer == null) {
//				_renderer = GetComponentInChildren<SpriteRenderer>();
//			}

//			return _renderer;
//		}
//	}
//    #endregion

//    private bool _keyProtected = false;

//	/// <summary>
//	/// Gets called during this objects initialization
//	/// </summary>
//	public void Awake () {
//		_renderer = GetComponentInChildren<SpriteRenderer>();
//	}

//    public void SetKeyColor(Color color)
//    {
//        spriteRenderer.color = color;
//        _keyProtected = true;
//    }

//    private void SetColor(Color color)
//    {
//        if (!_keyProtected)
//        {
//            spriteRenderer.color = color;
//        }
//    }

//	/// <summary>
//	/// Should get called when the associated door changes its state
//	/// </summary>
//	private void changedState () {
//		//TODO REPLACE
//		//switch (_associatedDoor.State.Value) {
//		//	case DoorState.Open: {
//  //                  spriteRenderer.sprite = openSprite;
//  //                  //if (!_associatedDoor.Accessible.Value) {
//  //                  //    SetColor(lockedColor);
//		//			if(_associatedDoor.Hacked.Value){
//  //                      SetColor(hackedColor);
//		//			}else {
//  //                      SetColor(neutralColor);
//  //                  }
//		//			break;
//		//		}
//		//	case DoorState.Closed: {
//  //                  spriteRenderer.sprite = closedSprite;
//  //                  //if (!_associatedDoor.Accessible.Value) {
//  //                  //    SetColor(lockedColor);
//		//			if (_associatedDoor.Hacked.Value) {
//  //                      SetColor(hackedColor);
//  //                  }
//  //                  else {
//  //                      SetColor(neutralColor);
//		//			}
//		//			break;
//		//		}
//		//}
//	}

//	void OnMouseOver () {
//		if (Input.GetMouseButtonDown(1)) {
//			//FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_door_open").start();
//            //FindObjectOfType<CameraMove>().MoveTo(_associatedDoor.DoorNode.transform.position.x, _associatedDoor.DoorNode.transform.position.y);
//            //FindObjectOfType<NetworkSelectionHighlight>().IndicateSelection(_associatedDoor.DoorNode.GetComponent<RectTransform>().anchoredPosition);
//            if(!_keyProtected)
//            //_associatedDoor.Hacked.Value = true;
//            changedState();
//		}
//	}


//	/// <summary>
//	/// Gets called when the icon on the minimap is clicked
//	/// </summary>
//	public void OnMouseClick () {
//		//TODO Make this jump to the icon on network view
//		//if (_associatedDoor.State.Value == DoorState.Open) {
//		//	_associatedDoor.SetState(DoorState.Closed);
//		//} else if (_associatedDoor.State.Value == DoorState.Closed) {
//		//	_associatedDoor.SetState(DoorState.Open);
//		//}
//	}
//}
