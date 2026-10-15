using System;

namespace Althis.Areas.HelpPage;

public class TextSample
{
	public string Text { get; private set; }

	public TextSample(string text)
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		Text = text;
	}

	public override bool Equals(object obj)
	{
		if (obj is TextSample textSample)
		{
			return Text == textSample.Text;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Text.GetHashCode();
	}

	public override string ToString()
	{
		return Text;
	}
}
