using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace NetFocus.DataStructure.Internal.Algorithm
{
	public class StatusItemControl : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public StatusItemControl()
		{
			// �õ����� Windows.Forms ���������������ġ�
			InitializeComponent();

			// TODO: �� InitializeComponent ���ú�����κγ�ʼ��

		}

		/// <summary> 
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlItemContainer = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// pnlItemContainer
			// 
			this.pnlItemContainer.AutoScroll = true;
			this.pnlItemContainer.BackColor = System.Drawing.Color.White;
			this.pnlItemContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlItemContainer.Location = new System.Drawing.Point(0, 0);
			this.pnlItemContainer.Name = "pnlItemContainer";
			this.pnlItemContainer.Size = new System.Drawing.Size(500, 160);
			this.pnlItemContainer.TabIndex = 0;
			// 
			// StatusItemControl
			// 
			this.Controls.Add(this.pnlItemContainer);
			this.Name = "StatusItemControl";
			this.Size = new System.Drawing.Size(500, 160);
			this.Load += new System.EventHandler(this.StatusItemControl_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.StatusItemControl_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		Panel pnlItemContainer;
		EventHandler itemClickHandler = null;
		StatusItemCollection items = new StatusItemCollection();
		int currentIndex = - 1;
		/// <summary>
		/// return the current item index of this control
		/// </summary>
		public int CurrentSelectIndex
		{
			get
			{
				return currentIndex;
			}
			set
			{
				currentIndex = value;
			}
		}

		/// <summary>
		/// Return the StatusItem Collection of this control
		/// </summary>
		public StatusItemCollection Items
		{
			get
			{
				return items;
			}
			set
			{
				items = value;
			}
		}

		
		private void StatusItemControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			this.pnlItemContainer.Controls.Clear();
			//�����items�����е����һ��Ԫ�ؿ�ʼ������ӵ�����Contros������
			//ΪʲôҪ�����һ����ʼ����Ϊ��ÿ��Ԫ�ص�Dock���Ա�����ΪTop�����������ӽ�ȥ�Ļ�����������Ϸ���
			//���Ҫע�⡣
			for(int i = items.Count - 1;i >= 0;i--)
			{
				items[i].ItemClick -= itemClickHandler;
				items[i].ItemClick += itemClickHandler;
				this.pnlItemContainer.Controls.Add(items[i]);
			}

		}


		public event EventHandler SelectIndexChanged;

		void SelectItem(object sender, EventArgs e)
		{
			Panel pnl = sender as Panel;
			if(pnl != null)
			{
				int index = 0;
				bool stop = false;
				foreach(StatusItem item in items)
				{
					if(item != pnl)
					{
						item.BoolTag = false;
						if(stop == false)
						{
							index++;
						}
					}
					else
					{
						stop = true;
					}
					
				}
				currentIndex = index;
				OnSelectIndexChanged(e);
			}
		}
		public virtual void OnSelectIndexChanged(EventArgs e)
		{
			foreach(StatusItem item in items)
			{
				item.Invalidate();
			}
			if (SelectIndexChanged != null) 
			{
				SelectIndexChanged(pnlItemContainer, e);
			}
		}

		private void StatusItemControl_Load(object sender, System.EventArgs e)
		{
			itemClickHandler = new EventHandler(SelectItem);
		}


	}
}
