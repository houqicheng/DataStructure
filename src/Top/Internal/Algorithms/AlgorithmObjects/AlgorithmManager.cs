using System;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Globalization;
using System.Configuration;
using System.Xml;

using NetFocus.Components.AddIns;
using NetFocus.DataStructure.Properties;
using NetFocus.DataStructure.Services;
using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.Gui.Pads;


namespace NetFocus.DataStructure.Internal.Algorithm
{
	public delegate void PictureBoxPaintEventHandler(Graphics g);
	
	public class AlgorithmManager
	{
		static AlgorithmManager algorithmManager = new AlgorithmManager();
		string algorithmExampleDataFile = Application.StartupPath + Path.DirectorySeparatorChar + ConfigurationSettings.AppSettings["AlgorithmExampleDataFile"];
		string algorithmFilesPath = Application.StartupPath + Path.DirectorySeparatorChar + ConfigurationSettings.AppSettings["AlgorithmFilesPath"];
		System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
		int interval = 1000;
		bool isByStep = false;
		bool isRunto  = false;
		IAlgorithm currentAlgorithm = null;  
		Hashtable exampleDataHashTable = new Hashtable(); //���ڱ��������㷨�������������
		Hashtable algorithmsHashTable = new Hashtable(); //���ڱ����Ѿ��򿪹����㷨�����һ��Hashtable
		Hashtable openingAlgorithms = new Hashtable();
		ArrayList algorithmList = new ArrayList(); //���ڱ��������㷨�����һ��Hashtable
		EventHandler updateAlgorithmStatusEventHandler = null;
		public EventHandler ClearPadsHandler = null;
		PropertyService propertyService = (PropertyService)ServiceManager.Services.GetService(typeof(PropertyService));
		Hashtable LoadExampleDatasFromStream(string filename)
		{
			XmlDocument doc = new XmlDocument();
			try 
			{
				doc.Load(filename);
				
				XmlNodeList nodes  = doc.DocumentElement.ChildNodes;
				foreach (XmlElement el in nodes)
				{
					exampleDataHashTable[el.Attributes["name"].Value] = el.SelectSingleNode("ExampleData");
				}
			} 
			catch
			{
				return null;
			}
			return exampleDataHashTable;
		}
		
		private AlgorithmManager()
		{
			ClearPadsHandler = new EventHandler(ClearAllPads);
			timer.Interval = interval = Convert.ToInt32(propertyService.GetProperty("NetFocus.DataStructure.AlgorithmExecuteSpeed", "1000"));
            InitializeAlgorithms("/DataStructure/Internal/Algorithm");
		}

		
		/// <summary>
		/// �㷨ִ�е��ٶ�
		/// </summary>
		public int ExecuteSpeed
		{
			get
			{
				return interval;
			}
			set
			{
				interval = value;
			}
		}
		
		/// <summary>
		/// Ӧ�ó���洢�㷨��ʼ�����ݵ��ļ�
		/// </summary>
		public string AlgorithmExampleDataFile
		{
			get
			{
				return algorithmExampleDataFile;
			}
		}
		
		/// <summary>
		/// ����㷨Դ�ļ���·��
		/// </summary>
		public string AlgorithmFilesPath
		{
			get
			{
				return this.algorithmFilesPath;
			}
		}
		
		/// <summary>
		/// �õ������㷨����������,���ŵ���ϣ����
		/// </summary>
		/// <returns></returns>
		public Hashtable GetExampleDatas()
		{
			return LoadExampleDatasFromStream(algorithmExampleDataFile);
		}

		/// <summary>
		/// �ṩ�����㷨��������Ψһ���ʵ�
		/// </summary>
		public static AlgorithmManager Algorithms{
			get{
				return algorithmManager;
			}
		
		}
		
	    /// <summary>
	    /// ���µ�ǰ�㷨״̬���¼��������
	    /// </summary>
		public EventHandler UpdateAlgorithmStatusEventHandler
		{
			get
			{
				return updateAlgorithmStatusEventHandler;
			}
			set
			{
				updateAlgorithmStatusEventHandler = value;
			}
		}
		/// <summary>
		/// ά����ǰ�Ѿ��򿪵��㷨����
		/// </summary>
		public Hashtable OpeningAlgorithms
		{
			get
			{
				return openingAlgorithms;
			}
		}
		/// <summary>
		/// һ����ʱ��,���ڿ����㷨��ʾʱ����ִ�м��
		/// </summary>
		public System.Windows.Forms.Timer Timer
		{
			get 
			{
				 return timer;
			}
		}
		/// <summary>
		/// ������ǰ�Ƿ�Ϊ��������
		/// </summary>
		public bool IsByStep
		{
			get
			{
				return isByStep;
			}
			set
			{
				isByStep = value;
			}
		}
		/// <summary>
		/// ������ǰ�Ƿ�Ϊ���е�����
		/// </summary>
		public bool IsRunto
		{
			get
			{
				return isRunto;
			}
			set
			{
				isRunto = value;
			}
		}
		/// <summary>
		/// ���ڱ��浱ǰ�㷨
		/// </summary>
		public IAlgorithm CurrentAlgorithm
		{
			get
			{
				return currentAlgorithm;	
			}
			set
			{
				currentAlgorithm = value;
			}
		}

		/// <summary>
		/// ͨ��һ��ָ����·����ʼ�����е��㷨����,�����浽һ��ArrayList��
		/// </summary>
		/// <param name="algorithmPath">һ���Ӳ���ļ��ж�ȡ���߼�·��(�������ӵ�·��)</param>
		public void InitializeAlgorithms(string algorithmPath) 
		{
			try
			{
				IAddInTreeNode treeNode = AddInTreeSingleton.AddInTree.GetTreeNode(algorithmPath);
				ArrayList childItems = treeNode.BuildChildItems(this);
				AddAlgorithms((IAlgorithm[])childItems.ToArray(typeof(IAlgorithm)));
			}
			catch
			{
			}
	
		}
		
		bool IsInstanceOfType(Type type, IAlgorithm algorithm) {
			Type algorithmType = algorithm.GetType();

			Type[] interfaces = algorithmType.GetInterfaces();

			foreach (Type iface in interfaces) {
				if (iface == type) {
					return true;
				}
			}
			
			while (algorithmType != typeof(System.Object)) {
				if (type == algorithmType) {
					return true;
				}
				algorithmType = algorithmType.BaseType;
			}
			return false;
		}

		/// <summary>
		/// �˺�������Ҫ������һ���㷨�����͵õ�һ���㷨,������һ����ϣ����Ϊ���档
		/// </summary>
		/// <param name="algorithmType"></param>
		/// <returns></returns>
		public IAlgorithm GetAlgorithm(Type algorithmType) 
		{	 
			IAlgorithm a = (IAlgorithm)algorithmsHashTable[algorithmType];
			if(a != null) {
				return a;	 
			}
			foreach(IAlgorithm algorithm in algorithmList) {
				if (IsInstanceOfType(algorithmType, algorithm)) {
					algorithmsHashTable[algorithmType]=algorithm;
					return algorithm;	 
				}
			}
			return null;

		}

		void AddAlgorithm(IAlgorithm algorithm) 
		{
			 algorithmList.Add(algorithm);
		}

		void AddAlgorithms(IAlgorithm[] algorithms) 
		{
			foreach(IAlgorithm algorithm in algorithms) {
				 AddAlgorithm(algorithm);
			}
		}
		
		
		Color ParseColor(string c)
		{
			int a = 255;
			int offset = 0;
			Color color;
			try
			{
				offset = 2;
				a = Int32.Parse(c.Substring(1,2), NumberStyles.HexNumber);
				int r = Int32.Parse(c.Substring(1 + offset,2), NumberStyles.HexNumber);
				int g = Int32.Parse(c.Substring(3 + offset,2), NumberStyles.HexNumber);
				int b = Int32.Parse(c.Substring(5 + offset,2), NumberStyles.HexNumber);

				color = Color.FromArgb(a, r, g, b);
			}
			catch
			{
				color = Color.FromName(c);
			}
			return color;
		}
		/// <summary>
		/// ������������
		/// </summary>
		public Graphics ClearAnimationPad()
		{
			IPadContent animationPad = WorkbenchSingleton.Workbench.GetPad(typeof(NetFocus.DataStructure.Gui.Pads.AnimationPad));
			if(animationPad == null)
			{
				Bitmap bmp1 = new Bitmap(1 ,1);
				Graphics g1 = Graphics.FromImage(bmp1);
				return g1;
			}
			int x = animationPad.Control.Left > 0 ? animationPad.Control.Left : 0;
			int y = animationPad.Control.Top > 0 ? animationPad.Control.Top : 0;
			int width = animationPad.Control.Width > 0 ? animationPad.Control.Width : 1;
			int height = animationPad.Control.Height > 0 ? animationPad.Control.Height : 1;

			Bitmap bmp = new Bitmap(width,height);
			Graphics g = Graphics.FromImage(bmp);
			g.DrawRectangle(new Pen(Color.Gray,1),x,y,width - 1,height - 1);
			((PictureBox)animationPad.Control).BackgroundImage = bmp;
			
			PropertyService propertyService = (PropertyService)ServiceManager.Services.GetService(typeof(PropertyService));
			Color c = ParseColor(propertyService.GetProperty("NetFocus.DataStructure.AnimationPadPanel.BackColor","White"));
			animationPad.Control.BackColor = c;
			
			return g;  //����,���Ҫ��Graphics���󷵻�,��Ϊһ���ض����㷨����Ҫ�����������һЩ�ض��Ķ�����Ϣ

		}
		/// <summary>
		/// ����ջ������
		/// </summary>
		public Graphics ClearStackPad()
		{
			IPadContent stackPad = WorkbenchSingleton.Workbench.GetPad(typeof(NetFocus.DataStructure.Gui.Pads.StackPad));
			if(stackPad == null)
			{
				Bitmap bmp1 = new Bitmap(1 ,1);
				Graphics g1 = Graphics.FromImage(bmp1);
				return g1;
			}
			int x = stackPad.Control.Left > 0 ? stackPad.Control.Left : 0;
			int y = stackPad.Control.Top > 0 ? stackPad.Control.Top : 0;
			int width = stackPad.Control.Width > 0 ? stackPad.Control.Width : 1;
			int height = stackPad.Control.Height > 0 ? stackPad.Control.Height : 1;

			Bitmap bmp = new Bitmap(width,height);
			Graphics g = Graphics.FromImage(bmp);
			g.DrawRectangle(new Pen(Color.Gray,1),x,y,width - 1,height - 1);
            ((PictureBox)stackPad.Control).BackgroundImage = bmp;
			
			return g;  //����,���Ҫ��Graphics���󷵻�,��Ϊһ���ض����㷨����Ҫ�����������һЩ�ض��Ķ�����Ϣ

		}

		/// <summary>
		/// ����������
		/// </summary>
		public void ClearPropertyPad()
		{
			IPadContent propertyPad = WorkbenchSingleton.Workbench.GetPad(typeof(PropertyPad));
			if(propertyPad != null) 
			{
				((PropertyGrid)propertyPad.Control).SelectedObject = null;
			}
		}
		
		/// <summary>
		/// ͬʱ��ն��������������,������ǰ�㷨����Ϊ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ClearAllPads(object sender,EventArgs e)
		{
			AlgorithmManager.Algorithms.CurrentAlgorithm = null;
			AlgorithmManager.Algorithms.Timer.Stop();
			AlgorithmManager.Algorithms.Timer.Tick -= AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
			ClearAnimationPad();
			ClearPropertyPad();
			ClearStackPad();
		}
		
		/// <summary>
		/// �ָ������Ѿ��򿪵��㷨����ʼ��״̬
		/// </summary>
		public void RecoverAllOpeningAlgorithms()
		{
			IAlgorithm tempAlgorithm = currentAlgorithm;

			foreach(DictionaryEntry entry in algorithmManager.openingAlgorithms)
			{
				currentAlgorithm = entry.Value as IAlgorithm;
				currentAlgorithm.Recover();
			}
			currentAlgorithm = tempAlgorithm;
			if(currentAlgorithm != null)
			{
				currentAlgorithm.Recover();
			}
			else
			{
				ClearAllPads(null,null);
			}
		}
		

	}
}
