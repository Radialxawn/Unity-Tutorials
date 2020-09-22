using UnityEditor;
using UnityEngine;
using System.IO;

namespace DNC {
	public static class EditorExtensions {
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

		/*||||||||||||||||||||||
		||| Create HLSL File |*/
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
		/*| Create HLSL File |||
		||||||||||||||||||||||*/

		private static void EngageRenameMode() {
			if (!EditorApplication.isCompiling) {
				EditorApplication.update -= EngageRenameMode;
				EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.F2, type = EventType.KeyDown });
				EditorApplication.update -= EngageRenameMode;
			}
		}

		/*||||||||||||||||||||||||||
		||| Create Editor Script |*/
		[MenuItem("Assets/Create Editor Script", priority = 81)]
		private static void CreateEditorScript() {
			string className = Selection.activeObject.name;
			string fileName = className + "Editor";
			string folderPath = GetSelectedPathOrFallback() + "/Editor";
			if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
			string filePath = folderPath + "/" + fileName + ".cs";

			if (File.Exists(filePath) == false) {
				using (StreamWriter outfile = new StreamWriter(filePath)) {
					outfile.WriteLine("using UnityEditor;");
					outfile.WriteLine("");
					outfile.WriteLine("[CustomEditor(typeof(" + className + "))]");
					outfile.WriteLine("public class " + fileName + " : Editor {");
					outfile.WriteLine("	" + className + " t;");
					outfile.WriteLine("");
					outfile.WriteLine("	private void OnEnable() {");
					outfile.WriteLine("		t = target as " + className + ";");
					outfile.WriteLine("	}");
					outfile.WriteLine("");
					outfile.WriteLine("	public override void OnInspectorGUI() {");
					outfile.WriteLine("		base.OnInspectorGUI();");
					outfile.WriteLine("	}");
					outfile.WriteLine("");
					outfile.WriteLine("	private void OnSceneGUI() {");
					outfile.WriteLine("		");
					outfile.WriteLine("	}");
					outfile.WriteLine("}");
				}
			}

			AssetDatabase.Refresh();
		}

		[MenuItem("Assets/Create Editor Script", isValidateFunction: true, priority = 81)]
		private static bool ValidateCreateEditorScript() {
			return Selection.activeObject.GetType().Equals(typeof(MonoScript));
		}
		/*| Create Editor Script |||
		||||||||||||||||||||||||||*/

		[MenuItem("DNC/Editor Helper/Open Persistent Data Path")]
		private static void OpenPersistentDataPath() {
			Application.OpenURL(Application.persistentDataPath);
		}

		private static readonly string sceneViewDataName = typeof(EditorExtensions).FullName + Application.dataPath + "SceneViewData";

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