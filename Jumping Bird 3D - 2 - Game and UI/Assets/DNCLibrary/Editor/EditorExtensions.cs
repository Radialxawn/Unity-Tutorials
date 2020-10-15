using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

namespace DNC {
	public static class EditorExtensions {
		[MenuItem("Assets/Create/Shader/HLSL File")]
		private static void CreateHLSLFile() {
			string name = "NewHLSLFile.hlsl";
			string path = GetSelectedPathOrFallback() + "/" + name;

			if (File.Exists(path) == false) {
				using (StreamWriter outfile = new StreamWriter(path)) {
					outfile.WriteLine("");
				}
			}

			AssetDatabase.Refresh();
			Selection.activeObject = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
			EditorApplication.update += EngageRenameMode;
		}

		private static void EngageRenameMode() {
			if (!EditorApplication.isCompiling) {
				EditorApplication.update -= EngageRenameMode;
				EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.F2, type = EventType.KeyDown });
				EditorApplication.update -= EngageRenameMode;
			}
		}

		[MenuItem("DNC/Editor Helper/Open Persistent Data Path")]
		private static void OpenPersistentDataPath() {
			Application.OpenURL(Application.persistentDataPath);
		}

		private const string sceneViewDataName = "SceneViewData";

		[MenuItem("DNC/Editor Helper/Scene View Data/Save")]
		private static void SaveSceneViewData() {
			if (EditorUtility.DisplayDialog("Save current scene view data", "Are you sure?", "Yes")) {
				SceneView sv = SceneView.lastActiveSceneView;
				EditorPrefs.SetString(sceneViewDataName, EditorJsonUtility.ToJson(sv));
			}
		}

		[MenuItem("DNC/Editor Helper/Scene View Data/Load")]
		private static void LoadSceneViewData() {
			if (EditorUtility.DisplayDialog("Load scene view data", "Are you sure?", "Yes")) {
				SceneView sv = SceneView.lastActiveSceneView;
				EditorJsonUtility.FromJsonOverwrite(EditorPrefs.GetString(sceneViewDataName), sv);
			}
		}

		public static string GetSelectedPathOrFallback() {
			string path = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
				path = AssetDatabase.GetAssetPath(obj);
				if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
					path = Path.GetDirectoryName(path);
					break;
				}
			}
			return path;
		}

		public static bool ScrollWheelOnLastRect() {
			Event e = Event.current;
			return GUILayoutUtility.GetLastRect().Contains(e.mousePosition) && e.type == EventType.ScrollWheel;
		}

		public static void EditorGUILayoutLabelFieldAutoSize(string label) {
			GUIContent content = new GUIContent(label);
			Vector2 size = GUI.skin.label.CalcSize(content);
			EditorGUILayout.LabelField(content, GUILayout.Width(size.x));
		}

		public static bool GUILayoutButtonAutoSize(string label) {
			GUIContent content = new GUIContent(label);
			Vector2 size = GUI.skin.label.CalcSize(content);
			if (GUILayout.Button(content, GUILayout.Width(size.x + 8))) { return true; }
			return false;
		}
	}
}