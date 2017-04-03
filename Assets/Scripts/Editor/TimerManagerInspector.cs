//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(Timers))]
//public class TimerManagerInspector : Editor {
//	public override void OnInspectorGUI () {
//		Timers instance = (Timers)target;

//		foreach (Timers.Timer t in instance.TimerList) {
//			EditorGUILayout.LabelField(t.Name + (t.IsPlaying ? " is Playing" : " is Stopped"));
//		}
//	}
//}
