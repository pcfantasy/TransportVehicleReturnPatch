using ColossalFramework;
using HarmonyLib;
using System.Reflection;

namespace TransportVehicleReturnPatch.Patch
{
    [HarmonyPatch]
    public static class TransportLineGetActiveVehiclePatch
	{
        public static MethodBase TargetMethod()
        {
            return typeof(TransportLine).GetMethod("GetActiveVehicle", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        public static void Postfix(ref ushort __result)
        {
			if (__result != 0)
			{
				var data = Singleton<VehicleManager>.instance.m_vehicles.m_buffer[__result];
				data.Info.m_vehicleAI.GetBufferStatus(__result, ref Singleton<VehicleManager>.instance.m_vehicles.m_buffer[__result], out string _, out int passengerCount, out int _);
				if (data.m_transportLine != 0)
				{
					if (passengerCount != 0)
					{
						if (TransportVehicleReturnPatch.debugMode)
							DebugLog.LogToFileOnly($"Reject return to depot PassengerCount = {passengerCount}, data.m_transportLine = {data.m_transportLine}");
						if (TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine] == 0)
						{
							TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine] = __result;
						}
						else
						{
							var tmpData = Singleton<VehicleManager>.instance.m_vehicles.m_buffer[TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine]];
							if (tmpData.m_transportLine != data.m_transportLine)
							{
								if (TransportVehicleReturnPatch.debugMode)
									DebugLog.LogToFileOnly($"Error in TransportLineGetActiveVehiclePatch: tmpData.m_transportLine = {tmpData.m_transportLine} data.m_transportLine = {data.m_transportLine}");
								TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine] = 0;
							}
						}
						__result = 0;
					}
				}
			}
        }
	}
}
