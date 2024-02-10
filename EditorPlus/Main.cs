using EditorPlus.Scripting;
using JSNet.API;
using Tweaks;
using static UnityModManagerNet.UnityModManager;

namespace EditorPlus
{
    public static class Main
    {
        public static ModEntry Mod {  get; private set; }
        public static ModEntry.ModLogger Logger => Mod.Logger;
        public static Api JSApi { get; private set; } = new Api();
        public static void Load(ModEntry modEntry)
        {
            JSApi.RegisterType(typeof(Impl));
            Runner.Run(Mod = modEntry);
        }
    }
}
