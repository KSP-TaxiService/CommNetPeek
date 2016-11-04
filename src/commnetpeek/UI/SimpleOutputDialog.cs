using System;
using System.Collections.Generic;
using System.Linq;

using CommNet;
using UnityEngine;
using UnityEngine.UI;

//TODO: CommLink.signalStrength bug
//TODO: need better way to find out if node has relay or not

namespace commnetpeek
{
    public class SimpleOutputDialog : UI.AbstractDialog
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

            //Label for the brief message
            listComponments.Add(new DialogGUIHorizontalLayout(true, false, 0, new RectOffset(), TextAnchor.UpperCenter, new DialogGUIBase[] { new DialogGUILabel(this. briefMessage, false, false) }));

            //A GUILayout for each message (eg label + textfield + button)
            List<DialogGUIHorizontalLayout> eachMessageGroupList = new List<DialogGUIHorizontalLayout>();
            while(messages.Count > 0)
            {
                DialogGUILabel messageLabel = new DialogGUILabel(messages.Dequeue(), false, false);
                DialogGUIHorizontalLayout lineGroup = new DialogGUIHorizontalLayout(true, false, 4, new RectOffset(), TextAnchor.UpperLeft, new DialogGUIBase[] { messageLabel });
                eachMessageGroupList.Add(lineGroup);
            }

            //Prepare a list container for the message GUILayouts
            DialogGUIBase[] messageLabels = new DialogGUIBase[eachMessageGroupList.Count + 1];
            messageLabels[0] = new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, true);
            for (int i = 0; i < eachMessageGroupList.Count; i++)
                messageLabels[i+1] = eachMessageGroupList[i];

            listComponments.Add(new DialogGUIScrollList(Vector2.one, false, true, new DialogGUIVerticalLayout(10, 100, 4, new RectOffset(5, 15, 0, 0), TextAnchor.UpperLeft, messageLabels)));
            
            return listComponments;
        }

        protected override bool runIntenseInfo(Vessel thisVessel, Part commandPart)
        {
            CommNetVessel commVesselInfo = thisVessel.connection;
            CommNetwork commNet = CommNetNetwork.Instance.CommNet;

            //Connection info
            messages.Enqueue(stringTitle("Connection"));

            //quick glance
            messages.Enqueue(string.Format("Control status: {0} (signal {1}%)", commVesselInfo.ControlState, CNPUtils.neatSignalStrength(commVesselInfo.SignalStrength)));
            messages.Enqueue(string.Format("Vessel status: {0}", thisVessel.situation));
            messages.Enqueue(string.Format("Signal delay (s): {0}", commVesselInfo.SignalDelay));

            processSignalPath(thisVessel, commandPart);

            processNeighbourNodes(thisVessel, commandPart);

            //RemoteTech info
            messages.Enqueue(stringNewline + stringTitle("RemoteTech"));
            messages.Enqueue("Not implemented yet");

            //Stock CommNet's debug
            messages.Enqueue(stringNewline + stringTitle("CommNetwork.CreateDebug()"));
            messages.Enqueue(commNet.CreateDebug());

            return true;
        }

        private string stringTitle(string title)
        {
            return stringSeparator + stringNewline +
                   "- " + title + stringNewline + 
                   stringSeparator;
        }

        private void processSignalPath(Vessel thisVessel, Part commandPart)
        {
            CommNetVessel commVesselInfo = thisVessel.connection;
            CommPath graphPath = commVesselInfo.ControlPath;
            List<CommLink> edges = graphPath.ToList();
            messages.Enqueue(string.Format("{0}Signal path: {1}", stringNewline, edges.Count() == 0 ? "Non-existent" : ""));

            double totalDistanceCost = 0;
            bool firstNodeProcessed = false;
            for (int i = 0; i < edges.Count(); i++)
            {
                CommLink thisEdge = edges.ElementAt(i);
                totalDistanceCost += thisEdge.cost;

                if (!firstNodeProcessed)
                {
                    messages.Enqueue(string.Format("{0}1) PART - {1} @ {2}", stringTab, commandPart.partInfo.title, commVesselInfo.Vessel.mainBody.bodyName));
                    firstNodeProcessed = true;
                }

                CommNode destNode = thisEdge.b;
                Vessel destVessel = CNPUtils.findCorrespondingVessel(destNode);

                string nodeType = neatHopType(thisEdge.hopType);
                string nodeLocation = (destVessel == null)? FlightGlobals.GetHomeBodyName() : destVessel.mainBody.bodyName;
                string nodeName = destNode.name;
                double signalStrength = thisEdge.signalStrength;                

                messages.Enqueue(string.Format("{0}{1}) {2} - {3} @ {4} (signal {5}%)", 
                                                stringTab,
                                                i + 2,
                                                nodeType,
                                                CNPUtils.neatVesselName(nodeName),
                                                nodeLocation,
                                                CNPUtils.neatSignalStrength(signalStrength)));
            }
            messages.Enqueue(string.Format("{0}Distance: {1} (+{2:0.00}s)", stringTab, CNPUtils.neatDistance(totalDistanceCost), totalDistanceCost / 3E+08));
        }

        private void processNeighbourNodes(Vessel thisVessel, Part commandPart)
        {
            List<CommLink> neighbourLinks = CNPUtils.findNeighbourLinks(thisVessel);
            IEqualityComparer<CommNode> comparer = thisVessel.connection.Comm.Comparer;
            messages.Enqueue(string.Format("{0}Neighbour nodes: {1} node(s)", stringNewline, neighbourLinks.Count()));

            neighbourLinks = neighbourLinks.OrderBy(x => x.cost).ToList();

            for(int i=0; i< neighbourLinks.Count(); i++)
            {
                CommLink thisEdge = neighbourLinks.ElementAt(i);
                CommNode neighbourNode = comparer.Equals(thisEdge.a, thisVessel.connection.Comm) ? thisEdge.b : thisEdge.a;
                Vessel destVessel = CNPUtils.findCorrespondingVessel(neighbourNode);

                string neighbourAntType = neatAntennaType(neighbourNode);
                string neighbourLocation = (destVessel == null) ? FlightGlobals.GetHomeBodyName() : destVessel.mainBody.bodyName;
                double linkDistance = thisEdge.cost; // possible stock bug: cost is zero sometimes
                string neighbourName = neighbourNode.name;
                double signalStrength = thisEdge.signalStrength; // possible stock bug: signalStrength mysteriously gives cost instead of [0,1]

                messages.Enqueue(string.Format("{0}{1}) {2} to {3} ({4}) @ {5} (signal {6}%)",
                                                stringTab,
                                                i+1,
                                                CNPUtils.neatDistance(linkDistance),
                                                CNPUtils.neatVesselName(neighbourName),
                                                neighbourAntType,
                                                neighbourLocation,
                                                CNPUtils.neatSignalStrength(signalStrength)));
            }
        }

        private string neatHopType(HopType hopType)
        {
            if (hopType == HopType.ControlPoint)
                return "PILOT";
            else if (hopType == HopType.Home)
                return "GSTATION";
            else
                return "RELAY";
        }

        private string neatAntennaType(CommNode commNodeRef)
        {
            if (commNodeRef.antennaTransmit.power == 0.0)
                return "RELAY";
            else
                return "DIRECT";
        }
    }
}
