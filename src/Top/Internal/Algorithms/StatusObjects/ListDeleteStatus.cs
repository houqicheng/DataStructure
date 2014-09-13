using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;

namespace NetFocus.DataStructure.Internal.Algorithm
{

	public class ListDeleteStatus
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
				if(canEdit == true)
				{
					e = value;
					canEdit = false;
				}
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

		
		public ListDeleteStatus(string l,int i)
		{
			this.l = l;
			this.i = i;
			this.j = 0;
			this.p = null;
			this.length = l.Length;

			nodeColor = Color.DarkTurquoise;
			currentNodeColor = Color.Pink;
			headNodeColor = Color.Red;
			canEdit = false;

		}
	}
}
