using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// SMO
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer;

namespace CodeGenerator
{
	public partial class FWizard : CodeGenerator.FWizardBase
	{
		private Database _db;

		public FWizard(Database db)
		{
			InitializeComponent();
			_db = db;
		}

		private void FWizard_Load(object sender, EventArgs e)
		{
			this.OnHelpRequest += new EventHandler(FFilterSchemeWizard_OnHelpRequest);
			this.OnCanceling += new EventHandler<CancelEventArgs>(FFilterSchemeWizard_OnCanceling);
			this.OnStepChanging += new EventHandler<CancelEventArgs>(FFilterSchemeWizard_OnStepChanging);
			this.OnStepChanged += new EventHandler(FFilterSchemeWizard_OnStepChanged);
			this.OnFinished += new EventHandler(FFilterSchemeWizard_OnFinished);

			this.Text = "过滤方案创建向导";
			this.MaxStep = 4;
			this.CurrentStep = 1;
			FFilterSchemeWizard_OnStepChanged(null, null);
		}

		void FFilterSchemeWizard_OnHelpRequest(object sender, EventArgs e)
		{
			// todo: 显示帮助
		}

		void FFilterSchemeWizard_OnCanceling(object sender, CancelEventArgs e)
		{
			// todo: 如果进入到某步 有数据了， show 是否真的退出
		}

		void FFilterSchemeWizard_OnFinished(object sender, EventArgs e)
		{
			// todo: 保存设置，生成代码（可选），关窗，刷新方案列表控件
		}

		void FFilterSchemeWizard_OnStepChanged(object sender, EventArgs e)
		{
			// todo: 载入相应控件，还原或初始化数据
			if (this.CurrentStep == 1)
			{
				LoadStepControl(new CWizardTitleBase("基本设定", "请设置方案名，命名空间等信息。"), new CWizardContentBase());
			}
			else if (this.CurrentStep == 2)
			{
				LoadStepControl(new CWizardTitleBase("表格选择", "请选择需要生成代码的表格。"), new CWizardContentBase());
			}
			else if (this.CurrentStep == 3)
			{
				LoadStepControl(new CWizardTitleBase("视图选择", "请选择需要生成代码的视图。"), new CWizardContentBase());
			}
			else if (this.CurrentStep == 4)
			{
				LoadStepControl(new CWizardTitleBase("存储过程选择", "请选择需要生成代码的存储过程。"), new CWizardContentBase());
			}
			else if (this.CurrentStep == 5)
			{
				LoadStepControl(new CWizardTitleBase("函数选择", "请选择需要生成代码的函数。"), new CWizardContentBase());
			}
		}

		void FFilterSchemeWizard_OnStepChanging(object sender, CancelEventArgs e)
		{
			// todo: 判断是否满足上一步或下一步的条件

			e.Cancel = false;
		}


	}
}
