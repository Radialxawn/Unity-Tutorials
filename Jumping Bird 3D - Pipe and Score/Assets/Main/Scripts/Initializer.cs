#pragma warning disable 0649

using UnityEngine;

public class Initializer : MonoBehaviour {
	[SerializeField] private GameController gameController;
	[SerializeField] private UIController uIController;
	[SerializeField] private BirdController birdController;
	[SerializeField] private CameraHolder cameraHolder;

	private void Start() {
		gameController.InitializeSelf();
		uIController.InitializeSelf();
		birdController.InitializeSelf();
		cameraHolder.InitializeSelf();

		gameController.Initialize();
		uIController.Initialize();
		cameraHolder.Initialize();

		GameController.Setup(GameController.Mode.Mode2D);
	}
}