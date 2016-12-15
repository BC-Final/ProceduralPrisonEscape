using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolRoute : MonoBehaviour {
	[SerializeField]
	private bool _looping;
	public bool Looping { get { return _looping; } }

	[SerializeField]
	private List<GameObject> _waypoints;

	private void Start () {
		_waypoints = new List<GameObject>();

		foreach (Transform w in transform.GetComponentsInChildren<Transform>()) {
			if (w.gameObject != gameObject) {
				_waypoints.Add(w.gameObject);
			}
		}
	}

	public GameObject GetWaypoint (int pIndex) {
		return _waypoints[pIndex];
	}

	public int WaypointCount () {
		return _waypoints.Count;
	}

	public int GetNearestWaypointIndex (Vector3 pPosition) {
		float minDist = Mathf.Infinity;
		GameObject nearestWaypoint = null;

		foreach (GameObject w in _waypoints) {
			float currDist = Vector3.Distance(w.transform.position, pPosition);

			if (Vector3.Distance(w.transform.position, pPosition) < minDist) {
				minDist = currDist;
				nearestWaypoint = w;
			}
		}

		return _waypoints.IndexOf(nearestWaypoint);
	}

	private void OnDrawGizmos () {
		int count = 0;

		List<Transform> transList = new List<Transform>(transform.GetComponentsInChildren<Transform>());
		transList.RemoveAll(x => x.gameObject == gameObject);

		foreach (Transform t in transList) {
			count++;

			if (count == transList.Count) {
				if (_looping) {
					count = 0;
				} else {
					break;
				}
			}

			Gizmos.DrawLine(t.position, transList[count].position);
		}
	}
}
