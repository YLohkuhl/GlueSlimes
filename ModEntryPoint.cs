using MonomiPark.SlimeRancher.Regions;
using SRML;
using SRML.SR;
using SRML.SR.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GlueSlime
{
    public class Main : ModEntryPoint
    {
        public static Texture2D LoadImage(string filename)
        {
            var a = Assembly.GetExecutingAssembly();
            var spriteData = a.GetManifestResourceStream(a.GetName().Name + "." + filename);
            var rawData = new byte[spriteData.Length];
            spriteData.Read(rawData, 0, rawData.Length);
            var tex = new Texture2D(1, 1);
            tex.LoadImage(rawData);
            tex.filterMode = FilterMode.Bilinear;
            return tex;
        }
        public static Sprite CreateSprite(Texture2D texture) => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);

        // Called before GameContext.Awake
        // You want to register new things and enum values here, as well as do all your harmony patching
        public override void PreLoad()
        {
            //-- PRE LOAD --\\

            // OTHER STUFF
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            // START SLIMEPEDIA ENTRY: GLUE SLIME
            PediaRegistry.RegisterIdentifiableMapping((PediaDirector.Id)1007, ModdedIds.ModdedIds.GLUE_SLIME);
            PediaRegistry.RegisterIdentifiableMapping(ModdedIds.ModdedIds.GLUE_ENTRY, ModdedIds.ModdedIds.GLUE_SLIME);
            PediaRegistry.SetPediaCategory(ModdedIds.ModdedIds.GLUE_ENTRY, (PediaRegistry.PediaCategory)1);
            new SlimePediaEntryTranslation(ModdedIds.ModdedIds.GLUE_ENTRY)
                .SetTitleTranslation("Glue Slime")
                .SetIntroTranslation("Gooey, Hungry, Vegetarian Slime?")
                .SetDietTranslation("Veggie")
                .SetFavoriteTranslation("Heart Beet")
                .SetSlimeologyTranslation("Glue Slimes are your gooey little friends! They're made out of glue entirely, along with some slimey substance. They do get hungry to the point they may or may not eat something they shouldn't. Tarrs also dislike their gluey taste and will not eat them.")
                .SetRisksTranslation("There are no dangerous risk! Glue Slimes are usually friendly, but.. if they have no other food source, they may result to eating Pink Slimes. They're common so its easy for them to gobble on with no veggies around, so keep them away from your pink slimes if you must!")
                .SetPlortonomicsTranslation("Their plorts are made out of glue as well, great for gluing things together.. that's for sure!");
            // END SLIMEPEDIA ENTRY: GLUE SLIME

            // GLUE PLORT TRANSLATION
            TranslationPatcher.AddActorTranslation("l." + ModdedIds.ModdedIds.GLUE_PLORT.ToString().ToLower(), "Glue Plort");
            PediaRegistry.RegisterIdentifiableMapping(PediaDirector.Id.PLORTS, ModdedIds.ModdedIds.GLUE_PLORT);
            Identifiable.PLORT_CLASS.Add(ModdedIds.ModdedIds.GLUE_PLORT);
            Identifiable.NON_SLIMES_CLASS.Add(ModdedIds.ModdedIds.GLUE_PLORT);

            // START GLUE SLIME SPAWNER
            SRCallbacks.PreSaveGameLoad += (s =>
            {
                foreach (DirectedSlimeSpawner spawner in UnityEngine.Object.FindObjectsOfType<DirectedSlimeSpawner>()
                    .Where(ss =>
                    {
                        ZoneDirector.Zone zone = ss.GetComponentInParent<Region>(true).GetZoneId();
                        return zone == ZoneDirector.Zone.NONE || zone == ZoneDirector.Zone.RANCH || zone == ZoneDirector.Zone.REEF || zone == ZoneDirector.Zone.QUARRY || zone == ZoneDirector.Zone.MOSS || zone == ZoneDirector.Zone.DESERT || zone == ZoneDirector.Zone.DESERT || zone == ZoneDirector.Zone.SEA || zone == ZoneDirector.Zone.RUINS || zone == ZoneDirector.Zone.RUINS_TRANSITION || zone == ZoneDirector.Zone.WILDS || zone == ZoneDirector.Zone.OGDEN_RANCH || zone == ZoneDirector.Zone.VALLEY || zone == ZoneDirector.Zone.MOCHI_RANCH || zone == ZoneDirector.Zone.SLIMULATIONS || zone == ZoneDirector.Zone.VIKTOR_LAB;
                    }))
                {
                    foreach (DirectedActorSpawner.SpawnConstraint constraint in spawner.constraints)
                    {
                        List<SlimeSet.Member> members = new List<SlimeSet.Member>(constraint.slimeset.members)
                        {
                            new SlimeSet.Member
                            {
                                prefab = GameContext.Instance.LookupDirector.GetPrefab(ModdedIds.ModdedIds.GLUE_SLIME),
                                weight = 0.08f // The higher the value is the more often your slime will spawn
                            }
                        };
                        constraint.slimeset.members = members.ToArray();
                    }
                }
            });
            // END GLUE SLIME SPAWNER
        }

        // Called before GameContext.Start
        // Used for registering things that require a loaded gamecontext
        public override void Load()
        {
            //-- LOAD --\\

            // START LOAD GLUE SLIME
            (SlimeDefinition, GameObject) SlimeTuple = GlueSlime.CreateSlime(ModdedIds.ModdedIds.GLUE_SLIME, "Glue Slime"); //Insert your own Id in the first argumeter

            //Getting the SlimeDefinition and GameObject separated
            SlimeDefinition Slime_Slime_Definition = SlimeTuple.Item1;
            GameObject Slime_Slime_Object = SlimeTuple.Item2;

            Slime_Slime_Object.GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, Slime_Slime_Object);
            LookupRegistry.RegisterVacEntry(ModdedIds.ModdedIds.GLUE_SLIME, new Color32(255, 255, 255, byte.MaxValue), CreateSprite(LoadImage("glue_slime.png")));
            TranslationPatcher.AddPediaTranslation("t." + ModdedIds.ModdedIds.GLUE_SLIME.ToString().ToLower(), "Glue Slime");
            LookupRegistry.RegisterVacEntry(VacItemDefinition.CreateVacItemDefinition(ModdedIds.ModdedIds.GLUE_SLIME, new Color32(255, 255, 255, byte.MaxValue), CreateSprite(LoadImage("glue_slime.png"))));

            //And well, registering it!
            LookupRegistry.RegisterIdentifiablePrefab(Slime_Slime_Object);
            SlimeRegistry.RegisterSlimeDefinition(Slime_Slime_Definition);
            // END LOAD GLUE SLIME

            // START LOAD GLUE PLORT
            GameObject PlortTuple = GlueSlimePlort.GluePlort();

            GameObject Plort_Plort_Object = PlortTuple;

            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, Plort_Plort_Object);
            // Icon that is below is just a placeholder. You can change it to anything or add your own! 
            Sprite PlortIcon = CreateSprite(LoadImage("glue_slime_plort.png"));
            Color PureWhite = new Color32(255, 255, 255, byte.MaxValue); // RGB   
            LookupRegistry.RegisterVacEntry(VacItemDefinition.CreateVacItemDefinition(ModdedIds.ModdedIds.GLUE_PLORT, PureWhite, PlortIcon));
            AmmoRegistry.RegisterSiloAmmo(x => x == SiloStorage.StorageType.NON_SLIMES || x == SiloStorage.StorageType.PLORT, ModdedIds.ModdedIds.GLUE_PLORT);

            float price = 50f; //Starting price for plort   
            float saturated = 7f; //Can be anything. The higher it is, the higher the plort price changes every day. I'd recommend making it small so you don't destroy the economy lol.   
            PlortRegistry.AddEconomyEntry(ModdedIds.ModdedIds.GLUE_PLORT, price, saturated); //Allows it to be sold while the one below this adds it to plort market.   
            PlortRegistry.AddPlortEntry(ModdedIds.ModdedIds.GLUE_PLORT); //PlortRegistry.AddPlortEntry(YourCustomEnum, new ProgressDirector.ProgressType[1]{ProgressDirector.ProgressType.NONE});   
            DroneRegistry.RegisterBasicTarget(ModdedIds.ModdedIds.GLUE_PLORT);
            // END LOAD GLUE PLORT
        }

        // Called after all mods Load's have been called
        // Used for editing existing assets in the game, not a registry step
        public override void PostLoad()
        {
            //-- POST LOAD --\\
        }

    }
}