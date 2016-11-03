using System;
using System.Collections.Generic;
using System.Linq;

using CommNet;

//TODO: Cache map of vessel - non-null commNode at startup
//TODO: A method for finding neighbour nodes

namespace commnetpeek
{
    public class CNPUtils
    {
        public static string neatDistance(double distanceCost)
        {
            if (distanceCost > Math.Pow(10, 9))
                return string.Format("{0:0.##} Gm", distanceCost / Math.Pow(10, 9));
            else if (distanceCost > Math.Pow(10, 6))
                return string.Format("{0:0.##} Mm", distanceCost / Math.Pow(10, 6));
            else if (distanceCost > Math.Pow(10, 3))
                return string.Format("{0:0.##} km", distanceCost / Math.Pow(10, 3));
            else
                return string.Format("{0:0.##} m", distanceCost);
        }

        public static string neatSignalStrength(double rawStrength)
        {
            return Math.Ceiling(rawStrength * 100).ToString();
        }

        public static string neatVesselName(string name)
        {
            if (name.LastIndexOf(':') >= 0) // eg KCS:MonkeyIsland
                return name.Substring(name.LastIndexOf(':')+1);
            else // eg VesselName (unloaded)
                return name.Substring(0, name.LastIndexOf('('));
        }

        public static Vessel findCorrespondingVessel(CommNode commNodeRef)
        {
            List<Vessel> allVessels = FlightGlobals.Vessels;

            //brute-force search temporarily until I find a \omega(n) method
            for(int i=0; i< allVessels.Count(); i++)
            {
                Vessel thisVessel = allVessels.ElementAt(i);
                if (thisVessel.connection != null)
                {
                    if (thisVessel.connection.Comm.precisePosition.x == commNodeRef.precisePosition.x &&
                        thisVessel.connection.Comm.precisePosition.y == commNodeRef.precisePosition.y &&
                        thisVessel.connection.Comm.precisePosition.z == commNodeRef.precisePosition.z)
                        return thisVessel;
                }
            }

            //not found
            return null;
        }
    }
}
