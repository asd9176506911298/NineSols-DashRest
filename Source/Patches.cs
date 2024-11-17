using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DashRest;

[HarmonyPatch]
public class Patches {
    [HarmonyPatch(typeof(Player), "ResetJumpAndDash")]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var codes = instructions.ToList();

        if (codes.Count > 37) {
            codes[35].opcode = OpCodes.Nop;
            codes[36].opcode = OpCodes.Nop;
            codes[37].opcode = OpCodes.Nop;
        }

        return codes.AsEnumerable();
    }

}