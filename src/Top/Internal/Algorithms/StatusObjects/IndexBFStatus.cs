using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;

namespace NetFocus.DataStructure.Internal.Algorithm
{
	public class IndexBFStatus
	{
		string s,t;
		int pos,sLength,tLength;
	    int i,j;
		int findPosition;
		Color stringTColor;
		Color stringSColor;
		Color currentElementColor;
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
		
		[Description("��S�б��Ƚ�Ԫ�ص�λ��.")]
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
		

		[Description("�Ӵ�T�б��Ƚ�Ԫ�ص�λ��.")]
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
		
		[Description("��S.")]
		[Category("�㷨����")]
		public string S
		{
			get
			{
				return s;
			}
			set
			{
				
			}
		}
		
		[Description("��T.")]
		[Category("�㷨����")]
		public string T
		{
			get
			{
				return t;
			}
			set
			{
				
			}
		}
		
		[Description("���ҵ���ʼλ��.")]
		[Category("�㷨����")]
		public int Pos
		{
			get
			{
				return pos;
			}
			set
			{
				
			}
		}
		
		[Description("��S�ĳ���.")]
		[Category("�㷨����")]
		public int SLength
		{
			get
			{
				return sLength;
			}
			set
			{
				
			}
		}
		
		[Description("��T�ĳ���.")]
		[Category("�㷨����")]
		public int TLength
		{
			get
			{
				return tLength;
			}
			set
			{
				
			}
		}


		[Description("��ʾ��������д�S����ɫ.")]
		[Category("��������")]
		public Color ��SԪ����ɫ
		{
			get
			{
				return stringSColor;
			}
			set
			{
				stringSColor = value;
			}
		}
		[Description("��ʾ��������д�T����ɫ.")]
		[Category("��������")]
		public Color ��TԪ����ɫ
		{
			get
			{
				return stringTColor;
			}
			set
			{
				stringTColor = value;
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
		[Description("�ҵ���λ��.")]
		[Category("�㷨����")]
		public int �ҵ�λ��
		{
			get
			{
				return findPosition;
			}
			set
			{
				if(canEdit == true)
				{
					findPosition = value;
					canEdit = false;
				}
			}
		}
		


		public IndexBFStatus(string s,string t,int pos)
		{
			this.s = s;
			this.t = t;
			this.pos = pos;
			this.sLength = s.Length;
			this.tLength = t.Length;
			this.i = 0;
			this.j = 0;
			this.findPosition = -1;
			squareAppearance = GlyphAppearance.Popup;
			stringTColor = SystemColors.InactiveBorder;
			stringSColor = SystemColors.InactiveBorder;
			currentElementColor = Color.Red;
			canEdit = false;

		}


	}
}
