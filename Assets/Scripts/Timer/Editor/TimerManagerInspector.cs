using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Linq;

[CustomEditor(typeof(TimerManager))]
public class TimerManagerInspector : Editor {
	private Dictionary<Timer, bool> _showTimer = new Dictionary<Timer, bool>();

	GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);

	private void OnEnable () {
		myFoldoutStyle.fontStyle = FontStyle.Bold;
	}

	public override void OnInspectorGUI () {
		TimerManager tm = (TimerManager)target;

		foreach (Timer t in tm.Timers) {
			if (!_showTimer.ContainsKey(t)) {
				_showTimer.Add(t, false);
			}
		}

		List<Timer> toRemove = new List<Timer>();

		foreach (KeyValuePair<Timer, bool> t in _showTimer) {
			if (!tm.Timers.Contains(t.Key)) {
				toRemove.Add(t.Key);
			}
		}

		toRemove.ForEach(x => _showTimer.Remove(x));

		foreach (Timer t in tm.Timers) {
			_showTimer[t] = EditorGUILayout.Foldout(_showTimer[t], t.Name, myFoldoutStyle);

			if (_showTimer[t]) {
				EditorGUI.indentLevel++;
				EditorGUILayout.LabelField("Duration:\t" + t.Duration.ToString("N2") + " / " + t.CurrentTime.ToString("N2") + "   (" + (t.FinishedPercentage * 100.0f).ToString("N0") + "%)" + "   [" + (t.IsPlaying ? "Playing" : t.IsFinished ? "Finished" : "Paused") + "]");
				EditorGUILayout.LabelField("Loops:\t" + t.LoopCount + " / " + t.CurrentLoop);
				EditorGUILayout.LabelField("");

				EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
				EditorGUI.indentLevel++;
				EditorGUILayout.LabelField("Uses Random Duration:\t" + t.UsesRandomDuration + (t.UsesRandomDuration ? ("   (Min: " + t.MinDuration.ToString("N2") + "  Max: " + t.MaxDuration.ToString("N2") + ")") : ""));
				if (t.UsesRandomDuration)
					EditorGUILayout.LabelField("Change duration after loop:\t" + t.ChangeRandomTimeAfterLoop);
				EditorGUILayout.LabelField("Reset on finish:\t\t" + t.ResetsOnFinish);
				EditorGUILayout.LabelField("Killed on finish:\t\t" + t.KilledOnFinish);
				EditorGUILayout.LabelField("Use scaled time:\t\t" + t.UsesScaledDeltaTime);
				EditorGUILayout.LabelField("Killed on finish:\t\t" + t.KilledOnFinish);
				EditorGUI.indentLevel--;
				EditorGUILayout.LabelField("");
				EditorGUILayout.LabelField("Callbacks", EditorStyles.boldLabel);
				EditorGUI.indentLevel++;
				foreach (KeyValuePair<string, bool> c in t.CallbackInfo) {
					EditorGUILayout.LabelField("Source: " + c.Key);
					EditorGUILayout.LabelField("Call: " + (c.Value ? "On Finish" : "On Loop"));
					EditorGUILayout.LabelField("");
				}
				EditorGUI.indentLevel--;
				EditorGUI.indentLevel--;
			}
		}

		EditorUtility.SetDirty(target);
	}
}
