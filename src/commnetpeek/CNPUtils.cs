using System;
using System.Collections.Generic;
using System.Linq;

using CommNet;

//TODO: Cache map of vessel - non-null commNode at startup

namespace commnetpeek
{
    public class CNPUtils
    {
        public static string neatDistance(double distanceCost)
        {
            if (distanceCost > Math.Pow(10, 9))
                return string.Format("{0:0.00} Gm", distanceCost / Math.Pow(10, 9));
            else if (distanceCost > Math.Pow(10, 6))
                return string.Format("{0:0.00} Mm", distanceCost / Math.Pow(10, 6));
            else if (distanceCost > Math.Pow(10, 3))
                return string.Format("{0:0.00} km", distanceCost / Math.Pow(10, 3));
            else
                return string.Format("{0:0.00} m", distanceCost);
        }

        public static string neatSignalStrength(double rawStrength)
        {
            return Math.Ceiling(rawStrength * 100).ToString();
        }

        public static string neatVesselName(string name)
        {
            if (name.LastIndexOf(':') >= 0) // eg KCS: MonkeyIsland
                return name.Substring(name.LastIndexOf(':')+1).Trim();
            else // eg VesselName (unloaded)
                return name.Substring(0, name.LastIndexOf('(')).Trim();
        }

        public static Vessel findCorrespondingVessel(CommNode commNodeRef)
        {
            List<Vessel> allVessels = FlightGlobals.Vessels;
            IEqualityComparer<CommNode> comparer = commNodeRef.Comparer;

            //brute-force search temporarily until I find a \omega(n) method
            for(int i=0; i< allVessels.Count(); i++)
            {
                Vessel thisVessel = allVessels.ElementAt(i);
                if (thisVessel.connection != null)
                {
                    if(comparer.Equals(commNodeRef, thisVessel.connection.Comm))
                        return thisVessel;
                }
            }

            //not found
            return null;
        }

        public static List<CommLink> findNeighbourLinks(Vessel sourceVessel)
        {
            List<CommLink> allCommLinks = CommNetNetwork.Instance.CommNet.Links;
            IEqualityComparer<CommNode> comparer = sourceVessel.connection.Comm.Comparer;
            List<CommLink> neigbourLinks = new List<CommLink>();

            for (int i = 0; i < allCommLinks.Count(); i++)
            {
                CommLink thisLink = allCommLinks.ElementAt(i);
                if(thisLink.Contains(sourceVessel.connection.Comm))
                //if (comparer.Equals(thisLink.a, sourceVessel.connection.Comm) || comparer.Equals(thisLink.b, sourceVessel.connection.Comm))
                    neigbourLinks.Add(thisLink);
            }

            return neigbourLinks;
        }
    }
}
