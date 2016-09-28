using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public partial class FWizardBase : Form
	{
		#region Constructors

		public FWizardBase()
		{
			InitializeComponent();

			this.FormClosing += new FormClosingEventHandler(FWizardBase_FormClosing);
		}

		private void FFilterWizard_Load(object sender, EventArgs e)
		{
			EnsureButtonsState();
		}

		#endregion

		#region Properties

		private int _currentStep = 1;

		/// <summary>
		/// 设置或获取向导当前处于第几步
		/// </summary>
		public int CurrentStep
		{
			get { return _currentStep; }
			set 
			{
				_currentStep = value;
				EnsureButtonsState();
			}
		}
		private int _maxStep = 1;

		/// <summary>
		/// 设置或获取向导最大步数
		/// </summary>
		public int MaxStep
		{
			get { return _maxStep; }
			set 
			{
				_maxStep = value;
				EnsureButtonsState();
			}
		}

		/// <summary>
		/// 前进或后退时发生（ Cancel 属性用于传递当前是前进 true  还是后退 false 同时用做是否取消行为判定）
		/// </summary>
		public event EventHandler<CancelEventArgs> OnStepChanging = null;
		/// <summary>
		/// 前进或后退之后发生
		/// </summary>
		public event EventHandler OnStepChanged = null;
		/// <summary>
		/// 用户请求 帮助 时发生
		/// </summary>
		public event EventHandler OnHelpRequest = null;
		/// <summary>
		/// 用户点击 完成 后发生
		/// </summary>
		public event EventHandler OnFinished = null;
		/// <summary>
		/// 用户取消向导时发生（可取消取消行为）
		/// </summary>
		public event EventHandler<CancelEventArgs> OnCanceling = null;

		/// <summary>
		/// 用来放 title 的 panel
		/// </summary>
		public Panel TitlePanel{
			get { return _Title_panel; }
		}

		/// <summary>
		/// 用来放内容的 panel
		/// </summary>
		public Panel ContentPanel
		{
			get { return _Content_panel; }
		}

		#endregion

		#region Event Methods

		protected virtual void _Help_button_Click(object sender, EventArgs e)
		{
			if (OnHelpRequest != null) OnHelpRequest(this, null);
		}

		protected virtual void _Back_button_Click(object sender, EventArgs e)
		{
			if (OnStepChanging != null)
			{
				// 用 Handled 属性来传当前按钮的方向 ( false 为 back )
				CancelEventArgs arg = new CancelEventArgs(false);
				OnStepChanging(this, arg);
				if (arg.Cancel) return;
			}
			_currentStep--;
			if (OnStepChanged != null) OnStepChanged(this, null);
			EnsureButtonsState();
		}

		protected virtual void _Next_button_Click(object sender, EventArgs e)
		{
			if (OnStepChanging != null)
			{
				// 用 Handled 属性来传当前按钮的方向 ( true 为 next )
				CancelEventArgs arg = new CancelEventArgs(true);
				OnStepChanging(this, arg);
				if (arg.Cancel) return;
			}
			_currentStep++;
			if (OnStepChanged != null) OnStepChanged(this, null);
			EnsureButtonsState();
		}

		protected virtual void _Finish_button_Click(object sender, EventArgs e)
		{
			if (OnFinished != null) OnFinished(this, null);
		}



		void FWizardBase_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (OnCanceling != null)
			{
				CancelEventArgs arg = new CancelEventArgs(false);
				OnCanceling(this, arg);
				e.Cancel = arg.Cancel;
			}
		}

		#endregion

		#region Utils Methods

		/// <summary>
		/// 载入每一步的控件
		/// </summary>
		protected virtual void LoadStepControl(CWizardTitleBase title, CWizardContentBase content)
		{
			title.Dock = DockStyle.Fill;
			TitlePanel.SuspendLayout();
			TitlePanel.Controls.Clear();
			TitlePanel.Controls.Add(title);
			TitlePanel.ResumeLayout(false);

			content.Dock = DockStyle.Fill;
			ContentPanel.SuspendLayout();
			ContentPanel.Controls.Clear();
			ContentPanel.Controls.Add(content);
			ContentPanel.ResumeLayout(false);
		}

		/// <summary>
		/// 确认设定按钮控件状态
		/// </summary>
		protected virtual void EnsureButtonsState()
		{
			_Back_button.Enabled = _Next_button.Enabled = true;
			_Finish_button.Enabled = false;

			if (_currentStep == 1) _Back_button.Enabled = false;

			if (_currentStep == _maxStep)
			{
				_Next_button.Enabled = false;
				_Finish_button.Enabled = true;
			}
		}

		#endregion
	}

}
