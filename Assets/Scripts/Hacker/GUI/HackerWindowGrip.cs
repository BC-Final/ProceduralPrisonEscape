using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HackerWindowGrip : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	private Vector2 _clickPosOnWindow;
	private RectTransform _parentTrans;
	private Canvas _canvas;
	private RectTransform _windowContainer;

	private bool _clicked;

	private void Start () {
		_parentTrans = transform.parent.GetComponent<RectTransform>();
		_windowContainer = _parentTrans.parent.GetComponent<RectTransform>();
		_canvas = GetComponentInParent<Canvas>();
	}

	//TODO Might wanna put this into OnGUI
	private void Update () {
		if (_clicked) {
			Vector2 clickPosWindowContainer;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_windowContainer.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out clickPosWindowContainer);
			_parentTrans.anchoredPosition = clickPosWindowContainer;
			_parentTrans.anchoredPosition = _parentTrans.anchoredPosition - _clickPosOnWindow;
		}
	}

	public void OnPointerDown (PointerEventData pData) {
		if (pData.button == PointerEventData.InputButton.Left) {
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentTrans.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out _clickPosOnWindow);
			_clicked = true;
		}
	}

	public void OnPointerUp (PointerEventData pData) {
		if (pData.button == PointerEventData.InputButton.Left) {
			_clicked = false;
		}
	}
}
