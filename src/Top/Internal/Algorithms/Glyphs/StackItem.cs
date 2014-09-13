using System;
using System.Drawing;

namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// һ����ջ�е�һ����Ŀ����Ӧ��ͼԪ(���ڱ������������㷨)
	/// </summary>
	public class StackItem : IGlyph
	{
		int x,y,width,height;
		GlyphAppearance appearance;
		string text;
		Color backColor;

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}


		public StackItem(int x,int y,int width,int height,string text) : this(x,y,width,height,Color.HotPink,text)
		{

		}
		public StackItem(int x,int y,int width,int height,Color backColor,string text) : this(x,y,width,height,Color.HotPink,GlyphAppearance.Popup,text)
		{

		}
		public StackItem(int x,int y,int width,int height,Color backColor,GlyphAppearance appearance,string text)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.backColor = backColor;
			this.appearance = appearance;
			this.text = text;
		}
		
		
		#region IGlyph ��Ա

		public void Draw(Graphics g)
		{
			g.FillRectangle(new SolidBrush(backColor),this.x,this.y,this.width ,this.height);
			Font f = new Font(FontFamily.GenericSansSerif,20f);
			
			g.DrawString(this.text,f,SystemBrushes.ControlText,this.x + this.width / 2,this.y);
		
			switch(appearance)
			{
				case GlyphAppearance.Flat:
				case GlyphAppearance.Solid:
					break;
				case GlyphAppearance.Popup:
					g.DrawLine(new Pen(Color.White,1),this.x,this.y,this.x,this.y + this.height);//��߿�;
					g.DrawLine(new Pen(Color.White,1),this.x,this.y,this.x + this.width,this.y);//�ϱ߿�;
					g.DrawLine(new Pen(Color.Gray,1),this.x ,this.y + this.height,this.x + this.width,this.y + this.height);//�±߿�;
					g.DrawLine(new Pen(Color.Gray,1),this.x + this.width,this.y,this.x + this.width,this.y + this.height);//�ұ߿�;
					break;
			}
		}

		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(this.x,this.y,this.width,this.height);
			}
			set
			{
				this.x = value.X;
				this.y = value.Y;
				this.width = value.Width;
				this.height = value.Height;
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
