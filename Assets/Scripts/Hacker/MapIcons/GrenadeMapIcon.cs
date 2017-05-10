using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMapIcon : AbstractMapIcon {
	public static void ProcessPacket(NetworkPacket.Update.Grenade pPacket) {
		GrenadeMapIcon icon = HackerPackageSender.GetNetworkedObject<GrenadeMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance(NetworkPacket.Update.Grenade pPacket) {
		GrenadeMapIcon icon = Instantiate(HackerReferenceManager.Instance.GrenadeIcon, new Vector3(pPacket.PosX, pPacket.PosY, 0) / MinimapManager.scale, Quaternion.identity).GetComponent<GrenadeMapIcon>();

		icon.Id = pPacket.Id;
		icon.fitCollider();
	}

	private void updateInstance(NetworkPacket.Update.Grenade pPacket) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldPos = transform.position;
		_newPos = new Vector3(pPacket.PosX, pPacket.PosY, 0.0f) / MinimapManager.scale;
	}

	private Vector3 _oldPos;
	private Vector3 _newPos;

	private float _lastUpdateTime = 0.0f;

	private float _currentLerpTime = 0.0f;
	private float _lerpTime = 0.1f;

	private void Start() {
		_lastUpdateTime = Time.time;

		_oldPos = transform.position;
		_newPos = transform.position;
	}

	private void Update() {
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime) {
			_currentLerpTime = _lerpTime;
		}

		if (_lerpTime == 0.0f) {
			_lerpTime = 0.5f;
		}

		float perc = _currentLerpTime / _lerpTime;

		transform.position = Vector3.Lerp(_oldPos, _newPos, perc);
	}

	public void Explosion() {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.Grenade(Id, 0.0f, 0.0f));
		gameObject.SetActive(false);
		Destroy(this.gameObject, 2.0f);
	}
}
