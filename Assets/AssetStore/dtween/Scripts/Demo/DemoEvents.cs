using UnityEngine;
using DavidOchmann.Animation;
    	
namespace Demo
{
	[ RequireComponent (typeof ( Mutate )) ]
	public class DemoEvents : MonoBehaviour
	{
		// Create private dTween instance.
		private DTween dTween = new DTween();


		public void Start()
		{ 
			tweenToDestination();
		}


		/** Tween to destintation position. */
		private void tweenToDestination()
		{
			// Tween Mutate.x to 200 and Mutate.rotation to 180.
			Tween tween = dTween.To( GetComponent<Mutate>(), 1, new { x = 200, rotationZ = 180 }, Quad.EaseInOut );

			// Call desinationTweenOnStartHandler at the start of the Tween.
			tween.OnStart += desinationTweenOnStartHandler;

			// Call desinationTweenOnCompleteHandler at the end of the Tween.
			tween.OnComplete += desinationTweenOnCompleteHandler;
		}

		private void desinationTweenOnStartHandler(Tween tween)
		{
			Debug.Log( "desinationTweenOnStartHandler" );
		}

		private void desinationTweenOnCompleteHandler(Tween tween)
		{
			// Start tween to start position ( x = -200 ).
			tweenToStart();
		}


		/** Tween to start position. */
		private void tweenToStart()
		{
			// Tween Mutate.x to -200 and Mutate.rotation to 180.
			Tween tween = dTween.To( GetComponent<Mutate>(), 1, new { x = -200, rotationZ = 0 }, Quad.EaseInOut );
			
			// Call startTweenOnCompleteHandler at the end of the Tween.
			tween.OnComplete += startTweenOnCompleteHandler;
		}

		private void startTweenOnCompleteHandler(Tween tween)
		{
			// Start tween to destination position ( x = 200 ).
			tweenToDestination();
		}


		/** Update loop. */
		public void FixedUpdate()
		{
			// Update dTween on the FixedUpdate interval. 
			// DTween only updates if you call this function.
			dTween.Update();
		}
	}
}