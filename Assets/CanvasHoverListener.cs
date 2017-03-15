using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using UnityEngine.EventSystems;

public class CanvasHoverListener : Singleton<CanvasHoverListener> {
	public bool MouseOverUI { get; private set; }

	private void Update () {
		MouseOverUI = EventSystem.current.IsPointerOverGameObject();
	}
}
