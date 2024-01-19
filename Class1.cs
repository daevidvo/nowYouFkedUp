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

            string str = "Assets/nowYouFuckedUp1.wav"; 

            sfxClip = val.LoadAsset<AudioClip>(str);

            harmony.PatchAll(typeof(modBase));
            harmony.PatchAll(typeof(springManPatch));

            log.LogInfo("daevid mod sounds loaded");
        }
    }
}

namespace nowYouFkedUp.Patches
{
    [HarmonyPatch(typeof(SpringManAI))]
    internal class springManPatch 
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void springManAudioPatch(SpringManAI __instance)
        {
            // TODO: make sfxClipArray to replace more than one springNoise
            __instance.springNoises[0] = modBase.sfxClip;
        }
    }
}
