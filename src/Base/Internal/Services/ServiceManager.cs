using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Resources;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

using NetFocus.Components.AddIns;


namespace NetFocus.DataStructure.Services
{
	/// <summary>
	/// ����һ���������з������,������singleton���ģʽ�ķ�ʽ������ṩ��Ҫ��ķ���.
	/// </summary>
	public class ServiceManager
	{
		static ArrayList serviceList       = new ArrayList();//��ŷ����һ���б�
		Hashtable servicesHashtable = new Hashtable();//���ڴ��һЩ�Ѿ������ʹ��ķ�������ʹ��Hashtable����Ϊ���ٶȸ���
		
		static ServiceManager defaultServiceManager = new ServiceManager();
		static bool isInitialized = false;

		/// <summary>
		/// �õ�ServiceManager����.
		/// </summary>
		public static ServiceManager Services {
			get {
                if (!isInitialized)
                {
                    isInitialized = true;
                    InitializeServicesSubsystem("/Workspace/Services");
                }
				return defaultServiceManager;
			}
		}		
		
		/// <summary>
		/// ����һ��˽�еĹ��캯��,��ʹ�ø��಻�ܱ���������ʵ����,�������Ա�֤Ӧ�ó���ֻ��һ��ServiceManager����,������Singleton���ģʽ�Ĺؼ�.
		/// </summary>
		private ServiceManager()
		{
			//����������ķ���.
			AddService(new PropertyService());
			AddService(new StringParserService());
			AddService(new ResourceService());
		}
		
		/// <remarks>
		/// ��ʼ��������ϵͳ,�ɲ���ļ��ж���Ĵ�������ȷ��Ҫ��ʼ����Щ����.
		/// </remarks>
		private static void InitializeServicesSubsystem(string servicesPath)
		{
			IAddInTreeNode treeNode = AddInTreeSingleton.AddInTree.GetTreeNode(servicesPath);
            ArrayList childItems = treeNode.BuildChildItems(defaultServiceManager);
            foreach (IService service in (IService[])childItems.ToArray(typeof(IService)))
            {
                serviceList.Add(service);
            }
			// ����ͨ����������ʼ�����еķ���.
			foreach (IService service in serviceList) 
			{
				service.InitializeService();
			}
		}
		
		/// <remarks>
		/// Calls UnloadService on all services. This method must be called ONCE.
		/// </remarks>
		public void UnloadAllServices()
		{
			foreach (IService service in serviceList) {
				service.UnloadService();
			}
		}
		
		
		public void AddService(IService service)
		{
			serviceList.Add(service);
		}
		
		public void AddServices(IService[] services)
		{
			foreach (IService service in services) {
				AddService(service);
			}
		}
		

		bool IsInstanceOfType(Type type, IService service)
		{
			Type serviceType = service.GetType();

			Type[] interfaces = serviceType.GetInterfaces();

			foreach (Type iface in interfaces) 
			{
				if (iface == type) 
				{
					return true;
				}
			}
			
			while (serviceType != typeof(System.Object)) 
			{
				if (type == serviceType) 
				{
					return true;
				}
				serviceType = serviceType.BaseType;
			}
			return false;
		}
		
		/// <remarks>
		/// ���ݷ��������,�ṩ�����һ���ض��ķ���.
		/// </remarks>
		public IService GetService(Type serviceType)
		{
			IService s = (IService)servicesHashtable[serviceType];
			if (s != null) //�����÷����Ѿ������ʹ�
			{
				return s;
			}
			
			foreach (IService service in serviceList) 
			{
				if (IsInstanceOfType(serviceType, service)) 
				{
					servicesHashtable[serviceType] = service;//����,ֻҪ�Ǳ���һ�α�����ķ���,���ᱻ����Hashtable��,
					return service;                          //���ڶ����������������ʱ,�Ϳ���ֱ�Ӵ����Hashtable��ȡ��,
					                                         //Ҳ������һ��Hashtable������������.
				}
			}
			
			return null;
		}
	}
}
