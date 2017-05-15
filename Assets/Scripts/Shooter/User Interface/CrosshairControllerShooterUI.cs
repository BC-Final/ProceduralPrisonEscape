using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CrosshairControllerShooterUI : MonoBehaviour {
	private List<CrosshairShooterUI> _crosshairs;
	private RectTransform _canvas;
	private WeaponHolder _holder;

	private void Start() {
		_canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
		_crosshairs = new List<CrosshairShooterUI>(GetComponentsInChildren<CrosshairShooterUI>());
		_holder = FindObjectOfType<WeaponHolder>();
	}

	public void Enable(float pTime) {
		_crosshairs.ForEach(x => x.Enable(pTime));
	}

	public void Disable (float pTime) {
		_crosshairs.ForEach(x => x.Disable(pTime));
	}

	private void Update() {
		setDistance(_holder.CurrentWeapon.CalcFinalSpreadConeRadius(), _holder.CurrentWeapon.SpreadConeLength);
	}

	private void setDistance(float pConeRadius, float pConeLength) {
		Vector3 worldPosition = (Camera.main.transform.position + Camera.main.transform.forward * pConeLength) + Camera.main.transform.up * pConeRadius;
		Vector2 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);
		Vector2 canvasPosition = new Vector2(((screenPosition.x * _canvas.sizeDelta.x) - (_canvas.sizeDelta.x * 0.5f)), ((screenPosition.y * _canvas.sizeDelta.y) - (_canvas.sizeDelta.y * 0.5f)));
		float canvasDistance = canvasPosition.magnitude;

		foreach (CrosshairShooterUI c in _crosshairs) {
			c.AdjustDistance(canvasDistance);
		}
	}
}
