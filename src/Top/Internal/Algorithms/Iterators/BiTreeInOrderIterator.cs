using System;
using System.Collections;
using System.Drawing;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// ��Զ��������ݽṹ��������ĵ�����
	/// </summary>
	public class BiTreeInOrderIterator : IIterator
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

		public BiTreeInOrderIterator(IBiTreeNode rootNode)
		{
			this.rootNode = rootNode;
		}

		#region IIterator ��Ա

		public IIterator First()
		{
			nodesStack.Clear();
			currentNode = rootNode;

			while(currentNode != null || nodesStack.Count > 0)
			{
				if(currentNode != null)
				{
					nodesStack.Push(currentNode);
					currentNode = currentNode.LeftChild;
				}
				else
				{
					currentNode = nodesStack.Pop() as IBiTreeNode;
					break;
				}
			}

			return this;
		}

		public IIterator Next()
		{
			currentNode = currentNode.RightChild;

			while(currentNode != null || nodesStack.Count > 0)
			{
				if(currentNode != null)
				{
					nodesStack.Push(currentNode);
					currentNode = currentNode.LeftChild;
				}
				else
				{
					currentNode = nodesStack.Pop() as IBiTreeNode;
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

		public void SetToRootNode()
		{
			currentNode = rootNode;
		}

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
