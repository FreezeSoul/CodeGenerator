using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator
{
	public class WaitCursor : IDisposable
	{
		Form _f;
		Cursor _c;

		public WaitCursor(Form f)
		{
			_f = f;
			_c = _f.Cursor;
			_f.Cursor = Cursors.WaitCursor;
		}
		public WaitCursor(Control c)
			: this(c.FindForm())
		{
		}

		#region IDisposable Members

		public void Dispose()
		{
			_f.Cursor = _c;
			_c = null;
			_f = null;
		}

		#endregion
	}
}
