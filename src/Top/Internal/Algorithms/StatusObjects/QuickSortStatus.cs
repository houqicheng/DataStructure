using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;


namespace NetFocus.DataStructure.Internal.Algorithm
{

	public class QuickSortStatus
	{
		string r;
		int i,j,low,high;
		Color commonColor;
		Color currentColor;
		Color headColor;
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

		public string R
		{
			get
			{
				return r;
			}
			set
			{
				if(canEdit == true)
				{
					r = value;
					canEdit = false;
				}
			}
		}
		public int I
		{
			get
			{
				return i;
			}
			set
			{
				if(canEdit == true)
				{
					i = value;
					canEdit = false;
				}
			}
		}
		public int J
		{
			get
			{
				return j;
			}
			set
			{
				if(canEdit == true)
				{
					j = value;
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

		
		[Description("��ʾ���������ÿ��Ԫ�صı���ɫ.")]
		[Category("��������")]
		public Color ͼ�α���ɫ
		{
			get
			{
				return commonColor;
			}
			set
			{
				commonColor = value;
			}
		}
		[Description("��ʾ������������ڱȽ�Ԫ�صı���ɫ.")]
		[Category("��������")]
		public Color ��ǰ����ɫ
		{
			get
			{
				return currentColor;
			}
			set
			{
				currentColor = value;
			}
		}
		[Description("��ʾ���������R��0��Ԫ�صı���ɫ.")]
		[Category("��������")]
		public Color ͷԪ�ر���ɫ
		{
			get
			{
				return headColor;
			}
			set
			{
				headColor = value;
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

		
		public QuickSortStatus(string r,int low,int high)
		{
			this.r = r;
			this.low = low;
			this.high = high;
			this.i = 0;
			this.j = 0;
			headColor = Color.Gold;
			commonColor = Color.HotPink;
			currentColor = Color.LightSeaGreen;
			squareAppearance = GlyphAppearance.Popup;
			canEdit = false;
		}


	}
}
