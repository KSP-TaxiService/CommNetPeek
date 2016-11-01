using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace commnetpeek
{
    public class SimpleOutputDialog : UI.AbstractDebugDialog
    {
        private string briefMessage;
        private Queue<string> messages;

        //Quick string additions
        public static readonly string stringSeparator = new String('-', 30);
        public static readonly string stringNewline = "\n";
        public static readonly string stringTab = "\t";

        public SimpleOutputDialog(string title, string briefMessage) : base(title, 
                                                                            0.2f,                        //x
                                                                            0.5f,                        //y
                                                                            (int)(Screen.width*0.3),     //width
                                                                            (int)(Screen.height* 0.4))   //height
        {
            this.briefMessage = briefMessage;
            messages = new Queue<string>();
        }

        protected override List<DialogGUIBase> drawContentComponents()
        {
            List<DialogGUIBase> listComponments = new List<DialogGUIBase>();

            //Label
            listComponments.Add(new DialogGUIHorizontalLayout(true, false, 0, new RectOffset(), TextAnchor.UpperCenter, new DialogGUIBase[] { new DialogGUILabel(this. briefMessage, false, false) }));

            //Message labels
            List<DialogGUIHorizontalLayout> scrollContentList = new List<DialogGUIHorizontalLayout>();
            while(messages.Count > 0)
            {
                DialogGUILabel messageLabel = new DialogGUILabel(messages.Dequeue(), false, false);
                DialogGUIHorizontalLayout lineGroup = new DialogGUIHorizontalLayout(true, false, 4, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUIBase[] { messageLabel });
                scrollContentList.Add(lineGroup);
            }

            //Scroll box for message labels
            DialogGUIBase[] scrollList = new DialogGUIBase[scrollContentList.Count];
            for (int i = 0; i < scrollContentList.Count; i++)
                scrollList[i] = scrollContentList[i];

            listComponments.Add(new DialogGUIScrollList(Vector2.one, false, true, new DialogGUIVerticalLayout(10, 100, 4, new RectOffset(6, 24, 10, 10), TextAnchor.UpperLeft, scrollList)));

            return listComponments;
        }

        protected override bool runIntenseInfo(Vessel thisVessel, Part commandPart)
        {
            //Connection info
            CommNet.CommNetVessel commVesselInfo = thisVessel.connection;
            messages.Enqueue(stringTitle("Connection"));
            messages.Enqueue(string.Format("Control status: {0} (signal %{1})", commVesselInfo.ControlState, Math.Ceiling(commVesselInfo.SignalStrength*100)));
            messages.Enqueue(string.Format("Vessel status: {0}", thisVessel.situation));
            messages.Enqueue(string.Format("Signal delay (s): {0}", commVesselInfo.SignalDelay));

            CommNet.CommPath path = commVesselInfo.ControlPath;
            string[] nodes = path.ToString().Split(';');

            messages.Enqueue(string.Format("{0}Signal path: {1} {2}", 12345787, stringNewline, path.ToString()));
            messages.Enqueue(stringTab + "1) YOU @ Kerbin");
            messages.Enqueue(stringTab + "2) RELAY - VESSEL8 @ Kerbin (signal %100)");
            messages.Enqueue(stringTab + "3) PILOT - VESSEL9 @ Mun (signal %100)");

            messages.Enqueue(string.Format("{0}Nearest neighbours (excluded the one in the signal path): {1} node(s)", stringNewline, 3));
            messages.Enqueue(stringTab + "1) 13k to VESSEL1 @ Kerbin (signal %2)");
            messages.Enqueue(stringTab + "2) 2Mm to VESSEL2 @ Mun (signal %100)");
            messages.Enqueue(stringTab + "3) 13Mm to VESSEL3 @ Minus (signal %100)");

            messages.Enqueue(stringTitle("RemoteTech"));
            messages.Enqueue("WIP");

            return true;
        }

        private string stringTitle(string title)
        {
            return stringSeparator + stringNewline +
                   "- " + title + stringNewline + 
                   stringSeparator;
        }
    }
}
