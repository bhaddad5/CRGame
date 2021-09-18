using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModel.Serializers
{
	public static class DataSerializerHelpers
	{
		public static string GetName(this Object tex)
		{
			if (tex == null)
				return null;
			return tex.name;
		}
	}
}