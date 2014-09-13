using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

using NetFocus.DataStructure.TextEditor;
using NetFocus.DataStructure.TextEditor.Document;
using NetFocus.DataStructure.Gui.Pads;
using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.Services;


namespace NetFocus.DataStructure.Internal.Algorithm
{
	public abstract class AbstractAlgorithm : IAlgorithm
	{
		object obj = null;
		int currentLine = 0;
		int[] lastLines;
		string[] codeFiles;
		int[] breakPoints;
		Type dialogType = null;

		public int[] BreakPoints
		{
			get
			{
				return breakPoints;
			}
			set
			{
				breakPoints = value;
			}
		}
		public Type DialogType
		{
			get
			{
				return dialogType;
			}
			set
			{
				dialogType = value;
			}
		}
		public int CurrentLine{
			get{
				return currentLine;
			}
			set{
				currentLine = value;
			}
		}
		public int[] LastLines
		{
			get
			{
				return lastLines;
			}
			set
			{
				lastLines = value;
			}
		}

		public string[] CodeFiles {
			get{
				return codeFiles;
			}
			set{
				codeFiles = value;
			}
		}

		public virtual object Status
		{
			get
			{
				return obj;
			}
			set
			{
				obj = value;
			}
		}
		

		public virtual bool ShowCustomizeDialog()
		{
			if(dialogType == null)
			{
				return false;
			}
			Form dialog = dialogType.Assembly.CreateInstance(dialogType.FullName) as Form;

			if(dialog != null && dialog.ShowDialog() == DialogResult.OK)
			{
				return true;
			}
			return false;
		}
		
		
		public virtual void Recover()
		{
			if(WorkbenchSingleton.Workbench.ActiveViewContent != null)
			{
				if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{					
					CurrentLine = 0;
					
					InitGraph();
					UpdateCurrentView();
					UpdatePropertyPad();
					UpdateAnimationPad();

					AlgorithmManager.Algorithms.Timer.Stop();
				}
			}
		}


		public virtual void ActiveWorkbenchWindow_CloseEvent(object sender, EventArgs e) 
		{
			AlgorithmManager.Algorithms.Timer.Tick -= AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;

			IViewContent content = sender as IViewContent;

			if(content != null && content.AlgorithmType != null && AlgorithmManager.Algorithms.OpeningAlgorithms.Contains(content.AlgorithmType))
			{
				AlgorithmManager.Algorithms.OpeningAlgorithms.Remove(content.AlgorithmType);
			}
			if(AlgorithmManager.Algorithms.OpeningAlgorithms.Count == 0 || (WorkbenchSingleton.Workbench.ActiveViewContent != null && WorkbenchSingleton.Workbench.ActiveViewContent.AlgorithmType == null))
			{
				AlgorithmManager.Algorithms.CurrentAlgorithm = null;
				UpdatePropertyPad();
				UpdateAnimationPad();
			}
			
		}
		
		
		void ActiveCurrentAlgorithm(object sender,EventArgs e)
		{
			AlgorithmManager.Algorithms.Timer.Stop();
			AlgorithmManager.Algorithms.Timer.Tick -= AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
			
			Type algorithmType = WorkbenchSingleton.Workbench.ActiveViewContent.AlgorithmType;

			if(algorithmType != null)
			{
				AlgorithmManager.Algorithms.CurrentAlgorithm = (IAlgorithm)AlgorithmManager.Algorithms.GetAlgorithm(algorithmType);
			}
			UpdateCurrentView();

			UpdatePropertyPad();

			UpdateAnimationPad();

		}

		
		public virtual bool GetData()
		{
			return false;
		}
		
		
		public virtual void Initialize(bool isOpen)
		{
			if(isOpen == true)
			{
				Recover();
				return;
			}
			//�����㷨�ļ�.
			IFileService fileService = (IFileService)ServiceManager.Services.GetService(typeof(IFileService));
			for(int i = 0; i < AlgorithmManager.Algorithms.CurrentAlgorithm.CodeFiles.Length; i++) 
			{
				fileService.OpenFile(AlgorithmManager.Algorithms.AlgorithmFilesPath + AlgorithmManager.Algorithms.CurrentAlgorithm.CodeFiles[i]);
				fileService.RecentOpenMemeto.RemoveLastFile();
			}

			//����ǰ������Ϊ0.
			CurrentLine = 0;
			
			//���㷨��Ϊֻ��.
			IViewContent content = WorkbenchSingleton.Workbench.ActiveViewContent;
			((TextEditorControl)content.Control).IsReadOnly = true;
			//����ǰ�򿪵��ļ����Ϊ�㷨,������һ��򿪵��ļ�
			content.AlgorithmType = AlgorithmManager.Algorithms.CurrentAlgorithm.GetType();
			
			content.ViewSelected += new EventHandler(ActiveCurrentAlgorithm);
			
			content.CloseEvent += new EventHandler(ActiveWorkbenchWindow_CloseEvent);

		
		}
		
		
		public abstract void InitGraph();
		
		public abstract void UpdateGraphAppearance();

		public virtual void UpdateAnimationPad()
		{
			AlgorithmManager.Algorithms.ClearStackPad();
		}
		
		
		public abstract void ExecuteAndUpdateCurrentLine();
		
		public void UpdateCurrentView()
		{
			if(WorkbenchSingleton.Workbench.WorkbenchLayout.ActiveViewContent != null && WorkbenchSingleton.Workbench.WorkbenchLayout.ActiveViewContent.AlgorithmType != null)
			{
				TextEditorControl textEditorControl = WorkbenchSingleton.Workbench.WorkbenchLayout.ActiveViewContent.Control as TextEditorControl;
				TextArea textArea = textEditorControl.ActiveTextAreaControl.TextArea;
				if(textArea != null)
				{
					int line = AlgorithmManager.Algorithms.CurrentAlgorithm.CurrentLine;

					if (line >= 0 && line < textArea.Document.TotalNumberOfLines) 
					{
						Point selectionStartPos = new Point(0, line);
						Point selectionEndPos = new Point(textArea.Document.GetLineSegment(line).Length + 1, line);
						textArea.SelectionManager.ClearSelection();
						textArea.SelectionManager.SetSelection(new DefaultSelection(textArea.Document, selectionStartPos, selectionEndPos));

						textArea.Caret.Position = selectionStartPos;//ѡ����Ӧ����֮�����ù���λ��
					}
				}
			}

		}

		
		public void UpdatePropertyPad()
		{
			IPadContent propertyPad = WorkbenchSingleton.Workbench.GetPad(typeof(PropertyPad));
			if(propertyPad != null) 
			{
				((PropertyGrid)propertyPad.Control).SelectedObject = null;
				if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{
					((PropertyGrid)propertyPad.Control).SelectedObject = AlgorithmManager.Algorithms.CurrentAlgorithm.Status;
				}
			}
		}

		//��ȡ���õĶϵ�
		void SetBreakPoints()
		{
			TextEditorControl textEditorControl = WorkbenchSingleton.Workbench.WorkbenchLayout.ActiveViewContent.Control as TextEditorControl;
			TextArea textArea = textEditorControl.ActiveTextAreaControl.TextArea;
			if(textEditorControl != null && textArea != null)
			{
				BreakPoints = (int[])textArea.Document.BookmarkManager.Marks.ToArray(typeof(int));
			}
		}
		//�ж��Ƿ��㷨�Ѿ����н���
		bool HaveFinished()
		{
			for(int i = 0;i < AlgorithmManager.Algorithms.CurrentAlgorithm.LastLines.Length;i++)
			{
				if(CurrentLine == AlgorithmManager.Algorithms.CurrentAlgorithm.LastLines[i])//�ж��Ƿ����е����һ��
				{	
					return true;
				}
			}
			return false;
		}
		//�ж��㷨�Ƿ����е��������˶ϵ����
		bool HaveRunToBreakPoints()
		{
			foreach(int runtoLine in AlgorithmManager.Algorithms.CurrentAlgorithm.BreakPoints)
			{
				if(CurrentLine == runtoLine)//�����ǰ�е���������һ�����öϵ����
				{
					return true;
				}
			}
			return false;
		}
		//һ���������ڲ������������ж��Ƿ����е��ϵ��л������
		int RunningExceptFinish()
		{
			if(HaveRunToBreakPoints() == true)
			{
				AlgorithmManager.Algorithms.Timer.Stop();

				bool finished = false;
				finished = HaveFinished();
					
				UpdateCurrentView();
				ExecuteAndUpdateCurrentLine();
				UpdatePropertyPad();
				UpdateAnimationPad();
					
				if(finished == true)
				{
					return 2;  //����2��ʾ�㷨�����Ѿ����е��������˶ϵ����,�����Ѿ����
				}
				return 1;  //����1��ʾ�㷨���е��������˶ϵ����
			}
			return 0;  //����0��ʾ�㷨û�����е��κ������˶ϵ����
		}
		/// <summary>
		/// �㷨���еĹǼܣ�һ��ģ�巽��
		/// </summary>
		public virtual void UpdateAlgorithmStatus(object sender,EventArgs e)
		{
			if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
			{
				if(AlgorithmManager.Algorithms.IsRunto == false)  //��������л��ߵ�������
				{
					SetBreakPoints();

					bool finished = false;
					finished = HaveFinished();
					
					if(HaveRunToBreakPoints() == true)
					{
						AlgorithmManager.Algorithms.Timer.Stop();  //������е������öϵ����,��ֹͣ��ʱ��
					}
					
					UpdateCurrentView();  //�����ĸ��ǹ��ӷ���
					ExecuteAndUpdateCurrentLine();
					UpdatePropertyPad();
					UpdateAnimationPad();
					
					if(AlgorithmManager.Algorithms.IsByStep == true)
					{
						AlgorithmManager.Algorithms.Timer.Stop();  //����ǵ�������,��ִ��һ�κ��ü�ʱ��ֹͣ
					}

					if(finished == true)
					{
						AlgorithmManager.Algorithms.Timer.Stop();
						MessageBox.Show("�㷨���н�����","��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Information);
					}
				}
				else  //�����ֱ��ִ�е�ĳ��
				{
					int flag1 = RunningExceptFinish();
					if(flag1 >= 1)
					{
						if(flag1 == 2)
						{
							MessageBox.Show("�㷨���н�����","��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Information);
						}
						return;
					}
					//ע��,Ҫ�ڵ�ǰ�е�ǰ��ͺ��涼Ҫ�ж��Ƿ����е��������˶ϵ����,��Ϊ�㷨����ʱ�������Ǵ�������.
					ExecuteAndUpdateCurrentLine();   

					int flag2 = RunningExceptFinish();
					if(flag2 >= 1)
					{
						if(flag2 == 2)
						{
							MessageBox.Show("�㷨���н�����","��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Information);
						}
						return;
					}
					
					//�����ǰ�е���������һ���˳��㷨����
					if(HaveFinished() == true)
					{
						UpdateCurrentView();
						ExecuteAndUpdateCurrentLine();
						UpdatePropertyPad();
						UpdateAnimationPad();

						AlgorithmManager.Algorithms.Timer.Stop();

						MessageBox.Show("�㷨���н�����","��Ϣ",MessageBoxButtons.OK,MessageBoxIcon.Information);
					}
					
				}
			}

		}


	}
}
