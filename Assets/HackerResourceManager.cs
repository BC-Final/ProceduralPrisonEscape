using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class HackerResourceManager : Singleton<HackerResourceManager> {
	[SerializeField]
	private int _hackerPoints = 0;

	public int HackerPoints {
		get {
			return _hackerPoints;
		}
	}

	public void RemoveHackerPoints (int pAmount) {
		if (_hackerPoints >= pAmount) {
			_hackerPoints -= pAmount;
		}
	}
}
