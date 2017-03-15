using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMapIcon : AbstractMapIcon {
	public static void CreateInstance (Vector2 pPos, float pRot, bool pOpen, bool pLocked) {
		GameObject go = (GameObject)Instantiate(HackerReferenceManager.Instance.DoorIcon, new Vector3(pPos.x / MinimapManager.scale, pPos.y / MinimapManager.scale, 0), Quaternion.Euler(0, pRot, 0));
		go.GetComponent<DoorMapIcon>()._open = pOpen;
		go.GetComponent<DoorMapIcon>()._locked = pLocked;
		go.GetComponent<DoorMapIcon>().StateChanged();
	}


	private bool _open;
	private bool _locked;

	#region Sprites
	[Header("Sprites")]
	[SerializeField]
	private Sprite _openSprite;
	[SerializeField]
	private Sprite _closedSprite;
	[SerializeField]
	private Sprite _lockedOpenSprite;
	[SerializeField]
	private Sprite _lockedClosedSprite;
	#endregion

	public void Toggle () {
		_open = !_open;

		StateChanged();
	}

	public void Lock () {
		_locked = true;

		StateChanged();
	}

	private void StateChanged () {
		if (_open) {
			if (_locked) {
				changeSprite(_lockedOpenSprite);
			} else {
				changeSprite(_openSprite);
			}
		} else {
			if (_locked) {
				changeSprite(_lockedClosedSprite);
			} else {
				changeSprite(_closedSprite);
			}
		}
	}
}
