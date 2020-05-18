using CitiesHarmony.API;
using ColossalFramework;
using ICities;
using System;
using System.IO;

namespace TransportVehicleReturnPatch
{
    public class TransportVehicleReturnPatch : IUserMod
    {
        public static ushort[] rejectPassengerVehicleID = new ushort[255];
        public static bool IsEnabled = false;
        public static bool debugMode = false;

        public string Name
        {
            get { return "Transport Vehicle Return Patch"; }
        }

        public string Description
        {
            get { return "Transport vehicle can not return if there are passergers inside"; }
        }

        public void OnEnabled()
        {
            IsEnabled = true;
            FileStream fs = File.Create("TransportVehicleReturnPatch.txt");
            fs.Close();
            HarmonyHelper.EnsureHarmonyInstalled();
        }

        public void OnDisabled()
        {
            IsEnabled = false;
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("Debug Mode");
            group.AddCheckbox("Debug Mode", debugMode, (index) => debugModeEnable(index));
        }

        public void debugModeEnable(bool index)
        {
            debugMode = index;
        }

        public static int GetInsidePassengerCount(ref Vehicle data)
        {
            CitizenManager instance = Singleton<CitizenManager>.instance;
            uint num = data.m_citizenUnits;
            int num2 = 0;
            int passengerCount = 0;
            while (num != 0u)
            {
                uint nextUnit = instance.m_units.m_buffer[(int)((UIntPtr)num)].m_nextUnit;
                for (int i = 0; i < 5; i++)
                {
                    uint citizen = instance.m_units.m_buffer[(int)((UIntPtr)num)].GetCitizen(i);
                    if (citizen != 0u)
                    {
                        passengerCount++;
                    }
                }
                num = nextUnit;
                if (++num2 > 524288)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                    break;
                }
            }
            return passengerCount;
        }
    }
}