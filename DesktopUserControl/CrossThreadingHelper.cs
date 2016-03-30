using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopUserControl
{
    public static class CrossThreadingHelper
    {
        public static void InvokeIfRequired(Control control, MethodInvoker method)
        {
            try
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(method);
                }
                else
                {
                    method();
                }
            }
            catch { }
        }
    }
}
