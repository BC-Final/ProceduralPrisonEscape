using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable {
	void AddToDestroyEvent (System.Action<GameObject> pAction);
	void RemoveFromDestroyEvent (System.Action<GameObject> pAction);
	Faction Faction { get; }
	GameObject GameObject { get; }
}
