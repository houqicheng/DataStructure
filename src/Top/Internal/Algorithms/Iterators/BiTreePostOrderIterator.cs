using System;
using System.Collections;
using System.Drawing;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// ��Զ��������ݽṹ��������ĵ�����
	/// </summary>
	public class BiTreePostOrderIterator : IIterator
	{
		IBiTreeNode rootNode = null;  //ǰ������ĸ����
		IBiTreeNode currentNode = null;  //����ʱ�ĵ�ǰ���
		IBiTreeNode currentBackupNode = null;//��������ոձ����Ľ��
		Stack nodesStack = new Stack();

		public Stack NodesStack
		{
			get
			{
				return nodesStack;
			}
		}

		public BiTreePostOrderIterator(IBiTreeNode rootNode)
		{
			this.rootNode = rootNode;
		}

		#region IIterator ��Ա
		
		public IIterator First()
		{
			nodesStack.Clear();
			currentBackupNode = null; //ע��,���ﲻ������,��һ�β��ñ���ոձ������Ľ��
			currentNode = rootNode;  //��ָ������
			nodesStack.Push(currentNode);

			while(nodesStack.Count > 0)
			{
				if(currentNode != null && currentNode != currentBackupNode)  //��ǰ��㲻�ղ���û�б����ʹ�
				{
					nodesStack.Push(currentNode);
					currentNode = currentNode.LeftChild;
				}
				else
				{	
					currentNode = nodesStack.Pop() as IBiTreeNode;  //��ǰcurrentNode��������߽��

					if(currentNode.RightChild != null && currentNode.RightChild != currentBackupNode)  //��ǰ�����ҽ�㲻�ղ���û�б����ʹ�
					{
						nodesStack.Push(currentNode);
						currentNode = currentNode.RightChild;
					}
					else
					{
						currentBackupNode = currentNode;
						break;
					}
				}
			}
			
			return this;
		}

		public IIterator Next()
		{
			while(nodesStack.Count > 0)
			{
				if(currentNode != null && currentNode != currentBackupNode)  //��ǰ��㲻�ղ���û�б����ʹ�
				{
					nodesStack.Push(currentNode);
					currentNode = currentNode.LeftChild;
				}
				else
				{	
					currentNode = nodesStack.Pop() as IBiTreeNode;  //��ǰcurrentNode��������߽��

					if(currentNode.RightChild != null && currentNode.RightChild != currentBackupNode)  //��ǰ�����ҽ�㲻�ղ���û�б����ʹ�
					{
						nodesStack.Push(currentNode);
						currentNode = currentNode.RightChild;
					}
					else
					{
						currentBackupNode = currentNode;
						break;
					}
				}
			}

			return this;
		}

		public bool IsDone()
		{
			return nodesStack.Count == 0;
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
		public void SetToNewNode(IBiTreeNode newNode)
		{
			currentNode = newNode;
		}

	}
}