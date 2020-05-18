﻿using ColossalFramework;
using HarmonyLib;
using System;
using System.Reflection;

namespace TransportVehicleReturnPatch.Patch
{
	[HarmonyPatch]
	public static class TrolleybusAILoadPassengersPatch
	{
		public static MethodBase TargetMethod()
		{
			return typeof(TrolleybusAI).GetMethod("LoadPassengers", BindingFlags.NonPublic | BindingFlags.Instance);
		}
		public static bool Prefix(ref TrolleybusAI __instance, ushort vehicleID, ref Vehicle data)
		{
			if (data.m_transportLine != 0)
			{
				if (TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine] == vehicleID)
				{
					int passengerCount = TransportVehicleReturnPatch.GetInsidePassengerCount(ref data);
					if (passengerCount == 0)
					{
						TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine] = 0;
						if (!data.m_flags.IsFlagSet(Vehicle.Flags.GoingBack))
							__instance.SetTransportLine(vehicleID, ref data, 0);
					}
					return false;
				}
				else if (TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine] != 0)
				{
					var tmpData = Singleton<VehicleManager>.instance.m_vehicles.m_buffer[TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine]];
					if (tmpData.m_transportLine != data.m_transportLine)
					{
						if (TransportVehicleReturnPatch.debugMode)
							DebugLog.LogToFileOnly($"Error in TrolleybusAILoadPassengersPatch: tmpData.m_transportLine = {tmpData.m_transportLine} data.m_transportLine = {data.m_transportLine}");
						TransportVehicleReturnPatch.rejectPassengerVehicleID[data.m_transportLine] = 0;
					}
				}
			}
			return true;
		}
	}
}