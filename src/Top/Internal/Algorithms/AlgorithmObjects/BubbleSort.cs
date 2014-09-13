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
	public class BubbleSort : AbstractAlgorithm
	{
		ArrayList statusItemList = new ArrayList();
		BubbleSortStatus status = null;
		string r;
		SquareLine squareLine;
		IIterator arrayIterator;
		int startX = 100;
		int maxHeight = 200;
		int heightUnit;
		int histogramWidth = 30;
		int histogramSpace = 30;
		int bottomY = 30;
		ArrayList indexArray = new ArrayList();

		public override object Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value as BubbleSortStatus;
			}
		}

		
		public override void ActiveWorkbenchWindow_CloseEvent(object sender, EventArgs e) 
		{
			arrayIterator = null;
			indexArray = new ArrayList();

			base.ActiveWorkbenchWindow_CloseEvent(sender,e);

		}
		

		public override void Recover()
		{
			arrayIterator = null;
			indexArray = new ArrayList();
			status = new BubbleSortStatus(this.r);
			base.Recover();
		}

		
		Image CreatePreviewImage(string r)
		{
			int width = 530;
			int startX = 10;
			int maxHeight = 145;
			int heightUnit;
			int histogramWidth = 20;
			int histogramSpace = 15;
			int bottomY = 30;

			heightUnit = (maxHeight - bottomY - 15) / r.Length;
			int height = 0;
			ArrayList squareArray = new ArrayList();
			IGlyph glyph;
			glyph = new MyRectangle(startX,maxHeight - bottomY - 45,histogramWidth,45,Color.Gold,GlyphAppearance.Popup,"?");
			squareArray.Add(glyph);
			for(int i=0;i<r.Length;i++)
			{
				height = GetHeight(r,i,heightUnit);
				glyph = new MyRectangle(startX + (i + 1)*(histogramWidth + histogramSpace),maxHeight - bottomY - height,histogramWidth,height,Color.HotPink,GlyphAppearance.Popup,r[i].ToString());
				squareArray.Add(glyph);
			}
			squareLine = new SquareLine(1,1,1,squareArray);

			//����ʼ�����еĵ�����
			IIterator arrayIterator = squareLine.CreateIterator();

			Bitmap bmp = new Bitmap(width,maxHeight);
			Graphics g = Graphics.FromImage(bmp);

			if(arrayIterator != null)
			{
				for(IIterator iterator = arrayIterator.First();!arrayIterator.IsDone();iterator = arrayIterator.Next())
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
				XmlNode node = table[typeof(BubbleSort).ToString()] as XmlElement;

				XmlNodeList childNodes  = node.ChildNodes;
		
				StatusItem statusItem = null;

				foreach (XmlElement el in childNodes)
				{
					string r = el.Attributes["OriginalString"].Value;

					statusItem = new StatusItem(new BubbleSortStatus(r));
					statusItem.Height = 145;
					statusItem.Image = CreatePreviewImage(r);
					statusItemList.Add(statusItem);
				}
			}
			DialogType = typeof(BubbleSortDialog);
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
					BubbleSortStatus tempStatus = selectedItem.ItemInfo as BubbleSortStatus;
					if(tempStatus != null)
					{
						r = tempStatus.R;
					}
				}
			}
			else  //˵���û�ѡ���Զ�������
			{
				r = status.R;
			}
			return true;
			
		}


		public override void Initialize(bool isOpen)
		{
			base.Initialize(isOpen);
			
			status = new BubbleSortStatus(r);

			InitGraph();
			
			WorkbenchSingleton.Workbench.ActiveViewContent.SelectView();
		}

	
		int GetHeight(string l,int index,int heightUnit)
		{
			int height = heightUnit;

			if(index >= l.Length || index < 0)
			{
				return 0;
			}
			char c = l[index];
			for(int i = 0;i < l.Length;i++)
			{
				if(c > l[i])
				{
					height += heightUnit;
				}
			}
			return height;

		}
		public override void InitGraph() 
		{
			heightUnit = (maxHeight - bottomY - 15) / status.N;
			int height = 0;
			ArrayList squareArray = new ArrayList();
			IGlyph glyph;
			glyph = new MyRectangle(startX,maxHeight - bottomY - 100,histogramWidth,100,status.ͷԪ�ر���ɫ,status.ͼ�����,"?");
			squareArray.Add(glyph);
			for(int i=0;i<status.N;i++)
			{
				height = GetHeight(status.R,i,heightUnit);
				glyph = new MyRectangle(startX + (i + 1)*(histogramWidth + histogramSpace),maxHeight - bottomY - height,histogramWidth,height,status.ͼ�α���ɫ,status.ͼ�����,status.R[i].ToString());
				squareArray.Add(glyph);
			}
			squareLine = new SquareLine(1,1,1,squareArray);

			//����ʼ�����еĵ�����
			arrayIterator = squareLine.CreateIterator();

		}

		
		string ch;
		void AssignValue(int index1,int index2)
		{
			int x,y,width,height;
			string text;
			IGlyph glyph2 = ((ArrayIterator)arrayIterator).GetGlyphByIndex(index2);
			IGlyph glyph1 = ((ArrayIterator)arrayIterator).GetGlyphByIndex(index1);
			if(glyph2 != null && glyph1 != null)
			{
				x = glyph1.Bounds.X;  //x���겻��
				y = glyph2.Bounds.Y;
				width = glyph2.Bounds.Width;
				height = glyph2.Bounds.Height;
				text = ((MyRectangle)glyph2).Text;

				glyph1.Bounds = new Rectangle(x,y,width,height);
				((MyRectangle)glyph1).Text = text;
			}

		}
		void AddIndexToIndexArray(int index)
		{
			if(indexArray.Contains(index) == true)
			{
				return;
			}
			indexArray.Add(index);

		}
		public override void ExecuteAndUpdateCurrentLine()
		{
			switch (CurrentLine)
			{
				case 0:
					CurrentLine = 2;
					AddIndexToIndexArray(status.J);
					AddIndexToIndexArray(status.J + 1);
					((ArrayIterator)arrayIterator).SetElementsBackColor(indexArray,status.��ǰ����ɫ,status.ͼ�α���ɫ);
					return;
				case 2:  //for(i = 1;i < n; i++){
					if(status.I >= status.N)
					{
						CurrentLine = 16;
						return;
					}
					break;
				case 3:  //flag = 1;
					status.CanEdit = true;
					status.Flag = 1;
					break;
				case 4:  //for(j = 1;j <= n - i;j++){
					if(status.J > (status.N - status.I))
					{
						CurrentLine = 12;
						return;
					}
					break;
				case 5:  //if(R[j+1].key < R[j].key){
					indexArray.Clear();
					AddIndexToIndexArray(status.J);
					AddIndexToIndexArray(status.J + 1);
					((ArrayIterator)arrayIterator).SetElementsBackColor(indexArray,status.��ǰ����ɫ,status.ͼ�α���ɫ);

					if(status.R[status.J + 1 - 1] >= status.R[status.J - 1])
					{
						status.CanEdit = true;
						status.J++;
						CurrentLine = 4;

						return;
					}
					break;
				case 6:  //flag = 0;
					status.CanEdit = true;
					status.Flag = 0;
					break;
				case 7:  //R[0] = R[j];
					AssignValue(0,status.J);
					ch = status.R[status.J - 1].ToString();
					break;
				case 8:  //R[j] = R[j+1];
					AssignValue(status.J,status.J + 1);
					
					string c = status.R[status.J + 1 - 1].ToString();
					status.CanEdit = true;
					status.R = status.R.Remove(status.J - 1,1);
					status.CanEdit = true;
					status.R = status.R.Insert(status.J - 1,c);
					break;
				case 9:  //R[j+1] = R[0];
					AssignValue(status.J + 1,0);
					status.CanEdit = true;
					status.R = status.R.Remove(status.J + 1 - 1,1);
					status.CanEdit = true;
					status.R = status.R.Insert(status.J + 1 - 1,ch);
					status.CanEdit = true;
					status.J++;
					CurrentLine = 4;

					return;
				case 12: //if(flag == 1){
					if(status.Flag == 0)
					{
						CurrentLine = 2;
						status.CanEdit = true;
						status.I++;
						status.CanEdit = true;
						status.J = 1;
						return;
					}
					break;
				case 13: //return;
					return;
				case 16: //return;
					return;

			}
			CurrentLine++;
		}
		

		public override void UpdateGraphAppearance()
		{
			for(IIterator iterator = arrayIterator.First();!arrayIterator.IsDone();iterator = arrayIterator.Next())
			{
				iterator.CurrentItem.BackColor = status.ͼ�α���ɫ;
				iterator.CurrentItem.Appearance = status.ͼ�����;
			}
			arrayIterator.First().CurrentItem.BackColor = status.ͷԪ�ر���ɫ;
			arrayIterator.First().CurrentItem.Appearance = status.ͼ�����;

			IGlyph glyph;
			glyph = ((ArrayIterator)arrayIterator).GetGlyphByIndex(status.J);
			if(glyph != null)
			{
				glyph.BackColor = status.��ǰ����ɫ;
				glyph.Appearance = status.ͼ�����;
			}
			glyph = ((ArrayIterator)arrayIterator).GetGlyphByIndex(status.J + 1);
			if(glyph != null)
			{
				glyph.BackColor = status.��ǰ����ɫ;
				glyph.Appearance = status.ͼ�����;
			}
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
			}
		}


	}
}
