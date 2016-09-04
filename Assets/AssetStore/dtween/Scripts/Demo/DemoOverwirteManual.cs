using UnityEngine;
using DavidOchmann.Animation;
    	
namespace Demo
{
	[ RequireComponent (typeof ( Mutate )) ]
	public class DemoOverwriteManual : MonoBehaviour
	{
		// Create private dTween instance.
		private DTween dTween = new DTween();
		private Tween mutateTween;


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
			// Removes tween when available.
			// Set second parameter to true, to jump to the end of the animation.
			dTween.Remove( mutateTween, false ); 

			// Loop tick animation.
			startTickTween();

			// Start 2 second Mutate Tween.
			startMutateTween();
		}


		/** Animate mutate to random position on screen. */
		private void startMutateTween()
		{
			var distance = 500;

			mutateTween = dTween.To( GetComponent<Mutate>(), 2, new
			{ 
				x = Random.Range( -distance, distance ),
				y = Random.Range( -distance, distance )

			}, Sine.EaseInOut );
		}
	}
}