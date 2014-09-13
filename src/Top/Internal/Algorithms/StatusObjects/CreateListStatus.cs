using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;

namespace NetFocus.DataStructure.Internal.Algorithm
{

	public class CreateListStatus
	{
		int i;
		string l;
		string p = null;
		int n;

		Color nodeColor;
		Color headNodeColor;
		Color insertNodeColor;
		bool canEdit;

		[Browsable(false)]
		public bool CanEdit
		{
			set
			{
				canEdit = value;	
			}
		}

		[Description("Pָ��ǰ���.")]
		[Category("�㷨����")]
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

		[Description("һ�����ڼ�������ʱ����")]
		[Category("�㷨����")]
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
		[Description("Ҫ�������ܹ������")]
		[Category("�㷨����")]
		public int N
		{
			get
			{
				return n;
			}
			set{}
		}
		[Description("��ʾ��ǰ����.")]
		[Category("�㷨����")]
		public string L
		{
			get
			{
				return l;
			}
			set{}
		}
		
		[Description("��ʾ���������ÿ��������ɫ.")]
		[Category("��������")]
		public Color �����ɫ
		{
			get
			{
				return nodeColor;
			}
			set
			{
				nodeColor = value;
			}
		}
		[Description("��ʾ���������ͷ������ɫ.")]
		[Category("��������")]
		public Color ͷ�����ɫ
		{
			get
			{
				return headNodeColor;
			}
			set
			{
				headNodeColor = value;
			}
		}
		[Description("��ʾ���������Ҫ����Ľ�����ɫ.")]
		[Category("��������")]
		public Color ��������ɫ
		{
			get
			{
				return insertNodeColor;
			}
			set
			{
				insertNodeColor = value;
			}
		}


		public CreateListStatus(string l)
		{
			this.l = l;
			this.p = null;
			this.n = l.Length;
			this.i = n;
			nodeColor = Color.DarkTurquoise;
			headNodeColor = Color.Red;
			insertNodeColor = Color.DarkOrange;
			canEdit = false;
		}
	}
}
