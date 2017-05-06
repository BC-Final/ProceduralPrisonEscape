using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassShooterUI : MonoBehaviour {
	[SerializeField]
	private float _width = 400.0f;

	[SerializeField]
	private RectTransform _north;

	[SerializeField]
	private RectTransform _south;

	[SerializeField]
	private RectTransform _west;

	[SerializeField]
	private RectTransform _east;

	private Transform _player;

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

		Debug.Log("X: " + xDot + "; Y: " + yDot);

		//if xDot > 0 dont show S
		//if xDot < 0 dont show N
		//if yDot > 0 dont show W
		//if yDot < 0 dont show E

		_south.gameObject.SetActive(xDot <= 0.0f);
		_north.gameObject.SetActive(xDot >= 0.0f);
		_west.gameObject.SetActive(yDot <= 0.0f);
		_east.gameObject.SetActive(yDot >= 0.0f);

		_south.anchoredPosition = new Vector2(Utilities.Math.Remap(Mathf.Abs(xDot), 0, 1, (yDot > 0) ? _width / 2.0f : -_width/2.0f, (yDot > 0) ? 0.0f : 0.0f), 0);
		_north.anchoredPosition = new Vector2(Utilities.Math.Remap(Mathf.Abs(xDot), 0, 1, (yDot < 0) ? _width / 2.0f : -_width / 2.0f, (yDot < 0) ? 0.0f : 0.0f), 0);
		_west.anchoredPosition = new Vector2(Utilities.Math.Remap(Mathf.Abs(yDot), 0, 1, (xDot < 0) ? _width / 2.0f : -_width / 2.0f, (xDot < 0) ? 0.0f : 0.0f), 0);
		_east.anchoredPosition = new Vector2(Utilities.Math.Remap(Mathf.Abs(yDot), 0, 1, (xDot > 0) ? _width / 2.0f : -_width / 2.0f, (xDot > 0) ? 0.0f : 0.0f), 0);

		//Vector3 perp = Vector3.Cross(Vector3.forward, _player.forward);
		//float dir = Vector3.Dot(perp, Vector3.up);


	}
}
