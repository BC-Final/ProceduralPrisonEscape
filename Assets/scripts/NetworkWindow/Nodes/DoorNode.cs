using UnityEngine;
using System.Collections;

public class DoorNode : AbstractNode {
	private HackerDoor _associatedDoor;

	public override void SetReferences (int pAssocId) {
		_associatedDoor = HackerDoor.GetDoorByID(pAssocId);
		_associatedDoor.DoorNode = this;
		_associatedDoor.State.OnValueChange += () => changedState();
	}

	//TODO OnDoubleClick to map icon
}
