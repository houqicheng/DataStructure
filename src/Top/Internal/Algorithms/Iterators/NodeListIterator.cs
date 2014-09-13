using System;
using System.Collections;
using System.Drawing;


namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// һ���������ݽṹ�ĵ�����(��װһ����������ݽṹ)
	/// </summary>
	public class NodeListIterator : IIterator
	{
		INode headNode = null;  //ָ��ͷ�ڵ�
		INode currentNode = null;  //ָ��ǰ�ڵ�

		public NodeListIterator(INode headNode)
		{
			this.headNode = headNode;
		}

		
		#region IIterator ��Ա

		public IIterator First()
		{
			currentNode = headNode;  //ʹ��ǰ�ڵ�ָ��ͷ�ڵ�
			return this;
		}

		public IIterator Next()
		{
			currentNode = currentNode.Next;  //ʹ��ǰ�ڵ�ָ����һ���ڵ�
			return this;
		}

		public bool IsDone()
		{
			return currentNode == null;
		}

		public IGlyph CurrentItem
		{
			get
			{
				return currentNode;
			}
		}

	
		#endregion


		public IGlyph GetNextGlyph()
		{
			return currentNode.Next;
		}
		

		public void SetCurrentItemNewColor(IGlyph glyph,Color newColor,Color oldColor)
		{
			INode currentNode = headNode.Next;
			
			while(currentNode != null)
			{
				if(currentNode.ToString() == glyph.ToString())
				{
					currentNode.BackColor = newColor;
				}
				else
				{
					currentNode.BackColor = oldColor;
				}
				currentNode = currentNode.Next;
			}
			
		}

		

		public void SetNodeUnVisible(int index)
		{
			int i = 0;
			INode preNode = headNode;

			while(preNode.Next != null && i < index - 1)
			{
				preNode = preNode.Next;
				i++;
			}
			if(preNode.Next != null)
			{
				if(index > 0)
				{
					preNode.Next.BackColor = Color.Transparent;
				}
				else
				{
					preNode.BackColor = Color.Transparent;
				}
			}
		}
		

		public void SetNodeVisible(int index,Color color)
		{
			int i = 0;
			INode preNode = headNode;

			while(preNode.Next != null && i < index - 1)
			{
				preNode = preNode.Next;
				i++;
			}
			if(preNode.Next != null)
			{
				if(index > 0)
				{
					preNode.Next.BackColor = color;
				}
				else
				{
					preNode.BackColor = color;
				}
			}
		}

		
		public void DeleteNodeAndRefresh(int index,int diameter,int lineLength)
		{
			int i = 0;
			INode preNode = headNode;

			while(preNode.Next != null && i < index - 1)
			{
				preNode = preNode.Next;
				i++;
			}
			
			if(preNode.Next != null)
			{
				preNode.Next = preNode.Next.Next;  //ɾ�����
				RefreshLaterNodes(preNode.Next,diameter,lineLength);
			}

		}

	    
		void RefreshLaterNodes(INode currentNode,int diameter,int lineLength)
		{
			INode node = currentNode;
			while(node != null)
			{
				node.Bounds = new Rectangle(node.Bounds.X - diameter - lineLength,node.Bounds.Y,node.Bounds.Width,node.Bounds.Height );
				node = node.Next;
				
			}

		}
		void RefreshLaterNodes1(INode currentNode,int diameter,int lineLength)
		{
			INode node = currentNode;
			while(node != null)
			{
				node.Bounds = new Rectangle(node.Bounds.X + diameter + lineLength,node.Bounds.Y,node.Bounds.Width,node.Bounds.Height );
				node = node.Next;
				
			}

		}

		
		public void InsertNodeAndRefresh(int index,INode node,int diameter,int lineLength)
		{
			int i = 0;
			INode preNode = headNode;

			while(preNode.Next != null && i < index - 1)
			{
				preNode = preNode.Next;
				i++;
			}
			
			if(preNode.Next != null)
			{
				node.Next = preNode.Next;//������
				preNode.Next = node; 
				RefreshLaterNodes1(node.Next,diameter,lineLength);
			}
			else
			{
				preNode.Next = node;

				//���ﲻ��Ҫ�ƶ������Ԫ�أ���Ϊ�ղŲ����Ԫ���Ѿ������һ��Ԫ����
				//RefreshLaterNodes1(node,diameter,lineLength);
			}
		}

		public void RefreshAllNodes(INode startNode,int startNodeX,int startNodeY,int diameter,int lineLength,Color refreshColor)
		{
			int i = 0;
			INode currentNode = startNode;
			while(currentNode != null)
			{
				currentNode.Bounds = new Rectangle(startNodeX + (i + 1)*(diameter + lineLength),startNodeY,startNode.Bounds.Width,startNode.Bounds.Height);
				currentNode.BackColor = refreshColor;
				currentNode = currentNode.Next;
				i++;
			}

		}
	
	
	}
}
