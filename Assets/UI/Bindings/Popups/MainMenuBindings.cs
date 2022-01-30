using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBindings : MonoBehaviour
{
	[SerializeField] private LoadSaveMenuBindings LoadSavePrefab;
	private LoadSaveMenuBindings loadSavePrefab;

	private MainGameManager mgm;
	public void Setup(MainGameManager mgm)
	{
		this.mgm = mgm;
	}

	public void SaveGame()
	{
		loadSavePrefab = GameObject.Instantiate(LoadSavePrefab, transform.parent);
		loadSavePrefab.Setup(mgm, true);
	}

	public void LoadGame()
	{
		loadSavePrefab = GameObject.Instantiate(LoadSavePrefab, transform.parent);
		loadSavePrefab.Setup(mgm, false);
	}

	public void CloseMenu()
	{
		GameObject.Destroy(transform.parent.gameObject);
	}

	public void QuitGame()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
