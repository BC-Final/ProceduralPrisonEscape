using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class HackerResourceManager : Singleton<HackerResourceManager> {
	[SerializeField]
	private float _hackerPoints = 0;

	[SerializeField]
	private float _maxPoints = 5000;

	[SerializeField]
	private float _regenerationPerSec = 5.0f;

	private void Update() {
		_hackerPoints += _regenerationPerSec * Time.deltaTime;
	}

	public int HackerPoints {
		get {
			return (int)_hackerPoints;
		}
	}

	public void RemoveHackerPoints (int pAmount) {
		if (_hackerPoints >= pAmount) {
			_hackerPoints -= pAmount;
		}
	}
}
