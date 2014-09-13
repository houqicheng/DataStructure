
using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using NetFocus.DataStructure.Properties;


namespace NetFocus.DataStructure.Services
{ 
	/// <summary>
	/// ��������ദ��ȫ�ֵ�������Ϣ.
	/// </summary>
	public class PropertyService : DefaultProperties, IService
	{
		
		readonly static string propertyFileName    = "DataStructureProperties.xml";
		readonly static string propertyFileVersion = "1.1";
		readonly static string propertyXmlRootNodeName  = "DataStructureProperties";
		readonly static string defaultPropertyDirectory = Application.StartupPath + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar + "options" + Path.DirectorySeparatorChar;
		readonly static string defaultDataDirectory = Application.StartupPath + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar;
		/// <summary>
		/// �������������ļ���·��.
		/// </summary>
		public string ConfigDirectory {
			get {
				return defaultPropertyDirectory;
			}
		}
		/// <summary>
		/// ���������ļ��е�·��.
		/// </summary>
		public string DataDirectory 
		{
			get 
			{
				return defaultDataDirectory;
			}
		}
		
		
		public PropertyService()
		{
			try {
				LoadProperties();
			}
			catch {
				MessageBox.Show("���ܼ��������ļ�!", "����",MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		
		
		void WritePropertiesToFile(string fileName)
		{
			//����һ���µ�Xml�ĵ�
			XmlDocument doc = new XmlDocument();
			//Ϊ���ĵ�����ļ�ͷXmlָ����ļ����ڵ���Ϣ
			doc.LoadXml("<?xml version=\"1.0\"?>\n<" + propertyXmlRootNodeName + " fileversion = \"" + propertyFileVersion + "\" />");
			
			doc.DocumentElement.AppendChild(ToXmlElement(doc));
			try
			{
				doc.Save(fileName);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}
		
		bool LoadPropertiesFromFile(string filename)
		{
			try {
				XmlDocument doc = new XmlDocument();
				doc.Load(filename);
				
				if (doc.DocumentElement.Attributes["fileversion"].InnerText != propertyFileVersion) {
					return false;//����ļ��汾��ƥ��,�򷵻�false;
				}
				SetValueFromXmlElement(doc.DocumentElement["Properties"]);
			} catch {
				return false;
			}
			return true;
		}
		

		void LoadProperties()
		{
			if (!LoadPropertiesFromFile(defaultPropertyDirectory + propertyFileName)) 
			{
				throw new Exception("���ܼ���ȫ�������ļ�!");//���ش������׳��쳣.
			}
		}
		
		void SaveProperties()
		{
			WritePropertiesToFile(defaultPropertyDirectory + propertyFileName);
		}
		
		
		#region ʵ��IService�ӿ�

		public virtual void InitializeService()
		{
			OnInitialize(EventArgs.Empty);
		}
		
		public virtual void UnloadService()
		{
			// ����ǰ�����˳�ʱ,�������е�����.
			SaveProperties();
			OnUnload(EventArgs.Empty);//��󴥷��¼�,�Ա���Ӧ�Ĵ������ᱻ���õ�.
		}
		
		protected virtual void OnInitialize(EventArgs e)
		{
			if (Initialize != null) {
				Initialize(this, e);
			}
		}
		
		protected virtual void OnUnload(EventArgs e)
		{
			if (Unload != null) {
				Unload(this, e);
			}
		}
		
		public event EventHandler Initialize;
		public event EventHandler Unload;	
		
		#endregion
	}
}
