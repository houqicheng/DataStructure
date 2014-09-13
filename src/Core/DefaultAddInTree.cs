
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Xml;

using NetFocus.Components.AddIns.Attributes;
using NetFocus.Components.AddIns.Codons;

namespace NetFocus.Components.AddIns
{
	/// <summary>
	/// Default implementation for the <see cref="IAddInTree"/> interface.
	/// </summary>
	public class DefaultAddInTree : IAddInTree
	{
		AddInCollection addIns = new AddInCollection();
		DefaultAddInTreeNode  root = new DefaultAddInTreeNode();//Ӧ�ó��������ò�����ĸ��ڵ�,�տ�ʼʱû���κ��ӽڵ�(��ChildNodes)
		CodonFactory    codonFactory    = new CodonFactory();
		Hashtable registeredAssemblies = new Hashtable();//���ڻ����Ѿ�װ�ع��ĳ���

		/// <summary>
		/// Returns the default codon factory. ICodon objects
		/// are created only with this factory during the tree 
		/// construction process.
		/// </summary>
		public CodonFactory CodonFactory {
			get {
				return codonFactory;
			}
		}

		/// <summary>
		/// Returns a collection of all loaded add ins.
		/// </summary>
		public AddInCollection AddIns {
			get {
				return addIns;
			}
		}
		
		/// <summary>
		/// Constructs a new instance of the <code>DefaultAddInTree</code> object.
		/// </summary>
		public DefaultAddInTree()
		{

		}
		
		/// <summary>
		/// Create a tree node
		/// </summary>
		/// <param name="currentNode">the local root node in the add in tree</param>
		/// <param name="path">the path of the node</param>
		/// <returns></returns>
		DefaultAddInTreeNode CreateTreeNode(DefaultAddInTreeNode currentNode, string path)
		{
			if (path == null || path.Length == 0) 
			{
				return currentNode;
			}
			string[] splittedPath = path.Split(new char[] {'/'});
			DefaultAddInTreeNode curNode = currentNode;
			int i = 0;
			//��㴴��Ҫ�����Ľڵ�,���ĳ��Ľڵ�δ����,�򴴽�֮,����Ϊ��ǰ�ڵ���ӽڵ�;
			//����Ѿ������˸ýڵ�,��ֱ�ӽ��ýڵ���Ϊ��ǰ�ڵ�
			while (i < splittedPath.Length) 
			{
				DefaultAddInTreeNode childNode = (DefaultAddInTreeNode)curNode.ChildNodes[splittedPath[i]];
				if (childNode == null) 
				{
					childNode = new DefaultAddInTreeNode();
					curNode.ChildNodes[splittedPath[i]] = childNode;
				}
				curNode = childNode;
				++i;
			}
			
			return curNode;
		}
		
		/// <summary>
		/// Add a <see cref="AddIn"/> object to the tree, inserting all it's extensions.
		/// </summary>
		public void InsertAddIn(AddIn addIn)
		{
			addIns.Add(addIn);
			foreach (AddIn.Extension extension in addIn.Extensions) {
				//�ȸ���һ����<Extension path = "/Workspace/Services">֮���extension.Path��չ·������һ���ڵ�
				DefaultAddInTreeNode currentNode = CreateTreeNode(root, extension.Path);
				//Ȼ��Ը���չ·���µĴ����Ӽ����б���е���,�ֱ𴴽�ΪcurrentNode���ӽڵ�,�Թ���һ����״�ṹ		
				foreach (ICodon codon in extension.CodonCollection) 
				{
					DefaultAddInTreeNode currentChildNode = CreateTreeNode(currentNode, codon.ID);
					if (currentChildNode.Codon != null) 
					{
						throw new Exception("�Ѿ�����һ����Ϊ : " + codon.ID + " �Ĵ�����");
					}
					currentChildNode.Codon              = codon;
				}

			}
		}
		
		/// <summary>
		/// Removes an AddIn from the AddInTree.
		/// </summary>
		public void RemoveAddIn(AddIn addIn)
		{ // TODO : Implement the RemoveAddInMethod
			throw new ApplicationException("Implement ME!");
		}
		
		/// <summary>
		/// Searches a requested path and returns the TreeNode in this path as value.
		/// If path is <code>null</code> or path.Length is zero the root node is returned.
		/// </summary>
		/// <param name="path">
		/// The path inside the tree structure.
		/// </param>
		/// <exception cref="TreePathNotFoundException">
		/// Is thrown when the path is not found in the tree.
		/// </exception>
		public IAddInTreeNode GetTreeNode(string path)
		{
			if (path == null || path.Length == 0) {
				return root;
			}
			string[] splittedPath = path.Split(new char[] {'/'});
			DefaultAddInTreeNode curPath = root;  //ÿ��Ҫ��ĳ���ڵ��·��ʱ,���ǴӸ��ڵ����
			int i = 0;
			
			while (i < splittedPath.Length) {
				DefaultAddInTreeNode nextPath = (DefaultAddInTreeNode)curPath.ChildNodes[splittedPath[i]];
				if (nextPath == null) {
					throw new Exception("�����·��û���ҵ� : " + path);
				}
				curPath = nextPath;
				++i;
			}
			
			return curPath;   //�����ҵ��Ĳ�����е��߼�·��,һ�����
		}
		
		/// <summary>
		/// This method loads an assembly and gets all 
		/// it's defined codons and conditions
		/// </summary>
		public Assembly LoadAssembly(string fileName)
		{
			Assembly assembly = (Assembly)registeredAssemblies[fileName];
			
			if (assembly == null) {
				assembly = Assembly.LoadFrom(fileName);
				registeredAssemblies[fileName] = assembly;
				LoadCodons(assembly);
			}
			
			return assembly;
		}

		/// <summary>
		/// This method does load all codons and conditions in the given assembly.
		/// It will create builders for them which could be used by the factories to
		/// create the codon and condition objects.
		/// </summary>
		void LoadCodons(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			foreach(Type type in types) {
				if (!type.IsAbstract) {
					if (type.IsSubclassOf(typeof(AbstractCodon)) && Attribute.GetCustomAttribute(type, typeof(CodonNameAttribute)) != null) {
						codonFactory.AddCodonBuilder(new CodonBuilder(type.FullName, assembly));
					}
				}
			}
		}
	}
}
