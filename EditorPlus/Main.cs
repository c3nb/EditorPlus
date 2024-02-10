using HarmonyLib;
using System.Reflection;
using static UnityModManagerNet.UnityModManager;
using static UnityModManagerNet.UnityModManager.ModEntry;

namespace EditorPlus
{
    public static class Main
    {
        public static ModEntry Mod { get; private set; }
        public static ModLogger Logger { get; private set; }
        public static Harmony Harmony { get; private set; }
        public static Assembly Ass { get; private set; }
        public static void Load(ModEntry modEntry)
        {
            Mod = modEntry;
            Logger = modEntry.Logger;
            Ass = Assembly.GetExecutingAssembly();
            modEntry.OnToggle = OnToggle;
        }
        public static bool OnToggle(ModEntry modEntry, bool toggle)
        {
            if (toggle)
            {
                Harmony = new Harmony(modEntry.Info.Id);
                Harmony.PatchAll(Ass);
            }
            else
            {
                Harmony.UnpatchAll(Harmony.Id);
                Harmony = null;
            }
            return true;
        }
    }
}
