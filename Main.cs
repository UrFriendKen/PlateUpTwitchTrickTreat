using KitchenMods;
using PreferenceSystem;
using System.IO;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenTwitchTrickTreat
{
    public class Main : IModInitializer
    {
        public const string MOD_GUID = $"IcedMilo.PlateUp.{MOD_NAME}";
        public const string MOD_NAME = "TwitchTrickTreat";
        public const string MOD_VERSION = "0.1.0";

        PreferenceSystemManager PrefManager;

        private string Chef2Path => Path.Combine(Application.dataPath, "Chef2.exe");

        public void PostActivate(KitchenMods.Mod mod)
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            PrefManager = new PreferenceSystemManager(MOD_GUID, MOD_NAME);
            PrefManager
                .AddConditionalBlocker(() => !Chef2Exists())
                    .AddButton("Open Chef (Halloween)", delegate (int _)
                    {
                        if (System.Diagnostics.Process.GetProcessesByName("Chef.exe").Length == 0)
                        {
                            System.Diagnostics.Process.Start(Chef2Path);
                        }
                    })
                .ConditionalBlockerDone()
                .AddConditionalBlocker(Chef2Exists)
                    .AddInfo("Chef2.exe not found!")
                .ConditionalBlockerDone()
                .AddSpacer()
                .AddSpacer();
            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.MainMenu);
            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.PauseMenu);
        }

        private bool Chef2Exists()
        {
            return File.Exists(Chef2Path);
        }

        public void PreInject()
        {
        }

        public void PostInject()
        {
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
