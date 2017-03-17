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
		}
	}

	private static void createInstance (NetworkPacket.Update.Pipe pPacket) {
		GaspipeMapIcon icon = Instantiate(HackerReferenceManager.Instance.DroneIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<GaspipeMapIcon>();

		icon._used.OnValueChange += icon.changedState;

		icon.Id = pPacket.Id;
		icon._used.Value = pPacket.Exploded;
	}

	private ObservedValue<bool> _used = new ObservedValue<bool>(false);

	public void NormalExplosion () {
		if (!_used.Value) {
			_used.Value = true;
			sendUpdate(PipeExplodeType.Normal);
		}
	}

	public void BigExplosion () {
		//TODO Set timer and then explode
		if (!_used.Value) {
			_used.Value = true;
			sendUpdate(PipeExplodeType.Charged);
		}
	}

	private void sendUpdate (PipeExplodeType pType) {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.Pipe(Id, pType));
	}

	private void changedState () {
		if (!_used.Value) {
			changeSprite(_normalSprite);
		} else {
			changeSprite(_explodedSprite);
		}
	}
}
