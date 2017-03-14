using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HackerContextMenuOption : MonoBehaviour, IPointerClickHandler {
	private HackerContextMenu _menu;

	public void SetValues (string pDisplayName, HackerContextMenu pMenu) {
		GetComponent<UnityEngine.UI.Text>().text = pDisplayName;
		_menu = pMenu;
	}

	public void OnPointerClick (PointerEventData pData) {
		if (pData.button == PointerEventData.InputButton.Left) {
			_menu.Callback(this.gameObject);
		}
	}

	//TODO Highlight the text if cursor is over it
}
