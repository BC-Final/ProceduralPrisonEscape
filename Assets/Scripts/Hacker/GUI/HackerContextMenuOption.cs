using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HackerContextMenuOption : MonoBehaviour {
	private UnityEvent _callback;

	private void Start () {
		GetComponent<Button>().onClick.AddListener(() => onClick());
	}

	private void onClick () {
		_callback.Invoke();
	}

	public void Initialize (string pDisplayName, UnityEvent pCallback, float pSpacing, int pPos, float pContentHeight) {
		_callback = pCallback;

		GetComponentInChildren<Text>().text = pDisplayName;

		//This was for spacing around button
		//(transform as RectTransform).anchoredPosition3D = new Vector3(pSpacing, -((pPos + 1) * pSpacing + pPos * contentHeight), -5.0f);
		//(transform as RectTransform).sizeDelta = new Vector2(contentHeight, (transform as RectTransform).sizeDelta.y);

		(transform as RectTransform).anchoredPosition3D = new Vector3(0.0f, -(pPos * pContentHeight), -5.0f);
		(transform as RectTransform).sizeDelta = new Vector2(GetPreferedWidth() + 2 * pSpacing, pContentHeight);
		(GetComponentInChildren<Text>().transform as RectTransform).sizeDelta = new Vector2(GetPreferedWidth(), pContentHeight - 2 * pSpacing);

		transform.localScale = Vector3.one;
	}

	public void AdjustWidth (float pWidth, float pContentHeight) {
		(transform as RectTransform).sizeDelta = new Vector2(pWidth, pContentHeight);
	}

	public float GetPreferedWidth () {
		return GetComponentInChildren<Text>().preferredWidth;
	}

	/*
	public void SetValues (string pDisplayName, HackerContextMenu pMenu) {
		GetComponent<UnityEngine.UI.Text>().text = pDisplayName;
		_menu = pMenu;
	}

	public void OnPointerClick (PointerEventData pData) {
		if (pData.button == PointerEventData.InputButton.Left) {
			_menu.Callback(this.gameObject);
		}
	}
	*/
}
