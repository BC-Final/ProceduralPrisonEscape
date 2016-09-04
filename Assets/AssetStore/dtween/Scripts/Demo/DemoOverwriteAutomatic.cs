using UnityEngine;
using DavidOchmann.Animation;
    	
namespace Demo
{
	[ RequireComponent (typeof ( Mutate )) ]
	public class DemoOverwriteAutomatic : MonoBehaviour
	{
		// Create private dTween instance. With overwrite set to true.
		// Overwrites every tween with an identical target object.
		private DTween dTween = new DTween( true );


		public void Start()
		{
			startTickTween();
		}

		public void FixedUpdate()
		{
			// Update dTween on the FixedUpdate interval. 
			// DTween only updates if you call this function.
			dTween.Update();
		}


		/** Tick animation loop. Interrupts DTween animation. */
		private void startTickTween()
		{
			// Call tweenTickCompleteHandler after 1 second.
			Tween tweenTick = dTween.To( 1 );
			tweenTick.OnComplete += tweenTickCompleteHandler;
		}

		private void tweenTickCompleteHandler(Tween tween)
		{
			// Loop tick animation.
			startTickTween();

			// Start 2 second Mutate Tween.
			startMutateTween();
		}


		/** Animate mutate to random position on screen. */
		private void startMutateTween()
		{
			var distance = 500;

			dTween.To( GetComponent<Mutate>(), 2, new
			{ 
				x = Random.Range( -distance, distance ),
				y = Random.Range( -distance, distance )

			}, Sine.EaseInOut );
		}
	}
}