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

		//(0, 1) = EAST
		//(0, -1) = WEST
		//(1, 0) = NORTH
		//(-1, 0) = SOUTH

		//if xDot > 0 dont show S
		//if xDot < 0 dont show N
		//if yDot > 0 dont show W
		//if yDot < 0 dont show E

		//_south.GetComponent<Text>().color = (xDot <= 0.0f) ? Color.black : Color.grey;
		//_north.GetComponent<Text>().color = (xDot >= 0.0f) ? Color.black : Color.grey;
		//_west.GetComponent<Text>().color = (yDot <= 0.0f) ? Color.black : Color.grey;
		//_east.GetComponent<Text>().color = (yDot >= 0.0f) ? Color.black : Color.grey;

		//_south.GetComponent<Text>().color = Color.Lerp((xDot < 0) ? _grey : _greyFaded, (xDot < 0) ? _black : _grey, Mathf.Abs(yDot));
		_south.GetComponent<Text>().color = Color.Lerp((xDot < 0) ? _black : _greyFaded, _grey, Mathf.Abs(yDot));
		_north.GetComponent<Text>().color = Color.Lerp((xDot > 0) ? _black : _greyFaded, _grey, Mathf.Abs(yDot));

		_west.GetComponent<Text>().color = Color.Lerp((yDot < 0) ? _black : _greyFaded, _grey, Mathf.Abs(xDot));
		_east.GetComponent<Text>().color = Color.Lerp((yDot > 0) ? _black : _greyFaded, _grey, Mathf.Abs(xDot));

		_south.anchoredPosition = new Vector2(yDot * _width / 2.0f, -xDot * _height / 2.0f);
		_north.anchoredPosition = new Vector3(-yDot * _width / 2.0f, xDot * _height / 2.0f);
		_east.anchoredPosition = new Vector2(xDot * _width / 2.0f, yDot * _height / 2.0f);
		_west.anchoredPosition = new Vector3(-xDot * _width / 2.0f, -yDot * _height / 2.0f);
	}
}
