using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using NineSolsAPI;

namespace DashRest;

[BepInDependency(NineSolsAPICore.PluginGUID)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class DashRest : BaseUnityPlugin {
    private ConfigEntry<float> _dashCooldown = null!;
    private Harmony _harmony = null!;

    private void Awake() {
        Log.Init(Logger);

        RCGLifeCycle.DontDestroyForever(gameObject);

        _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        _harmony.PatchAll();

        _dashCooldown = Config.Bind("Settings", "Dash Cooldown", 1.0f, "Cooldown time for dashing.");
        _dashCooldown.SettingChanged += (_, _) => SetDashCooldown(_dashCooldown.Value);

        Log.Info($"Plugin {PluginInfo.PLUGIN_GUID} loaded successfully.");
        SetDashCooldown(_dashCooldown.Value);
    }

    private void SetDashCooldown(float cooldownTime) {
        if (SaveManager.Instance == null) {
            Log.Warning("SaveManager.Instance is null; cannot set dash cooldown.");
            return;
        }

        var statData = SaveManager.Instance.allStatData.GetStat("RollCoolDown 閃避CD");
        if (statData != null) {
            Traverse.Create(statData.Stat).Field("_value").SetValue(cooldownTime);
            Log.Info($"Dash cooldown set to {cooldownTime} seconds.");
        } else {
            Log.Warning("RollCoolDown 閃避CD stat not found; cannot set dash cooldown.");
        }
    }

    private void OnDestroy() {
        _harmony?.UnpatchSelf();
        Log.Info("Plugin unpatched and destroyed.");
    }
}
