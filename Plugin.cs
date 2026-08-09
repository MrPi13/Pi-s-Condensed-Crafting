using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using UnityEngine;
using CUCoreLib.ContentReload;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using CUCoreLib.Saving;
using Newtonsoft.Json.Linq;

namespace PisCondensedCrafting
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency("net.cucorelib", BepInDependency.DependencyFlags.HardDependency)] 
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "MrPi13.PisCondensedCrafting";
        public const string ModName = "Pi's Condensed Crafting";
        public const string ModVersion = "09.08.26";

        internal static new ManualLogSource Logger;
        private readonly Harmony _harmony = new(ModGUID);
        public static Plugin Instance { get; private set; } = null!;

        public void Awake()
        {
            Logger = base.Logger;
            Instance = this;
            _harmony.PatchAll();
            Logger.LogInfo($"Plugin {ModName} is loaded!");
        }
    }
}
