

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

using NetFocus.DataStructure.Properties;

namespace NetFocus.DataStructure.Services
{
	/// <summary>
	/// �����Դ�����ദ������Ӧ�ó�������Ҫ����Դ,�����ַ�����Դ��ͼ����Դ..
	/// </summary>
	public class ResourceService : AbstractService
	{

		readonly static string stringResources  = "StringResources";
		readonly static string imageResources   = "BitmapResources";
		
		ResourceManager strings = null;
		ResourceManager icons    = null;
		
		public ResourceService()
		{
			strings = new ResourceManager(stringResources, Assembly.GetEntryAssembly());
			icons    = new ResourceManager(imageResources, Assembly.GetEntryAssembly());
		}

		/// <summary>
		///�ṩ����ļ������ط���.
		///<summary>
		public Font LoadFont(string fontName, int size)
		{
			return LoadFont(fontName, size, FontStyle.Regular);
		}
		
		public Font LoadFont(string fontName, int size, FontStyle style)
		{
			try {
				return new Font(fontName, size, style);
			} catch (Exception) {
				return SystemInformation.MenuFont;
			}
		}
		
		public Font LoadFont(string fontName, int size, GraphicsUnit unit)
		{
			return LoadFont(fontName, size, FontStyle.Regular, unit);
		}
		
		public Font LoadFont(string fontName, int size, FontStyle style, GraphicsUnit unit)
		{
			try {
				return new Font(fontName, size, style, unit);
			} catch (Exception) {
				return SystemInformation.MenuFont;
			}
		}
		
		
		/// <summary>
		///��ȡһ���ַ�������.
		/// </summary>
		public string GetString(string name)
		{
			string s = strings.GetString(name);
			
			if (s == null) {
				throw new Exception("��Դδ�ҵ� :<" + name + ">");
			}
			
			return s;
		}
		
		
		/// <summary>
		///��ȡһ��ͼ�����
		/// </summary>
		public Icon GetIcon(string name)
		{
			object iconobj = icons.GetObject(name);
			
			if (iconobj == null) {
				return null;
			}
			
			if (iconobj is Icon) {
				return (Icon)iconobj;
			} else {
				return Icon.FromHandle(((Bitmap)iconobj).GetHicon());//ע�������һ��λͼת������һ��ͼ��
			}
		}
		
		
		/// <summary>
		///��ȡһ��λͼ����.
		/// </summary>
		public Bitmap GetBitmap(string name)
		{
			object bitmapObj = icons.GetObject(name);
			if (bitmapObj == null){
				return null;
			}
			return (Bitmap)bitmapObj;
		}
	}
}
