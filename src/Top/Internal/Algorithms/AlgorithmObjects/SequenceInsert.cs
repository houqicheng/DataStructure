using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Threading;
using System.Globalization;
using System.Xml;

using NetFocus.DataStructure.Gui.Views;
using NetFocus.DataStructure.Services;
using NetFocus.DataStructure.Properties;
using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.TextEditor;
using NetFocus.DataStructure.TextEditor.Document;
using NetFocus.DataStructure.Gui.Pads;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;
using NetFocus.DataStructure.Gui.Algorithm.Dialogs;


namespace NetFocus.DataStructure.Internal.Algorithm
{
	/// <summary>
	/// ��װһ���㷨: "�����Ա��е�I��λ��֮ǰ���һ��Ԫ��"
	/// </summary>
	public class SequenceInsert : AbstractAlgorithm
	{
		IIterator arrayIterator;
		IIterator nullIterator;
		SequenceInsertStatus status;
		int squareSpace = 5;
		int squareSize = 50;
		string l;
		int i;
		char e;

		public override object Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value as SequenceInsertStatus;
			}
		}


		public override void ActiveWorkbenchWindow_CloseEvent(object sender, EventArgs e) 
		{
			arrayIterator = null;
			nullIterator = null;
			
			base.ActiveWorkbenchWindow_CloseEvent(sender,e);
			
		}

		
		public override void Recover()
		{
			status = new SequenceInsertStatus(l,i,e);

			base.Recover();
		}

		
		Image CreatePreviewImage(string s,int pos,char c)
		{
			int height = 80;
			int width = 530;
			int space = 2;
			int size = 35;
			int leftSpan = 15;
			int topSpan = 40;
			ArrayList squareArray = new ArrayList();
			IGlyph glyph;
			for(int i=0;i<s.Length;i++)
			{
				glyph = new Square(leftSpan + i*(size + space),topSpan,size,Color.HotPink,GlyphAppearance.Flat,s[i].ToString());
				squareArray.Add(glyph);
			}
			SquareLine squareLine = new SquareLine(1,1,1,squareArray);

			Rectangle rec = new Rectangle(0,0,0,0);
			if(pos >= 1 && pos <= s.Length)
			{
				rec = ((IGlyph)squareArray[pos-1]).Bounds;
			}
			else if(pos == s.Length + 1)
			{
				Rectangle lastRectangle = ((IGlyph)squareArray[pos-2]).Bounds;
				rec = new Rectangle(lastRectangle.X + lastRectangle.Width + space,lastRectangle.Y - lastRectangle.Height,lastRectangle.Width,lastRectangle.Height);
			}
			Square insertSquare = new Square(rec.X,topSpan - size - 2,rec.Width,Color.Green,GlyphAppearance.Flat,c.ToString());

			//����ʼ�����еĵ�����
			IIterator arrayIterator = squareLine.CreateIterator();
			IIterator nullIterator = insertSquare.CreateIterator();

			Bitmap bmp = new Bitmap(width,height);
			Graphics g = Graphics.FromImage(bmp);

			for(IIterator iterator = arrayIterator.First();!arrayIterator.IsDone();iterator = arrayIterator.Next())
			{
				iterator.CurrentItem.Draw(g);
			}
			nullIterator.CurrentItem.Draw(g);

			return bmp;

		}
		public override bool GetData()
		{
			ArrayList statusItemList = new ArrayList();
			
			statusItemList.Clear();

			StatusItemControl statusItemControl = new StatusItemControl();

			Hashtable table = AlgorithmManager.Algorithms.GetExampleDatas();
			
			if(table != null)
			{
				XmlNode node = table[typeof(SequenceInsert).ToString()] as XmlElement;

				XmlNodeList childNodes  = node.ChildNodes;
		
				StatusItem statusItem = null;

				foreach (XmlElement el in childNodes)
				{
					string s = el.Attributes["OriginalString"].Value;
					int pos = Convert.ToInt32(el.Attributes["InsertPosition"].Value);
					char c = Convert.ToChar(el.Attributes["InsertData"].Value);

					statusItem = new StatusItem(new SequenceInsertStatus(s,pos,c));
					statusItem.Height = 80;
					statusItem.Image = CreatePreviewImage(s,pos,c);
					statusItemList.Add(statusItem);
				}
			}
			DialogType = typeof(SequenceInsertDialog);

			InitDataForm form = new InitDataForm();

			form.StatusItemList = statusItemList;

			if(form.ShowDialog() != DialogResult.OK)
			{
				return false;
			}
			if(form.SelectedIndex >= 0)  //˵���û���ͨ��ѡ��ĳ��ģ������ʼ�����ݵ�
			{
				StatusItem selectedItem = form.StatusItemList[form.SelectedIndex] as StatusItem;
				if(selectedItem != null)
				{
					SequenceInsertStatus tempStatus = selectedItem.ItemInfo as SequenceInsertStatus;
					if(tempStatus != null)
					{
						l = tempStatus.L.Substring(0,tempStatus.Length);
						i = tempStatus.I;
						e = tempStatus.E;
					}
				}
			}
			else  //˵���û�ѡ���Զ�������
			{
				l = status.L.Substring(0,status.Length);
				i = status.I;
				e = status.E;
			}
			return true;
			
		}

		
		public override void Initialize(bool isOpen)
		{
			base.Initialize(isOpen);
			
			status = new SequenceInsertStatus(l,i,e);

			InitGraph();
			
			WorkbenchSingleton.Workbench.ActiveViewContent.SelectView();
			
		}


		public override void InitGraph() 
		{
			SquareLine squareLine;
			Square insertSquare;
			ArrayList squareArray = new ArrayList();
			IGlyph glyph;
			for(int i=0;i<status.Length;i++)
			{
				glyph = new Square(40 + i*(squareSize + squareSpace),20 + squareSize + 10,squareSize,status.ͼ�α���ɫ,status.ͼ�����,status.L[i].ToString());
				squareArray.Add(glyph);
			}
			//��������һ�������Ԫ��,�Ա������Ԫ�غ����������һ��Ԫ��
			squareArray.Add(new Square(1,1,2,Color.Transparent,""));
			squareLine = new SquareLine(20,80,status.Length*(squareSize+20+10),squareArray);

			Rectangle rec = new Rectangle(0,0,0,0);
			if(status.I >= 1 && status.I <= status.Length)
			{
				rec = ((IGlyph)squareArray[status.I-1]).Bounds;
			}
			else if(status.I == status.Length + 1)
			{
				Rectangle lastRectangle = ((IGlyph)squareArray[status.I-2]).Bounds;
				rec = new Rectangle(lastRectangle.X + lastRectangle.Width + squareSpace,lastRectangle.Y - 10 - lastRectangle.Height,lastRectangle.Width,lastRectangle.Height);
			}
			insertSquare = new Square(rec.X,20,rec.Width,status.����Ԫ�ر���ɫ,status.ͼ�����,status.E.ToString());

			//����ʼ�����еĵ�����
			arrayIterator = squareLine.CreateIterator();
			nullIterator = insertSquare.CreateIterator();

		}
		
		
		public override void ExecuteAndUpdateCurrentLine()
		{
			switch (CurrentLine)
			{
				case 0:
					CurrentLine = 4;
					return;
				case 4:
					//�ж�IF����Ƿ���������������CurrentLine = 5;��������CurrentLine = 8;
					if(status.I >=1 && status.I <= status.Length + 1)
					{
						CurrentLine = 8;
						return;
					}
					break;
				case 8:
					//�ж�for��������Ƿ���������������CurrentLine = 9;��������CurrentLine = 11;
					if(status.J < status.I -1)
					{
						CurrentLine = 11;
						return;
					}
					break;
				case 9:
					//�ƶ�һ��ͼ��Ԫ��.
					IGlyph tempGlyph = ((ArrayIterator)arrayIterator).GetGlyphByIndex(status.J);
					IGlyph tempGlyph1 = new Square(tempGlyph.Bounds.X + tempGlyph.Bounds.Width + squareSpace,tempGlyph.Bounds.Y,squareSize,status.ͼ�α���ɫ,status.ͼ�����,((Square)tempGlyph).Text);
					((ArrayIterator)arrayIterator).MoveGlyphHorizon(status.J,tempGlyph1,1);
					string c = status.L[status.J].ToString();
					status.CanEdit = true;
					status.L = status.L.Remove(status.J + 1,1);
					status.CanEdit = true;
					status.L = status.L.Insert(status.J,c);
					status.CanEdit = true;
					status.J--;
					CurrentLine = 8;

					return;
				case 11:
					//����Ԫ��.
					((ArrayIterator)arrayIterator).MoveGlyphVertical(nullIterator.CurrentItem,nullIterator.CurrentItem.Bounds.Y + nullIterator.CurrentItem.Bounds.Height + 10);
					status.CanEdit = true;
					status.L = status.L.Remove(status.I - 1,1);
					status.CanEdit = true;
					status.L = status.L.Insert(status.I - 1,status.E.ToString());
					break;
				case 12:
					status.CanEdit = true;
					status.Length++;
					CurrentLine = 14;
					return;
				case 14:
					return;
			}
			CurrentLine++;

		}


		public override void UpdateAnimationPad()
		{
			base.UpdateAnimationPad();
			Graphics g = AlgorithmManager.Algorithms.ClearAnimationPad();

			if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
			{
				if(arrayIterator != null)
				{
					for(IIterator iterator = arrayIterator.First();!arrayIterator.IsDone();iterator = arrayIterator.Next())
					{
						iterator.CurrentItem.Draw(g);
					}
				}
				if(nullIterator != null)
				{
					nullIterator.CurrentItem.Draw(g);
				}
			}
			
		}
		

		public override void UpdateGraphAppearance()
		{
			for(IIterator iterator = arrayIterator.First();!arrayIterator.IsDone();iterator = arrayIterator.Next())
			{
				iterator.CurrentItem.BackColor = status.ͼ�α���ɫ;
				iterator.CurrentItem.Appearance = status.ͼ�����;
			}
			nullIterator.CurrentItem.BackColor = status.����Ԫ�ر���ɫ;
			nullIterator.CurrentItem.Appearance =  status.ͼ�����;

		}
		
		

	}
}
