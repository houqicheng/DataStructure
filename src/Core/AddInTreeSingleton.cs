
using System;
using System.Configuration;
using System.Reflection;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Xml;

using NetFocus.Components.AddIns.Codons;

namespace NetFocus.Components.AddIns
{
	/// <summary>
	/// Here is the ONLY point to get an <see cref="IAddInTree"/> object,the example of singleton design pattern
	/// </summary>
	public class AddInTreeSingleton : DefaultAddInTree,IConfigurationSectionHandler
	{
		static IAddInTree addInTree = null;
		static bool ignoreDefaultCoreAddInDirectory = false;
		static string[] addInDirectories       = null;
		readonly static string defaultCoreAddInDirectory = Application.StartupPath + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "AddIns";

		public static IAddInTree AddInTree 
		{
			get {
				if (addInTree == null) { //��������˶��Գ�ʼ��
					CreateAddInTree();
				}
				return addInTree;
			}
		}

		
		public static bool SetAddInDirectories(string[] addInDirectories, bool ignoreDefaultCoreAddInDirectory)
		{
			if (addInDirectories == null || addInDirectories.Length < 1) 
			{
				// ·��Ϊ��,����.
				return false;
			}
			AddInTreeSingleton.addInDirectories = addInDirectories;
			AddInTreeSingleton.ignoreDefaultCoreAddInDirectory = ignoreDefaultCoreAddInDirectory;
			return true;
		}

		
		public static string[] GetAddInDirectories(out bool ignoreDefaultAddInPath)
		{
			//ͨ����仰������IConfigurationSectionHandler�ӿ��ж����Create����
			ArrayList addInDirs = ConfigurationManager.GetSection("AddInDirectories") as ArrayList;

			if (addInDirs != null) 
			{
				int count = addInDirs.Count;
				if (count <= 1) //�����һ����û��ָ���Զ������ļ���·��
				{
					ignoreDefaultAddInPath = false;
					return null;
				}
				ignoreDefaultAddInPath = (bool) addInDirs[0];

				string [] directories = new string[count-1];
				for (int i = 0; i < count - 1; i++) 
				{
					directories[i] = addInDirs[i+1] as string;//������Ԫ��ǰ��.
				}
				return directories;
			}
			ignoreDefaultAddInPath = false;
			return null;
		}
		
		
		/// <summary>
		/// Initialize all addIn object from addIn file collection, and insert them into the addIn tree.
		/// </summary>
		static void InsertAddIns(StringCollection addInFiles)
		{
			foreach (string addInFile in addInFiles) 
			{
				AddIn addIn = new AddIn();//���½�һ�����ʵ��
				try 
				{
					addIn.Initialize(addInFile);//ͨ����ǰ����ļ�����ʼ��������ʵ��
					addInTree.InsertAddIn(addIn);//�������ʼ���õĲ�����뵽�������
				}
				catch (Exception e) 
				{
					throw new Exception("��ʼ������ļ� " + addInFile + " ʱ���� : \n" + e.Message, e);
				}
			}
			
		}

		
		/// <summary>
		/// Create a addIn tree
		/// </summary>
		public static void CreateAddInTree()
		{
			if(addInTree == null)
			{
				addInTree = new DefaultAddInTree();
			
				InternalFileService fileUtilityService = new InternalFileService();

				StringCollection addInFiles = null;
			
				if (ignoreDefaultCoreAddInDirectory == false) //���û�к���Ĭ�ϵĲ��·��,������Ĭ�ϵĲ��·��
				{
					addInFiles = fileUtilityService.SearchDirectory(defaultCoreAddInDirectory, "*.addin");
					InsertAddIns(addInFiles);
				}
				else  //�������Ĭ�ϵĲ���ļ���·��
				{
					if (addInDirectories != null) 
					{
						foreach(string path in addInDirectories) 
						{
							addInFiles = fileUtilityService.SearchDirectory(Application.StartupPath + Path.DirectorySeparatorChar + path, "*.addin");
							InsertAddIns(addInFiles);
						}
					}
				}
			}

		}
		
		
		#region IConfigurationSectionHandler ��Ա
		
		//�˺����ڱ�Ӧ�ó����ж�ȡ�����ļ��е�<AddInDirectories>Ԫ��
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			ArrayList addInDirectories = new ArrayList();
			XmlNode attr = section.Attributes.GetNamedItem("ignoreDefaultPath");

			if (attr != null) 
			{
				try 
				{
					addInDirectories.Add(Convert.ToBoolean(attr.Value));
				} 
				catch (InvalidCastException) 
				{
					addInDirectories.Add(false);//����ļ���ȡ�쳣,��Ĭ������Ϊ��,��������Ĭ�ϵĲ���ļ�·��
				}
			} 
			else 
			{
				addInDirectories.Add(false);//��������Բ�����ͬ��Ĭ������Ϊ��,��������Ĭ�ϵĲ���ļ�·��
			}
               
			XmlNodeList addInDirList = section.SelectNodes("AddInDirectory");//��ȡ�ӽڵ�

			foreach (XmlNode addInDir in addInDirList) 

			{
				XmlNode path = addInDir.Attributes.GetNamedItem("path");
				if (path != null) 
				{
					addInDirectories.Add(path.Value);//�����е��Զ������ļ���·���ַ����ӵ�addInDirectories��
				}
			}
			return addInDirectories;//��󷵻��Զ������ļ���·���ַ����Լ��Ƿ�Ҫ����Ĭ�ϲ���ļ�·����һ������ֵ.

		}

		
		#endregion

	}
}
