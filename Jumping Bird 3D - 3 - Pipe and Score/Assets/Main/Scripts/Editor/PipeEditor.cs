using UnityEditor;

[CustomEditor(typeof(Pipe))]
public class PipeEditor : Editor {
	Pipe t;

	private void OnEnable() {
		t = target as Pipe;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		t.EditorOnInspectorGUI();
	}

	private void OnSceneGUI() {
		t.EditorOnSceneGUI();
	}
}
