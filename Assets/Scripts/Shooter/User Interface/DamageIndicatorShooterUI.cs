using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageIndicatorShooterUI : MonoBehaviour {
	private GameObject _damageSource;
	private Tweener _fadeTween;
	private DamageIndicatorControllerShooterUI _hitCtrl;
	private Transform _player;
	private Vector2 _uiDirection;
	private RectTransform _trans;

	private void Awake () {
		_hitCtrl = GetComponentInParent<DamageIndicatorControllerShooterUI>();
		_player = FindObjectOfType<PlayerHealth>().transform;
		_trans = GetComponent<RectTransform>();
	}

	private void Update () {
		setPosition();
	}

	private void kill () {
		_hitCtrl.RemoveHitMarker(this.gameObject);
		Destroy(this.gameObject);
	}

	public GameObject GetSource () {
		return _damageSource;
	}

	public void Show (Transform pSource) {
		_damageSource = pSource.gameObject;

		setPosition();

		if (_fadeTween != null) {
			_fadeTween.Restart();
		} else {
			GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
			_fadeTween = GetComponent<UnityEngine.UI.Image>().DOColor(new Color(1, 1, 1, 0), _hitCtrl.ShowDuration).OnKill(() => kill());
		}
	}

	//public Vector2 GetDirection () {
	//	return _uiDirection;
	//}

	private void setPosition () {
		if (_damageSource != null) {
			Vector3 worldDirection = _player.InverseTransformDirection((_damageSource.transform.position - _player.position).normalized);

			_uiDirection = new Vector2(worldDirection.x, worldDirection.z).normalized;

			_trans.anchoredPosition = _uiDirection * _hitCtrl.CenterDistance;

			transform.LookAt(_hitCtrl.transform, Vector3.forward);
			transform.Rotate(-90, 0, 0);
		}
	}
}
