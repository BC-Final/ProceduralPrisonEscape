using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitDirection : MonoBehaviour {
	[SerializeField]
	private Transform _hitMarker;

	[SerializeField]
	private float _showDuration;

	[SerializeField]
	private float _centerDistance = 600.0f;

	private Tweener _fadeTween;

	//TODO Move hitmarker to always point at source
	//TODO Show multiple hitmarkers when multiple sources are inflicting damage

	public void ShowHitMarker (Vector3 pRelativeDamageDirection) {
		pRelativeDamageDirection.y = .0f;
		pRelativeDamageDirection.Normalize();

		Vector2 dir = new Vector2(pRelativeDamageDirection.x, pRelativeDamageDirection.z);
		_hitMarker.GetComponent<RectTransform>().anchoredPosition = dir * _centerDistance;
		_hitMarker.LookAt(transform, Vector3.forward);
		_hitMarker.Rotate(-90, 0, 0);

		_hitMarker.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);

		if (_fadeTween != null) {
			_fadeTween.Kill();
		}

		_fadeTween = _hitMarker.GetComponent<UnityEngine.UI.Image>().DOColor(new Color(1, 1, 1, 0), _showDuration);
	}
}
