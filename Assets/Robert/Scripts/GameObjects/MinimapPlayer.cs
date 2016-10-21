using UnityEngine;
using System.Collections;

public class MinimapPlayer : MonoBehaviour {

	Vector3 oldPos;
	Vector3 newPos;

	float lerpTime = 0.5f;
	float currentLerpTime;

	// Update is called once per frame
	void Update () {
		//increment timer once per frame
		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime)
		{
			currentLerpTime = lerpTime;
		}

		//lerp!
		float perc = currentLerpTime / lerpTime;
		transform.position = Vector3.Lerp(oldPos, newPos, perc);
	}

	public void SetNewPos(Vector3 nPos)
	{
		currentLerpTime = 0f;
		oldPos = newPos;
		newPos = nPos;
	}
}