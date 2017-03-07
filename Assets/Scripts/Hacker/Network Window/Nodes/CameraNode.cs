using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNode : AbstractNode {
	[SerializeField]
	private float _suspicionPacketIntervall;
	private Timers.Timer _suspicionTimer;

	private HackerCamera _associatedCamera;

	public override void SetReferences (int pAssocId) {
		_associatedCamera = HackerCamera.GetCameraById(pAssocId);
		_associatedCamera.CamerNode = this;

		_associatedCamera.SeesPlayer.OnValueChange += () => StartStopSuspicion();
	}

	public override void SetAccessible (bool pAccessible) {
		base.SetAccessible(pAccessible);

		_associatedCamera.Accessible.Value = pAccessible;
	}

	public override void ToggleContext (bool pShow, HackerAvatar pAvatar) {
		base.ToggleContext(pShow, pAvatar);

		ContextWindow.Instance.GetContext<CameraContext>().gameObject.SetActive(pShow);

		if (pShow) {
			ContextWindow.Instance.GetContext<CameraContext>().RegisterButton("hack", () => StartHack(pAvatar));
			ContextWindow.Instance.GetContext<CameraContext>().RegisterButton("disable", () => _associatedCamera.DisableCamera());
			//TODO Add Reinforce
		} else {
			ContextWindow.Instance.GetContext<CameraContext>().UnregisterAllButtons();
		}
	}

	public void StartStopSuspicion () {
		if (_associatedCamera.SeesPlayer.Value) {
			_suspicionTimer = Timers.CreateTimer().SetTime(_suspicionPacketIntervall).SetLoop(-1).SetCallback(() => SendSuspicionPacket()).Start();
		} else {
			_suspicionTimer.Stop();
		}
	}

	protected void SendSuspicionPacket () {
		GameObject go = (Instantiate(HackerReferenceManager.Instance.SuspicionPacket, transform.position, Quaternion.identity, GetComponentInParent<Canvas>().transform) as GameObject);
		go.GetComponent<Packet>().Send(this, _nearestSecurityNode);
	}

	protected override void OnGUI () {
		base.OnGUI();

		if (_currentNode) {
			ContextWindow.Instance.GetContext<CameraContext>().SetHackProgress(_hackTimer.FinishedPercent);
		}
	}

	public override bool StartHack (AbstractAvatar pAvatar) {
		return base.StartHack(pAvatar);
	}
}
