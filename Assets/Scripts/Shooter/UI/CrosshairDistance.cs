using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CrosshairDistance : MonoBehaviour {
	private List<Crosshair> _crosshairs;
	private RectTransform _canvas;

	private void Start() {
		_canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
		_crosshairs = new List<Crosshair>(GetComponentsInChildren<Crosshair>());
	}

	public void Enable() { 
		_crosshairs.ForEach(x => x.GetComponent<Image>().enabled = true);
	}

	public void Disable () {
		_crosshairs.ForEach(x => x.GetComponent<Image>().enabled = false);
	}

	public void SetDistance(float pConeRadius, float pConeLength) {
		Vector3 worldPosition = (Camera.main.transform.position + Camera.main.transform.forward * pConeLength) + Camera.main.transform.up * pConeRadius;
		Vector2 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);
		Vector2 canvasPosition = new Vector2(((screenPosition.x * _canvas.sizeDelta.x) - (_canvas.sizeDelta.x * 0.5f)), ((screenPosition.y * _canvas.sizeDelta.y) - (_canvas.sizeDelta.y * 0.5f)));
		float canvasDistance = canvasPosition.magnitude;

		foreach (Crosshair c in _crosshairs) {
			c.AdjustDistance(canvasDistance);
		}
	}
}
