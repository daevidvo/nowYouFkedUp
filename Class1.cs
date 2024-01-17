using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            log.LogInfo("test mod");
            
            harmony.PatchAll(typeof(modBase));
        }
    }
}
