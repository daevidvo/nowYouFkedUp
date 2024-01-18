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
        internal static AudioClip[] sfxClipArray;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            log.LogInfo("daevid's mod is loading");

            for (int i = 0; i < 4; i+=1)
            {
                string location = ((BaseUnityPlugin)Instance).Info.Location;
                string dllName = "nowYouFkedUp.dll";
                string dir = location.TrimEnd(dllName.ToCharArray());
                string bundle = String.Format("{0}nowyoufuckedup{1}", dir, i+1);
                AssetBundle val = AssetBundle.LoadFromFile(bundle);

                if (val == null)
                {
                    log.LogError("failed to load sound");
                    return;
                }

                string str = String.Format("nowYouFuckedUp{0}.wav", i+1);

                sfxClip = val.LoadAsset<AudioClip>(str);

                sfxClipArray[i] = sfxClip;
            }


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
            __instance.springNoises = modBase.sfxClipArray;
        }
    }
}
