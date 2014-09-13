using System;
using System.Collections;
using System.Drawing;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// һ����ջ���ݽṹ�ĵ�����(��װһ����ջ�����ݽṹ)
	/// </summary>
	public class StackIterator : IIterator
	{
		ArrayList arrayList = null;
		int currentIndex = -1;

		public StackIterator(ArrayList arrayList)
		{
			this.arrayList = arrayList;
		}

		
		#region IIterator ��Ա

		public IIterator First()
		{
			if(arrayList.Count > 0)
			{
				currentIndex = 0;  //ջ��Ԫ��Ϊ��һ��Ԫ��
			}
			else
			{
				currentIndex = -1;
			}
			return this;
		}

		public IIterator Next()
		{
			currentIndex += 1;  //ָ���һ
			return this;
		}

		public bool IsDone()
		{
			return currentIndex >= arrayList.Count || currentIndex == -1;
		}

		public IGlyph CurrentItem
		{
			get
			{
				if(currentIndex >= 0 && currentIndex < arrayList.Count)
				{
					return (IGlyph)arrayList[currentIndex];
				}
				return null;
			}
		}

		#endregion

		public void RefreshItems(int newWidth,int newHeight)
		{
			if(newWidth <= 0 || newHeight <= 0)
			{
				return;
			}
			int y = 0;
			for(int i = 0;i < arrayList.Count;i++)
			{
				y = newHeight - (i + 1) * (2 + 28) - 1;
				IGlyph glyph = ((IGlyph)arrayList[i]);
				glyph.Bounds = new Rectangle(glyph.Bounds.X,y,newWidth,glyph.Bounds.Height);
			}
		}
		public void PushGlyph(IGlyph glyph)
		{
			arrayList.Add(glyph);
		}
		public void Pop()
		{
			arrayList.RemoveAt(arrayList.Count -1);
		}
		public int ItemCount
		{
			get
			{
				return arrayList.Count;
			}
		}

	}
}
