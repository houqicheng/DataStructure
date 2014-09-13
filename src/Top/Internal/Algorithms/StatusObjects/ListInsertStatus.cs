using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;

namespace NetFocus.DataStructure.Internal.Algorithm
{

	public class ListInsertStatus
	{
		int i;
		char e;
		string l;
		int length;
		int j;
		string p = null;
		Color nodeColor;
		Color currentNodeColor;
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

		[Description("Pָ��ǰ�ڵ��ǰ�����.")]
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

		[Description("�����Դ���Ҫɾ����λ��.")]
		[Category("�㷨����")]
		public int I
		{
			get
			{
				return i;
			}
			set{}
		}
		[Description("һ����ʱ����,�������ұ�ɾ�ڵ��ǰ��.")]
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
		[Description("��ǰ����ĳ���.")]
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

		[Description("�������汻ɾ���Ľڵ�.")]
		[Category("�㷨����")]
		public char E
		{
			get
			{
				return e;
			}
			set
			{
			}
		}
		
		[Description("��ʾ��ǰ����.")]
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
		[Description("��ʾ��������е�ǰPָ����ָ������ɫ.")]
		[Category("��������")]
		public Color ��ǰ�����ɫ
		{
			get
			{
				return currentNodeColor;
			}
			set
			{
				currentNodeColor = value;
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

		public ListInsertStatus(string l,int i,char e)
		{
			this.l = l;
			this.i = i;
			this.e = e;
			this.length = l.Length;
			this.j = 0;
			this.p = null;
			nodeColor = Color.DarkTurquoise;
			currentNodeColor = Color.Pink;
			headNodeColor = Color.Red;
			insertNodeColor = Color.DarkOrange;
			canEdit = false;
		}
	}
}
