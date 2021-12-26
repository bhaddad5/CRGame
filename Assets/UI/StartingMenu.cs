using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.UI;

public class StartingMenu : MonoBehaviour
{
	[SerializeField] private MainGameManager MainGameManager;

	[SerializeField] private LoadSaveMenuManager LoadSavePrefab;
	private LoadSaveMenuManager loadSavePrefab;

	[SerializeField] private Button ContinueGameButton;
	private string latestSave = null;
	void Awake()
	{
		latestSave = LoadSaveHelpers.GetOrderedSaveFiles().FirstOrDefault();
		ContinueGameButton.interactable = latestSave != null;
	}

	public void NewGame()
	{
		MainGameManager.InitializeGame(null);
		GameObject.Destroy(gameObject);
	}

	public void LoadGame()
	{
		loadSavePrefab = GameObject.Instantiate(LoadSavePrefab, transform);
		loadSavePrefab.Setup(MainGameManager, false);
	}

	public void ContinueGame()
	{
		MainGameManager.InitializeGame(latestSave);
		GameObject.Destroy(gameObject);
	}

	public void Quit()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
