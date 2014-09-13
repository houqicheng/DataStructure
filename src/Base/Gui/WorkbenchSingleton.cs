using System;
using System.CodeDom.Compiler;
using System.Windows.Forms;

namespace NetFocus.DataStructure.Gui
{
	public class WorkbenchSingleton : DefaultWorkbench
	{
		
		static IWorkbench workbench    = null;
		

		static void CreateWorkspace()//�������������ռ�.
		{
			DefaultWorkbench w = new DefaultWorkbench();//�½�һ���յĹ���̨ʵ��.	
			workbench = w;				
			w.InitializeWorkspace();//��ʼ���˵�,������,״̬��֮��Ķ���.

		}
		

		public static IWorkbench Workbench 
		{
			get {
				if (workbench == null) {  //���Գ�ʼ��
					CreateWorkspace();
				}
				return workbench;
			}
		}
		
	}
}
