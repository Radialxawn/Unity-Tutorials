#pragma warning disable 0649
#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Assertions.Must;

public class PipePair : MonoBehaviour {
	[SerializeField] Pipe top;
	[SerializeField] Pipe bot;
	[SerializeField] private Trigger middleTrigger;
	[SerializeField] private CapsuleCollider middleCapsule;
	[SerializeField] private PipePairData pipePairData;

	private float targetDistance;
	private float currentDistance;

	private Vector3 suckingAcceleration;
	private bool isSucking = false;

	private Coroutine fadeCoroutine;
	private float fadeSpeed;
	public bool IsFading { get; private set; }

	public void InitializeSelf() {
		middleTrigger.Enter += MiddleTriggerEnter;
		middleTrigger.Stay += MiddleTriggerStay;
	}

	private void Start() {
		InitializeSelf();
	}

	private void MiddleTriggerEnter(Collider other) {
		if (other.CompareTag(GameController.PlayerTag)) {
			if (GameController.GameStarted) {
				GameController.AddScore(1f);
			}
		}
	}

	private void MiddleTriggerStay(Collider other) {
		if (isSucking) {
			if (other.CompareTag(GameController.PlayerTag)) {
				other.attachedRigidbody.AddForce(suckingAcceleration, ForceMode.Acceleration);
				Debug.Log(name + " -> Sucking");
			}
		}
	}

	private void Open() {
		top.Open();
		bot.Open();
		targetDistance = pipePairData.OpenDistance;
	}

	private void Suck(bool up) {
		top.Suck(up);
		bot.Suck(!up);
		suckingAcceleration.y = up ? pipePairData.SuckUpAcceleration : pipePairData.SuckDownAcceleration;
		targetDistance = pipePairData.SuckingDistance;
	}

	private void Close() {
		top.Close();
		bot.Close();
		targetDistance = pipePairData.CloseDistance;
	}

	private void FadeToTargetDistance(float deltaTime) {
		currentDistance = Mathf.Lerp(currentDistance, targetDistance, deltaTime * fadeSpeed);
		top.transform.localPosition = Vector3.zero.SetY(+0.5f * currentDistance);
		bot.transform.localPosition = Vector3.zero.SetY(-0.5f * currentDistance);
	}

	private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
	private IEnumerator FadeCoroutine() {
		while (!currentDistance.Approximately(targetDistance, 1e-2f)) {
			FadeToTargetDistance(Time.deltaTime);
			yield return waitForEndOfFrame;
		}
		MoveToTargetDistanceDirectly();
		yield break;
	}

	private void StartFade() {
		if (Application.isPlaying && gameObject.activeInHierarchy) {
			if (fadeCoroutine != null) { StopCoroutine(fadeCoroutine); }
			fadeCoroutine = StartCoroutine(FadeCoroutine());
			IsFading = true;
		}
	}

	public void MoveToTargetDistanceDirectly() {
		currentDistance = targetDistance;
		middleCapsule.height = currentDistance;
		top.transform.localPosition = Vector3.zero.SetY(+0.5f * currentDistance);
		bot.transform.localPosition = Vector3.zero.SetY(-0.5f * currentDistance);
	}

	public void FadeIn(bool directly) {
		fadeSpeed = pipePairData.FadeInSpeed;
		if (directly) { MoveToTargetDistanceDirectly(); } else { StartFade(); }
	}

	public void FadeOut(bool directly) {
		targetDistance = pipePairData.OutDistance;
		fadeSpeed = pipePairData.FadeOutSpeed;
		if (directly) { MoveToTargetDistanceDirectly(); } else { StartFade(); }
	}

	public void SetNewTargetState(bool canClose) {
		isSucking = Random.Range(0f, 1f) < pipePairData.SuckRate;
		if (isSucking) {
			Suck(Random.Range(0f, 1f) < 0.5f ? true : false);
		} else {
			if (canClose ? Random.Range(0f, 1f) < pipePairData.CloseRate : false) { Close(); } else { Open(); }
		}
	}

#if UNITY_EDITOR
	public void EditorOnInspectorGUI() {
		bool distanceChanged = false;
		if (GUILayout.Button("Open")) {
			Open();
			distanceChanged = true;
		}
		if (GUILayout.Button("Suck")) {
			Suck(Random.Range(0f, 1f) < 0.5f);
			distanceChanged = true;
		}
		if (GUILayout.Button("Close")) {
			Close();
			distanceChanged = true;
		}
		if (GUILayout.Button("SetNewTargetState(canClose = true)")) {
			SetNewTargetState(true);
			distanceChanged = true;
		}
		if (GUILayout.Button("SetNewTargetState(canClose = false)")) {
			SetNewTargetState(false);
			distanceChanged = true;
		}
		if (Application.isPlaying) {
			if (GUILayout.Button("FadeIn")) { FadeIn(false); }
			if (GUILayout.Button("FadeOut")) { FadeOut(false); }
		} else {
			if (distanceChanged) {
				MoveToTargetDistanceDirectly();
			}
		}
	}
#endif

}