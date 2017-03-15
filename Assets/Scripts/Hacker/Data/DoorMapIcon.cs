using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMapIcon : AbstractMapIcon {
	public static void CreateInstance (Vector2 pPos, float rot, DoorState pState) {
		//TODO Assign values
	}

	public enum DoorState {
		Open,
		Closed,
		Locked
	}

	[SerializeField]
	private Sprite _openSprite;

	[SerializeField]
	private Sprite _closedSprite;

	[SerializeField]
	private Sprite _lockedSprite;

	private DoorState _currentDoorState;

	public void Toggle () {
		//TODO Assign what to do
	}

	public void Lock () {
		//TODO Assign what to do
	}
}
