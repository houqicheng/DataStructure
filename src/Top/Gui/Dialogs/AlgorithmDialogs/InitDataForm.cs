using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Threading;
using System.Globalization;
using System.Xml;

using NetFocus.DataStructure.Gui.Views;
using NetFocus.DataStructure.Properties;
using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.TextEditor;
using NetFocus.DataStructure.TextEditor.Document;
using NetFocus.DataStructure.Gui.Pads;
using NetFocus.DataStructure.Internal.Algorithm;
using NetFocus.DataStructure.Internal.Algorithm.Glyphs;


namespace NetFocus.DataStructure.Gui.Algorithm.Dialogs
{
	/// <summary>
	/// InitDataForm ��ժҪ˵����
	/// </summary>
	public class InitDataForm : System.Windows.Forms.Form
	{
		private NetFocus.DataStructure.Internal.Algorithm.StatusItemControl statusItemControl1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCustomize;
		private ArrayList statusItemList = new ArrayList();
		private int selectedIndex = -1;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ArrayList StatusItemList
		{
			get
			{
				return statusItemList;
			}
			set
			{
				statusItemList = value;
			}
		}


		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}
		}
		

		public InitDataForm()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCustomize = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(176, 312);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "ȷ ��";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCustomize
			// 
			this.btnCustomize.Location = new System.Drawing.Point(272, 312);
			this.btnCustomize.Name = "btnCustomize";
			this.btnCustomize.Size = new System.Drawing.Size(64, 24);
			this.btnCustomize.TabIndex = 2;
			this.btnCustomize.Text = "�Զ���";
			this.btnCustomize.Click += new System.EventHandler(this.btnCustomize_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(80, 312);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(64, 24);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "ɾ ��";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(368, 312);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "ȡ ��";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// InitDataForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(532, 353);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnCustomize);
			this.Controls.Add(this.btnOK);
			
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InitDataForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "��ʼ������";
			this.Load += new System.EventHandler(this.InitDataForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		void InitItemControl()
		{
			this.SuspendLayout();

			this.statusItemControl1 = new StatusItemControl();
			this.statusItemControl1.CurrentSelectIndex = -1;
			this.statusItemControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.statusItemControl1.Location = new System.Drawing.Point(0, 0);
			this.statusItemControl1.Name = "statusItemControl1";
			this.statusItemControl1.Size = new System.Drawing.Size(532, 300);
			this.statusItemControl1.TabIndex = 0;

			this.Controls.Add(this.statusItemControl1);

			this.ResumeLayout(false);

			foreach(StatusItem statusItem in statusItemList)
			{
				statusItemControl1.Items.Add(statusItem);
			}

		}
		private void InitDataForm_Load(object sender, System.EventArgs e)
		{
			InitItemControl();

		}


		private void btnOK_Click(object sender, System.EventArgs e)
		{
			selectedIndex = statusItemControl1.CurrentSelectIndex;

			if(selectedIndex == -1)
			{
				MessageBox.Show("��ѡ��һ��Ҫ��ʾ�����ݣ�","��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			
			this.DialogResult = DialogResult.OK;

			this.Close();

		}
		
		private void btnCustomize_Click(object sender, System.EventArgs e)
		{
			if(AlgorithmManager.Algorithms.CurrentAlgorithm.ShowCustomizeDialog() == true)
			{
				selectedIndex = -1;  //˵���û���ͨ���Զ��巽ʽ�������ݣ�Ӧ�ð�selectedIndex����Ϊ-1
			
				this.DialogResult = DialogResult.OK;

				this.Close();
			}

		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			selectedIndex = statusItemControl1.CurrentSelectIndex;

			if(selectedIndex == -1)
			{
				MessageBox.Show("��ѡ��һ��Ҫɾ�������ݣ�","��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}

			if(MessageBox.Show("ȷ���Ƿ�ɾ����","����",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
			{

				XmlDocument doc = new XmlDocument();
				doc.Load(AlgorithmManager.Algorithms.AlgorithmExampleDataFile);
				XmlNode parentNode = null;
				XmlNodeList nodes  = doc.DocumentElement.ChildNodes;
				if(selectedIndex >= 0 && AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{
					foreach (XmlElement el in nodes)
					{
						if(el.Attributes["name"].Value == AlgorithmManager.Algorithms.CurrentAlgorithm.GetType().ToString())
						{
							parentNode = el.ChildNodes[0];
							break;
						}
					}
					if(parentNode != null)
					{
						parentNode.RemoveChild(parentNode.ChildNodes[selectedIndex]);
					}
				
					doc.Save(AlgorithmManager.Algorithms.AlgorithmExampleDataFile);

					statusItemList.RemoveAt(selectedIndex);
				
					statusItemControl1.Items.RemoveAt(selectedIndex);

					this.Controls.Remove(this.statusItemControl1);

					InitItemControl();

				}
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;

			this.Close();

		}
	

	}
}
