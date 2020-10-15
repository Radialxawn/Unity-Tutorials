#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class CameraHolder : MonoBehaviour {
	public static CameraHolder I;

	[SerializeField] private float speed = 0.1f;
	[SerializeField] private Vector3 offset2D;
	[SerializeField] private Vector3 viewpointOffset2D;
	[SerializeField] private Vector3 offset3D;
	[SerializeField] private Vector3 viewpointOffset3D;

	private Camera zCamera;
	private Transform target;
	private bool in2DMode;

	public void InitializeSelf() {
		I = this;
		zCamera = GetComponentInChildren<Camera>();
	}

	public void Initialize() {
		target = BirdController.I.transform;
	}

	public void Setup() {
		in2DMode = GameController.In2DMode;
		if (in2DMode) {
			SetTo2DView();
		} else {
			SetTo3DView();
		}
	}

	private void SetTo2DView() {
		zCamera.orthographic = in2DMode = true;
		transform.position = target.position + offset2D;
		transform.rotation = Quaternion.LookRotation(offset2D.To(viewpointOffset2D), Vector3.up);
	}

	private void SetTo3DView() {
		zCamera.orthographic = in2DMode = false;
		transform.position = target.position + offset3D;
		transform.rotation = Quaternion.LookRotation(offset3D.To(viewpointOffset3D), Vector3.up);
	}

	private void LateUpdate() {
		if (in2DMode) {
			transform.position = transform.position.SetZ(target.position.z + offset2D.z);
			transform.rotation = Quaternion.LookRotation(offset2D.To(viewpointOffset2D), Vector3.up);
		} else {
			transform.position = Vector3.Lerp(transform.position, target.position + offset3D, speed);
			transform.rotation = Quaternion.LookRotation(offset3D.To(viewpointOffset3D), Vector3.up);
		}
	}

#if UNITY_EDITOR
	public void EditorOnInspectorGUI() {
		if (GUILayout.Button("InitializeSelf")) {
			InitializeSelf();
		}
		if (GUILayout.Button("SetTo2DView")) {
			SetTo2DView();
		}
		if (GUILayout.Button("SetTo3DView")) {
			SetTo3DView();
		}
		if (GUILayout.Button("Find bird as target")) {
			var bird = FindObjectOfType<BirdController>();
			if (bird != null) {
				target = bird.transform;
			} else {
				Debug.Log("No bird out there");
			}
		}
	}

	public void EditorOnSceneGUI() {
		if (target != null) {
			bool changed = false;
			Handles.color = Color.red;
			Vector3 viewpoint = target.position + (in2DMode ? viewpointOffset2D : viewpointOffset3D);
			EditorGUI.BeginChangeCheck();
			viewpoint = Handles.FreeMoveHandle(viewpoint, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
			if (EditorGUI.EndChangeCheck()) {
				Vector3 viewpointOffset = viewpoint - target.position;
				Undo.RecordObject(this, "Viewpoint Offset");
				if (in2DMode) { viewpointOffset2D = viewpointOffset; } else { viewpointOffset3D = viewpointOffset; }
				changed = true;
			}
			Handles.DrawLine(target.position, viewpoint);

			Handles.color = Color.green;
			Vector3 position = transform.position;
			EditorGUI.BeginChangeCheck();
			position = Handles.FreeMoveHandle(position, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
			if (EditorGUI.EndChangeCheck()) {
				Vector3 offset = position - target.position;
				Undo.RecordObject(this, "Offset");
				if (in2DMode) { offset2D = offset; } else { offset3D = offset; }
				changed = true;
			}
			Handles.DrawLine(target.position, position);

			if (changed) {
				if (in2DMode) { SetTo2DView(); } else { SetTo3DView(); }
			}
		}
	}
#endif

}