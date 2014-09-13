using System;
using System.Windows.Forms;
using NetFocus.DataStructure.Internal.Algorithm;
using NetFocus.Components.AddIns.Codons;
using NetFocus.DataStructure.Gui.Pads;
using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.Gui.Views;
using NetFocus.DataStructure.TextEditor;
using NetFocus.DataStructure.TextEditor.Document;
using NetFocus.DataStructure.TextEditor.Actions;


namespace NetFocus.DataStructure.Commands.Algorithm
{
	public class AlgorithmExecuteCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			if(WorkbenchSingleton.Workbench.ActiveViewContent != null && WorkbenchSingleton.Workbench.ActiveViewContent.AlgorithmType != null)
			{
				if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{
					TextEditorControl textEditorControl = WorkbenchSingleton.Workbench.WorkbenchLayout.ActiveViewContent.Control as TextEditorControl;
					TextArea textArea = textEditorControl.ActiveTextAreaControl.TextArea;
					if(textEditorControl != null && textArea != null)
					{
						AlgorithmManager.Algorithms.CurrentAlgorithm.BreakPoints = (int[])textArea.Document.BookmarkManager.Marks.ToArray(typeof(int));
					}

					//��ȷ�����㷨�������м�ʱ����Interval���Ժ�Tick�¼��Ĵ������
					AlgorithmManager.Algorithms.Timer.Interval = AlgorithmManager.Algorithms.ExecuteSpeed;
					AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler = new EventHandler(AlgorithmManager.Algorithms.CurrentAlgorithm.UpdateAlgorithmStatus);
				
					AlgorithmManager.Algorithms.Timer.Tick -=AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
					AlgorithmManager.Algorithms.Timer.Tick +=AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
					AlgorithmManager.Algorithms.IsByStep = false;
					AlgorithmManager.Algorithms.IsRunto = false;
					AlgorithmManager.Algorithms.Timer.Start();
				}
			}

		}
	}
	public class AlgorithmExecuteToCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			if(WorkbenchSingleton.Workbench.ActiveViewContent != null && WorkbenchSingleton.Workbench.ActiveViewContent.AlgorithmType != null)
			{
				if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{
					TextEditorControl textEditorControl = WorkbenchSingleton.Workbench.WorkbenchLayout.ActiveViewContent.Control as TextEditorControl;
					TextArea textArea = textEditorControl.ActiveTextAreaControl.TextArea;
					if(textEditorControl != null && textArea != null)
					{
						AlgorithmManager.Algorithms.CurrentAlgorithm.BreakPoints = (int[])textArea.Document.BookmarkManager.Marks.ToArray(typeof(int));
					}
					
					if(AlgorithmManager.Algorithms.CurrentAlgorithm.BreakPoints.Length > 0)
					{
						//���������ü�ʱ����Interval����֮ǰҪ�����ֵ,�Ա��´���ִ�е�ʱ���ȡ��ֵ
						if(AlgorithmManager.Algorithms.Timer.Interval != 10)
						{
							AlgorithmManager.Algorithms.ExecuteSpeed = AlgorithmManager.Algorithms.Timer.Interval;
						}						
						AlgorithmManager.Algorithms.Timer.Interval = 10;
						AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler = new EventHandler(AlgorithmManager.Algorithms.CurrentAlgorithm.UpdateAlgorithmStatus);
				
						AlgorithmManager.Algorithms.Timer.Tick -=AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
						AlgorithmManager.Algorithms.Timer.Tick +=AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
						AlgorithmManager.Algorithms.IsRunto = true;
						AlgorithmManager.Algorithms.IsByStep = false;
						AlgorithmManager.Algorithms.Timer.Start();
					}
				}
			}
		}
	}
	public class AlgorithmStepByCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			if(WorkbenchSingleton.Workbench.ActiveViewContent != null && WorkbenchSingleton.Workbench.ActiveViewContent.AlgorithmType != null)
			{
				if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{
					TextEditorControl textEditorControl = WorkbenchSingleton.Workbench.WorkbenchLayout.ActiveViewContent.Control as TextEditorControl;
					TextArea textArea = textEditorControl.ActiveTextAreaControl.TextArea;
					if(textEditorControl != null && textArea != null)
					{
						AlgorithmManager.Algorithms.CurrentAlgorithm.BreakPoints = (int[])textArea.Document.BookmarkManager.Marks.ToArray(typeof(int));
					}
					//���������ü�ʱ����Interval����֮ǰҪ�����ֵ,�Ա��´���ִ�е�ʱ���ȡ��ֵ
					if(AlgorithmManager.Algorithms.Timer.Interval != 10)
					{
						AlgorithmManager.Algorithms.ExecuteSpeed = AlgorithmManager.Algorithms.Timer.Interval;
					}
					AlgorithmManager.Algorithms.Timer.Interval = 10;
					AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler = new EventHandler(AlgorithmManager.Algorithms.CurrentAlgorithm.UpdateAlgorithmStatus);
				
					AlgorithmManager.Algorithms.Timer.Tick -=AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
					AlgorithmManager.Algorithms.Timer.Tick +=AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
					AlgorithmManager.Algorithms.IsByStep = true;
					AlgorithmManager.Algorithms.IsRunto = false;
					AlgorithmManager.Algorithms.Timer.Start();
				}
			}

		}
	}
	public class AlgorithmRecoverCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			if(WorkbenchSingleton.Workbench.ActiveViewContent != null && WorkbenchSingleton.Workbench.ActiveViewContent.AlgorithmType != null)
			{
				if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{
					AlgorithmManager.Algorithms.CurrentAlgorithm.Recover();
					AlgorithmManager.Algorithms.IsByStep = false;
					AlgorithmManager.Algorithms.IsRunto = false;
				}
			}
		}
	}
	public class AlgorithmRecoverAllCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			AlgorithmManager.Algorithms.RecoverAllOpeningAlgorithms();
			AlgorithmManager.Algorithms.IsByStep = false;
			AlgorithmManager.Algorithms.IsRunto = false;
		}
	}
	public class AlgorithmDataCommand : AbstractMenuCommand
	{
		bool HaveFinished()
		{
			if(AlgorithmManager.Algorithms.CurrentAlgorithm == null)
			{
				return true;
			}
			for(int i = 0;i < AlgorithmManager.Algorithms.CurrentAlgorithm.LastLines.Length;i++)
			{
				if(AlgorithmManager.Algorithms.CurrentAlgorithm.CurrentLine == AlgorithmManager.Algorithms.CurrentAlgorithm.LastLines[i])//�ж��Ƿ����е����һ��
				{	
					return true;
				}
			}
			return false;
		}
		public override void Run()
		{
			if(WorkbenchSingleton.Workbench.ActiveViewContent != null && WorkbenchSingleton.Workbench.ActiveViewContent.AlgorithmType != null)
			{
				if(AlgorithmManager.Algorithms.CurrentAlgorithm != null)
				{
					AlgorithmManager.Algorithms.Timer.Stop();

					if(AlgorithmManager.Algorithms.CurrentAlgorithm.GetData() == true)
					{
						AlgorithmManager.Algorithms.CurrentAlgorithm.Initialize(true);
					}
					else
					{
						if(HaveFinished() == false)
						{
							AlgorithmManager.Algorithms.Timer.Start();
						}
					}
				}
			}
		}
	}
	public class AlgorithmPauseCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			AlgorithmManager.Algorithms.Timer.Stop();
			AlgorithmManager.Algorithms.Timer.Tick -=AlgorithmManager.Algorithms.UpdateAlgorithmStatusEventHandler;
			AlgorithmManager.Algorithms.IsByStep = false;
			AlgorithmManager.Algorithms.IsRunto = false;
		}
	}
	public class AlgorithmExplainCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			//TODO
		}
	}
	public abstract class AbstractEditActionMenuCommand : AbstractMenuCommand
	{
		public abstract IEditAction EditAction 
		{
			get;
		}
		
		public override void Run()
		{
			IViewContent content = WorkbenchSingleton.Workbench.ActiveViewContent;
			
			if (content == null || !(content is TextEditorView)) 
			{
				return;
			}
			TextEditorControl textEditor = content.Control as TextEditorControl;
			EditAction.Execute(textEditor.ActiveTextAreaControl.TextArea);
		}
	}
	public class ToggleBookmark : AbstractEditActionMenuCommand
	{
		public override IEditAction EditAction 
		{
			get 
			{
				return new NetFocus.DataStructure.TextEditor.Actions.ToggleBookmark();
			}
		}
	}
	public class ClearBookmarks : AbstractEditActionMenuCommand
	{
		public override IEditAction EditAction 
		{
			get 
			{
				return new NetFocus.DataStructure.TextEditor.Actions.ClearAllBookmarks();
			}
		}
	}
}
