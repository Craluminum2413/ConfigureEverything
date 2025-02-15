using HarmonyLib;
using Vintagestory.API.Common;

namespace ConfigureEverything.HarmonyPatches;

public class HarmonyPatches : ModSystem
{
    public string HarmonyID => Mod.Info.ModID;
    public Harmony HarmonyInstance => new(HarmonyID);


    // 11th November 2024, 22:33:44
    // Waiting for Tyron to add these values back:
    // EntityBehaviorControlledPhysics.climbUpSpeed
    // EntityBehaviorControlledPhysics.climbDownSpeed
    // Without those values, ConfigClimbingSpeed is absolutely useless


    //public static ConfigClimbingSpeed ConfigClimbingSpeed { get; private set; }
    public static ConfigSwimmingSpeed ConfigSwimmingSpeed { get; private set; }

    public override void Dispose()
    {
        HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        PatchAll(api);
    }

    private void PatchAll(ICoreAPI api)
    {
        if (api.Side.IsServer())
        {
            //ConfigClimbingSpeed = ModConfig.ReadConfig<ConfigClimbingSpeed>(api, $"ConfigureEverything/{api.Side}/ClimbingSpeed.json");
            ConfigSwimmingSpeed = ModConfig.ReadConfig<ConfigSwimmingSpeed>(api, $"ConfigureEverything/{api.Side}/SwimmingSpeed.json");

            //if (ConfigClimbingSpeed?.Enabled == true)
            //{
            //    HarmonyInstance.Patch(original: EntityBehaviorControlledPhysics_Patch.TargetMethod(), postfix: EntityBehaviorControlledPhysics_Patch.GetPostfix());
            //}
            if (ConfigSwimmingSpeed?.Enabled == true)
            {
                HarmonyInstance.Patch(original: EntityBoat_SpeedMultiplier_Patch.TargetMethod(), postfix: EntityBoat_SpeedMultiplier_Patch.GetPostfix());
                HarmonyInstance.Patch(original: Entity_GetInfoText_Patch.TargetMethod(), postfix: Entity_GetInfoText_Patch.GetPostfix());
                HarmonyInstance.Patch(original: CollectibleObject_GetHeldItemInfo_Patch.TargetMethod(), postfix: CollectibleObject_GetHeldItemInfo_Patch.GetPostfix());
            }
        }
    }
}