
using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Windows.Forms;
using System.Reflection;


namespace NetFocus.DataStructure.Properties
{
	/// <summary>
	/// �����Խӿڵ�һ��Ĭ��ʵ��.
	/// </summary>
	public class DefaultProperties : IProperties
	{
		#region ʵ��IProperties�ӿڵĴ���
		
		Hashtable properties = new Hashtable();//������Ե�һ����ϣ��.
		
		public object GetProperty(string key, object defaultvalue)
		{
			if (!properties.ContainsKey(key)) {//����ǵ�һ����������,���������hash table��.
				if (defaultvalue != null) {
					properties[key] = defaultvalue;
				}
				return defaultvalue;
			}
			
			object obj = properties[key];

			if (defaultvalue is IXmlConvertable && obj is XmlElement) {
				obj = properties[key] = ((IXmlConvertable)defaultvalue).FromXmlElement((XmlElement)((XmlElement)obj).FirstChild);
			}
			return obj;
		}
		

		public object GetProperty(string key)
		{
			return GetProperty(key, (object)null);
		}
		
		
		public int GetProperty(string key, int defaultvalue)
		{
			return int.Parse(GetProperty(key, (object)defaultvalue).ToString());
		}

		
		public bool GetProperty(string key, bool defaultvalue)
		{
			return bool.Parse(GetProperty(key, (object)defaultvalue).ToString());
		}

		
		public short GetProperty(string key, short defaultvalue)
		{
			return short.Parse(GetProperty(key, (object)defaultvalue).ToString());
		}

		
		public byte GetProperty(string key, byte defaultvalue)
		{
			return byte.Parse(GetProperty(key, (object)defaultvalue).ToString());
		}
		
		
		public string GetProperty(string key, string defaultvalue)
		{
			return GetProperty(key, (object)defaultvalue).ToString();
		}

		
		public System.Enum GetProperty(string key, System.Enum defaultvalue)
		{
			try {
				return (System.Enum)Enum.Parse(defaultvalue.GetType(), GetProperty(key, (object)defaultvalue).ToString());
			} catch (Exception) {
				return defaultvalue;
			}
		}

		
		public void SetProperty(string key, object val)
		{
			object oldValue = properties[key];
			properties[key] = val;
			OnPropertyChanged(new PropertyEventArgs(this, key, oldValue, val));
		}

		
		protected virtual void OnPropertyChanged(PropertyEventArgs e)
		{
			if (PropertyChanged != null) 
			{
				PropertyChanged(this, e);
			}
		}
		
		
		public event PropertyEventHandler PropertyChanged;

		public IProperties Clone()  //ǳ����
		{
			DefaultProperties df = new DefaultProperties();
			df.properties = (Hashtable)properties.Clone();//ע��:������ǳ����.
			return df;
		}
		
		
		#endregion
		
		#region ����������ʵ��IXmlConvertable�ӿ�

		/// <summary>
		/// ��һ��XmlElementԪ�ؽڵ㴴��һ��DefaultProperties����
		/// </summary>
		public virtual object FromXmlElement(XmlElement element)
		{
			DefaultProperties defaultProperties = new DefaultProperties();
			defaultProperties.SetValueFromXmlElement(element);
			return defaultProperties;
		}
		
		/// <summary>
		///����һ��Properties�ڵ�,�ѵ�ǰ�ڴ��е���������ֵ��ӵ�Properties�ڵ���,����Properties�ڵ㷵��
		/// </summary>
		public virtual XmlElement ToXmlElement(XmlDocument doc)
		{
			XmlElement propertiesnode  = doc.CreateElement("Properties");
			
			foreach (DictionaryEntry entry in properties) {//ע��Hashtable�м�ֵ�Եı�ʾ����,��DictionaryEntry
				if (entry.Value != null) {
					if (entry.Value is XmlElement) { // write unchanged XmlElement back
						//����doc.ImportNode�����������ǽ�һ��XmlElement���뵽һ��Xml�ĵ���,���Ѹ�XmlElementԪ�ط���
						//ע��:������ΪXmlElement�Ǽ̳���XmlNode���,���Բ���������,
						//��ʵImportNode����Ҫ��Ĳ�������ΪXmlNode����,���Ҫע��
						XmlNode unChangedNode = doc.ImportNode((XmlElement)entry.Value, true);
						propertiesnode.AppendChild(unChangedNode);
					} else if (entry.Value is IXmlConvertable) { // An Xml convertable object
						XmlElement convertableNode = doc.CreateElement("XmlConvertableProperty");//����һ��Ԫ��
						
						XmlAttribute key = doc.CreateAttribute("key");//����һ������
						key.InnerText = entry.Key.ToString();
						convertableNode.Attributes.Append(key);//��������������ӵ���Ԫ����
						//�ݹ����entry.ValueԪ��,������Ϊ����,���Ըú�������Ҫһ��XmlDocument����,Ϊ��ȷ��XmlDocument����ͬһ��
						convertableNode.AppendChild(((IXmlConvertable)entry.Value).ToXmlElement(doc));
						//�����Ӹ�convertableNodeԪ��
						propertiesnode.AppendChild(convertableNode);
					} else {//����һ��һ���<Property>Ԫ��,ֻҪΪ�䴴���������Լ���
						XmlElement el = doc.CreateElement("Property");
						
						XmlAttribute key   = doc.CreateAttribute("key");
						key.InnerText      = entry.Key.ToString();
						el.Attributes.Append(key);
	
						XmlAttribute val   = doc.CreateAttribute("value");
						val.InnerText      = entry.Value.ToString();
						el.Attributes.Append(val);
						
						propertiesnode.AppendChild(el);
					}
				}
			}
			return propertiesnode;  //��󷵻�һ���ܵ�Xml�ڵ�
		}
		

		#endregion
		

		public DefaultProperties()
		{
		}
		
		
		/// <summary>
		/// ��ȫ�������ļ������е�����Ԫ�ص�ֵװ�ص��ڴ��е�һ��Hashtable��,���ڳ�ʼ��Ӧ�ó��������ȫ������
		/// </summary>
		protected void SetValueFromXmlElement(XmlElement element)
		{
			XmlNodeList nodes = element.ChildNodes;
			foreach (XmlElement el in nodes) 
			{
				if (el.Name == "Property") 
				{  //�����һ����ͨ�����Խڵ�
					properties[el.Attributes["key"].InnerText] = el.Attributes["value"].InnerText;//������ڵ��InnerTextֵ��ΪHashtable�е�ǰ���ֵ
				} 
				else if (el.Name == "XmlConvertableProperty") 
				{  //�����һ�������ӽڵ�����Խڵ�
					properties[el.Attributes["key"].InnerText] = el;//��������Խڵ���Ϊֵ.
				} 
				else 
				{
					throw new Exception("����ʶ���Xml�ڵ� : " + el.Name);//������Բ���������������ʽ,���׳��쳣.
				}
			}
		}
		

	}
}
