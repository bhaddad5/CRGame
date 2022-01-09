using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.UI;

public class StartingMenuBindings : MonoBehaviour
{
	[SerializeField] private MainGameManager MainGameManager;

	[SerializeField] private LoadSaveMenuBindings LoadSavePrefab;
	[SerializeField] private PlayerNamePickerBindings NamePickerPrefab;

	[SerializeField] private Button ContinueGameButton;
	private string latestSave = null;
	void Awake()
	{
		latestSave = LoadSaveHelpers.GetOrderedSaveFiles().FirstOrDefault();
		ContinueGameButton.interactable = latestSave != null;
	}

	public void NewGame()
	{
		var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
		var namePicker = GameObject.Instantiate(NamePickerPrefab, popupParent.transform);
		namePicker.Setup(MainGameManager.Data.PlayerName, (playerName) =>
		{
			MainGameManager.InitializeGame(null, playerName);
			GameObject.Destroy(gameObject);
		});
	}

	public void LoadGame()
	{
		//var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
		var loadSavePrefab = GameObject.Instantiate(LoadSavePrefab, transform);
		loadSavePrefab.Setup(MainGameManager, false);
	}

	public void ContinueGame()
	{
		MainGameManager.InitializeGame(latestSave, null);
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
