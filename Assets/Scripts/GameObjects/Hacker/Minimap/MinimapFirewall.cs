using UnityEngine;
using System.Collections;

public class MinimapFirewall : MonoBehaviour {

	[SerializeField]
	Sprite normalSprite;
	[SerializeField]
	Sprite destroyedSprite;

	[SerializeField]
	SpriteRenderer _renderer;

	public void ChangeState(bool destroyedState)
	{
		GetRenderer();
		if (destroyedState)
		{
			_renderer.sprite = destroyedSprite;
		}
		else
		{
			_renderer.sprite = normalSprite;
		}
	}

	private void GetRenderer()
	{
		Debug.Log("Getting Renderer");
		if (_renderer == null)
		{
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}
	}
}
