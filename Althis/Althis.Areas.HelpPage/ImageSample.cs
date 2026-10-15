using System;

namespace Althis.Areas.HelpPage;

public class ImageSample
{
	public string Src { get; private set; }

	public ImageSample(string src)
	{
		if (src == null)
		{
			throw new ArgumentNullException("src");
		}
		Src = src;
	}

	public override bool Equals(object obj)
	{
		if (obj is ImageSample imageSample)
		{
			return Src == imageSample.Src;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Src.GetHashCode();
	}

	public override string ToString()
	{
		return Src;
	}
}
