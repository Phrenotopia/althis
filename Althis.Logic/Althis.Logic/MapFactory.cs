using System;
using System.IO;
using Althis.Models;

namespace Althis.Logic;

public class MapFactory
{
	public Map Create(string mapname, string filepath)
	{
		Map map = ReadInMap(mapname, filepath);
		MapPopulator.PopulateMap(map);
		return map;
	}

	private static Map ReadInMap(string mapname, string filepath)
	{
		FileStream fileStream = File.OpenRead(filepath + mapname + ".map");
		byte[] array = new byte[fileStream.Length];
		fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
		fileStream.Close();
		fileStream.Dispose();
		Map map = new Map().Initialize(array);
		MapHolder.Maps.Add(map);
		return map;
	}
}
