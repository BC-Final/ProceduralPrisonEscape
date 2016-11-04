using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class pickup_ammo : MonoBehaviour {
	[SerializeField]
	private string _ammoName;

	[SerializeField]
	private int _content;
}
