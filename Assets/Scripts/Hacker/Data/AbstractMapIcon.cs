using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
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

	[SerializeField]
	protected ActionData[] actions;

	private void OnMouseDown () {
		if (!CanvasHoverListener.Instance.MouseOverUI) {
			HackerContextMenu.Display(actions);
		}
	}
}
