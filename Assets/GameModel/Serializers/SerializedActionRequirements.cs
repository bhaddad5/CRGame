using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedActionRequirements
	{
		public float RequiredPower;

		//If there is a npc
		public bool RequiredControl;
		public bool RequiresAmbitionAtOrBelowValue;
		public float RequiredAmbition;
		public bool RequiresPrideAtOrBelowValue;
		public float RequiredPride;

		public List<string> RequiredInteractionsReferences;
		public List<string> RequiredNotCompletedInteractionsReferences;
		public List<string> RequiredPoliciesReferences;
		public List<string> RequiredDepartmentsControledReferences;
		public List<string> RequiredTrophiesReferences;

		public static SerializedActionRequirements Serialize(ActionRequirements ob)
		{
			List<string> interactions = new List<string>();
			foreach (var interaction in ob.RequiredInteractions)
			{
				interactions.Add(interaction.Id);
			}

			List<string> exclusiveInteractions = new List<string>();
			foreach (var interaction in ob.RequiredNotCompletedInteractions)
			{
				exclusiveInteractions.Add(interaction.Id);
			}

			List<string> policies = new List<string>();
			foreach (var policy in ob.RequiredPolicies)
			{
				policies.Add(policy.Id);
			}

			List<string> departments = new List<string>();
			foreach (var location in ob.RequiredDepartmentsControled)
			{
				departments.Add(location.Id);
			}

			List<string> trophies = new List<string>();
			foreach (var trophy in ob.RequiredTrophies)
			{
				trophies.Add(trophy.Id);
			}

			return new SerializedActionRequirements()
			{
				RequiredInteractionsReferences = interactions,
				RequiredNotCompletedInteractionsReferences = exclusiveInteractions,
				RequiredPoliciesReferences = policies,
				RequiredDepartmentsControledReferences = departments,
				RequiredTrophiesReferences = trophies,
				RequiresAmbitionAtOrBelowValue = ob.RequiresAmbitionAtOrBelowValue,
				RequiredAmbition = ob.RequiredAmbition,
				RequiresPrideAtOrBelowValue = ob.RequiresPrideAtOrBelowValue,
				RequiredPride = ob.RequiredPride,
				RequiredControl = ob.RequiredControl,
				RequiredPower = ob.RequiredPower,
			};
		}

		public static ActionRequirements Deserialize(SerializedActionRequirements ob)
		{
			var res = new ActionRequirements()
			{
				RequiresAmbitionAtOrBelowValue = ob.RequiresAmbitionAtOrBelowValue,
				RequiredAmbition = ob.RequiredAmbition,
				RequiresPrideAtOrBelowValue = ob.RequiresPrideAtOrBelowValue,
				RequiredPride = ob.RequiredPride,
				RequiredControl = ob.RequiredControl,
				RequiredPower = ob.RequiredPower,
			};
			
			return res;
		}

		public static ActionRequirements ResolveReferences(DeserializedDataAccessor dda, ActionRequirements data, SerializedActionRequirements ob)
		{
			data.RequiredInteractions = new List<Interaction>();
			foreach (var interaction in ob.RequiredInteractionsReferences)
			{
				data.RequiredInteractions.Add(dda.FindInteractionById(interaction));
			}

			data.RequiredNotCompletedInteractions = new List<Interaction>();
			foreach (var interaction in ob.RequiredNotCompletedInteractionsReferences)
			{
				data.RequiredNotCompletedInteractions.Add(dda.FindInteractionById(interaction));
			}

			data.RequiredPolicies = new List<Policy>();
			foreach (var policy in ob.RequiredPoliciesReferences)
			{
				data.RequiredPolicies.Add(dda.FindPolicyById(policy));
			}

			data.RequiredDepartmentsControled = new List<Location>();
			foreach (var location in ob.RequiredDepartmentsControledReferences)
			{
				data.RequiredDepartmentsControled.Add(dda.FindLocationById(location));
			}

			data.RequiredTrophies = new List<Trophy>();
			foreach (var trophy in ob.RequiredTrophiesReferences)
			{
				data.RequiredTrophies.Add(dda.FindTrophyById(trophy));
			}

			return data;
		}
	}
}