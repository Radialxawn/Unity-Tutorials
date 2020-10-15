using UnityEditor;

[CustomEditor(typeof(PipePair))]
public class PipePairEditor : Editor {
	PipePair t;

	private void OnEnable() {
		t = target as PipePair;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		t.EditorOnInspectorGUI();
	}

	private void OnSceneGUI() {
		
	}
}
