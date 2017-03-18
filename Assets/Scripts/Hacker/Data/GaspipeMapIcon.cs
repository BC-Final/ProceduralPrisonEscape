using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class GaspipeMapIcon : AbstractMapIcon {
	#region Sprites
	[Header("Sprites")]

	[SerializeField]
	private Sprite _normalSprite;

	[SerializeField]
	private Sprite _explodedSprite;
	#endregion

	public static void ProcessPacket (NetworkPacket.Update.Pipe pPacket) {
		GaspipeMapIcon icon = HackerPackageSender.GetNetworkedObject<GaspipeMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance (NetworkPacket.Update.Pipe pPacket) {
		GaspipeMapIcon icon = Instantiate(HackerReferenceManager.Instance.DroneIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<GaspipeMapIcon>();

		icon._used.OnValueChange += icon.changedState;

		icon.Id = pPacket.Id;
		icon._used.Value = pPacket.Exploded;
	}

	private void updateInstance (NetworkPacket.Update.Pipe pPacket) {
		_used.Value = pPacket.Exploded;
	}

	private ObservedValue<bool> _used = new ObservedValue<bool>(false);

	public void NormalExplosion () {
		if (!_used.Value) {
			sendUpdate(false);
		}
	}

	public void BigExplosion () {
		//TODO Start ticking animation
		if (!_used.Value) {
			sendUpdate(true);
		}
	}

	private void sendUpdate (bool pChargedExplosion) {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.Pipe(Id, pChargedExplosion));
	}

	private void changedState () {
		if (!_used.Value) {
			changeSprite(_normalSprite);
		} else {
			changeSprite(_explodedSprite);
		}
	}
}
