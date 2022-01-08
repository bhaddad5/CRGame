using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiDisplay : MonoBehaviour
{
	[SerializeField] private LoadSaveMenuManager LoadSavePrefab;
	private LoadSaveMenuManager loadSavePrefab;

	private MainGameManager mgm;
	public void Setup(MainGameManager mgm)
	{
		this.mgm = mgm;
	}

	public void SaveGame()
	{
		loadSavePrefab = GameObject.Instantiate(LoadSavePrefab, transform);
		loadSavePrefab.Setup(mgm, true);
	}

	public void LoadGame()
	{
		loadSavePrefab = GameObject.Instantiate(LoadSavePrefab, transform);
		loadSavePrefab.Setup(mgm, false);
	}

	public void CloseMenu()
	{
		GameObject.Destroy(gameObject);
	}

	public void QuitGame()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
