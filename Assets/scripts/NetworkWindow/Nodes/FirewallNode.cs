using UnityEngine;
using System.Collections;

public class FirewallNode : AbstractNode {
	private HackerFireWall _associatedFirewall;
	public HackerFireWall AssociatedFirewall { get { return _associatedFirewall; } }

	public bool Accessible { get { return _associatedFirewall.Destroyed.Value; } }

	public override void SetReferences (int pAssocId) {
		_associatedFirewall = HackerFireWall.GetFireWallByID(pAssocId);
		_associatedFirewall.FirwallNode = this;
		_associatedFirewall.Destroyed.OnValueChange += () => changedState();
	}


	//TODO OnDoubleClick to map icon
}
