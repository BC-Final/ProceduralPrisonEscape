//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//public class DynamicContentSize : MonoBehaviour {
//	private RectTransform _transform;
//	private Text _text;
//	private RectTransform _textTransform;

//	private void Start() {
//		_transform = GetComponent<RectTransform>();
//		_text = GetComponentInChildren<Text>();
//		_textTransform = _text.GetComponent<RectTransform>();
//	}

//	private void Update() {
//		//Debug.Log(_transform.sizeDelta);
//	}

//	private void OnGUI() {
//		_transform.sizeDelta = new Vector2(0.0f, _text.preferredHeight + _textTransform.offsetMin.y + Mathf.Abs(_textTransform.offsetMax.y));
//	}
//}