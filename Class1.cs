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

        internal static AudioClip sfxClip1;
        internal static AudioClip sfxClip2;
        internal static AudioClip sfxClip3;
        internal static AudioClip sfxClip4;
        internal static AudioClip sfxClip5;
        internal static AudioClip[] sfxClipArray = { sfxClip1, sfxClip2, sfxClip3, sfxClip4, sfxClip5 };

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

            for (int i= 0; i< sfxClipArray.Length; i+=1)
            {
                string str = String.Format("Assets/nowYouFuckedUp{0}.wav", i+1);

                sfxClipArray[i] = val.LoadAsset<AudioClip>(str);

                log.LogInfo(str);
                log.LogInfo(sfxClipArray.Length);
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
            for (int i = 0; i < modBase.sfxClipArray.Length; i+=1)
            {
                __instance.springNoises[i] = modBase.sfxClipArray[i];
            }
        }
    }
}
