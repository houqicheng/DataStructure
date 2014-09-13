
using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

using NetFocus.DataStructure.TextEditor.Document;
using NetFocus.DataStructure.Gui.XmlForms;
using NetFocus.DataStructure.Services;
using NetFocus.DataStructure.Gui.OptionPanels.HighlightingEditor.Nodes;

namespace NetFocus.DataStructure.Gui.OptionPanels.HighlightingEditor.Panels
{
	public abstract class NodeOptionPanel : BaseXmlUserControl
	{
		//�˽ڵ��ʾ������ýڵ㲢��ʾ��ǰ�����Ǹ�treeNode�ڵ�.
		//eg.������DigitsNode�ڵ�,�����ʾDigitsOptionPanel���,��ôDigitsNode�ڵ����DigitsOptionPanel����ParentNode,�����ڵ�.
		protected AbstractNode parentNode;
		
		public AbstractNode ParentNode {
			get {
				return parentNode;
			}
		}
		
		
		public NodeOptionPanel(AbstractNode parentNode) {
			this.parentNode = parentNode;
			this.Dock = DockStyle.Fill;
			this.ClientSize = new Size(320, 392);
		}
		
		
		public virtual bool ValidateSettings()
		{
			return true;
		}
		
		
		protected void ValidationMessage(string str)
		{
			MessageService.ShowWarning("${res:Dialog.HighlightingEditor.ValidationError}\n\n" + str);
		}

		
		protected static Font ParseFont(string font)
		{
			string[] descr = font.Split(new char[]{',', '='});
			return new Font(descr[1], Single.Parse(descr[3]));
		}
			
		//����Ԥ����ǩ.
		protected static void PreviewUpdate(Label label, EditorHighlightColor color)
		{
			if (label == null) return;
			
			if (color == null) {
				label.ForeColor = label.BackColor = Color.Transparent;
				return;
			}
			if (color.NoColor) {
				label.ForeColor = label.BackColor = Color.Transparent;
				return;
			}
			
			label.ForeColor = color.GetForeColor();
			label.BackColor = color.GetBackColor();
			
			FontStyle fs = FontStyle.Regular;
			if (color.Bold)   fs |= FontStyle.Bold;
			if (color.Italic) fs |= FontStyle.Italic;
			
			label.Font = new Font(label.Font, fs);
		}
		
		
		public abstract void StoreSettings();
		
		public abstract void LoadSettings();
	}
}
