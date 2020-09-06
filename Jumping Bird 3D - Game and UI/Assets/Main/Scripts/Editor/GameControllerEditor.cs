using UnityEditor;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor {
	GameController t;

	private void OnEnable() {
		t = target as GameController;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		t.EditorOnInspectorGUI();
	}
}