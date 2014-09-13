using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

using NetFocus.DataStructure.Gui.Views;
using NetFocus.DataStructure.Services;
using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.TextEditor;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;
using NetFocus.DataStructure.Gui.Algorithm.Dialogs;


namespace NetFocus.DataStructure.Internal.Algorithm
{

	public class SequenceMerge : AbstractAlgorithm 
	{
		ArrayList statusItemList = new ArrayList();
		IIterator arrayIteratorLa;
		IIterator arrayIteratorLb;
		IIterator arrayIteratorLc;
		IGlyph currentGlyph;
		int squareSpace = 5;
		int squareSize = 50;
		string la,lb;
		int k = 0;
		
		SequenceMergeStatus status = null;

		public override object Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value as SequenceMergeStatus;
			}
		}

		
		public override void ActiveWorkbenchWindow_CloseEvent(object sender, EventArgs e) 
		{
			arrayIteratorLa = null;
			arrayIteratorLb = null;
			arrayIteratorLc = null;
			k = 0;
			
			base.ActiveWorkbenchWindow_CloseEvent(sender,e);
			
		}


		public override void Recover()
		{
			k = 0;
			status = new SequenceMergeStatus(la,lb);
			base.Recover();
		}

		Image CreatePreviewImage(string s1,string s2)
		{
			int height = 80;
			int width = 530;
			int space = 2;
			int size = 30;
			int leftSpan = 15;
			int topSpan = 5;
			SquareLine squareLineLa;
			SquareLine squareLineLb;

			ArrayList squareArrayLa = new ArrayList();
			ArrayList squareArrayLb = new ArrayList();
			IGlyph glyph;
			for(int i=0;i<s1.Length;i++)
			{
				glyph = new Square(leftSpan + i*(size + space),topSpan,size,Color.DarkCyan,GlyphAppearance.Flat,s1[i].ToString());
				squareArrayLa.Add(glyph);
			}
			for(int i=0;i<s2.Length;i++)
			{
				glyph = new Square(leftSpan + i*(size + space),topSpan + size + 2,size,Color.DarkCyan,GlyphAppearance.Flat,s2[i].ToString());
				squareArrayLb.Add(glyph);
			}

			squareLineLa = new SquareLine(1,1,1,squareArrayLa);
			squareLineLb = new SquareLine(1,1,1,squareArrayLb);

			IIterator arrayIteratorLa = squareLineLa.CreateIterator();
			IIterator arrayIteratorLb = squareLineLb.CreateIterator();

			Bitmap bmp = new Bitmap(width,height);
			Graphics g = Graphics.FromImage(bmp);

			if(arrayIteratorLa != null)
			{
				for(IIterator iterator = arrayIteratorLa.First();!arrayIteratorLa.IsDone();iterator = arrayIteratorLa.Next())
				{
					iterator.CurrentItem.Draw(g);
				}
			}

			if(arrayIteratorLb != null)
			{
				for(IIterator iterator = arrayIteratorLb.First();!arrayIteratorLb.IsDone();iterator = arrayIteratorLb.Next())
				{
					iterator.CurrentItem.Draw(g);
				}
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
				XmlNode node = table[typeof(SequenceMerge).ToString()] as XmlElement;

				XmlNodeList childNodes  = node.ChildNodes;
		
				StatusItem statusItem = null;

				foreach (XmlElement el in childNodes)
				{
					string s1 = el.Attributes["String1"].Value;
					string s2 = el.Attributes["String2"].Value;

					statusItem = new StatusItem(new SequenceMergeStatus(s1,s2));
					statusItem.Height = 80;
					statusItem.Image = CreatePreviewImage(s1,s2);
					statusItemList.Add(statusItem);
				}
			}

			DialogType = typeof(SequenceMergeDialog);

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
					SequenceMergeStatus tempStatus = selectedItem.ItemInfo as SequenceMergeStatus;
					if(tempStatus != null)
					{
						la = tempStatus.La;
						lb = tempStatus.Lb;
					}
				}
			}
			else  //˵���û�ѡ���Զ�������
			{
				la = status.La;
				lb = status.Lb;

			}
			return true;
			
		}


		public override void Initialize(bool isOpen)
		{
			base.Initialize(isOpen);

			status = new SequenceMergeStatus(la,lb);

			InitGraph();
			
			WorkbenchSingleton.Workbench.ActiveViewContent.SelectView();


		}
		
		
		public override void InitGraph() 
		{
			SquareLine squareLineLa;
			SquareLine squareLineLb;
			SquareLine squareLineLc;

			ArrayList squareArrayLa = new ArrayList();
			ArrayList squareArrayLb = new ArrayList();
			ArrayList squareArrayLc = new ArrayList();
			IGlyph glyph;
			for(int i=0;i<status.LaLength;i++)
			{
				glyph = new Square(40 + i*(squareSize + squareSpace),10 + 5,squareSize,status.ͼ�α���ɫ,status.ͼ�����,status.La[i].ToString());
				squareArrayLa.Add(glyph);
			}
			for(int i=0;i<status.LbLength;i++)
			{
				glyph = new Square(40 + i*(squareSize + squareSpace),10 + squareSize + 2 * 5,squareSize,status.ͼ�α���ɫ,status.ͼ�����,status.Lb[i].ToString());
				squareArrayLb.Add(glyph);
			}

			squareLineLa = new SquareLine(20,30,status.LaLength*(squareSize+20+10),squareArrayLa);
			squareLineLb = new SquareLine(20,110,status.LbLength*(squareSize+20+10),squareArrayLb);
			squareLineLc = new SquareLine(20,190,status.LaLength*(squareSize+20+10) + status.LbLength*(squareSize+20+10),squareArrayLc);
			//����ʼ�����еĵ�����
			arrayIteratorLa = squareLineLa.CreateIterator();
			arrayIteratorLb = squareLineLb.CreateIterator();
			arrayIteratorLc = squareLineLc.CreateIterator();

		}


		public override void ExecuteAndUpdateCurrentLine()
		{
			IGlyph glyphI = null;
			IGlyph glyphJ = null;
			IGlyph insertingGlyph = null;

			switch (CurrentLine)
			{
				case 0:
					//i=j=k=0;
					status.CanEdit = true;
					status.I = 0;
					status.CanEdit = true;
					status.J = 0;
					status.CanEdit = true;
					status.K = 0;
					CurrentLine = 4;
					return;
				case 5:
					//�ж�while��i<La.length && j<Lb.length���Ƿ���������������CurrentLine = 6;��������CurrentLine = 12;
					if(status.I >=status.LaLength || status.J >= status.LbLength)
					{
						CurrentLine = 12;
						return;
					}
					CurrentLine = 7;
					return;
				case 7:
					//�ж�If(La.elem[i]<=Lb.elem[j])�Ƿ���������������CurrentLine = 8;��������CurrentLine = 10;
					glyphI = ((ArrayIterator)arrayIteratorLa).GetGlyphByIndex(status.I);
					glyphJ = ((ArrayIterator)arrayIteratorLb).GetGlyphByIndex(status.J);					
					if(((Square)glyphI).Text[0] > ((Square)glyphJ).Text[0])
					{
						CurrentLine = 10;
						return;
					}
					break;
				case 8:
					//Lc.elem[k++]=La.elem[i++];
					glyphI = ((ArrayIterator)arrayIteratorLa).GetGlyphByIndex(status.I);
					currentGlyph = glyphI;
					((ArrayIterator)arrayIteratorLa).SetBackColor(status.I ,status.��ǰԪ�ر���ɫ,status.ͼ�α���ɫ,true);
					((ArrayIterator)arrayIteratorLb).SetBackColor(-1,status.ͼ�α���ɫ ,status.ͼ�α���ɫ,true);
					insertingGlyph = new Square(40 + k*(squareSize + squareSpace),10 + 2 * squareSize + 3 * 5,squareSize,status.��ǰԪ�ر���ɫ,glyphI.Appearance,((Square)glyphI).Text);
					((ArrayIterator)arrayIteratorLc).InsertGlyph(insertingGlyph);
					status.CanEdit = true;
					status.Lc = status.Lc.Insert(status.Lc.Length,((Square)glyphI).Text);
					status.CanEdit = true;
					status.LcLength += 1;
					k += 1;
					status.CanEdit = true;
					status.I += 1;
					status.CanEdit = true;
					status.K += 1;
					CurrentLine = 5;
					return;
				case 10:
					//Lc.elem[k++]=Lb.elem[j++];
					glyphJ = ((ArrayIterator)arrayIteratorLb).GetGlyphByIndex(status.J);
					currentGlyph = glyphJ;
					((ArrayIterator)arrayIteratorLb).SetBackColor(status.J,status.��ǰԪ�ر���ɫ ,status.ͼ�α���ɫ,true);
					((ArrayIterator)arrayIteratorLa).SetBackColor(-1,status.ͼ�α���ɫ ,status.ͼ�α���ɫ,true);
					insertingGlyph = new Square(40 + k*(squareSize + squareSpace),10 + 2 * squareSize + 3 * 5,squareSize,status.��ǰԪ�ر���ɫ,glyphJ.Appearance,((Square)glyphJ).Text);
					((ArrayIterator)arrayIteratorLc).InsertGlyph(insertingGlyph);
					status.CanEdit = true;
					status.Lc = status.Lc.Insert(status.Lc.Length,((Square)glyphJ).Text);
					status.CanEdit = true;
					status.LcLength += 1;
					k += 1;
					status.CanEdit = true;
					status.J += 1;
					status.CanEdit = true;
					status.K += 1;
					CurrentLine = 5;
					return;
				case 12:
					//�ж�While(i<La.length)�Ƿ����
					if(status.I >=status.LaLength)
					{
						CurrentLine = 14;
						return;
					}
					break;
				case 13:
					//Lc.elem[k++]=La.elem[i++];
					glyphI = ((ArrayIterator)arrayIteratorLa).GetGlyphByIndex(status.I);
					currentGlyph = glyphI;
					((ArrayIterator)arrayIteratorLa).SetBackColor(status.I,status.��ǰԪ�ر���ɫ ,status.ͼ�α���ɫ,true);
					((ArrayIterator)arrayIteratorLb).SetBackColor(-1,status.ͼ�α���ɫ ,status.ͼ�α���ɫ,true);
					insertingGlyph = new Square(40 + k*(squareSize + squareSpace),10 + 2 * squareSize + 3 * 5,squareSize,status.��ǰԪ�ر���ɫ,glyphI.Appearance,((Square)glyphI).Text);
					((ArrayIterator)arrayIteratorLc).InsertGlyph(insertingGlyph);
					status.CanEdit = true;
					status.Lc = status.Lc.Insert(status.Lc.Length,((Square)glyphI).Text);
					status.CanEdit = true;
					status.LcLength += 1;
					k += 1;
					status.CanEdit = true;
					status.I += 1;
					status.CanEdit = true;
					status.K += 1;
					CurrentLine = 12;
					return;
				case 14:
					//�ж�While(j<Lb.length)�Ƿ����
					if(status.J >=status.LbLength)
					{
						CurrentLine = 16;
						return;
					}
					break;
				case 15:
					//Lc.elem[k++]=Lb.elem[j++];
					glyphJ = ((ArrayIterator)arrayIteratorLb).GetGlyphByIndex(status.J);
					currentGlyph = glyphJ;
					((ArrayIterator)arrayIteratorLb).SetBackColor(status.J ,status.��ǰԪ�ر���ɫ,status.ͼ�α���ɫ,true);
					((ArrayIterator)arrayIteratorLa).SetBackColor(-1,status.ͼ�α���ɫ ,status.ͼ�α���ɫ,true);
					insertingGlyph = new Square(40 + k*(squareSize + squareSpace),10 + 2 * squareSize + 3 * 5,squareSize,status.��ǰԪ�ر���ɫ,glyphJ.Appearance,((Square)glyphJ).Text);
					((ArrayIterator)arrayIteratorLc).InsertGlyph(insertingGlyph);
					status.CanEdit = true;
					status.Lc = status.Lc.Insert(status.Lc.Length,((Square)glyphJ).Text);
					status.CanEdit = true;
					status.LcLength += 1;
					k += 1;
					status.CanEdit = true;
					status.J += 1;
					status.CanEdit = true;
					status.K += 1;
					CurrentLine = 14;
					return;
				case 16:
					//Lc.length=k;
					status.LcLength = status.K;
					break;
				case 17:
					return;
			}
			CurrentLine++;
		}

		
		public override void UpdateGraphAppearance()
		{
			for(IIterator iterator = arrayIteratorLa.First();!arrayIteratorLa.IsDone();iterator = arrayIteratorLa.Next())
			{
				iterator.CurrentItem.BackColor = status.ͼ�α���ɫ;
				iterator.CurrentItem.Appearance = status.ͼ�����;
			}
			for(IIterator iterator = arrayIteratorLb.First();!arrayIteratorLb.IsDone();iterator = arrayIteratorLb.Next())
			{
				iterator.CurrentItem.BackColor = status.ͼ�α���ɫ;
				iterator.CurrentItem.Appearance = status.ͼ�����;
			}
			for(IIterator iterator = arrayIteratorLc.First();!arrayIteratorLc.IsDone();iterator = arrayIteratorLc.Next())
			{
				iterator.CurrentItem.BackColor = status.��ǰԪ�ر���ɫ;
				iterator.CurrentItem.Appearance = status.ͼ�����;
			}
			if(currentGlyph != null)
			{
				currentGlyph.BackColor = status.��ǰԪ�ر���ɫ;
			}
		}
		
		
		public override void UpdateAnimationPad() 
		{
			base.UpdateAnimationPad();
			Graphics g = AlgorithmManager.Algorithms.ClearAnimationPad();
			
			if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
			{
				if(arrayIteratorLa != null)
				{
					for(IIterator iterator = arrayIteratorLa.First();!arrayIteratorLa.IsDone();iterator = arrayIteratorLa.Next())
					{
						iterator.CurrentItem.Draw(g);
					}
				}
				if(arrayIteratorLb != null)
				{
					for(IIterator iterator = arrayIteratorLb.First();!arrayIteratorLb.IsDone();iterator = arrayIteratorLb.Next())
					{
						iterator.CurrentItem.Draw(g);
					}
				}
				if(arrayIteratorLc != null)
				{
					for(IIterator iterator = arrayIteratorLc.First();!arrayIteratorLc.IsDone();iterator = arrayIteratorLc.Next())
					{
						iterator.CurrentItem.Draw(g);
					}
				}
			}
		}


	}
}
