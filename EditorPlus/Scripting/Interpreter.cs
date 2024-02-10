using JSNet;
using System.Collections.Generic;

namespace EditorPlus.Scripting
{
    public static class Interpreter
    {
        private static Dictionary<string, Script> cache = new Dictionary<string, Script>();
        public static object Interpret(string expr)
        {
            if (!cache.TryGetValue(expr, out Script script))
                cache[expr] = script = new Script(Main.JSApi.PrepareInterpreter(), expr);
            return script.Eval();
        }
    }
}
