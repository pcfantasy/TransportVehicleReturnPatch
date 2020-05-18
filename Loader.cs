using ICities;
using TransportVehicleReturnPatch.Util;
using CitiesHarmony.API;

namespace TransportVehicleReturnPatch
{
    public class Loader : LoadingExtensionBase
    {
        public static LoadMode CurrentLoadMode;
        public static bool DetourInited = false;
        public static bool HarmonyDetourInited = false;
        public static bool HarmonyDetourFailed = true;

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            Loader.CurrentLoadMode = mode;
            if (TransportVehicleReturnPatch.IsEnabled)
            {
                if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame)
                {
                    DebugLog.LogToFileOnly("OnLevelLoaded");
                    for (int i = 0; i < TransportVehicleReturnPatch.rejectPassengerVehicleID.Length; i++)
                    {
                        TransportVehicleReturnPatch.rejectPassengerVehicleID[i] = 0;
                    }
                    InitDetour();
                    HarmonyInitDetour();
                    if (mode == LoadMode.NewGame)
                    {
                        DebugLog.LogToFileOnly("New Game");
                    }
                }
            }
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            if (TransportVehicleReturnPatch.IsEnabled)
            {
                if (CurrentLoadMode == LoadMode.LoadGame || CurrentLoadMode == LoadMode.NewGame)
                {
                    RevertDetour();
                    HarmonyRevertDetour();
                }
            }
        }

        public override void OnReleased()
        {
            base.OnReleased();
        }

        public void HarmonyInitDetour()
        {
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                if (!HarmonyDetourInited)
                {
                    DebugLog.LogToFileOnly("Init harmony detours");
                    HarmonyDetours.Apply();
                    HarmonyDetourInited = true;
                }
            }
        }

        public void HarmonyRevertDetour()
        {
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                if (HarmonyDetourInited)
                {
                    DebugLog.LogToFileOnly("Revert harmony detours");
                    HarmonyDetours.DeApply();
                    HarmonyDetourInited = false;
                    HarmonyDetourFailed = true;
                }
            }
        }

        public void InitDetour()
        {
            DetourInited = true;
        }

        public void RevertDetour()
        {
            DetourInited = false;
        }
    }
}

