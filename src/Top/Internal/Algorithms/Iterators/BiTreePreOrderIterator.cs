using System;
using System.Collections;
using System.Drawing;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// ��Զ��������ݽṹǰ������ĵ�����
	/// </summary>
	public class BiTreePreOrderIterator : IIterator
	{
		IBiTreeNode rootNode = null;  //ǰ������ĸ����
		IBiTreeNode currentNode = null;  //����ʱ�ĵ�ǰ���
		Stack nodesStack = new Stack();

		public Stack NodesStack
		{
			get
			{
				return nodesStack;
			}
		}
		public BiTreePreOrderIterator(IBiTreeNode rootNode)
		{
			this.rootNode = rootNode;
		}
	

		#region IIterator ��Ա

		public IIterator First()
		{
			nodesStack.Clear();
			currentNode = rootNode;

			return this;
		}

		public IIterator Next()
		{

			while(currentNode != null || nodesStack.Count > 0)
			{
				if(currentNode != null)
				{
					nodesStack.Push(currentNode);
					currentNode = currentNode.LeftChild;
				}
				else
				{
					currentNode = (IBiTreeNode)nodesStack.Pop();
					currentNode = currentNode.RightChild;
				}

				if(currentNode != null)
				{
					break;
				}
			}

			return this;
		}

		public bool IsDone()
		{
			return !(currentNode != null || nodesStack.Count > 0);
		}

		public IGlyph CurrentItem
		{
			get
			{
				return currentNode;
			}
		}

		
		#endregion

		public void SetToLeftChild()
		{
			currentNode = currentNode.LeftChild;
		}
		public void SetToRightChild()
		{
			currentNode = currentNode.RightChild;
		}
		public void PopupToCurrentNode()
		{
			currentNode = (IBiTreeNode)nodesStack.Pop();
		}
		public void PushCurrentNode()
		{
			nodesStack.Push(currentNode);
		}

	}
}
