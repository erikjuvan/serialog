using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serialog
{
    // List View No Flicker
    // https://stackoverflow.com/questions/442817/c-sharp-flickering-listview-on-update
    internal class ListViewNF : System.Windows.Forms.ListView
    {
        public ListViewNF()
        {
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

        }  
        
        public event EventHandler<EventArgs> Scrolled;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_MOUSEWHEEL = 0x20a;

            if (m.Msg == WM_MOUSEWHEEL && Scrolled != null)
            {
                Scrolled(this, new EventArgs());
            }
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }
}
