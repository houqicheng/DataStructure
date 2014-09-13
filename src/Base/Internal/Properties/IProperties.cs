
using System;

namespace NetFocus.DataStructure.Properties
{
	/// <summary>
	/// ����һ���������Ա���ʵ�ֵĽӿ�.
	/// </summary>
	public interface IProperties : IXmlConvertable
	{

		//����˸����صĻ�ȡ����ֵ�ķ���(����Ĭ��ֵ�����͵Ĳ�ͬ,���ص�ֵ������Ҳ��ͬ).
		object GetProperty(string key, object defaultvalue);

		object GetProperty(string key);

		int GetProperty(string key, int defaultvalue);

		bool GetProperty(string key, bool defaultvalue);

		short GetProperty(string key, short defaultvalue);

		byte GetProperty(string key, byte defaultvalue);

		string GetProperty(string key, string defaultvalue);

		System.Enum GetProperty(string key, System.Enum defaultvalue);

		//�������Ե�ֵ
		void SetProperty(string key, object val);

		IProperties Clone();//���Ƶ�ǰ�����Զ���.
		
		event PropertyEventHandler PropertyChanged;//����һ���¼�,��ʾ������ֵ�ı�ʱ�������¼�.
	}
}
