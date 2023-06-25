using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBindings : MonoBehaviour
{
	[SerializeField] private LoadSaveMenuBindings LoadSavePrefab;
	private LoadSaveMenuBindings loadSaveMenu;

	[SerializeField] private SettingsBindings SettingsPrefab;
	private SettingsBindings settingsMenu;

	private MainGameManager mgm;
	public void Setup(MainGameManager mgm)
	{
		this.mgm = mgm;
	}

	public void SaveGame()
	{
		if (mgm.DebugAll)
			return;
		loadSaveMenu = GameObject.Instantiate(LoadSavePrefab, transform.parent);
		loadSaveMenu.Setup(mgm, true, gameObject, null);
	}

	public void LoadGame()
	{
		if (mgm.DebugAll)
			return;

		loadSaveMenu = GameObject.Instantiate(LoadSavePrefab, transform.parent);
		loadSaveMenu.Setup(mgm, false, gameObject, transform.parent.gameObject);
	}

	public void SettingsMenu()
	{
		settingsMenu = GameObject.Instantiate(SettingsPrefab, transform.parent);
		settingsMenu.Setup();
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
