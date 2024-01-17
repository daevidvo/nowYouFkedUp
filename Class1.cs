using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using nowYouFkedUp.Patches;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace nowYouFkedUp
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class modBase : BaseUnityPlugin
    {
        private const string modGUID = "daevidLethalMod";
        private const string modName = "Now You Fked Up";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static modBase Instance;

        internal ManualLogSource log;

        internal static AudioClip sfxClip;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            log.LogInfo("daevid's mod is loading");

            string location = ((BaseUnityPlugin)Instance).Info.Location;
            string dllName = "nowYouFkedUp.dll";
            string dir = location.TrimEnd(dllName.ToCharArray());
            string bundle = dir + "nowyoufkedup";
            AssetBundle val = AssetBundle.LoadFromFile(bundle);

            if (val == null)
            {
                log.LogError("failed to load sound");
                return;
            }

            sfxClip = val.LoadAsset<AudioClip>("Assets/now_you_fucked_up.wav");
            harmony.PatchAll(typeof(modBase));
            harmony.PatchAll(typeof(FlowermanPatch));
            log.LogInfo("daevid mod sounds loaded");
        }
    }
}

namespace nowYouFkedUp.Patches
{
    [HarmonyPatch(typeof(FlowermanAI))]
    internal class FlowermanPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void flowermanAudioPatch(FlowermanAI __instance)
        {

            __instance.crackNeckSFX = modBase.sfxClip;
        }
    }
}
