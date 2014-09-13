
using System;
using System.Drawing;
using System.Windows.Forms;
using NetFocus.DataStructure.Services;

namespace NetFocus.DataStructure.Gui.Components
{
	public class SdStatusBar : AxStatusBar
	{
		ProgressBar statusProgressBar      = new ProgressBar();
		
		AxStatusBarPanel txtStatusBarPanel    = new AxStatusBarPanel();
		AxStatusBarPanel cursorStatusBarPanel = new AxStatusBarPanel();
		AxStatusBarPanel modeStatusBarPanel   = new AxStatusBarPanel();

		public AxStatusBarPanel CursorStatusBarPanel {
			get {
				return cursorStatusBarPanel;
			}
		}
		
		public AxStatusBarPanel ModeStatusBarPanel {
			get {
				return modeStatusBarPanel;
			}
		}
		
		
		public SdStatusBar(IStatusBarService manager)
		{
			txtStatusBarPanel.Width = 500;
			txtStatusBarPanel.AutoSize = StatusBarPanelAutoSize.Spring;
			this.Panels.Add(txtStatusBarPanel);//��״̬���������ʾ���ݵ�һ�����.
			
			statusProgressBar.Width  = 200;
			statusProgressBar.Height = 14;
			statusProgressBar.Location = new Point(160, 4);
			statusProgressBar.Minimum = 0;
			statusProgressBar.Visible = false;
			Controls.Add(statusProgressBar);//��״̬���������ʾ���ȵ�һ�����.
			
			cursorStatusBarPanel.Width = 150;
			cursorStatusBarPanel.AutoSize = StatusBarPanelAutoSize.None;
			cursorStatusBarPanel.Alignment = HorizontalAlignment.Left;
			Panels.Add(cursorStatusBarPanel);//��״̬���������ʾ���λ�õ�һ�����.
				
			modeStatusBarPanel.Width = 35;
			modeStatusBarPanel.AutoSize = StatusBarPanelAutoSize.None;
			modeStatusBarPanel.Alignment = HorizontalAlignment.Right;
			Panels.Add(modeStatusBarPanel);//��״̬���������ʾģʽ��һ�����.
			
			this.ShowPanels = true;
		}
		
		
		public void ShowErrorMessage(string message)
		{
			txtStatusBarPanel.Text = "���� : " + message;
		}
		
		
		public void ShowErrorMessage(Image image, string message)
		{
			txtStatusBarPanel.Text = "���� : " + message;
		}
		
		
		public void SetMessage(string message)
		{
			txtStatusBarPanel.Text = message;
		}
		
		
		public void SetMessage(Image image, string message)
		{
			txtStatusBarPanel.Text = message;
		}
		
		
	}
}
