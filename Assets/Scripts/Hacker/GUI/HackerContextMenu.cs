using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HackerContextMenu : MonoBehaviour, IPointerExitHandler {
	public static void Display (AbstractMapIcon.ActionData[] pActionData) {
		HackerContextMenu hcm = GameObject.Instantiate(HackerReferenceManager.Instance.ContextMenu, FindObjectOfType<Canvas>().transform).GetComponent<HackerContextMenu>();

		hcm.display(pActionData);
	}

	private static HackerContextMenu _instance;

	private void Awake () {
		if (_instance != null) {
			Destroy(_instance.gameObject);
		}

		_instance = this;
	}

	[SerializeField]
	private float _minContentWidth = 20.0f;

	[SerializeField]
	private float _spacing = 2.5f;

	[SerializeField]
	private float _contentHeight = 20.0f;

	/*
	private List<ContextMenuOptionData> _options = new List<ContextMenuOptionData>();

	private class ContextMenuOptionData {
		public ContextMenuOptionData (string pDisplayName, UnityEvent pCallback) {
			DisplayName = pDisplayName;
			Callback = pCallback;
			ContextOption = null;
		}

		public string DisplayName;
		public UnityEvent Callback;
		public GameObject ContextOption;
	}
	*/

	/*
	private void addOption (string pDisplayName, System.Action pCallback) {
		_options.Add(new ContextMenuOptionData(pDisplayName, pCallback));
	}

	public void Callback (GameObject pSender) {
		_options.Find(x => x.ContextOption == pSender).Callback();
		Hide();
	}
	*/

	public void OnPointerExit (PointerEventData pPointerEventData) {
		hide();
	}

	/*
	public void Display () {
		//TODO What if clicked at a corner?
		Vector2 clickPosCanvas;
		Canvas canvas = FindObjectOfType<Canvas>();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out clickPosCanvas);
		(transform as RectTransform).anchoredPosition = clickPosCanvas;

		updateOptions();
		this.gameObject.SetActive(true);
	}
	*/

	private void hide () {
		//Destroy(this.gameObject);
	}

	private void display (AbstractMapIcon.ActionData[] pActionData) {
		Vector2 clickPosOnCanvas;
		Canvas canvas = GetComponentInParent<Canvas>();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out clickPosOnCanvas);
		(transform as RectTransform).anchoredPosition = clickPosOnCanvas;

		float maxContentWidth = 0.0f;
		int counter = 0;

		List<HackerContextMenuOption> options = new List<HackerContextMenuOption>();

		foreach (AbstractMapIcon.ActionData data in pActionData) {
			HackerContextMenuOption option = Instantiate(HackerReferenceManager.Instance.ContextMenuOption, transform).GetComponent<HackerContextMenuOption>();
			options.Add(option);

			//TODO Improve the HP cost display

			string hpCost = "";

			if (data.HackerPointsCost > 0) {
				hpCost = " " + data.HackerPointsCost + " HP";
			}
			
			option.Initialize(data.DisplayName + hpCost , data.Action, _spacing, counter, _contentHeight);

			option.GetComponent<Button>().onClick.AddListener(() => hide());

			maxContentWidth = (maxContentWidth < (option.GetPreferedWidth() + 2 * _spacing)) ? option.GetPreferedWidth() + 2 * _spacing : maxContentWidth;

			counter++;
		}

		foreach (HackerContextMenuOption opt in options) {
			opt.AdjustWidth(maxContentWidth, _contentHeight);
		}

		(transform as RectTransform).sizeDelta = new Vector2(maxContentWidth, _contentHeight * pActionData.Length);

		this.gameObject.SetActive(true);
	}

	/*
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
	*/
}
