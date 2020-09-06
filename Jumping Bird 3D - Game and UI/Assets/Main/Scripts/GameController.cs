#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

using UnityEngine;

public class GameController : MonoBehaviour {
	public static GameController I;

	private static float score = 0f;

	public static int Score { get { return (int)score; } }

	public enum Mode { Mode2D, Mode3D }
	public static Mode CurrentMode { get; private set; } = Mode.Mode2D;
	public static bool In2DMode { get { return CurrentMode == Mode.Mode2D; } }
	public static bool In3DMode { get { return CurrentMode == Mode.Mode3D; } }

	public static bool GameStarted = false;

	public const string PlayerTag = "Player";
	public const string GroundTag = "Ground";
	public const string ScoreTag = "Score";
	public const string PipeTag = "Pipe";

	public static Action OnGameStart;
	public static Action OnScore;
	public static Action OnGameOver;

	public void InitializeSelf() {
		I = this;
	}

	public void Initialize() {
		BirdController.I.SetDeadlyTags(GroundTag, PipeTag);
		BirdController.I.OnDead += GameOver;
	}

	public static void Setup(Mode mode) {
		score = 0f;
		CurrentMode = mode;
		GameStarted = false;
		CameraHolder.I.Setup();
		BirdController.I.Revive();

		Debug.Log("Setup Game");
	}

	public static void StartGame() {
		if (!GameStarted) {
			GameStarted = true;
			OnGameStart?.Invoke();
			BirdController.I.Dash();

			Debug.Log("Game Started");
		}
	}

	public static void AddScore(float value) {
		score += value;
		OnScore?.Invoke();
	}

	private static void GameOver() {
		GameStarted = false;

		// save high score

		// show ads ?

		Debug.Log("Game Over");

		OnGameOver?.Invoke();
	}

#if UNITY_EDITOR
	public void EditorOnInspectorGUI() {
		if (GUILayout.Button("Refresh Tag Array")) {
			SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty tagsProperty = tagManager.FindProperty("tags");
			tagsProperty.ClearArray();
			tagManager.ApplyModifiedProperties();

			string[] tagsToAdd = { PlayerTag, GroundTag, ScoreTag, PipeTag };
			string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;

			for (int i = 0; i < tagsToAdd.Length; i++) {
				bool alreadyHas = false;
				string tagToAdd = tagsToAdd[i];
				for (int j = 0; j < allTags.Length; j++) {
					if (tagToAdd.Equals(allTags[j])) {
						alreadyHas = true;
						break;
					}
				}
				if (!alreadyHas) {
					tagsProperty.InsertArrayElementAtIndex(0);
					SerializedProperty t = tagsProperty.GetArrayElementAtIndex(0);
					t.stringValue = tagToAdd;
				}
			}

			tagManager.ApplyModifiedProperties();
		}

		if (GUILayout.Button("Setup 2D")) { Setup(Mode.Mode2D); }
		if (GUILayout.Button("Setup 3D")) { Setup(Mode.Mode3D); }

		if (GUILayout.Button("Start Game")) { StartGame(); }
	}
#endif

}