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
    }
}