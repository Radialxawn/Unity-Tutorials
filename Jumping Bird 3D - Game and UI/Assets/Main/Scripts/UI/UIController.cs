#pragma warning disable 0649
#if UNITY_EDITOR
using UnityEditor;
#endif

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private Button play2DButton;
	[SerializeField] private Button play3DButton;

	[Header("------------------------------------")]
	[SerializeField] private GameObject inGame;
	[SerializeField] private TextMeshProUGUI inGameScoreText;
	[SerializeField] private GameplayInput gameplayInput;

	[Header("------------------------------------")]
	[SerializeField] private GameObject gameOver;
	[SerializeField] private TextMeshProUGUI gameOverScoreText;
	[SerializeField] private TextMeshProUGUI gameOverBestScoreText;
	[SerializeField] private Button mainMenuButton;
	[SerializeField] private Button replayButton;

	public void InitializeSelf() {
		play2DButton.onClick.AddListener(OnPlay2DButtonClick);
		play3DButton.onClick.AddListener(OnPlay3DButtonClick);
		mainMenuButton.onClick.AddListener(ShowMainMenu);
		replayButton.onClick.AddListener(OnReplayButtonClick);
	}

	public void Initialize() {
		GameController.OnGameStart += OnGameStart;
		GameController.OnGameOver += ShowGameOver;
		gameplayInput.DownAction += GameController.StartGame;
		gameplayInput.DownAction += BirdController.I.Jump;
		ShowMainMenu();
	}

	private void OnPlay2DButtonClick() {
		GameController.Setup(GameController.Mode.Mode2D);
		gameplayInput.SwipeLeftAction -= BirdController.I.TurnLeft;
		gameplayInput.SwipeRightAction -= BirdController.I.TurnRight;
		ShowInGame();
	}

	private void OnPlay3DButtonClick() {
		GameController.Setup(GameController.Mode.Mode3D);
		gameplayInput.SwipeLeftAction += BirdController.I.TurnLeft;
		gameplayInput.SwipeRightAction += BirdController.I.TurnRight;
		ShowInGame();
	}

	private void OnReplayButtonClick() {
		GameController.Setup(GameController.CurrentMode);
		ShowInGame();
	}

	private void OnGameStart() {
		inGameScoreText.gameObject.SetActive(true);
		inGameScoreText.text = "0";
	}

	private void ShowMainMenu() {
		mainMenu.SetActive(true);
		inGame.SetActive(false);
		gameOver.SetActive(false);
	}

	private void ShowInGame() {
		inGameScoreText.gameObject.SetActive(false);
		mainMenu.SetActive(false);
		inGame.SetActive(true);
		gameOver.SetActive(false);
	}

	private void ShowGameOver() {
		mainMenu.SetActive(false);
		inGame.SetActive(false);
		gameOver.SetActive(true);
	}

#if UNITY_EDITOR
	public void EditorOnInspectorGUI() {
		if (GUILayout.Button("Main Menu")) { ShowMainMenu(); }
		if (GUILayout.Button("In Game")) { ShowInGame(); }
		if (GUILayout.Button("Game Over")) { ShowGameOver(); }
	}
#endif

}