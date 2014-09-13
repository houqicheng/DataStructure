using System;
using System.Drawing;


namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	public enum GlyphAppearance
	{
		Flat = 0,
		Popup = 1,
		Solid = 2
	}
	
	
	public interface IGlyph
	{
		/// <summary>
		/// һ�����������Լ��ķ���
		/// </summary>
		/// <param name="g"></param>
		void Draw(Graphics g);
		/// <summary>
		/// ��ǰͼԪ����ı߿�
		/// </summary>
		System.Drawing.Rectangle Bounds
		{
			get;
			set;
		}

		/// <summary>
		/// ��ǰͼԪ����ı���ɫ
		/// </summary>
		Color BackColor
		{
			get;
			set;
		}

		/// <summary>
		/// ��ǰͼԪ��������,��Flat,Popup,Solid���ַ�ʽ
		/// </summary>
		GlyphAppearance Appearance
		{
			get;
			set;
		}

		/// <summary>
		/// ��ʶĳ�����Ƿ��뵱ǰͼԪ�ཻ
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		bool Intersects(Point p);
		/// <summary>
		/// Ϊ��ǰͼԪ�������ͼ�ζ��󴴽�һ��������
		/// </summary>
		/// <returns></returns>
		IIterator CreateIterator();

	}
}
