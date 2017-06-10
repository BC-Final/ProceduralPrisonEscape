﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public abstract class AbstractMapIcon : MonoBehaviour, IHackerNetworked {

	private int _id = 0;

	public int Id {
		get { return _id; }
		set {
			if (_id == 0) {
				_id = value;
				HackerPackageSender.RegisterNetworkObject(this);
			} else {
				Debug.LogError("Why are you trying to assign an already set network id?");
			}
		}
	}

	private void OnDestroy() {
		HackerPackageSender.UnregisterNetworkedObject(this);
	}
	[SerializeField]
	public bool IsInteractable;
	[System.Serializable]
	public struct ActionData {
		public bool Disabled;
		public string DisplayName;
		public int HackerPointsCost;
		public UnityEvent Action;
	}

	private SpriteRenderer _spriteRenderer;

	private SpriteRenderer spriteRenderer {
		get {
			if (_spriteRenderer == null) {
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}
			return _spriteRenderer;
		}
	}

	private BoxCollider _boxCollider;

	private BoxCollider boxCollider {
		get {
			if (_boxCollider == null) {
				_boxCollider = GetComponent<BoxCollider>();
			}

			return _boxCollider;
		}
	}

	[SerializeField]
	protected ActionData[] actions;

	private void OnMouseDown() {
		if (!CanvasHoverListener.Instance.MouseOverUI) {
			HackerContextMenu.Display(actions);
		}
	}

	protected void fitCollider() {
		Vector2 size = spriteRenderer.sprite.bounds.size;
		gameObject.GetComponent<BoxCollider>().size = new Vector3(size.x, size.y, 0.2f);
	}

	protected void changeSprite(Sprite pSprite) {
		spriteRenderer.sprite = pSprite;

		fitCollider();
		//gameObject.GetComponent<BoxCollider2D>().center = new Vector2((S.x / 2), 0);
	}

	protected void changeColor(Color pColor) {
		spriteRenderer.color = pColor;
	}

	public static void ProcessPacket(NetworkPacket.GameObjects.PickUpIcon.sIconUpdate pPacket) {
		AbstractMapIcon icon = HackerPackageSender.GetNetworkedObject<AbstractMapIcon>(pPacket.Id);

		if (icon != null) {
			icon.updateInstance(pPacket);
		}
	}

	Sequence mySequence;
	/// <summary>
	/// Gives feedback when an Icon is interactable
	/// </summary>
	virtual public void OnMouseEnter() {
		if (IsInteractable)
		{
			mySequence = DOTween.Sequence();
			mySequence.Append(transform.DOPunchScale(Vector3.one * 0.25f, 1f, 1, 0).SetLoops(1000, LoopType.Restart)).OnComplete(() => {
			});
		}		
	}

	virtual public void OnMouseExit() {
		mySequence.Kill(true);
		
	}

	public void updateInstance(NetworkPacket.GameObjects.PickUpIcon.sIconUpdate pPacket) {
		spriteRenderer.enabled = !pPacket.used;
	}
}
