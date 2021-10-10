using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedLocation
	{
		public string Id;
		public string Name;

		public Vector2 UiPosition;

		public bool ClosedOnWeekends;
		public bool Accessible;
		public bool ShowTrophyCase;
		public bool ShowCar;

		public Texture2D Icon;
		public Texture2D BackgroundImage;

		public List<SerializedPolicy> Policies;
		public List<SerializedMission> Missions;
		public List<SerializedNpc> Npcs;

		public static SerializedLocation Serialize(Location ob)
		{
			List<SerializedNpc> npcs = new List<SerializedNpc>();
			foreach (var npc in ob.Npcs)
			{
				npcs.Add(SerializedNpc.Serialize(npc));
			}

			List<SerializedMission> missions = new List<SerializedMission>();
			foreach (var mission in ob.Missions)
			{
				missions.Add(SerializedMission.Serialize(mission));
			}

			List<SerializedPolicy> policies = new List<SerializedPolicy>();
			foreach (var policy in ob.Policies)
			{
				policies.Add(SerializedPolicy.Serialize(policy));
			}

			return new SerializedLocation()
			{
				Id = ob.Id,
				Name = ob.Name,
				ClosedOnWeekends = ob.ClosedOnWeekends,
				Accessible = ob.Accessible,
				Npcs = npcs,
				Policies = policies,
				Missions = missions,
				UiPosition = ob.UiPosition,
				BackgroundImage = ob.BackgroundImage,
				Icon = ob.Icon,
				ShowCar = ob.ShowCar,
				ShowTrophyCase = ob.ShowTrophyCase,
			};
		}

		public static Location Deserialize(SerializedLocation ob)
		{
			List<Npc> npcs = new List<Npc>();
			foreach (var npc in ob.Npcs)
			{
				npcs.Add(SerializedNpc.Deserialize(npc));
			}

			List<Mission> missions = new List<Mission>();
			foreach (var mission in ob.Missions)
			{
				missions.Add(SerializedMission.Deserialize(mission));
			}

			List<Policy> policies = new List<Policy>();
			foreach (var policy in ob.Policies)
			{
				policies.Add(SerializedPolicy.Deserialize(policy));
			}

			var res = ScriptableObject.CreateInstance<Location>();
			res.Id = ob.Id;
			res.Name = ob.Name;
			res.ClosedOnWeekends = ob.ClosedOnWeekends;
			res.Accessible = ob.Accessible;
			res.Npcs = npcs;
			res.Policies = policies;
			res.Missions = missions;
			res.UiPosition = ob.UiPosition;
			res.BackgroundImage = ob.BackgroundImage;
			res.Icon = ob.Icon;
			res.ShowCar = ob.ShowCar;
			res.ShowTrophyCase = ob.ShowTrophyCase;

			return res;
		}

		public static Location ResolveReferences(DeserializedDataAccessor deserializer, Location data, SerializedLocation ob)
		{
			for (int i = 0; i < data.Npcs.Count; i++)
			{
				data.Npcs[i] = SerializedNpc.ResolveReferences(deserializer, data.Npcs[i], ob.Npcs[i]);
			}

			for (int i = 0; i < data.Missions.Count; i++)
			{
				data.Missions[i] = SerializedMission.ResolveReferences(deserializer, data.Missions[i], ob.Missions[i]);
			}

			for (int i = 0; i < data.Policies.Count; i++)
			{
				data.Policies[i] = SerializedPolicy.ResolveReferences(deserializer, data.Policies[i], ob.Policies[i]);
			}

			return data;
		}
	}
}