using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class NpcScreenBindings : MonoBehaviour
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private TMP_Text Age;
		[SerializeField] private TMP_Text Ambition;
		[SerializeField] private TMP_Text Pride;
		[SerializeField] private TMP_Text Education;
		[SerializeField] private TMP_Text Bio;
		[SerializeField] private Image Picture;
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform InfoBox;
		[SerializeField] private Transform InteractionsParent;

		[SerializeField] private NpcInteractionEntryBindings InteractionEntryPrefab;

		private Npc npc;
		private Action onClose;
		public void Setup(Npc npc, MainGameManager mgm, Action onClose)
		{
			this.npc = npc;
			this.onClose = onClose;

			this.npc.PersonalLayout.ApplyToRectTransform(Picture.GetComponent<RectTransform>());

			if (!npc.IsControllable)
				InfoBox.gameObject.SetActive(false);

			var allInteractions = new List<Interaction>(npc.Interactions);
			allInteractions.RemoveAll(i => i == null);
			allInteractions.Sort((i1, i2) =>
			{
				return ((int)i1.Category).CompareTo((int)i2.Category);
			});

			foreach (var interaction in allInteractions)
			{
				if (!interaction.InteractionVisible(mgm))
					continue;
				var interactButton = Instantiate(InteractionEntryPrefab);
				interactButton.Setup(interaction, mgm, this);
				interactButton.transform.SetParent(InteractionsParent);
			}

			Name.text = $"{this.npc.FirstName} {this.npc.LastName}";

			if (this.npc.Trained)
				Name.text += " (Trained)";
			else if (this.npc.Controlled)
				Name.text += " (Controlled)";

			Age.text = $"{this.npc.Age} years old";
			Ambition.text = $"Ambition: {this.npc.Ambition}";
			Pride.text = $"Pride: {this.npc.Pride}";
			Picture.sprite = this.npc.GetCurrentPicture().ToSprite();
			Picture.preserveAspect = true;
			Bio.text = $"Notes: {this.npc.Bio}";
			Education.text = $"Education: {this.npc.Education}";
			BackgroundImage.sprite = this.npc.BackgroundImage.ToSprite();
		}

		public void CloseNpc()
		{
			GameObject.Destroy(gameObject);
			onClose();
		}
	}
}