using System.Text;
using HarmonyLib;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace ConfigureEverything.HarmonyPatches;

public static class Entity_Info_Patch
{
    [HarmonyPatch(typeof(Entity), nameof(Entity.GetInfoText))]
    public static void Postfix(ref string __result, Entity __instance)
    {
        if (__instance is EntityBoat && HarmonyPatches.ConfigSwimmingSpeed.SpeedMultiplier.ContainsKey(__instance.Code.ToString()))
        {
            StringBuilder dsc = new(__result);
            dsc.Append(Lang.Get("walk-multiplier") + HarmonyPatches.ConfigSwimmingSpeed.SpeedMultiplier.GetValueSafe(__instance.Code.ToString()));
            __result = dsc.ToString();
        }
    }
}
