

using System.Xml;

namespace NetFocus.DataStructure.Properties
{
	/// <summary>
	/// ������붨�帴�ӵ�����,�����ʵ�ָýӿ�,�Ա㽫���Ե���Ϣ����ΪXML����ʽ.
	/// </summary>
	public interface IXmlConvertable
	{
		/// <summary>
		/// ��һ��XML Element ת����һ������(ע��:�˶���Ҳ�ǿ��Ա�����ΪXML Element��)
		/// </returns>
		object FromXmlElement(XmlElement element);
		
		/// <summary>
		/// �������һ�����󱣴�Ϊһ��XML ElementԪ��.
		/// </returns>
		XmlElement ToXmlElement(XmlDocument doc);
	}
}
