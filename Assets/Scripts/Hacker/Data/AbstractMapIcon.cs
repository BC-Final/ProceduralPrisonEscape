using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class AbstractMapIcon : MonoBehaviour {
	[System.Serializable]
	public struct ActionData {
		public string DisplayName;
		public int HackerPointsCost;
		public UnityEvent Action;
	}

	private SpriteRenderer _spriteRenderer;

	protected SpriteRenderer spriteRenderer {
		get {
			if (_spriteRenderer == null) {
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}

			return _spriteRenderer;
		}
	}

	public ActionData[] Actions;
}
