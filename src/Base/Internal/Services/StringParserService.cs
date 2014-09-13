
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

namespace NetFocus.DataStructure.Services
{
	/// <summary>
	/// ��������ڷ���һ���ַ���,���õ���ȷ���ַ���,��Ҫ���ڶ�ȡ��Դ�ļ��е���Ϣ.
	/// </summary>
	public class StringParserService : AbstractService
	{
		PropertyDictionary properties = new PropertyDictionary();
		
		public PropertyDictionary Properties {
			get {
				return properties;
			}
		}
		
		
		public StringParserService()
		{
            IDictionary variables = Environment.GetEnvironmentVariables();
            foreach (string name in variables.Keys) {
                properties.Add("env:" + name, (string)variables[name]);
            }
		}
		

		public void Parse(ref string[] inputs)
		{
			for (int i = inputs.GetLowerBound(0); i <= inputs.GetUpperBound(0); ++i) 
			{
				inputs[i] = Parse(inputs[i], null);
			}
		}
		

		public string Parse(string input)
		{
			return Parse(input, null);
		}
			
		
		public string Parse(string input, string[,] customTags)
		{
			string output = input;
			if (input != null) {
				const string pattern = @"\$\{([^\}]*)\}";
				foreach (Match m in Regex.Matches(input, pattern)) {
					if (m.Length > 0) {
						string token         = m.ToString();
						string propertyName  = m.Groups[1].Captures[0].Value;
						string propertyValue = null;
						switch (propertyName.ToUpper()) {
							case "DATE": // current date
								propertyValue = DateTime.Today.ToShortDateString();
								break;
							case "TIME": // current time
								propertyValue = DateTime.Now.ToShortTimeString();
								break;
							default:
								propertyValue = null;
								if (customTags != null) {
									for (int j = 0; j < customTags.GetLength(0); ++j) {
										if (propertyName.ToUpper() == customTags[j, 0].ToUpper()) {
											propertyValue = customTags[j, 1];
											break;
										}
									}
								}
								if (propertyValue == null) {
									propertyValue = properties[propertyName.ToUpper()];
								}
								if (propertyValue == null) {
									int k = propertyName.IndexOf(':');
									if (k > 0) {
										switch (propertyName.Substring(0, k).ToUpper()) {
											case "RES":
												ResourceService resourceService = (ResourceService)ServiceManager.Services.GetService(typeof(ResourceService));
												propertyValue = Parse(resourceService.GetString(propertyName.Substring(k + 1)), customTags);
												break;
											case "PROPERTY":
												PropertyService propertyService = (PropertyService)ServiceManager.Services.GetService(typeof(PropertyService));
												propertyValue = propertyService.GetProperty(propertyName.Substring(k + 1)).ToString();
												break;
										}
									}
								}
								break;
						}
						if (propertyValue != null) {
							output = output.Replace(token, propertyValue);
						}
					}
				}
			}
			return output;
		}
	}
	

	/// <summary>
	/// ���ﶨ����һ���µļ̳���DictionaryBase�����ԭ������Ϊ��Ҫ����һЩֻ�����ַ�������,����Щ���Է���һ��������StringCollection��
	/// </summary>
	public class PropertyDictionary : DictionaryBase
	{
		/// <summary>
		/// ���ڱ���һЩֻ�������Ե����Ƶ�һ���ַ�������
		/// </summary>
		StringCollection readOnlyProperties = new StringCollection();
		
		/// <summary>
		/// ���һ��ֻ�������Եļ�ֵ����Ϣ��һ���ַ���������
		/// </summary>
		public void AddReadOnly(string name, string value) 
		{
			if (!readOnlyProperties.Contains(name)) {
				readOnlyProperties.Add(name);
				Dictionary.Add(name, value);
			}
		}
		
		
		/// <summary>
		/// ���һ���ɶ�д�����Եļ�ֵ����Ϣ��һ���ַ���������
		/// </summary>
		public void Add(string name, string value) 
		{
			if (!readOnlyProperties.Contains(name)) {
				Dictionary.Add(name, value);
			}
		}
		
		
		public string this[string name] {  //һ��Ĭ�ϵĴ�����������
			get { 
				return (string)Dictionary[(object)name.ToUpper()];
			}
			set {
				Dictionary[name.ToUpper()] = value;
			}
		}
		
		
		protected override void OnClear() 
		{
			readOnlyProperties.Clear();
		}
	}	
	

}
