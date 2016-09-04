using UnityEngine;
using DavidOchmann.Animation;
    	
namespace Demo
{
	[ RequireComponent (typeof ( Mutate )) ]
	public class DemoMutate : MonoBehaviour
	{
		// Create private dTween instance.
		private DTween dTween = new DTween();


		public void Start()
		{
			// The Mutate compontent is a shorthand for setting a GameObjects 
			// x, y, scale and rotation values.

			// After 2 seconds, tween Mutate.x in 1 second to 200 and Mutate.y to 180
			// with an in / out quad transition.
			dTween.To( GetComponent<Mutate>(), 2, new { delay = 1, x = 200, rotationZ = 180 }, Quad.EaseInOut );
		}

		public void FixedUpdate()
		{
			// Update dTween on the FixedUpdate interval. 
			// DTween only updates if you call this function.
			dTween.Update();
		}
	}
}