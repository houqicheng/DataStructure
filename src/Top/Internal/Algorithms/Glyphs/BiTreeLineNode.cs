using System;
using System.Drawing;


namespace NetFocus.DataStructure.Internal.Algorithm.Glyphs
{
	/// <summary>
	/// ��ʾһ�����Ӷ������н����߶�ͼԪ����,��ʵ�������Ҳ���Կ�����һ���������еĽ��
	/// </summary>
	public class BiTreeLineNode : IBiTreeNode
	{
		Color color;
		GlyphAppearance appearance = GlyphAppearance.Flat;
		int x1,y1,x2,y2;
		int width = 2;
		IBiTreeNode leftChild = null;  //���ӽ��
		IBiTreeNode rightChild = null;  //���ӽ��

		public BiTreeLineNode(int x1,int y1,int x2,int y2) : this(x1,y1,x2,y2,2)
		{
		}

		public BiTreeLineNode(int x1,int y1,int x2,int y2,int width) : this(x1,y1,x2,y2,width,Color.HotPink)
		{
		}

		public BiTreeLineNode(int x1,int y1,int x2,int y2,int width,Color color)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
			this.width = width;
			this.color = color; 
		}

		
		#region IGlyph ��Ա

		public void Draw(Graphics g)
		{
			g.DrawLine(new Pen(this.color,this.width),x1,y1,x2,y2);
		}

		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(x1,y1,x2-x1,y2-y1);  //���ﲻ��ȥ����,��Ϊ����������˵,����Bounds����Ҫ;
			}
			set
			{
				this.x1 = value.X;
				this.y1 = value.Y;
				this.x2 = value.X + value.Width;
				this.y2 = value.Y + value.Height;
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

		#region IBiTreeNode ��Ա

		public string Text
		{
			get
			{
				return null;
			}
			set
			{

			}
		}

		public IBiTreeNode LeftChild
		{
			get
			{
				return leftChild;
			}
			set
			{
				leftChild = value;
			}

		}

		public IBiTreeNode RightChild
		{
			get
			{
				return rightChild;
			}
			set
			{
				rightChild = value;
			}
		}

		
		#endregion
	}
}
