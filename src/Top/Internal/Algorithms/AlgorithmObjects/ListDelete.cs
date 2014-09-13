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
	public class ListDelete : AbstractAlgorithm
	{
		IIterator circleNodeIterator;//���������������ָ��
		IIterator lineNodeIterator;//������������������ָ��
		IIterator nullIteratorP;//��ͷp
		IIterator nullIteratorQ;//��ͷq
		IIterator swerveLineIterator;//������ת�����
		IIterator circleNodeIterator1;//ָ��p
		ArrayList statusItemList = new ArrayList();
		int diameter = 50;
		int lineLength = 40;
		int lineWidth = 2;

		string l;
		int i;
		ListDeleteStatus status = null;

		public override object Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value as ListDeleteStatus;
			}
		}
		
		
		public override void ActiveWorkbenchWindow_CloseEvent(object sender, EventArgs e) 
		{
			
			circleNodeIterator = null;
			lineNodeIterator   = null;
			nullIteratorP      = null;
			nullIteratorQ      = null;
			circleNodeIterator1= null;
			swerveLineIterator = null;

			base.ActiveWorkbenchWindow_CloseEvent(sender,e);


		}

	
		public override void Recover()
		{
			circleNodeIterator = null;
			lineNodeIterator   = null;
			nullIteratorP      = null;
			nullIteratorQ      = null;
			circleNodeIterator1= null;
			swerveLineIterator = null;

			status = new ListDeleteStatus(l,i);
			base.Recover();
		}


		Image CreatePreviewImage(string s,int pos)
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
				if(i != pos - 1)
				{
					currentNode = new LinkCircleNode(leftSpan + (i + 1) * (lineLength + diameter),topSpan,diameter,Color.DarkTurquoise,s[i].ToString());
				}
				else
				{
					currentNode = new LinkCircleNode(leftSpan + (i + 1) * (lineLength + diameter),topSpan,diameter,Color.Yellow,s[i].ToString());
				}
				currentLineNode = new LinkLineNode(leftSpan + diameter + (i + 1) * (lineLength + diameter),topSpan + diameter/2,leftSpan + diameter + (i + 1) * (lineLength + diameter) + lineLength,topSpan + diameter/2,lineWidth,Color.Red);
				previousNode.Next = currentNode;
				previousLineNode.Next = currentLineNode;

				previousNode = currentNode;
				previousLineNode = currentLineNode;

			}

			currentNode.Next = null;
			currentLineNode.Next = null;

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
				XmlNode node = table[typeof(ListDelete).ToString()] as XmlElement;

				XmlNodeList childNodes  = node.ChildNodes;
		
				StatusItem statusItem = null;

				foreach (XmlElement el in childNodes)
				{
					string l = el.Attributes["OriginalString"].Value;
					int i = Convert.ToInt32(el.Attributes["DeletePosition"].Value);

					statusItem = new StatusItem(new ListDeleteStatus(l,i));
					statusItem.Height = 80;
					statusItem.Image = CreatePreviewImage(l,i);
					statusItemList.Add(statusItem);
				}
			}
			DialogType = typeof(ListDeleteDialog);

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
					ListDeleteStatus tempStatus = selectedItem.ItemInfo as ListDeleteStatus;
					if(tempStatus != null)
					{
						l = tempStatus.L;
						i = tempStatus.I;
					}
				}
			}
			else  //˵���û�ѡ���Զ�������
			{
				l = status.L;
				i = status.I;
			}
			return true;
			
		}


		public override void Initialize(bool isOpen)
		{
			base.Initialize(isOpen);
			
			status = new ListDeleteStatus(l,i);

			InitGraph();
			
			WorkbenchSingleton.Workbench.ActiveViewContent.SelectView();

		}
		
		
		public override void InitGraph() 
		{
			LinkCircleNode previousNode = null; //��ǰҪ�����Ľڵ��ǰ���ڵ�
			LinkCircleNode currentNode = null;  //��ǰҪ�����Ľڵ�
			LinkCircleNode headNode = null;     //ͷ�ڵ�

			//����һ����Ҳ����һ���ڵ�
			LinkLineNode   headLineNode = null;
			LinkLineNode   currentLineNode = null;     
			LinkLineNode   previousLineNode = null;

			headNode = new LinkCircleNode(40,50,diameter,status.ͷ�����ɫ,"H");  //�ȴ���һ��ͷ�ڵ�
			headLineNode = new LinkLineNode(40 + diameter,50 + diameter/2,40 + diameter + lineLength,50 + diameter/2,lineWidth,Color.Red);

			for(int i=0;i<status.Length;i++)
			{
				currentNode = new LinkCircleNode(40 + (i+1)*(diameter + lineLength),50,diameter,status.�����ɫ,status.L[i].ToString());
				currentLineNode = new LinkLineNode(40 + diameter + (i+1)*(diameter + lineLength),50 + diameter/2,40 + diameter + (i+1)*(diameter + lineLength) + lineLength,50 + diameter/2,lineWidth,Color.Red);
				
				if(i == 0)  //˵����ǰ�����Ľڵ�Ϊ��һ���ڵ�
				{
					headNode.Next = currentNode;
					headLineNode.Next = currentLineNode;
				}
				else
				{
					previousNode.Next = currentNode;
					previousLineNode.Next = currentLineNode;
				}
				previousNode = currentNode;
				previousLineNode = currentLineNode;

				
			}

			currentNode.Next = null;  //����Ҫע��,Ҫ�����һ���ڵ��Next����Ϊnull
			currentLineNode.Next = null;

			circleNodeIterator = headNode.CreateIterator(); 
			circleNodeIterator1 = headNode.CreateIterator(); 
			lineNodeIterator = headLineNode.CreateIterator();

		}

		
		Point[] GetPoints(int x,int y,int width,int height)
		{
			Point point0 = new Point(x - lineLength / 2,y);
			Point point1 = new Point(x,y);
			Point point2 = new Point(x,y + height);
			Point point3 = new Point(x + width,y + height);
			Point point4 = new Point(x + width,y);
			Point[] points = new Point[]{point0,point1,point2,point3,point4};

			return points;
		}
		public override void ExecuteAndUpdateCurrentLine()
		{
			switch (CurrentLine)
			{
				case 0:
					CurrentLine = 2;
					return;
				case 2:
					circleNodeIterator1 = circleNodeIterator1.First();//ʹpָ��ͷ���
					status.CanEdit = true;
					status.J = 0;
					nullIteratorP = new Pointer(circleNodeIterator1.CurrentItem.Bounds.X - 33,circleNodeIterator1.CurrentItem.Bounds.Y - 33,Color.Purple,"p").CreateIterator();
					status.CanEdit = true;
					status.P = "p��ǰָ����" + ((LinkCircleNode)circleNodeIterator1.CurrentItem).Text;
					break;
				case 3:  //while(p->next && j<i-1)
					//�ж�while��������Ƿ���������������CurrentLine = 4;��������CurrentLine = 6;
					if(circleNodeIterator1 == null || status.J >= status.I - 1)
					{
						CurrentLine = 6;
						return;
					}
					break;
				case 4:  //p = p->next; j++;
					circleNodeIterator1 = circleNodeIterator1.Next();  //p����һ�����
					((NodeListIterator)circleNodeIterator).SetCurrentItemNewColor(circleNodeIterator1.CurrentItem,status.��ǰ�����ɫ,status.�����ɫ);
					status.CanEdit = true;
					status.P = "p��ǰָ����" + ((LinkCircleNode)circleNodeIterator1.CurrentItem).Text;
					//��ָ��p���õ��µ�λ��
					((Pointer)nullIteratorP.CurrentItem).SetToNewPosition(circleNodeIterator1.CurrentItem.Bounds.X - 33,circleNodeIterator1.CurrentItem.Bounds.Y - 33);
					status.CanEdit = true;
					status.J++;
					CurrentLine = 3;
					return;
				case 6:
					CurrentLine = 8;
					return;
				case 8:  //q = p->next;
					IGlyph glyph = ((NodeListIterator)circleNodeIterator1).GetNextGlyph();//ע��:���ﲻ��ȡ��p��ָ��ͼԪ,���Ұ�pָ������һ��
					nullIteratorQ = new Pointer(glyph.Bounds.X - 33,glyph.Bounds.Y - 33,Color.Red,"q").CreateIterator();
					break;
				case 9:  //p->next = q->next;
					int x,y,width,height;
					x = circleNodeIterator1.CurrentItem.Bounds.X + diameter + lineLength / 2;
					y = circleNodeIterator1.CurrentItem.Bounds.Y + diameter / 2;
					width = diameter + lineLength;
					height = diameter;
					swerveLineIterator = new SwerveLine(new Rectangle(x,y,width,height),GetPoints(x,y,width,height),2,Color.Red).CreateIterator();
					((NodeListIterator)lineNodeIterator).SetNodeUnVisible(status.I - 1);
					status.CanEdit = true;
					status.L = status.L.Remove(status.I - 1,1);
					status.CanEdit = true;
					status.Length -= 1; 
					break;
				case 10:  //e = q->data; free(q);
					status.CanEdit = true;
					status.E = ((LinkCircleNode)((NodeListIterator)circleNodeIterator1).GetNextGlyph()).Text[0];
					//ɾ��q��ָ�Ľ��
					((NodeListIterator)circleNodeIterator).DeleteNodeAndRefresh(status.I,diameter,lineLength);
					//ɾ��һ��������,������Ϊ���е������߶�һ��,�������ָ��һ��ɾ������������,����,������ָ��Ϊstatus.Length,��ɾ�����һ����
					((NodeListIterator)lineNodeIterator).DeleteNodeAndRefresh(status.Length,diameter,lineLength);
					//ɾ��qָ��
					nullIteratorQ = null;
					//���ղ�����Ϊ͸������������ʾ����
					((NodeListIterator)lineNodeIterator).SetNodeVisible(status.I - 1,Color.Red);
					//ɾ��ת�����
					swerveLineIterator = null;
					break;
				case 11:
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
				if(circleNodeIterator1 != null && circleNodeIterator1.CurrentItem != null)
				{
					circleNodeIterator1.CurrentItem.BackColor = status.��ǰ�����ɫ;
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
				if(nullIteratorP != null)
				{
					nullIteratorP.CurrentItem.Draw(g);
				}
				if(nullIteratorQ != null)
				{
					nullIteratorQ.CurrentItem.Draw(g);
				}
				if(swerveLineIterator != null)
				{
					swerveLineIterator.CurrentItem.Draw(g);
				}
			}
		}


	}
}
