using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelBuilder : EditorWindow {
	[MenuItem ("Window/Level Builder")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(LevelBuilder));
	}

	private void OnGUI() {
		GUILayout.Label("Prefabs", EditorStyles.boldLabel);
	}
}
