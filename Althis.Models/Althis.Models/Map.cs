using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Althis.Models;

public class Map
{
	public int Id { get; set; }

	public int MasterId { get; set; }

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public int Width { get; set; }

	public int Height { get; set; }

	public int TraitSections { get; set; }

	public List<Tile> Tiles { get; set; }

	public Map Initialize(byte[] mapBytes)
	{
		Guid = Guid.NewGuid();
		Random random = new Random();
		Id = random.Next(0, 1000000);
		ReadHeader(mapBytes);
		ReadInMasterId(mapBytes);
		ReadInName(mapBytes);
		ReadInTiles(mapBytes);
		return this;
	}

	private void ReadHeader(byte[] mapBytes)
	{
		Width = mapBytes[16] * 256 + mapBytes[17];
		Height = mapBytes[18] * 256 + mapBytes[19];
		TraitSections = mapBytes[20];
		_ = Width;
		_ = Height;
	}

	private void ReadInMasterId(byte[] mapBytes)
	{
		MasterId = BitConverter.ToInt32(new byte[4]
		{
			mapBytes[28],
			mapBytes[29],
			mapBytes[30],
			mapBytes[31]
		}, 0);
	}

	private void ReadInName(byte[] mapBytes)
	{
		int num = 0;
		byte[] array = new byte[16];
		for (int i = 32; i < 48; i++)
		{
			byte b = mapBytes[i];
			array[num++] = (byte)((b != 0) ? b : 32);
		}
		Name = Encoding.ASCII.GetString(array).Trim();
	}

	private void ReadInTiles(byte[] mapBytes)
	{
		int num = mapBytes[21];
		int num2 = 1;
		int num3 = 1;
		int num4 = num;
		Tiles = new List<Tile>();
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				Tile tile = new Tile(num2++, i, j, Id);
				for (int k = 1; k <= TraitSections; k++)
				{
					tile.AddTrait(num3++, k, mapBytes[num4 * k]);
				}
				num4++;
				Tiles.Add(tile);
				foreach (Tile tile2 in Tiles)
				{
					tile2.Initialize();
				}
			}
		}
	}

	internal IEnumerable<int> GetBytes()
	{
		List<int> list = new List<int>();
		foreach (Tile tile in Tiles)
		{
			list.Add(tile.Elevation);
		}
		return list;
	}

	internal IEnumerable<Tile> GetTiles()
	{
		return Tiles;
	}

	public Tile GetTile(int x, int y)
	{
		IEnumerable<Tile> source = Tiles.Where((Tile t) => t.LatitudeTenth == x && t.LongitudeTenth == y);
		return source.First();
	}
}
