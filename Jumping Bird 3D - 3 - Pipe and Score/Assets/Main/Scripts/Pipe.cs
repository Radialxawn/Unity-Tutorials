#pragma warning disable 0649
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Pipe : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private Renderer normalRenderer;
	[SerializeField] private Renderer skinnedRenderer;

	private void ChangeRenderer(bool normal) {
		normalRenderer.gameObject.SetActive(normal);
		skinnedRenderer.gameObject.SetActive(!normal);
		animator.enabled = !normal;
	}

	public void Open() {
		ChangeRenderer(true);
	}

	public void Suck(bool playAnimation) {
		if (playAnimation) {
			ChangeRenderer(false);
		} else {
			ChangeRenderer(true);
		}
	}

	public void Close() {
		ChangeRenderer(true);
		// the same as Open() function but this is for later use
	}

#if UNITY_EDITOR
	[Header("Editor properties")]
	[SerializeField] private Transform editorColliderGroup;
	[SerializeField] private int editorColliderCount = 5;
	[SerializeField] private float editorColliderRadius = 0.02f;
	[SerializeField] private Vector3 editorStartPoint, editorEndPoint;

	public void EditorOnInspectorGUI() {
		if (GUILayout.Button("Generate Colliders")) {
			for (int i = editorColliderGroup.childCount - 1; i > -1; i--) {
				DestroyImmediate(editorColliderGroup.GetChild(i).gameObject);
			}
			for (int i = 0; i < editorColliderCount; i++) {
				GameObject go = new GameObject("CapsuleCollider", typeof(CapsuleCollider));
				go.transform.SetParent(editorColliderGroup);
			}
			EditorUtility.SetDirty(this);
		}

		if (GUILayout.Button("Arrange Colliders")) {
			Vector3 firstDirection = (editorEndPoint - editorStartPoint).normalized;
			Vector3 firstCenter = (editorStartPoint + editorEndPoint) * 0.5f;
			for (int i = 0; i < editorColliderGroup.childCount; i++) {
				Transform child = editorColliderGroup.GetChild(i);
				CapsuleCollider cc = child.GetComponent<CapsuleCollider>();
				cc.radius = editorColliderRadius;
				cc.height = (editorStartPoint - editorEndPoint).magnitude + editorColliderRadius * 2f;
				Quaternion rotator = Quaternion.AngleAxis(i * 360f / editorColliderCount, Vector3.up);
				child.position = rotator * firstCenter;
				child.rotation = Quaternion.LookRotation(rotator * firstDirection) * Quaternion.AngleAxis(90f, Vector3.right);
				child.gameObject.tag = GameController.PipeTag;
			}
			EditorUtility.SetDirty(this);
		}

		if (GUILayout.Button("Open")) { Open(); }
		if (GUILayout.Button("Suck(PlayAnimation = true)")) { Suck(true); }
		if (GUILayout.Button("Suck(PlayAnimation = false)")) { Suck(false); }
		if (GUILayout.Button("Close")) { Close(); }
	}

	public void EditorOnSceneGUI() {
		Handles.color = Color.red;
		EditorGUI.BeginChangeCheck();
		editorStartPoint = Handles.FreeMoveHandle(editorStartPoint, Quaternion.identity, 2f * editorColliderRadius, Vector3.zero, Handles.SphereHandleCap);
		editorEndPoint = Handles.FreeMoveHandle(editorEndPoint, Quaternion.identity, 2f * editorColliderRadius, Vector3.zero, Handles.SphereHandleCap);
		if (EditorGUI.EndChangeCheck()) {
			editorStartPoint.x = editorEndPoint.x = 0f;
			EditorUtility.SetDirty(this);
		}
	}
#endif

}

















































































/*
#pragma warning disable 0649
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Pipe : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private Renderer normalRenderer;
	[SerializeField] private Renderer skinnedRenderer;
	[SerializeField] private ParticleSystem suckingWind;
	[SerializeField] private PipeData pipeData;

	private MaterialPropertyBlock materialPropertyBlock;

	public void InitializeSelf() {
		materialPropertyBlock = new MaterialPropertyBlock();
		normalRenderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetFloat("_ShadeScale", pipeData.shadeScale);
		ApplyMaterialProperties();
	}

	private void ApplyMaterialProperties() {
		if (normalRenderer.gameObject.activeSelf) { normalRenderer.SetPropertyBlock(materialPropertyBlock); }
		if (skinnedRenderer.gameObject.activeSelf) { skinnedRenderer.SetPropertyBlock(materialPropertyBlock); }
	}

	private void SetTextureOffset(Vector2 offset) {
		materialPropertyBlock.SetVector("_ColorTex_ST", new Vector4(1f, 1f, offset.x, offset.y));
		ApplyMaterialProperties();
	}

	public void Open() {
		normalRenderer.gameObject.SetActive(true);
		skinnedRenderer.gameObject.SetActive(false);
		suckingWind.gameObject.SetActive(false);
		SetTextureOffset(pipeData.openTextureOffset);
	}

	public void Suck(bool playAnimation) {
		if (playAnimation) {
			normalRenderer.gameObject.SetActive(false);
			skinnedRenderer.gameObject.SetActive(true);
			suckingWind.gameObject.SetActive(true);
			suckingWind.Play();
		} else {
			normalRenderer.gameObject.SetActive(true);
			skinnedRenderer.gameObject.SetActive(false);
			suckingWind.gameObject.SetActive(false);
		}
		SetTextureOffset(pipeData.suckTextureOffset);
	}

	public void Close() {
		normalRenderer.gameObject.SetActive(true);
		skinnedRenderer.gameObject.SetActive(false);
		suckingWind.gameObject.SetActive(false);
		SetTextureOffset(pipeData.closeTextureOffset);
	}

#if UNITY_EDITOR
	[Header("Editor properties")]
	[SerializeField] private Transform editorColliderGroup;
	[SerializeField] private int editorColliderCount = 5;
	[SerializeField] private float editorColliderRadius = 0.02f;
	[SerializeField] private Vector3 editorStartPoint, editorEndPoint;

	public void EditorOnInspectorGUI() {
		if (GUILayout.Button("Generate Colliders")) {
			for (int i = editorColliderGroup.childCount - 1; i > -1; i--) {
				DestroyImmediate(editorColliderGroup.GetChild(i).gameObject);
			}
			for (int i = 0; i < editorColliderCount; i++) {
				GameObject go = new GameObject("CapsuleCollider", typeof(CapsuleCollider));
				go.transform.SetParent(editorColliderGroup);
			}
			EditorUtility.SetDirty(this);
		}

		if (GUILayout.Button("Arrange Colliders")) {
			Vector3 firstDirection = (editorEndPoint - editorStartPoint).normalized;
			Vector3 firstCenter = (editorStartPoint + editorEndPoint) * 0.5f;
			for (int i = 0; i < editorColliderGroup.childCount; i++) {
				Transform child = editorColliderGroup.GetChild(i);
				CapsuleCollider cc = child.GetComponent<CapsuleCollider>();
				cc.radius = editorColliderRadius;
				cc.height = (editorStartPoint - editorEndPoint).magnitude + editorColliderRadius * 2f;
				Quaternion rotator = Quaternion.AngleAxis(i * 360f / editorColliderCount, Vector3.up);
				child.position = rotator * firstCenter;
				child.rotation = Quaternion.LookRotation(rotator * firstDirection) * Quaternion.AngleAxis(90f, Vector3.right);
				child.gameObject.tag = GameController.PipeTag;
			}
			EditorUtility.SetDirty(this);
		}

		if (GUILayout.Button("InitializeSelf")) { InitializeSelf(); }
		if (GUILayout.Button("Open")) { Open(); }
		if (GUILayout.Button("Suck(PlayAnimation = true)")) { Suck(true); }
		if (GUILayout.Button("Suck(PlayAnimation = false)")) { Suck(false); }
		if (GUILayout.Button("Close")) { Close(); }
	}

	public void EditorOnSceneGUI() {
		Handles.color = Color.red;
		EditorGUI.BeginChangeCheck();
		editorStartPoint = Handles.FreeMoveHandle(editorStartPoint, Quaternion.identity, 2f * editorColliderRadius, Vector3.zero, Handles.SphereHandleCap);
		editorEndPoint = Handles.FreeMoveHandle(editorEndPoint, Quaternion.identity, 2f * editorColliderRadius, Vector3.zero, Handles.SphereHandleCap);
		if (EditorGUI.EndChangeCheck()) {
			editorStartPoint.x = editorEndPoint.x = 0f;
			EditorUtility.SetDirty(this);
		}
	}
#endif
}
*/
