using System;
using System.Drawing;


namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// ����һ�ö������е�һ�����
	/// </summary>
	public interface IBiTreeNode : IGlyph
	{
		/// <summary>
		/// �����ı�
		/// </summary>
		string Text
		{
			get;
			set;
		}
		/// <summary>
		/// the left child of the current biTree node
		/// </summary>
		IBiTreeNode LeftChild
		{
			get;
			set;
		}

		/// <summary>
		/// the right child of the current biTree node
		/// </summary>
		IBiTreeNode RightChild
		{
			get;
			set;
		}
	}
}
