using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class SectorDoorMapIcon : AbstractMapIcon {

	public static void ProcessPacket (NetworkPacket.Update.SectorDoor pPacket) {
		SectorDoorMapIcon icon = HackerPackageSender.GetNetworkedObject<SectorDoorMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance (NetworkPacket.Update.SectorDoor pPacket) {
		SectorDoorMapIcon icon = Instantiate(HackerReferenceManager.Instance.SectorDoorIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, -10.0f), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<SectorDoorMapIcon>();

		icon.Id = pPacket.Id;
		icon._open.Value = pPacket.Open;
		icon._locked.Value = pPacket.Locked;

		//TODO Research why onValueChanged is not called
		icon.stateChanged();
	}

	private void updateInstance (NetworkPacket.Update.SectorDoor pPacket) {
		_open.Value = pPacket.Open;
		_locked.Value = pPacket.Locked;
	}

	private void Awake () {
		_open.OnValueChange += stateChanged;
		_locked.OnValueChange += stateChanged;
	}

	private ObservedValue<bool> _open = new ObservedValue<bool>(false);
	private ObservedValue<bool> _locked = new ObservedValue<bool>(false);

	#region Sprites
	[Header("Sprites")]
	[SerializeField]
	private Sprite _openSprite;
	[SerializeField]
	private Sprite _closedSprite;
	[SerializeField]
	private Sprite _lockedClosedSprite;
	#endregion

	public void Toggle () {
		//TODO Send state update but only change when receiving (if it causes problems)
		if (!_locked.Value) {
			_open.Value = !_open.Value;
			sendUpdate();
		} else {
			//TODO Play Feedback sound
		}
	}

	private void sendUpdate () {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.SectorDoor(Id, _open.Value));
	}

	private void stateChanged () {
		if (_open.Value) {
			changeSprite(_openSprite);
		} else {
			if (_locked.Value) {
				changeSprite(_lockedClosedSprite);
			} else{
                changeSprite(_closedSprite);
            }
        }
	}
}
