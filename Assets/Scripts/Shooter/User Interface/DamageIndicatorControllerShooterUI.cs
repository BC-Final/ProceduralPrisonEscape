using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class DamageIndicatorControllerShooterUI : MonoBehaviour {
	[SerializeField]
	public float ShowDuration = 1.5f;

	[SerializeField]
	public float CenterDistance = 600.0f;

	[SerializeField]
	private int _maxHitMarkers = 4;

	[SerializeField]
	private float _minAngle = 20.0f;


	private Transform _player;

	private List<DamageIndicatorShooterUI> _markers = new List<DamageIndicatorShooterUI>();

	private void Start () {
		_player = FindObjectOfType<PlayerHealth>().transform;
	}

	public void RemoveHitMarker (GameObject pMarker) {
		_markers.RemoveAll(x => x.gameObject == pMarker);
	}

	public void ShowHitMarker (Transform pSender) {
		///float angle = 0;

		DamageIndicatorShooterUI nearest = _markers.Find(x => x.GetSource() == pSender.gameObject);

		if (nearest != null) {
			nearest.Show(pSender);
			return;
		}

		//nearest = GetNearestHitMarker(GetDirection(pSender), out angle);

		//if (angle <= _minAngle || _markers.Count == _maxHitMarkers) {
		//	nearest.Show(pSender);
		//} else {
			_markers.Add(Instantiate(ShooterReferenceManager.Instance.DamageIndicator, transform).GetComponent<DamageIndicatorShooterUI>());
			_markers.Last().Show(pSender);
		//}
	}

	//private HitMarker GetNearestHitMarker (Vector2 pDirection, out float pAngle) {
	//	HitMarker result = null;
	//	pAngle = Mathf.Infinity;

	//	foreach (HitMarker marker in _markers) {
	//		float currAngle = Vector2.Angle(pDirection, marker.GetDirection());

	//		if (currAngle < pAngle) {
	//			result = marker;
	//			pAngle = currAngle;
	//		}
	//	}

	//	return result;
	//}

	//private Vector2 GetDirection (Transform pSender) {
	//	Vector3 worldDirection = _player.InverseTransformDirection(pSender.position - _player.position);

	//	return new Vector2(worldDirection.x, worldDirection.z).normalized;
	//}
}
