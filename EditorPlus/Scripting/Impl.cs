using Jint.Native;
using Jint.Native.Function;
using JSNet.API;
using JSNet.Utils;
using System.Collections.Generic;
using System.Linq;

namespace EditorPlus.Scripting
{
    public static class Impl
    {
        public static Dictionary<string, FIWrapper> registeredFunctions = new Dictionary<string, FIWrapper>();
        [Api("registerFunction", Comment = new string[]
        {
            "Register JS Function At Editor Input Field"
        })]
        public static void RegisterFunction(string name, JsValue func)
        {
            if (!(func is FunctionInstance fi)) return;
            var wrapper = new FIWrapper(fi);
            registeredFunctions[name] = wrapper;
            string argsString = string.Empty;
            if (wrapper.args.Any())
            {
                argsString = wrapper.args.Aggregate("", (c, n) => $"{c}{n}, ");
                argsString = argsString.Remove(argsString.Length - 2, 2);
            }
            Main.JSApi.RegisterMethod(wrapper.Wrap(), new ApiAttribute(name));
            Main.Logger.Log($"Registered \"{name}({argsString})\" Function Successfully.");
        }
    }
}
