using UnityEngine;
using System.Collections;

public class HackerFireWall : FireWall {

	MinimapFirewall _minimapIcon;

	public override void ChangeState(bool nDestroyed)
	{
		_minimapIcon.ChangeState(nDestroyed);
		base.ChangeState(nDestroyed);
	}

	public void SetMinimapIcon(MinimapFirewall minimapFirewall)
	{
		_minimapIcon = minimapFirewall;
	}

}
