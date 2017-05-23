using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerAbilityCaster : MonoBehaviour {

	static private List<GrenadeMapIcon> nades = new List<GrenadeMapIcon>();

	public static void AddGrenade(GrenadeMapIcon nade)
	{
		nades.Add(nade);
	}

	public static void RemoveGrenade(GrenadeMapIcon nade)
	{
		nades.Remove(nade);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.G) && nades.Count > 0) {
			nades[0].Explosion();
		}
	}
}
