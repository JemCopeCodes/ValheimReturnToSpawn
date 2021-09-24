using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using HarmonyLib;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using UnityEngine;

namespace ReturnToSpawn
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInProcess("valheim.exe")]
    public class ReturnToSpawn : BaseUnityPlugin
    {
        public const string PluginGUID = "JemCopeCodes.ValheimMod.comfytools.ReturnToSpawn";
        public const string PluginName = "ReturnToSpawn";
        public const string PluginVersion = "1.0.0";


        static ConfigEntry<bool> _isModEnabled;

        Harmony _harmony;

        void Awake()
        {
           
            _isModEnabled = Config.Bind("_Global", "isModEnabled", true, "Enable or disable this mod.");
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
        }


        [HarmonyPatch(typeof(Chat))]
        class ChatPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Chat.InputText))]
            static bool InputTextPrefix(ref Chat __instance)
            {
                if (_isModEnabled.Value && ParseText(__instance, __instance.m_input.text))
                {
                    return false;
                }

                return true;
            }
        }

        public void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        static bool ParseText(MonoBehaviour component, string text)
        {
            if (text.StartsWith("/spawn"))
            {
                Debug.Log($"Spawn command received!");
                return true;
            }
            return false;
        }
    }
}
