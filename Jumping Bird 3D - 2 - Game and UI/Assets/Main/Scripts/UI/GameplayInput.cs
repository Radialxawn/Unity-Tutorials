using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Image))]
public class GameplayInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
	[SerializeField] private float minSwipeDistance = 0.1f;

	private Vector2 downPosition;
	private bool upOrExited = false;

	public void OnPointerDown(PointerEventData eventData) {
		downPosition = eventData.position;
		upOrExited = false;
		DownAction?.Invoke();
		Debug.Log("Down at " + downPosition);
	}

	public void OnPointerUp(PointerEventData eventData) {
		OnPointerUpOrExit(eventData);
	}

	public void OnPointerExit(PointerEventData eventData) {
		OnPointerUpOrExit(eventData);
	}

	private void OnPointerUpOrExit(PointerEventData eventData) {
		if (upOrExited) { return; }
		Vector2 deltaPosition = (eventData.position - downPosition) / Screen.width;
		float sqrDistance = deltaPosition.sqrMagnitude;

		if (sqrDistance > minSwipeDistance * minSwipeDistance) {
			if (deltaPosition.x < 0f) {
				SwipeLeftAction?.Invoke();
				Debug.Log("Swipe Left");
			} else {
				SwipeRightAction?.Invoke();
				Debug.Log("Swipe Right");
			}
		}

		upOrExited = true;
	}

	public Action DownAction;
	public Action SwipeLeftAction;
	public Action SwipeRightAction;

#if UNITY_EDITOR
	private void Update() {
		if (Input.GetKeyDown(KeyCode.A)) { SwipeLeftAction?.Invoke(); }
		if (Input.GetKeyDown(KeyCode.S)) { DownAction?.Invoke(); }
		if (Input.GetKeyDown(KeyCode.D)) { SwipeRightAction?.Invoke(); }
	}
#endif

}