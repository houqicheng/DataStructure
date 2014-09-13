using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;

namespace NetFocus.DataStructure.Internal.Algorithm
{

	public class SequenceDeleteStatus
	{
		int i;
		string e;
		string l;
		int length;
		int j;
		Color squareBackColor;
		Color insertBackColor;
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

		[Description("Ҫɾ����Ԫ�ص�λ��.")]
		[Category("�㷨����")]
		public int I
		{
			get
			{
				return i;
			}
			set
			{
			}
		}
		[Description("һ����ʱ����,�����ƶ������е�Ԫ��.")]
		[Category("�㷨����")]
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
		[Description("��ǰ���Ա�ĳ���.")]
		[Category("�㷨����")]
		public int Length
		{
			get
			{
				return length;
			}
			set
			{
				if(canEdit == true)
				{
					length = value;
					canEdit = false;
				}
			}
		}

		[Description("���汻ɾ��Ԫ�ص�ֵ.")]
		[Category("�㷨����")]
		public string E
		{
			get
			{
				return e;
			}
			set
			{
				if(canEdit == true)
				{
					e = value;
					canEdit = false;
				}
			}
		}
		
		[Description("��ʾ��ǰ���Ա��ַ���.")]
		[Category("�㷨����")]
		public string L
		{
			get
			{
				return l;
			}
			set
			{
				if(canEdit == true)
				{
					l = value;
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
				return squareBackColor;
			}
			set
			{
				squareBackColor = value;
			}
		}
		[Description("��ʾ���������Ҫɾ��Ԫ�صı���ɫ.")]
		[Category("��������")]
		public Color ɾ��Ԫ�ر���ɫ
		{
			get
			{
				return insertBackColor;
			}
			set
			{
				insertBackColor = value;
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

		
		public SequenceDeleteStatus(string l, int i)
		{
			this.l = l;
			this.i = i;
			this.length = l.Length;
			this.j = i;
			this.e = null;
			squareBackColor = Color.DarkCyan;
			insertBackColor = Color.Red;
			squareAppearance = GlyphAppearance.Flat;
			canEdit = false;

		}
	}
}
