﻿using UnityEngine;
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
			changedState();
		}
	}



	/// <summary>
	/// Lazily initializes renderer. This is necessary because sometimes its not initialized on start
	/// </summary>
	new private SpriteRenderer renderer {
		get {
			if (_renderer == null) {
				_renderer = GetComponentInChildren<SpriteRenderer>();
			}

			return _renderer;
		}
	}
	#endregion




	/// <summary>
	/// Gets called when the associated firewall changes its state
	/// </summary>
	private void changedState () {
		if (_associatedFirewall.Destroyed.Value) {
			renderer.sprite = destroyedSprite;
		} else {
			renderer.sprite = normalSprite;
		}
	}
}
