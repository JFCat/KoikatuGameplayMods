﻿using System;
using System.ComponentModel;
using BepInEx;
using Harmony;
using KKABMX.Core;
using KKAPI;
using KKAPI.Chara;
using KKAPI.MainGame;

namespace KK_Pregnancy
{
    [BepInPlugin(GUID, GUID, Version)]
    [BepInDependency(KKABMX_Core.GUID)]
    [BepInDependency(KoikatuAPI.GUID)]
    public partial class PregnancyPlugin : BaseUnityPlugin
    {
        public const string GUID = "KK_Pregnancy";
        internal const string Version = "1.1.1";

        [DisplayName("Enable conception")]
        [Description("If disabled no new characters will be able to get pregnant. Doesn't affect already pregnant characters.")]
        public static ConfigWrapper<bool> ConceptionEnabled { get; private set; }

        [DisplayName("Show pregnancy icon early")]
        [Description("By default pregnancy status icon in class roster is shown after a few days (the girl had a chance to do the test). " +
                     "Turning this on will make the icon show up at the end of the current day.")]
        public static ConfigWrapper<bool> ShowPregnancyIconEarly { get; private set; }

        [DisplayName("Pregnancy progression speed")]
        [Description("How much faster does the in-game pregnancy progresses than the standard 40 weeks. " +
                     "It also reduces the time characters leave school for after birth.\n\n" +
                     "x1 is 40 weeks, x2 is 20 weeks, x4 is 10 weeks, x10 is 4 weeks.")]
        [AcceptableValueList(new object[] { 1, 2, 4, 10 })]
        public static ConfigWrapper<int> PregnancyProgressionSpeed { get; private set; }

        private void Start()
        {
            if (!KoikatuAPI.CheckRequiredPlugin(this, KoikatuAPI.GUID, new Version("1.4"))) return;
            if (!KoikatuAPI.CheckRequiredPlugin(this, KKABMX_Core.GUID, new Version("3.2.3"))) return;

            PregnancyProgressionSpeed = new ConfigWrapper<int>(nameof(PregnancyProgressionSpeed), this, 4);
            ConceptionEnabled = new ConfigWrapper<bool>(nameof(ConceptionEnabled), this, true);
            ShowPregnancyIconEarly = new ConfigWrapper<bool>(nameof(ShowPregnancyIconEarly), this, false);

            CharacterApi.RegisterExtraBehaviour<PregnancyCharaController>(GUID);
            GameAPI.RegisterExtraBehaviour<PregnancyGameController>(GUID);

            var hi = HarmonyInstance.Create(GUID);
            Hooks.InitHooks(hi);
            PregnancyGui.Init(hi, this);
        }
    }
}