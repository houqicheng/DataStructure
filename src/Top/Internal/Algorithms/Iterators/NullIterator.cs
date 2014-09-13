using System;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// һ���յĵ�����,��һ��ͼ��Ԫ����Ҷ�ӽڵ�(��û���ӽڵ�)ʱ���õ��˵�����
	/// </summary>
	public class NullIterator : IIterator
	{
		IGlyph glyph;
		
		public NullIterator(IGlyph glyph)
		{
			this.glyph = glyph;
		}
		
		#region IIterator ��Ա

		public IIterator First()
		{
			return null;
		}

		public IIterator Next()
		{
			return null;
		}

		public bool IsDone()
		{
			return true;//��Ϊ��Ҷ�ӽڵ�,�������Ƿ�����.
		}

		public IGlyph CurrentItem
		{
			get
			{
				return this.glyph;
			}
		}

		#endregion
	}
}
