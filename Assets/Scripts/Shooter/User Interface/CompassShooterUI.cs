using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassShooterUI : MonoBehaviour {
	[SerializeField]
	private float _width = 400.0f;

	[SerializeField]
	private float _height = 100.0f;

	[SerializeField]
	private RectTransform _north;

	[SerializeField]
	private RectTransform _south;

	[SerializeField]
	private RectTransform _west;

	[SerializeField]
	private RectTransform _east;

	private Transform _player;

	private Color _black = new Color(0.0f, 0.0f, 0.0f, 1.0f);
	private Color _grey = new Color(0.25f, 0.25f, 0.25f, 1.0f);
	private Color _greyFaded = new Color(0.25f, 0.25f, 0.25f, 0.50f);

	private void Start() {
		_player = FindObjectOfType<PlayerHealth>().transform;
	}

	private void Update() {
		float xDot = Vector3.Dot(Vector3.forward, _player.forward);
		float yDot = Vector3.Dot(Vector3.right, _player.forward);

		float southScale = Utilities.Math.Remap(-xDot, -1, 1, 1f, 0.75f);
		_south.localScale = Vector2.one * southScale;

		float northScale = Utilities.Math.Remap(xDot, -1, 1, 1f, 0.75f);
		_north.localScale = Vector2.one * northScale;

		float eastScale = Utilities.Math.Remap(yDot, -1, 1, 1f, 0.75f);
		_east.localScale = Vector2.one * eastScale;

		float westScale = Utilities.Math.Remap(-yDot, -1, 1, 1f, 0.75f);
		_west.localScale = Vector2.one * westScale;

		_south.GetComponentInChildren<Text>().color = Color.Lerp((xDot < 0) ? _black : _greyFaded, _grey, Mathf.Abs(yDot));
		_north.GetComponentInChildren<Text>().color = Color.Lerp((xDot > 0) ? _black : _greyFaded, _grey, Mathf.Abs(yDot));
		_west.GetComponentInChildren<Text>().color = Color.Lerp((yDot < 0) ? _black : _greyFaded, _grey, Mathf.Abs(xDot));
		_east.GetComponentInChildren<Text>().color = Color.Lerp((yDot > 0) ? _black : _greyFaded, _grey, Mathf.Abs(xDot));

		_south.anchoredPosition = new Vector2(yDot * _width / 2.0f, -xDot * _height / 2.0f);
		_north.anchoredPosition = new Vector3(-yDot * _width / 2.0f, xDot * _height / 2.0f);
		_east.anchoredPosition = new Vector2(xDot * _width / 2.0f, yDot * _height / 2.0f);
		_west.anchoredPosition = new Vector3(-xDot * _width / 2.0f, -yDot * _height / 2.0f);
	}
}
