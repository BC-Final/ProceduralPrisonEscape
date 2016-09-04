using UnityEngine;
using DavidOchmann.Animation;
    	
namespace Demo
{
	public class DemoBasicTo : MonoBehaviour
	{
		// Create private dTween instance.
		private DTween dTween = new DTween();


		public void Start()
		{
			// After 1 seconds, tween transform.localPosition.x in 2 seconds to 200
			// with an in / out back transition.
			dTween.To( transform.localPosition, 2, new { delay = 1, x = 200 }, Back.EaseInOut );

			// Hook an update function to the DTween OnUpdate event.
			dTween.OnUpdate += dTweenOnUpdateHandler;
		}

		private void dTweenOnUpdateHandler(Tween tween)
		{
			// Overwrite the localPosition property with every update.
			transform.localPosition = (Vector3)tween.target;
		}


		public void FixedUpdate()
		{
			// Update dTween on the FixedUpdate interval. 
			// DTween only updates if you call this function.
			dTween.Update();
		}
	}
}