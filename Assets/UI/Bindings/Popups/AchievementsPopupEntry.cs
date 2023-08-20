using Assets.GameModel;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsPopupEntry : MonoBehaviour
{
	[SerializeField] private Button ExpandButton;

	[SerializeField] private TMP_Text Title;
	[SerializeField] private TMP_Text Description;
	[SerializeField] private Image Image;

	[SerializeField] private Sprite NotYetAchievedImage;

	private Achievement achievement;
	private MainGameManager mgm;
	public void Setup(Achievement achievement, MainGameManager mgm)
	{
		this.mgm = mgm;
		this.achievement = achievement;

		ExpandButton.interactable = achievement.Completed;

		Title.text = $"{achievement.Name}";
		
		if (achievement.Completed)
		{
			Image.sprite = achievement.Image.ToSprite();
			Description.text = $"{achievement.Description}";
		}
		else
		{
			Image.sprite = NotYetAchievedImage;
			Description.text = achievement.ToUnlockDescription;
		}
	}

	public void OpenAchievementPopup()
	{
		if (!achievement.Completed)
			return;

		var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
		Popup popup = new Popup();
		popup.Title = achievement.Name;
		popup.Text = achievement.Description;
		popup.Textures = new List<Texture2D>() { achievement.Image };
		GameObject.Instantiate(UiPrefabReferences.Instance.GetPrefabByName("Popup Display"), popupParent.transform).GetComponent<PopupBindings>().Setup(popup, 0, mgm, () => {});
	}
}
