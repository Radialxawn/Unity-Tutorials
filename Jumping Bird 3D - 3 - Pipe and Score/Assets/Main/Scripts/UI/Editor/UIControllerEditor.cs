using UnityEditor;

[CustomEditor(typeof(UIController))]
public class UIControllerEditor : Editor {
	UIController t;

	private void OnEnable() {
		t = target as UIController;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		t.EditorOnInspectorGUI();
	}
}