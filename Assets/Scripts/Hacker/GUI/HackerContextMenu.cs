using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HackerContextMenu : MonoBehaviour, IPointerExitHandler {
	private static HackerContextMenu _instance;

	private void Awake () {
		if (_instance != null) {
			Destroy(_instance.gameObject);
		}

		_instance = this;
	}

	[SerializeField]
	private float _minWidth = 20.0f;

	[SerializeField]
	private float _spacing = 2.5f;

	[SerializeField]
	private float _textHeight = 20.0f;

	private List<ContextMenuOptionData> _options = new List<ContextMenuOptionData>();

	private class ContextMenuOptionData {
		public ContextMenuOptionData (string pDisplayName, System.Action pCallback) {
			DisplayName = pDisplayName;
			Callback = pCallback;
			ContextOption = null;
		}

		public string DisplayName;
		public System.Action Callback;
		public GameObject ContextOption;
	}

	public void AddOption (string pDisplayName, System.Action pCallback) {
		_options.Add(new ContextMenuOptionData(pDisplayName, pCallback));
	}

	public void Callback (GameObject pSender) {
		_options.Find(x => x.ContextOption == pSender).Callback();
		Hide();
	}

	public void OnPointerExit (PointerEventData pData) {
		Hide();
	}

	public void Display () {
		//TODO What if clicked at a corner?
		Vector2 clickPosCanvas;
		Canvas canvas = FindObjectOfType<Canvas>();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out clickPosCanvas);
		(transform as RectTransform).anchoredPosition = clickPosCanvas;

		updateOptions();
		this.gameObject.SetActive(true);
	}

	public void Hide () {
		Destroy(this.gameObject);
	}

	private void updateOptions () {
		foreach (Transform t in transform.GetComponentInChildren<Transform>()) {
			Destroy(t.gameObject);
		}

		float maxWidth = 0.0f;

		for(int i = 0; i < _options.Count; ++i) {
			GameObject g = Instantiate(Resources.Load("pfb_context_option"), transform as RectTransform) as GameObject;
			_options[i].ContextOption = g;
			(g.transform as RectTransform).anchoredPosition3D = new Vector3(_spacing, - ((i + 1) * _spacing + i * _textHeight), -5.0f);
			g.GetComponent<HackerContextMenuOption>().SetValues(_options[i].DisplayName, this);
			g.GetComponent<RectTransform>().sizeDelta = new Vector2(_textHeight, g.GetComponent<RectTransform>().sizeDelta.y);
			g.transform.localScale = Vector3.one;

			if (g.transform.GetComponent<Text>().preferredWidth > maxWidth) {
				maxWidth = g.transform.GetComponent<Text>().preferredWidth;
			}
		}

		(transform as RectTransform).sizeDelta = new Vector2(Mathf.Max(_minWidth, maxWidth + 2 * _spacing), _textHeight * _options.Count + (_options.Count+1) * _spacing);
	}
}
