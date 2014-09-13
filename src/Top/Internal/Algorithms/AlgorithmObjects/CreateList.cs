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
	public class CreateList : AbstractAlgorithm
	{
		IIterator circleNodeIterator;//���������������ָ��
		IIterator lineNodeIterator;//������������������ָ��
		IIterator iteratorInsertNode;//��ʾҪ���뵽�����еĽ��
		IIterator nullIteratorBezierLine;//���ڻ�һ������������
		ArrayList statusItemList = new ArrayList();

		int diameter = 50;
		int lineLength = 40;
		int lineWidth = 2;

		CreateListStatus status = null;

		string l;

		public override object Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value as CreateListStatus;
			}
		}

		
		public override void ActiveWorkbenchWindow_CloseEvent(object sender, EventArgs e) 
		{

			circleNodeIterator = null;
			lineNodeIterator   = null;
			iteratorInsertNode = null;
			nullIteratorBezierLine = null;

			base.ActiveWorkbenchWindow_CloseEvent(sender,e);

		}

		public override void Recover()
		{
			circleNodeIterator = null;
			lineNodeIterator   = null;
			iteratorInsertNode = null;
			nullIteratorBezierLine = null;

			status = new CreateListStatus(l);
			base.Recover();
		}

		
		Image CreatePreviewImage(string s)
		{
			int height = 80;
			int width = 530;
			int lineLength = 18;
			int diameter = 30;
			int leftSpan = 3;
			int topSpan = 20;


			IIterator circleNodeIterator;//���������������ָ��
			IIterator lineNodeIterator;//������������������ָ��

			LinkCircleNode headNode = null;
			LinkLineNode   headLineNode = null;
			LinkCircleNode previousNode = null;
			LinkLineNode   previousLineNode = null;
			LinkCircleNode currentNode = null;
			LinkLineNode   currentLineNode = null;
			previousNode = headNode = new LinkCircleNode(leftSpan,topSpan,diameter,Color.Red,"H");
			previousLineNode = headLineNode = new LinkLineNode(leftSpan + diameter,topSpan + diameter/2,leftSpan + diameter + lineLength,topSpan + diameter/2,lineWidth,Color.Red);

			for(int i = 0;i < s.Length;i++)
			{
				currentNode = new LinkCircleNode(leftSpan + (i + 1) * (lineLength + diameter),topSpan,diameter,Color.DarkTurquoise,s[i].ToString());
				currentLineNode = new LinkLineNode(leftSpan + diameter + (i + 1) * (lineLength + diameter),topSpan + diameter/2,leftSpan + diameter + (i + 1) * (lineLength + diameter) + lineLength,topSpan + diameter/2,lineWidth,Color.Red);
				
				previousNode.Next = currentNode;
				previousLineNode.Next = currentLineNode;

				previousNode = currentNode;
				previousLineNode = currentLineNode;

			}

			if(currentNode != null)
			{
				currentNode.Next = null;
				currentLineNode.Next = null;
			}
			circleNodeIterator = headNode.CreateIterator(); 
			lineNodeIterator = headLineNode.CreateIterator();

			Bitmap bmp = new Bitmap(width,height);
			Graphics g = Graphics.FromImage(bmp);

			if(circleNodeIterator != null)
			{
				for(IIterator iterator = circleNodeIterator.First();!circleNodeIterator.IsDone();iterator = circleNodeIterator.Next())
				{
					iterator.CurrentItem.Draw(g);
				}
			}
			if(lineNodeIterator != null)
			{
				for(IIterator iterator = lineNodeIterator.First();!lineNodeIterator.IsDone();iterator = lineNodeIterator.Next())
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
				XmlNode node = table[typeof(CreateList).ToString()] as XmlElement;

				XmlNodeList childNodes  = node.ChildNodes;
		
				StatusItem statusItem = null;

				foreach (XmlElement el in childNodes)
				{
					string s = el.Attributes["ListString"].Value;

					statusItem = new StatusItem(new CreateListStatus(s));
					statusItem.Height = 80;
					statusItem.Image = CreatePreviewImage(s);
					statusItemList.Add(statusItem);
				}
			}
			DialogType = typeof(CreateListDialog);

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
					CreateListStatus tempStatus = selectedItem.ItemInfo as CreateListStatus;
					if(tempStatus != null)
					{
						l = tempStatus.L;
					}
				}
			}
			else  //˵���û�ѡ���Զ�������
			{
				l = status.L;
			}
			return true;
			
		}

		
		public override void Initialize(bool isOpen)
		{
			base.Initialize(isOpen);
			
			status = new CreateListStatus(l);

			InitGraph();
			
			WorkbenchSingleton.Workbench.ActiveViewContent.SelectView();
	
		}

		
		public override void InitGraph() 
		{
			//����㷨�տ�ʼ����Ҫ���κ�ͼ�εĳ�ʼ��

		}

		
		Point[] GetPoints()
		{
			int x0,y0,x1,y1,x2,y2,x3,y3;
			x0 = iteratorInsertNode.First().CurrentItem.Bounds.X + diameter;
			y0 = iteratorInsertNode.First().CurrentItem.Bounds.Y + diameter / 2;
			x1 = x0 + 30;
			y1 = y0 - diameter / 5;
			x2 = x0 - 30;
			y2 = y0 - 2 * diameter / 5;
			x3 = x0 - 5;
			y3 = y0 - diameter;
			Point point0 = new Point(x0,y0);
			Point point1 = new Point(x1,y1);
			Point point2 = new Point(x2,y2);
			Point point3 = new Point(x3,y3);

			Point[] points = new Point[]{point0,point1,point2,point3};

			return points;
		}
		public override void ExecuteAndUpdateCurrentLine()
		{
			switch (CurrentLine)
			{
				case 0:
					CurrentLine = 3;
					return;
				case 3:  //L=(LinkList)malloc(sizeof(Lnode));  L->next=NULL;
					//����һ��ͷ�ڵ�
					LinkCircleNode headNode = null;
					LinkLineNode   headLineNode = null;
					headNode = new LinkCircleNode(40,50,diameter,status.ͷ�����ɫ,"H");
					headLineNode = new LinkLineNode(40 + diameter,50 + diameter/2,40 + diameter + lineLength,50 + diameter/2,lineWidth,Color.Red);
					headNode.Next = null;
					headLineNode.Next = null;
					circleNodeIterator = headNode.CreateIterator(); 
					lineNodeIterator = headLineNode.CreateIterator();
					break;
				case 4:  //for(i=n;i>0;i--){
					if(status.I == 0)
					{
						CurrentLine = 9;
						return;
					}
					break;
				case 5:  //p=(LinkList)malloc(sizeof(Lnode));  scanf(&p->data);
					if(iteratorInsertNode != null)
					{
						((NodeListIterator)circleNodeIterator).RefreshAllNodes((LinkCircleNode)iteratorInsertNode.First().CurrentItem,40,50,diameter,lineLength,status.�����ɫ);
					}
					iteratorInsertNode = new LinkCircleNode(40 + diameter - 4,50 + diameter,diameter,status.��������ɫ,status.L[status.I - 1].ToString()).CreateIterator();
					status.CanEdit = true;
					status.P = "p��ǰָ����" + ((LinkCircleNode)iteratorInsertNode.First().CurrentItem).Text;
					break;
				case 6:  //p->next=L->next;
					((LinkCircleNode)iteratorInsertNode.First().CurrentItem).Next = ((LinkCircleNode)circleNodeIterator.First().CurrentItem).Next;
					//���ɱ���������
					nullIteratorBezierLine = new BezierLine(new Rectangle(1,1,1,1),GetPoints(),lineWidth,Color.Red).CreateIterator();
					break;
				case 7:  //L->next=p;
					((LinkCircleNode)circleNodeIterator.First().CurrentItem).Next = (LinkCircleNode)iteratorInsertNode.First().CurrentItem;
					//ˢ�����еĽ���λ��
					((NodeListIterator)circleNodeIterator).RefreshAllNodes((LinkCircleNode)iteratorInsertNode.First().CurrentItem,40,50,diameter,lineLength,status.�����ɫ);
					//���õ�ǰ������ɫ
					((NodeListIterator)circleNodeIterator).SetCurrentItemNewColor(iteratorInsertNode.First().CurrentItem,status.��������ɫ,status.�����ɫ);
					//����������һ����ͷ,���������뵽��ͷ����,���ˢ�������ͷ����
					LinkLineNode lineNode = new LinkLineNode(1,1,1 + lineLength,1,2,Color.Red);
					lineNode.Next = ((LinkLineNode)lineNodeIterator.First().CurrentItem).Next;
					((LinkLineNode)lineNodeIterator.First().CurrentItem).Next = lineNode;
					((NodeListIterator)lineNodeIterator).RefreshAllNodes((LinkLineNode)lineNodeIterator.First().CurrentItem,40 - lineLength,50 + diameter/2,diameter,lineLength,Color.Red);
					//ɾ������������
					nullIteratorBezierLine = null;
					//i--
					status.CanEdit = true;
					status.I--;
					CurrentLine = 4;
					return;
				case 9:
					return;
			}
			CurrentLine++;
		}
		

		public override void UpdateGraphAppearance()
		{
			if(circleNodeIterator != null)
			{
				for(IIterator iterator = circleNodeIterator.First();!circleNodeIterator.IsDone();iterator = circleNodeIterator.Next())
				{
					iterator.CurrentItem.BackColor = status.�����ɫ;
				}
				//����ͷ���
				IGlyph headNode = circleNodeIterator.First().CurrentItem;
				headNode.BackColor = status.ͷ�����ɫ;
				//����ǰ���
				if(iteratorInsertNode != null)
				{
					iteratorInsertNode.First().CurrentItem.BackColor = status.��������ɫ;
				}
			}
		}
		
		
		public override void UpdateAnimationPad() 
		{
			base.UpdateAnimationPad();
			Graphics g = AlgorithmManager.Algorithms.ClearAnimationPad();
			
			if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
			{
				if(circleNodeIterator != null)
				{
					for(IIterator iterator = circleNodeIterator.First();!circleNodeIterator.IsDone();iterator = circleNodeIterator.Next())
					{
						iterator.CurrentItem.Draw(g);
					}
				}
				if(lineNodeIterator != null)
				{
					for(IIterator iterator = lineNodeIterator.First();!lineNodeIterator.IsDone();iterator = lineNodeIterator.Next())
					{
						iterator.CurrentItem.Draw(g);
					}
				}
				if(iteratorInsertNode != null)
				{
					iteratorInsertNode.First().CurrentItem.Draw(g);
				}
				if(nullIteratorBezierLine != null)
				{
					nullIteratorBezierLine.CurrentItem.Draw(g);
				}
			}
		}


	}
}
