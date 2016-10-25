using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commnetpeek
{
    //This class is coupled with the commnetpeek_module.cfg
    public class CommNetPeekModule : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveUnfocused = true, guiName = "CommNet Peek", active = true)]
        public void KSPEventPeek()
        {
            DebugWindow debug_window = new DebugWindow();
            debug_window.launch();
        }
    }
}
