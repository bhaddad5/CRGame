using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public class StartingMenu : MonoBehaviour
{
	[SerializeField] private MainGameManager MainGameManager;

	[SerializeField] private LoadSaveMenuManager LoadSavePrefab;
	private LoadSaveMenuManager loadSavePrefab;

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

	public void Quit()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
