using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNode : AbstractNode {
	private HackerTurret _associatedTurret;

	public override void SetReferences (int pAssocId) {
		_associatedTurret = HackerTurret.GetTurretById(pAssocId);
		_associatedTurret.TurretNode = this;
	}

	public override void SetAccessible (bool pAccessible) {
		base.SetAccessible(pAccessible);

		_associatedTurret.Accessible.Value = pAccessible;
	}

	public override void ToggleContext (bool pShow, HackerAvatar pAvatar) {
		base.ToggleContext(pShow, pAvatar);

		ContextWindow.Instance.GetContext<TurretContext>().gameObject.SetActive(pShow);

		if (pShow) {
			ContextWindow.Instance.GetContext<TurretContext>().RegisterButton("hack", () => StartHack(pAvatar));
			ContextWindow.Instance.GetContext<TurretContext>().RegisterButton("disable", () => _associatedTurret.DisableTurret());
			//TODO Add Reinforce
		} else {
			ContextWindow.Instance.GetContext<TurretContext>().UnregisterAllButtons();
		}
	}

	protected override void OnGUI () {
		base.OnGUI();

		if (_currentNode) {
			ContextWindow.Instance.GetContext<TurretContext>().SetHackProgress(_hackTimer.FinishedPercent);
		}
	}

	public override bool StartHack (AbstractAvatar pAvatar) {
		return base.StartHack(pAvatar);
	}
}