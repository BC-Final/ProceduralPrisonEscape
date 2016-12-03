using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
	[SerializeField]
	private Vector2 _direction;

	private RectTransform _rectTransform;

	private void Start() {
		_rectTransform = GetComponent<RectTransform>();
	}

	public void AdjustDistance(float pDistance) {
		_rectTransform.anchoredPosition = _direction * pDistance;
	}
}
