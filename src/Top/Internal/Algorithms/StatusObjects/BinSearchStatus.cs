using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;

namespace NetFocus.DataStructure.Internal.Algorithm
{
	public class BinSearchStatus
	{
		int mid,low,high;
		int n;
		char key;
		string r;
		Color stringRColor;
		Color currentElementColor;
		Color overElementColor;
		Color headElementColor;
		GlyphAppearance squareAppearance;
		bool canEdit;

		[Browsable(false)]
		public bool CanEdit
		{
			set
			{
				canEdit = value;	
			}
		}
		
		public int Mid
		{
			get
			{
				return mid;
			}
			set
			{
				if(canEdit == true)
				{
					mid = value;
					canEdit = false;
				}
			}
		}
		public int Low
		{
			get
			{
				return low;
			}
			set
			{
				if(canEdit == true)
				{
					low = value;
					canEdit = false;
				}
			}
		}
		public int High
		{
			get
			{
				return high;
			}
			set
			{
				if(canEdit == true)
				{
					high = value;
					canEdit = false;
				}
			}
		}


		public int N
		{
			get
			{
				return n;
			}
			set
			{
			}
		}
		public char Key
		{
			get
			{
				return key;
			}
			set
			{
			}
		}
		public string R
		{
			get
			{
				return r;
			}
			set
			{
			}
		}


		[Description("��ʾ������������Ա����ɫ.")]
		[Category("��������")]
		public Color ���Ա���ɫ
		{
			get
			{
				return stringRColor;
			}
			set
			{
				stringRColor = value;
			}
		}
		[Description("��ʾ���������R[0]Ԫ�ص���ɫ.")]
		[Category("��������")]
		public Color ͷԪ����ɫ
		{
			get
			{
				return headElementColor;
			}
			set
			{
				headElementColor = value;
			}
		}
		[Description("��ʾ��������бȽϹ�Ԫ�ص���ɫ.")]
		[Category("��������")]
		public Color �ȽϹ�Ԫ����ɫ
		{
			get
			{
				return overElementColor;
			}
			set
			{
				overElementColor = value;
			}
		}
		[Description("��ʾ��������е�ǰ���Ƚ�Ԫ�ص���ɫ.")]
		[Category("��������")]
		public Color ��ǰԪ����ɫ
		{
			get
			{
				return currentElementColor;
			}
			set
			{
				currentElementColor = value;
			}
		}

		[Description("��ʾ��ǰ��ʾ�Ķ�������ʾ��ʽ.")]
		[Category("��������")]
		public GlyphAppearance ͼ�����
		{
			get
			{
				return squareAppearance;
			}
			set
			{
				squareAppearance = value;
			}
		}

		public BinSearchStatus(string r,char key)
		{
			this.r = r;
			this.key = key;
			this.n = r.Length;
			this.low = -1;
			this.high = -1;
			this.mid = -1;
			squareAppearance = GlyphAppearance.Popup;
			headElementColor = Color.HotPink;
			overElementColor = Color.LightGray;
			stringRColor = Color.Teal;
			currentElementColor = Color.Red;
			canEdit = false;
		}

	}
}
