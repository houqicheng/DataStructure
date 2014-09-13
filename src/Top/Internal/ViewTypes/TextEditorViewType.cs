
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

using NetFocus.DataStructure.Gui;
using NetFocus.DataStructure.Gui.Pads;
using NetFocus.DataStructure.TextEditor;
using NetFocus.DataStructure.TextEditor.Undo;
using NetFocus.DataStructure.TextEditor.Document;
using NetFocus.DataStructure.Properties;
using NetFocus.Components.AddIns;
using NetFocus.DataStructure.Services;
using NetFocus.Components.AddIns.Codons;
using NetFocus.DataStructure.Gui.Views;


namespace NetFocus.DataStructure.ViewTypes
{
	/// <summary>
	/// ���ڰ�һ���ı��༭��.
	/// </summary>
	public class TextEditorViewType : IViewType
	{
		static TextEditorViewType()
		{
			PropertyService propertyService = (PropertyService)ServiceManager.Services.GetService(typeof(PropertyService));
			if(propertyService != null)
			{
				//ʵ����һ����������ʾ�﷨�Ķ���.(�ö�����������ļ��ĸ�������ʾ����)
				SyntaxModeProvider syntaxModeProvider = new SyntaxModeProvider(Path.Combine(propertyService.DataDirectory,"modes"));

				HighlightingManager.Manager.AddSyntaxModeProvider(syntaxModeProvider);//���һ���﷨��Ŀ��ʾ�ṩ��.
			}
		}
		
		
		public virtual bool CanCreateContentForFile(string fileName)
		{
			return true;//��Ϊ���ı��༭��,����һֱ������.
		}
		
		
		public virtual bool CanCreateContentForLanguage(string language)
		{
			return true;//��Ϊ���ı��༭��,����һֱ������.
		}
		

		public virtual IViewContent CreateContentForFile(string fileName)
		{

			TextEditorView t = new TextEditorView();
			
			t.LoadFile(fileName);

			return t;
		}
		

		public virtual IViewContent CreateContentForLanguage(string language, string content)
		{
			TextEditorView t = new TextEditorView();
			
			StringParserService stringParserService = (StringParserService)ServiceManager.Services.GetService(typeof(StringParserService));
			((TextEditorControl)t.Control).Document.TextContent = stringParserService.Parse(content);
			((TextEditorControl)t.Control).Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterByName(language);

			return t;
		}		


	}
	
}
