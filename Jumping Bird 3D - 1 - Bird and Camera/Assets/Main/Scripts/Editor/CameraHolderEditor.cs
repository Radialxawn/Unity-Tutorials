using UnityEditor;

[CustomEditor(typeof(CameraHolder))]
public class CameraHolderEditor : Editor {
	CameraHolder t;

	private void OnEnable() {
		t = target as CameraHolder;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		t.EditorOnInspectorGUI();
	}

	private void OnSceneGUI() {
		t.EditorOnSceneGUI();
	}
}