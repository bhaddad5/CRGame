using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel.Save
{
	public static class SaveLoadHandler
	{
		public static string SaveToJson(GameData data)
		{
			SaveGameState state = SaveGameState.FromData(data);
			
			return JsonUtility.ToJson(state);
		}


		public static void LoadAndApplyToGameData(string saveJson, GameData data)
		{
			SaveGameState save = JsonUtility.FromJson<SaveGameState>(saveJson);
			save.ApplyToData(data);
		}
	}
}