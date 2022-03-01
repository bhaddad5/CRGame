using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingMenuBindings : MonoBehaviour
{
	[SerializeField] private MainGameManager MainGameManager;

	[SerializeField] private LoadSaveMenuBindings LoadSavePrefab;
	[SerializeField] private PlayerNamePickerBindings NamePickerPrefab;

	[SerializeField] private Button ContinueGameButton;

	[SerializeField] private TMP_Text VersionText;

	private string latestSave = null;
	void Awake()
	{
		latestSave = LoadSaveHelpers.GetOrderedSaveFiles().FirstOrDefault();
		ContinueGameButton.interactable = latestSave != null;
		VersionText.text = $"Company Man v{MainGameManager.MajorVersion}.{MainGameManager.MinorVersion}.{MainGameManager.Patch} \"{MainGameManager.VersionName}\"";
	}

	public void NewGame()
	{
		var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
		var namePicker = GameObject.Instantiate(NamePickerPrefab, popupParent.transform);
		namePicker.Setup(MainGameManager.DefaultFirstName, MainGameManager.DefaultLastName, (firstName, lastName) =>
		{
			MainGameManager.InitializeGame(null, firstName, lastName);
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
		MainGameManager.InitializeGame(latestSave, null, null);
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
