using System;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// һ��������������һ��ͳһ�ķ�ʽ��˳���ȡһ�����Ӷ��󼯺�,���������������ڲ��ṹ
	/// </summary>
	public interface IIterator
	{
		/// <summary>
		/// ����������ָ��ָ���һ��Ԫ��
		/// </summary>
		/// <returns></returns>
		IIterator First();
		/// <summary>
		/// ����������ָ��ָ����һ��Ԫ��
		/// </summary>
		/// <returns></returns>
		IIterator Next();
		/// <summary>
		/// ��ʶ�Ƿ����������Ԫ�صĵ�������
		/// </summary>
		/// <returns></returns>
		bool IsDone();
		/// <summary>
		/// ��ǰ������ָ����ָ���ͼ�ζ���
		/// </summary>
		IGlyph CurrentItem
		{
			get;
		}
		
	}
}
