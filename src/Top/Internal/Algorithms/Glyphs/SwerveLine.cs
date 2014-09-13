using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	public class SwerveLine : IGlyph
	{
		/// <summary>
		/// ��ʾһ���ɶ����߶���϶��ɵ�����(�ڴ�������ɾ������ʱ���õ�)
		/// </summary>
		Rectangle rectangle;
		Point[] points;
		int width = 2;
		Color color = Color.HotPink;
		GlyphAppearance appearance = GlyphAppearance.Flat;

		public SwerveLine(Rectangle rectangle,Point[] points) : this(rectangle,points,2)
		{

		}
		public SwerveLine(Rectangle rectangle,Point[] points,int width) : this(rectangle,points,width,Color.HotPink)
		{

		}
		public SwerveLine(Rectangle rectangle,Point[] points,int width,Color color)
		{
			this.rectangle = rectangle;
			this.points = points;
			this.width = width;
			this.color = color;
		}

		
		#region IGlyph ��Ա

		public void Draw(Graphics g)
		{
			Pen pen   = new Pen(color,width);
			g.DrawLines(pen,points);
		}

		public Rectangle Bounds
		{
			get
			{
				return rectangle;
			}
			set
			{
				rectangle = new Rectangle(value.X,value.Y,value.Width,value.Height);
			}
		}

		public Color BackColor
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public GlyphAppearance Appearance
		{
			get
			{
				return appearance;
			}
			set
			{
				appearance = value;
			}
		}

		public bool Intersects(Point p)
		{
			return false;
		}

		public IIterator CreateIterator()
		{
			return new NullIterator(this);
		}

		
		#endregion
	}
}
