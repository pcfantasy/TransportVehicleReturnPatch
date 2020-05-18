using HarmonyLib;
using System;
using System.Reflection;

namespace TransportVehicleReturnPatch.Patch
{
    [HarmonyPatch]
    public static class TransportManagerReleaseLineImplementationPatch
    {
        public static MethodBase TargetMethod()
        {
            return typeof(TransportManager).GetMethod("ReleaseLineImplementation", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(ushort), typeof(TransportLine).MakeByRefType() }, null);
        }
        public static void Prefix(ushort lineID)
        {
            TransportVehicleReturnPatch.rejectPassengerVehicleID[lineID] = 0;
        }
    }
}