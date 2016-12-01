using UnityEngine;
using System.Collections;

public class MinimapFirewall : MonoBehaviour {

	#region Sprites
	[SerializeField]
	private Sprite normalSprite;
	[SerializeField]
	private Sprite destroyedSprite;
	#endregion

	#region References
	private SpriteRenderer _renderer;
	private HackerFireWall _associatedFirewall;
	#endregion


	#region Properties
	/// <summary>
	/// Sets the associated Firewall and subscribes to its state change
	/// </summary>
	public HackerFireWall AssociatedFirewall {
		set {
			_associatedFirewall = value;
			_associatedFirewall.Destroyed.OnValueChange += () => changedState();
		}
	}
	#endregion



	/// <summary>
	/// Gets called after this object is initialized
	/// </summary>
	private void Start () {
		_renderer = GetComponentInChildren<SpriteRenderer>();
	}



	/// <summary>
	/// Gets called when the associated firewall changes its state
	/// </summary>
	private void changedState () {
		if (_associatedFirewall.Destroyed.Value) {
			_renderer.sprite = destroyedSprite;
		} else {
			_renderer.sprite = normalSprite;
		}
	}
}
