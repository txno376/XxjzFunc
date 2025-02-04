using config.export.excel;
using GS;
using GS.Event;
using Il2CppSystem.Collections.Generic;
using System;

namespace XxjzFunc
{
    public class Funccase
    {
        public static void ChangeAge(GSActor gSActor, int param1, int param2, int param3) 
        {
            Random random = new Random();
            int num0 = random.Next(param2, param3);
            int value = gSActor.age;
            int cache;
            switch (param1)
            {
                case 0:
                    {
                        cache = (value + num0 > 0) ? (value + num0) : 0;
                        if (cache > 0)
                        {
                            gSActor.age = cache;
                        }
                        else
                        {
                            gSActor.OnDying(6);
                        }
                        break;
                    }
                case 1:
                    {
                        cache = (value + param2 > 0) ? (value + param2) : 0;
                        if (cache > 0)
                        {
                            gSActor.age = cache;
                        }
                        else
                        {
                            gSActor.OnDying(6);
                        }
                        break;
                    }
                case 2:
                    {
                        cache = num0 > 0 ? num0 : 0;
                        if (cache > 0)
                        {
                            gSActor.age = cache;
                        }
                        else
                        {
                            gSActor.OnDying(6);
                        }
                        break;
                    }
                case 3:
                    {
                        cache = param2 > 0 ? param2 : 0;
                        if (cache > 0)
                        {
                            gSActor.age = cache;
                        }
                        else
                        {
                            gSActor.OnDying(6);
                        }
                        break;
                    }
            }
        }

        public static void ByAttGetItem(GSActor gSActor, int param1, int param2, int param3, EventNode context)
        {
            switch (param1) 
            {
                case 0: //通过血脉获取
                    {
                        int bloodid = 0, bloodvalue = 0;
                        foreach (KeyValuePair<int, int> keyValuePair in gSActor.bloods)
                        {
                            bool flag = keyValuePair.Value > bloodvalue;
                            if (flag)
                            {
                                bloodid = keyValuePair.Key;
                                bloodvalue = keyValuePair.Value;
                            }
                        }
                        if (bloodvalue > 0)
                        {
                            int value = bloodvalue - param2 * 10;
                            if (value > 0)
                            {
                                gSActor.bloods[bloodid] = value;
                                int itemid = param3 + bloodid * 10 + param2;
                                gSActor.force.itemMgr.AddNewItem(itemid, 1);
                            }
                            else
                            {
                                gSActor.bloods.Remove(bloodid);
                            }
                        }
                        break;
                    }
                case 1: //通过年龄获取
                    {
                        int value = gSActor.age - param2;
                        if (value > 0)
                        {
                            gSActor.age = value;
                        }
                        else 
                        {
                            gSActor.OnDying(6);
                        }
                        gSActor.force.itemMgr.AddNewItem(param3, 1);
                        break;
                    }
                case 2: //通过境界获取
                    {
                        int value = gSActor.realm - param2;
                        if (value > 0)
                        {
                            gSActor.ChangeRealmEffect(-param2);
                        }
                        else
                        {
                            gSActor.OnDying(6);
                        }
                        gSActor.force.itemMgr.AddNewItem(param3, 1);
                        break;
                    }
                case 3: //通过天赋获取
                    {
                        RoleTalent rtalent = new RoleTalent();
                        int value = 1;
                        foreach (RoleTalent roleTalent in gSActor.talents)
                        {
                            value = roleTalent.id;
                            if (value == context.eventTmpl.fourthValue) 
                            {
                                rtalent = roleTalent;
                            }
                        }
                        if (rtalent != null)
                        {
                            gSActor.DelTalent(rtalent);
                            gSActor.force.itemMgr.AddNewItem(param3, 1);
                        }
                        break;
                    }
            }
        }
    }
}
