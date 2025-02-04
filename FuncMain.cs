using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using config.export.excel;
using GS;
using GS.Event;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using System;

namespace XxjzFunc
{
    [BepInPlugin("XxjzFuncMod", "XxjzFunc", "1.0.0")]
    public class FuncMain : BasePlugin
    {
        private static ConfigEntry<int> 境界数;
        private static Harmony harmony;
        public override void Load()
        {
            FuncMain.境界数 = base.Config.Bind<int>("XxjzFunc", "境界数", 71, "境界数量");
            FuncMain.harmony = Harmony.CreateAndPatchAll(typeof(FuncMain), null);
            base.Log.LogError("FuncMain OK!");
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GSActor), "ExecFunc")]
        public static bool Ascension(ref int funcType, ref int param1, ref int param2, ref int param3, ref EventNode context, ref GSActor __instance)
        {
            Console.WriteLine($"{funcType}---{param1}---{param2}---{param3}--");
            switch (funcType)
            {
                case 64: //改年龄
                    {
                        Funccase.ChangeAge(__instance, param1, param2, param3);
                        return false;
                    }
                case 65: //改血脉
                    {
                        __instance.ChangeBloods(param1, param2);
                        return false;
                    }
                case 66: //改境界
                    {
                        if (param1 == 0)
                        {
                            __instance.ChangeRealmEffect(param2);
                        }
                        if (param1 == 1)
                        {
                            __instance.ChangeRealmEffect(-param2);
                        }
                        return false;
                    }
                case 67: //通过血脉获得丹药
                    {
                        Funccase.ByAttGetItem(__instance, param1, param2, param3, context);
                        return false;
                    }
                case 68: //获得天赋
                    {
                        RoleTalent roleTalent = RoleTalentSelectHelper.AddRandomTalent(__instance, param1);
                        if (roleTalent != null)
                        {
                            __instance.AddTalent(roleTalent, true);
                        }
                        return false;
                    }
                case 69: //获得血脉天赋
                    {
                        RoleTalent roleTalent = RoleTalent.Find(param1);
                        if (roleTalent != null)
                        {
                            __instance.AddBloodTalent(roleTalent, true);
                        }
                        return false;
                    }
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RoleState), "InitRealms")]
        public static bool InitRealmsMod()
        {
            Console.WriteLine("--InitRealms------------------------------------");
            int valuecount = FuncMain.境界数.Value;
            bool flag = RoleState.realms == null;
            if (flag)
            {
                RoleState.realms = new RoleState[valuecount];
                foreach (KeyValuePair<int, RoleState> keyValuePair in RoleState.configs)
                {
                    bool flag2 = keyValuePair.Key >= 0 && keyValuePair.Key < valuecount;
                    if (flag2)
                    {
                        RoleState.realms[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
            }
            return false;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoleState), "GetRealm")]
        public static void GetRealm(ref int realm, ref RoleState __result)
        {
            int valuecount = FuncMain.境界数.Value;
            if (realm > 70)
            {
                Console.WriteLine(realm + "--GetRealm--70+---------------------------------");
                bool flag = RoleState.realms == null;
                if (flag)
                {
                    RoleState.InitRealms();
                }
                __result = (realm < valuecount) ? RoleState.realms[realm] : null;
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GSItemMgr), "UseItem")]
        public static void UseItem(ref bool __result, ref GSItem item, ref GSActor p, ref GSItemMgr __instance)
        {
            if (__result && item.proto.createSkillId < 7000000)
            {
                Console.WriteLine(item.proto.createSkillId + "EventID");
                EventTmpl eventTmpl = EventTmpl.Find(item.proto.createSkillId);
                if (eventTmpl == null)
                {
                    Console.WriteLine("Not Find EventID");
                }
                else
                {
                    EventNode context = EventNode.Create(eventTmpl, EventNode.AquireFromPool());
                    int[] execFunc = eventTmpl.execFunc;
                    bool flag13 = execFunc != null && execFunc.Length != 0;
                    if (flag13)
                    {
                        int j = 0;
                        int num6 = execFunc.Length;
                        while (j < num6)
                        {
                            p.ExecFunc(execFunc[j], execFunc[j + 1], execFunc[j + 2], execFunc[j + 3], context);
                            j += 4;
                        }
                    }
                }
            }
        }

    }
}
