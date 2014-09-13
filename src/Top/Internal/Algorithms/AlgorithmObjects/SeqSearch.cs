using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Threading;
using System.Xml;

using NetFocus.DataStructure.Gui.Views;
using NetFocus.DataStructure.Services;
using NetFocus.DataStructure.Properties;
using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.TextEditor;
using NetFocus.DataStructure.TextEditor.Document;
using NetFocus.DataStructure.Gui.Pads;
using NetFocus.DataStructure.Gui.Algorithm.Dialogs;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;

namespace NetFocus.DataStructure.Internal.Algorithm
{

	public class SeqSearch : AbstractAlgorithm
	{
		ArrayList statusItemList = new ArrayList();
		SquareLine squareLine;
		IIterator arrayIterator;
		IIterator nullIterator;
		SeqSearchStatus status = null;
		int squareSpace = 5;
		int squareSize = 50;
		string r;
		char key;

		public override object Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value as SeqSearchStatus;
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
			arrayIterator = null;
			nullIterator = null;
			status = new SeqSearchStatus(r,key);
			base.Recover();
		}

		
		Image CreatePreviewImage(string r,char key)
		{
			int height = 80;
			int width = 530;
			int space = 2;
			int size = 29;
			int leftSpan = 5;
			int topSpan = 5;

			ArrayList squareArray = new ArrayList();
			IGlyph glyph;
			glyph = new Square(leftSpan,topSpan + size + 2,size,Color.HotPink,GlyphAppearance.Flat,"?");
			squareArray.Add(glyph);
			for(int i=0;i<r.Length;i++)
			{
				glyph = new Square(leftSpan + (i + 1)*(size + space),topSpan + size + 2,size,Color.Teal,GlyphAppearance.Flat,r[i].ToString());
				squareArray.Add(glyph);
			}
			squareLine = new SquareLine(0,0,1,squareArray);

			//����ʼ�����еĵ�����
			IIterator arrayIterator = squareLine.CreateIterator();
			IIterator nullIterator = null;
			glyph = ((ArrayIterator)arrayIterator).GetGlyphByIndex(r.Length);
			if(glyph != null)
			{
				nullIterator = new Square(glyph.Bounds.X,glyph.Bounds.Y - size - 2,size,Color.Red,GlyphAppearance.Flat,key.ToString()).CreateIterator();
			}

			Bitmap bmp = new Bitmap(width,height);
			Graphics g = Graphics.FromImage(bmp);

			for(IIterator iterator = arrayIterator.First();!arrayIterator.IsDone();iterator = arrayIterator.Next())
			{
				iterator.CurrentItem.Draw(g);
			}
			if(nullIterator != null)
			{
				nullIterator.CurrentItem.Draw(g);
			}

			return bmp;

		}
		public override bool GetData()
		{
			statusItemList.Clear();

			StatusItemControl statusItemControl = new StatusItemControl();

			Hashtable table = AlgorithmManager.Algorithms.GetExampleDatas();
			
			if(table != null)
			{
				XmlNode node = table[typeof(SeqSearch).ToString()] as XmlElement;

				XmlNodeList childNodes  = node.ChildNodes;
		
				StatusItem statusItem = null;

				foreach (XmlElement el in childNodes)
				{
					string r = el.Attributes["OriginalString"].Value;
					char key = Convert.ToChar(el.Attributes["Key"].Value);

					statusItem = new StatusItem(new SeqSearchStatus(r,key));
					statusItem.Height = 80;
					statusItem.Image = CreatePreviewImage(r,key);
					statusItemList.Add(statusItem);
				}
			}
			DialogType = typeof(SeqSearchDialog);
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
					SeqSearchStatus tempStatus = selectedItem.ItemInfo as SeqSearchStatus;
					if(tempStatus != null)
					{
						r = tempStatus.R;
						key = tempStatus.Key;
					}
				}
			}
			else  //˵���û�ѡ���Զ�������
			{
				r = status.R;
				key = status.Key;
			}
			return true;
			
		}


		public override void Initialize(bool isOpen)
		{
			base.Initialize(isOpen);

			status = new SeqSearchStatus(r,key);

			InitGraph();
			
			WorkbenchSingleton.Workbench.ActiveViewContent.SelectView();
		}

		
		public override void InitGraph() 
		{
			ArrayList squareArray = new ArrayList();
			IGlyph glyph;
			glyph = new Square(40,20 + squareSize + 10,squareSize,status.ͷԪ����ɫ,status.ͼ�����,"?");
			squareArray.Add(glyph);
			for(int i=0;i<status.N;i++)
			{
				glyph = new Square(40 + (i + 1)*(squareSize + squareSpace),20 + squareSize + 10,squareSize,status.���Ա���ɫ,status.ͼ�����,status.R[i].ToString());
				squareArray.Add(glyph);
			}
			squareLine = new SquareLine(20,80,(status.N + 1)*(squareSize+20+10),squareArray);

			//����ʼ�����еĵ�����
			arrayIterator = squareLine.CreateIterator();

			glyph = ((ArrayIterator)arrayIterator).GetGlyphByIndex(status.N);
			if(glyph != null)
			{
				nullIterator = new Square(glyph.Bounds.X,glyph.Bounds.Y - squareSize - 10,squareSize,status.��ǰԪ����ɫ,status.ͼ�����,status.Key.ToString()).CreateIterator();
			}

		}

		ArrayList GetIndexArray()
		{
			int i = status.I + 1;
			ArrayList returnArray = new ArrayList();
			if(i == 0)
			{
				return returnArray;
			}
			while(i <= status.N)
			{
				returnArray.Add(i);
				i++;
			}
			return returnArray;
		}
		public override void ExecuteAndUpdateCurrentLine()
		{
			switch (CurrentLine)
			{
				case 0:
					CurrentLine = 4;
					return;
				case 4: //R[0].key = k;
					break;
				case 5: //i = n;
					status.CanEdit = true;
					status.I = status.N;
					break;
				case 6: //while(R[i].key != k)
					//�ж�while��������Ƿ���������������CurrentLine = 7;��������CurrentLine = 9;
					IGlyph glyph = ((ArrayIterator)arrayIterator).GetGlyphByIndex(status.I);
					if(glyph != null)
					{
						((ArrayIterator)arrayIterator).SetElementsBackColor(GetIndexArray(),status.�ȽϹ�Ԫ����ɫ,status.���Ա���ɫ);
						((ArrayIterator)arrayIterator).SetBackColor(status.I,status.��ǰԪ����ɫ,status.���Ա���ɫ,false);
						nullIterator = new Square(glyph.Bounds.X,glyph.Bounds.Y - squareSize - 10,squareSize,status.��ǰԪ����ɫ,status.ͼ�����,status.Key.ToString()).CreateIterator();
					}
					if(status.R[status.I - 1] == status.Key)
					{
						CurrentLine = 9;
						return;
					}
					break;
				case 7: //i--;
					status.CanEdit = true;
					status.I--;
					CurrentLine = 6;
					return;
				case 9:
					return;
			}
			CurrentLine++;
		}
		

		public override void UpdateGraphAppearance()
		{
			((ArrayIterator)arrayIterator).SetElementsBackColor(GetIndexArray(),status.�ȽϹ�Ԫ����ɫ,status.���Ա���ɫ);
			
			((ArrayIterator)arrayIterator).SetBackColor(status.I,status.��ǰԪ����ɫ,status.���Ա���ɫ,false);

			if(nullIterator != null)
			{
				nullIterator.CurrentItem.BackColor = status.��ǰԪ����ɫ;
				nullIterator.CurrentItem.Appearance =  status.ͼ�����;
			}
			arrayIterator.First().CurrentItem.BackColor = status.ͷԪ����ɫ;
			arrayIterator.First().CurrentItem.Appearance = status.ͼ�����;

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

	}
}
