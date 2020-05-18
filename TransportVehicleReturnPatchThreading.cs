using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using TransportVehicleReturnPatch.Util;

namespace TransportVehicleReturnPatch
{
    public class TransportVehicleReturnPatchThreading : ThreadingExtensionBase
    {
        public static bool isFirstTime = true;
        public const int HarmonyPatchNum = 12;

        public override void OnBeforeSimulationFrame()
        {
            base.OnBeforeSimulationFrame();
            if (Loader.CurrentLoadMode == LoadMode.LoadGame || Loader.CurrentLoadMode == LoadMode.NewGame)
            {
                if (TransportVehicleReturnPatch.IsEnabled)
                {
                    CheckDetour();
                }
            }
        }

        public void CheckDetour()
        {
            if (isFirstTime && Loader.DetourInited)
            {
                isFirstTime = false;
                DebugLog.LogToFileOnly("ThreadingExtension.OnBeforeSimulationFrame: First frame detected. Checking detours.");

                if (!Loader.HarmonyDetourInited)
                {
                    string error = "TransportVehicleReturnPatch HarmonyDetourInit is failed, Send TransportVehicleReturnPatch.txt to Author.";
                    DebugLog.LogToFileOnly(error);
                    UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Incompatibility Issue", error, true);
                }
                else
                {
                    var harmony = new Harmony(HarmonyDetours.ID);
                    var methods = harmony.GetPatchedMethods();
                    int i = 0;
                    foreach (var method in methods)
                    {
                        var info = Harmony.GetPatchInfo(method);
                        if (info.Owners?.Contains(harmony.Id) == true)
                        {
                            DebugLog.LogToFileOnly($"Harmony patch method = {method.FullDescription()}");
                            if (info.Prefixes.Count != 0)
                            {
                                DebugLog.LogToFileOnly("Harmony patch method has PreFix");
                            }
                            if (info.Postfixes.Count != 0)
                            {
                                DebugLog.LogToFileOnly("Harmony patch method has PostFix");
                            }
                            i++;
                        }
                    }

                    if (i != HarmonyPatchNum)
                    {
                        string error = $"TransportVehicleReturnPatch HarmonyDetour Patch Num is {i}, Right Num is {HarmonyPatchNum} Send TransportVehicleReturnPatch.txt to Author.";
                        DebugLog.LogToFileOnly(error);
                        UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Incompatibility Issue", error, true);
                    }
                }
            }
        }
    }
}
