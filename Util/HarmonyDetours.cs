using HarmonyLib;

namespace TransportVehicleReturnPatch.Util
{
    internal static class HarmonyDetours
    {
        public const string ID = "pcfantasy.TransportVehicleReturnPatch";
        public static void Apply()
        {
            var harmony = new Harmony(ID);
            harmony.PatchAll(typeof(HarmonyDetours).Assembly);
            Loader.HarmonyDetourFailed = false;
            DebugLog.LogToFileOnly("Harmony patches applied");
        }

        public static void DeApply()
        {
            var harmony = new Harmony(ID);
            harmony.UnpatchAll(ID);
            DebugLog.LogToFileOnly("Harmony patches DeApplied");
        }
    }
}
