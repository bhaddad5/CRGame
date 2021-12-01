using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.GameModel.Save
{
	public static class SaveLoadHandler
	{
		public static string SaveToJson(GameData data)
		{
			SaveGameState state = SaveGameState.FromData(data);
			
			var res = JsonUtility.ToJson(state);

			Debug.Log(res);

			var test = JsonUtility.FromJson<SaveGameState>(res);

			return res;
		}


		public static void LoadAndApplyToGameData(string saveFile, GameData data)
		{
			string saveJson = File.ReadAllText(saveFile);

			SaveGameState save = JsonUtility.FromJson<SaveGameState>(saveJson);
			save.ApplyToData(data);
		}
	}
}