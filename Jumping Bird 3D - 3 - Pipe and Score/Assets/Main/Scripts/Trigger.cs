using UnityEngine;

public class Trigger : MonoBehaviour {
	public Callback Enter;
	private void OnTriggerEnter(Collider other) {
		Enter?.Invoke(other);
	}

	public Callback Stay;
	private void OnTriggerStay(Collider other) {
		Stay?.Invoke(other);
	}

	public Callback Exit;
	private void OnTriggerExit(Collider other) {
		Exit?.Invoke(other);
	}

	public delegate void Callback(Collider other);
}