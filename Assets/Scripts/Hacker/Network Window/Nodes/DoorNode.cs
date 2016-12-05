using UnityEngine;
using System.Collections;

public class DoorNode : AbstractNode {
	private HackerDoor _associatedDoor;

	public override void SetReferences (int pAssocId) {
		_associatedDoor = HackerDoor.GetDoorByID(pAssocId);
		_associatedDoor.DoorNode = this;
		_associatedDoor.State.OnValueChange += () => changedState();
	}

	public override void SetAccessible (bool pAccessible) {
		base.SetAccessible(pAccessible);

		_associatedDoor.Accessible.Value = pAccessible;
	}

	public override void ToggleContext (bool pShow, HackerAvatar pAvatar) {
		base.ToggleContext(pShow, pAvatar);

		DoorContext.Instance.gameObject.SetActive(pShow);

		if (pShow) {
			(DoorContext.Instance as DoorContext).RegisterHackButton(() => StartHack(pAvatar));
			(DoorContext.Instance as DoorContext).RegisterToggleButton(() => toggleDoor());
		} else {
			(DoorContext.Instance as DoorContext).UnregisterHackButton();
			(DoorContext.Instance as DoorContext).UnregisterToggleButton();
		}
	}

	private void toggleDoor () {
		if (_associatedDoor.State.Value == DoorState.Open) {
			_associatedDoor.SetState(DoorState.Closed);
		} else {
			_associatedDoor.SetState(DoorState.Open);
		}
	}

	protected override void OnGUI () {
		base.OnGUI();

		if (_currentNode) {
			DoorContext.Instance.SetHackProgress(_hackTimer.FinishedPercent);
		}
	}

	//TODO OnDoubleClick to map icon
}
