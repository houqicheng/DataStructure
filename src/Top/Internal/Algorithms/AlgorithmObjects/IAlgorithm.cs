// created on 2005-5-11 at 14:22
using System.Drawing;
using System.Windows.Forms;
using System;

namespace NetFocus.DataStructure.Internal.Algorithm {
	
	public interface IAlgorithm {

		/// <summary>
		/// �����㷨�ĵ�ǰ��
		/// </summary>
		int CurrentLine{
			get;
			set;
		}
		/// <summary>
		/// �����㷨ִ�����ʱ���кż���
		/// </summary>
		int[] LastLines{
			get;
			set;
		}
		/// <summary>
		/// ��ʾ�㷨ʱ�����Դ�����ļ�
		/// </summary>
		string[] CodeFiles{
			get;
			set;
		}
		/// <summary>
		/// ����ǰ�㷨�����״̬
		/// </summary>
		object Status
		{
			get;
			set;
		}

		/// <summary>
		/// ��ǰ�㷨�Ķϵ㼯��
		/// </summary>
		int[] BreakPoints
		{
			get;
			set;
		}
		//���ڽ��ܵ�ǰ�㷨�Զ������ݵĶԻ��������
		Type DialogType
		{
			get;
			set;
		}
		/// <summary>
		/// Ϊ��ǰ�㷨��ʾ�Զ���Ի�������������
		/// </summary>
		bool ShowCustomizeDialog();
		/// <summary>
		/// �ָ��㷨���ճ�ʼ��ʱ��״̬
		/// </summary>
		void Recover();
		/// <summary>
		/// �õ���ʾ�㷨����Ҫ�ĳ�ʼ������
		/// </summary>
		bool GetData();
		/// <summary>
		/// ��ʼ����ǰ�㷨����
		/// </summary>
		void Initialize(bool isOpen);
		/// <summary>
		/// ��ʼ����ǰ�㷨�����еĶ�����ʾ
		/// </summary>
		void InitGraph();
		/// <summary>
		/// ִ�в����µ�ǰ�㷨�ĵ�ǰ��
		/// </summary>
		void ExecuteAndUpdateCurrentLine();
		/// <summary>
		/// ���µ�ǰ�㷨�����ж��������
		/// </summary>
		void UpdateGraphAppearance();
		/// <summary>
		/// ���µ�ǰ�㷨�������ڵ���ͼ,ʹ����¸�������ʾ����
		/// </summary>
		void UpdateCurrentView();
		/// <summary>
		/// ����ǰ�㷨�����״̬�ı�,���ߴ�һ���㷨���󻻵���һ���㷨����ʱ,�����������
		/// </summary>
		void UpdatePropertyPad();
		/// <summary>
		/// ���¶������
		/// </summary>
		void UpdateAnimationPad();
		/// <summary>
		/// ���µ�ǰ�㷨�����״̬,����һ��ģ�巽��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void UpdateAlgorithmStatus(object sender,EventArgs e);

	} 

}

