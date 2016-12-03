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

	//TODO OnDoubleClick to map icon
}
