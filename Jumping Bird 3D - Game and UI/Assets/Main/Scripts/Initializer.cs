#pragma warning disable 0649

using UnityEngine;

public class Initializer : MonoBehaviour {
	[SerializeField] private BirdController birdController;
	[SerializeField] private CameraHolder cameraHolder;

	private void Start() {
		birdController.InitializeSelf();
		cameraHolder.InitializeSelf();

		cameraHolder.Initialize();
	}
}
