using System;
using System.Drawing;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// ����һ�������еĽڵ����
	/// </summary>
	public interface INode : IGlyph
	{
		INode Next
		{
			get;
			set;
		}
		string Text
		{
			get;
			set;
		}
	}
}
