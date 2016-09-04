using UnityEngine;
using DavidOchmann.Animation;
using System.Collections.Generic;

namespace Demo
{
	[ RequireComponent (typeof ( Mutate )) ]
	public class DemoAddList : MonoBehaviour
	{
		// Create private dTween instance.
		private DTween dTween = new DTween();


		public void Start()
		{
			// The Mutate compontent is a shorthand for setting a GameObjects 
			// x, y, scale and rotation values.
			Mutate mutate = GetComponent<Mutate>();


			// Setup single tween duration.
			float duration = 1f;

			// Add Tween List object to dTween. Move the mutate object in a rectangular motion. 
			List<Tween> tweens = dTween.Add( new List<Tween>
			{
				new DTween().To( mutate, duration, new { x = 100, y = 100 }, Quad.EaseInOut ),
				new DTween().To( mutate, duration, new { delay = duration, x = 100, y = -100 }, Quad.EaseInOut ),
				new DTween().To( mutate, duration, new { delay = duration * 2, x = -100, y = -100 }, Quad.EaseInOut ),
				new DTween().To( mutate, duration, new { delay = duration * 3, x = -100, y = 100 }, Quad.EaseInOut )
			} );

			// Take the last element of the list. After its animation has completed call Start and repeat.
			Tween tween = tweens[ tweens.Count - 1 ];
			tween.OnComplete += tweenOnCompleteHandler; 
		}

		private void tweenOnCompleteHandler(Tween tween)
		{
			Start();
		}

		public void FixedUpdate()
		{
			// Update dTween on the FixedUpdate interval. 
			// DTween only updates if you call this function.
			dTween.Update();
		}
	}
}