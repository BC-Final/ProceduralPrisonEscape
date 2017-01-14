using UnityEngine;
using System.Collections.Generic;

public class SecurityNode : AbstractNode {
	//FIX Where should the deactivated time be set?
	[SerializeField]
	private float _adminDeactivateDuration;

	private AdminAvatar _avatar;
	private LinkedList<AbstractNode> _knownHackedNodes = new LinkedList<AbstractNode>();
	public LinkedList<AbstractNode> KnownHackedNodes { get { return _knownHackedNodes; } } 

	protected override void Awake() {
		base.Awake();
	}

	public void CreateAvatar () {
		_avatar = (Instantiate(HackerReferenceManager.Instance.AdminAvatar, transform.position, Quaternion.identity, transform.parent) as GameObject).GetComponent<AdminAvatar>();
		_avatar.SpawnNode = this;
	}

	protected override void GotHacked() {
		base.GotHacked();

		GameStateManager.Instance.DisableAlarm();
		_avatar.Deactivate(_adminDeactivateDuration);
		_knownHackedNodes.AddFirst(this);
	}

	public override void ToggleContext (bool pShow, HackerAvatar pAvatar) {
		base.ToggleContext(pShow, pAvatar);


		ContextWindow.Instance.GetContext<SecurityContext>().gameObject.SetActive(pShow);

		if (pShow) {
			ContextWindow.Instance.GetContext<SecurityContext>().RegisterButton("hack", () => StartHack(pAvatar));
		} else {
			ContextWindow.Instance.GetContext<SecurityContext>().UnregisterAllButtons();
		}
	}



	public override void ReceivePacket(AbstractNode pSender, Packet pPacket) {
		if (pPacket is AlarmPacket) {
			_knownHackedNodes.AddFirst(pSender);
			GameStateManager.Instance.TriggerAlarm();
		} else if (pPacket is SuspicionPacket) {
			_knownHackedNodes.AddFirst(pSender);
			GameStateManager.Instance.IncreaseSuspicion();
		} else {
			//Debug.Log("What should " + this.GetType().Name + "do with " + pPacket.GetType().Name + "?");
		}
	}

	protected override void OnGUI () {
		base.OnGUI();

		if (_currentNode) {
			ContextWindow.Instance.GetContext<SecurityContext>().SetHackProgress(_hackTimer.FinishedPercent);
		}
	}
}
