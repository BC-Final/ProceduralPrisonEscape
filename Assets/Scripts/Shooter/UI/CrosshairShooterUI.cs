using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CrosshairShooterUI : MonoBehaviour {
	[SerializeField]
	private Vector2 _direction;

	private Tweener _alphaTween;
	private RectTransform _rectTransform;
	private Image _image;

	private void Start() {
		_rectTransform = GetComponent<RectTransform>();
		_image = GetComponent<Image>();
	}

	public void AdjustDistance(float pDistance) {
		_rectTransform.anchoredPosition = _direction * pDistance;
	}

	public void Enable (float pTime) {
		_alphaTween.Kill();
		_alphaTween = _image.DOFade(1f, pTime);
	}

	public void Disable (float pTime) {
		_alphaTween.Kill();
		_alphaTween = _image.DOFade(0f, pTime);
	}
}
