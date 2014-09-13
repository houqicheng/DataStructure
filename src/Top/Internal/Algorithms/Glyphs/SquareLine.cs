using System;
using System.Collections;
using System.Drawing;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// һ�����ͼ԰,�����кܶ��Square����
	/// </summary>
	public class SquareLine : IGlyph
	{
		Color backColor = Color.Transparent;
		int x,y,width;
		GlyphAppearance appearance = GlyphAppearance.Flat;
		ArrayList squareArray;

		
		public SquareLine(int x,int y,int width,ArrayList squareArray) : this(x,y,width,Color.HotPink,squareArray)
		{
		}

		public SquareLine(int x,int y,int width,Color backColor,ArrayList squareArray) : this(x,y,width,backColor,GlyphAppearance.Flat,squareArray)
		{
		}
		public SquareLine(int x,int y,int width,Color backColor,GlyphAppearance appearance,ArrayList squareArray)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.backColor = backColor;
			this.appearance = appearance;
			this.squareArray = squareArray;
		}

		
		#region IGlyph ��Ա

		public void Draw(System.Drawing.Graphics g)
		{
			// TODO:  ��� SquareLine.Draw ʵ��
			
		}

		
		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(this.x,this.y,this.width,100);
			}
			set
			{
				this.x = value.X;
				this.y = value.Y;
				this.width = value.Width;
			}
		}


		public Color BackColor
		{
			get
			{
				return backColor;
			}
			set
			{
				backColor = value;
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


		public bool Intersects(System.Drawing.Point p)
		{
			// TODO:  ��� SquareLine.Intersects ʵ��
			return false;
		}

		
		public IIterator CreateIterator()
		{
			return new ArrayIterator(squareArray);
		}

		
		#endregion

	}
}
