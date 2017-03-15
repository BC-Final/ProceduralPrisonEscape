//using UnityEngine;
//using System.Collections;

//public class FirewallNode : AbstractNode {
//	private HackerFirewall _associatedFirewall;
//	public HackerFirewall AssociatedFirewall { get { return _associatedFirewall; } }

//	public bool Accessible { get { return _associatedFirewall.Destroyed.Value; } }

//	public override void SetReferences (int pAssocId) {
//		_associatedFirewall = HackerFirewall.GetFireWallByID(pAssocId);
//		_associatedFirewall.FirwallNode = this;
//		_associatedFirewall.Destroyed.OnValueChange += () => changedState();
//	}



//	public override void ToggleContext (bool pShow, HackerAvatar pAvatar) {
//		base.ToggleContext(pShow, pAvatar);

//		ContextWindow.Instance.GetContext<FirewallContext>().gameObject.SetActive(pShow);
//	}


//	//TODO OnDoubleClick to map icon
//}
