using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalArea : MonoBehaviour {
	[SerializeField]
	private Vector2 _size = Vector3.one;

	public Vector2[] GetBounds() {
		Vector2[] points = new Vector2[4];

		points[0] = new Vector2(transform.position.x, transform.position.z) + new Vector2(_size.x, -_size.y);
		points[1] = new Vector2(transform.position.x, transform.position.z) + new Vector2(_size.x, _size.y);
		points[2] = new Vector2(transform.position.x, transform.position.z) + new Vector2(-_size.x, _size.y);
		points[3] = new Vector2(transform.position.x, transform.position.z) + new Vector2(-_size.x, -_size.y);

		return points;
	}

	private void OnDrawGizmosSelected() {
		Gizmos.DrawWireCube(transform.position + new Vector3(0.0f, -(transform.position.y / 2), 0.0f), new Vector3(_size.x, transform.position.y, _size.y));
	}
}
