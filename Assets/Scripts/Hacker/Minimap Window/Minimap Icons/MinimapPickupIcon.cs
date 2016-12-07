using UnityEngine;
using System.Collections;

public class MinimapPickupIcon : MonoBehaviour {

	private SpriteRenderer _renderer;

	public void SetSprite(Sprite sprite)
	{
		GetRenderer();
		_renderer.sprite = sprite;
	}

	public void ChangeState(bool collected)
	{
		GetRenderer();
		_renderer.enabled = !collected;
	}

	private void GetRenderer()
	{
		if (_renderer == null)
		{
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}
	}
}
