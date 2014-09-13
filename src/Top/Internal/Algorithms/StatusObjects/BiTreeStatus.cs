using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml;

using NetFocus.DataStructure.Internal.Algorithm.Glyphs;


namespace NetFocus.DataStructure.Internal.Algorithm
{
	public class BiTreeStatus
	{
		string p;

		Color traversedColor;
		Color commonColor;
		Color currentColor;
		Color visitingColor;
		bool canEdit;

		[Browsable(false)]
		public bool CanEdit
		{
			set
			{
				canEdit = value;	
			}
		}

		[Description("����P��ǰָ��Ľ���ֵ.")]
		public string P
		{
			get
			{
				return p;
			}
			set
			{
				if(canEdit == true)
				{
					p = value;
					canEdit = false;
				}
			}
		}
		[Description("��ʾ��������ж�������������������ɫ.")]
		[Category("��������")]
		public Color �����������ɫ
		{
			get
			{
				return traversedColor;
			}
			set
			{
				traversedColor = value;
			}
		}
		[Description("��ʾ��������ж�����������ɫ.")]
		[Category("��������")]
		public Color �����ɫ
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
		[Description("��ʾ��������ж�������ǰ����������ɫ.")]
		[Category("��������")]
		public Color ��ǰ�����ɫ
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
		[Description("��ʾ��������ж�������ǰ�����������ɫ.")]
		[Category("��������")]
		public Color ��������ɫ
		{
			get
			{
				return visitingColor;
			}
			set
			{
				visitingColor = value;
			}
		}

		public BiTreeStatus()
		{
			this.p = null;
			traversedColor = Color.LightSkyBlue;
			commonColor = Color.HotPink;
			currentColor = Color.LightSeaGreen;
			visitingColor = Color.Gold;
			canEdit = false;
		}
	}
}
