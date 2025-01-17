using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Reflection;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using CalamityMod.Projectiles.Rogue;
using CalRemix.Core.World;
using CalamityMod.NPCs.Providence;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using System;
using Terraria;
using Steamworks;
using CalamityMod.Items.Accessories;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Mounts;
using System.Collections.Generic;
using Terraria.GameContent;
using CalamityMod.Buffs.Mounts;
using CalRemix.UI;

namespace CalRemix.Core.Retheme
{
    public class SneakersRetheme
    {
        public static HelperMessage sneakerIntroMessage;
        public static HelperMessage platinumNetWorthMessage;

        public static SetFactory SneakersFactory = new SetFactory(ItemLoader.ItemCount);
        public static bool[] SneakerList = SneakersFactory.CreateBoolSet(
            //Vanilla
              ItemID.RoyalGel,
              ItemID.EoCShield, 
              ItemID.WormScarf,
              ItemID.BrainOfConfusion, 
              ItemID.HiveBackpack,
              ItemID.BoneHelm,
              ItemID.BoneGlove, 
              ItemID.DemonHeart,

              ItemID.VolatileGelatin, 
              ItemID.MechanicalBatteryPiece,
              ItemID.MechanicalWagonPiece, 
              ItemID.MechanicalWheelPiece,
              ItemID.MinecartMech, 
              ItemID.MinecartPowerup, 
              ItemID.SporeSac, 
              ItemID.ShinyStone, 
              ItemID.ShrimpyTruffle, 
              ItemID.EmpressFlightBooster, 
              ItemID.SuspiciousLookingTentacle, 

             //Calamity
              ItemType<OceanCrest>(),
              ItemType<FungalClump>(),
              ItemType<BloodyWormTooth>(), 
              ItemType<RottenBrain>(), 
              ItemType<ManaPolarizer>(),

              ItemType<AquaticEmblem>(),
              ItemType<FlameLickedShell>(),
              ItemType<VoidofCalamity>(), 
              ItemType<LeviathanAmbergris>(),
              ItemType<GravistarSabaton>(), 
              ItemType<ToxicHeart>(), 
              ItemType<HideofAstrumDeus>(),
              ItemType<BloodflareCore>(),
              ItemType<DynamoStemCells>(), 

              ItemType<SpectralVeil>(),
              ItemType<TheEvolution>(),
              ItemType<Affliction>(), 
              ItemType<OldDukeScales>(),
              ItemType<BlazingCore>(), 
              ItemType<NebulousCore>(),
              ItemType<YharimsGift>(), 
              ItemType<ExoThrone>(), 
              ItemType<Calamity>()
            
            );

        public static void Load()
        {
            On_Player.IsItemSlotUnlockedAndUsable += HideDemonHeartSlot;
        }

        private static bool HideDemonHeartSlot(On_Player.orig_IsItemSlotUnlockedAndUsable orig, Player self, int slot)
        {
            if (slot == 8 && CalRemixWorld.sneakerheadMode)
                return false;
            return orig(self, slot);
        }

        public static void LoadHelperMessages()
        {
            sneakerIntroMessage = HelperMessage.New("SneakersIntroduction", "IS THAT A PAIR OF BRANDED SNEAKERS????? OUT OF THE BOX??? UNCREASED??? My friend, my buddy, my good pal. You've just struck gold! Kicks this pristine usually fly off the shelves! AND it's a limited rerun? Oh my lord, please keep going and try to collect more sneakers from bosses, okay? Your net worth is sure to soar up in no time!", "FannySneakers")
                .AddDelay(1f).NeedsActivation().SetHoverTextOverride("That's amazing Fanny, I'm glad I have a friend that's knowledgeable in the hobby of sneaker collection!");


            HelperMessage.New("SneakersExtraSlot", "Ohh my friend this is marvelous! I am overjoyed! I am ecstatic! Over the moon, I tell you! A brand new slot, for you? So you can display TWOOOOO sneakers at once? Oh my lordy lord that is just brilliant. I'm already seeing your net worth skyrocket! SKY!RO!CKET! I can already imagine the faces your friends will make when they see you proudly displaying your newly aquired sneakers!", "FannySneakers",
               (ScreenHelperSceneMetrics m) => Main.LocalPlayer.extraAccessory)
                .AddDelay(1f).SetHoverTextOverride("Now that I can finally use both my feet, I'll finally be able to wear the shoes I've been collecting!");

            HelperMessage.New("SneakersExtraSlot2", "Hold on one second here, buddy! WEAR YOUR SNEAKERS??????? Are you OUT OF YOUR MIND??? My godly kicks are uncreasable, but the ones you've been collecting are far from that. Imagine how much the resell value would be impacted if you were to crease or dirty your jordans??? I know you're new to this hobby, but this is silly, you have to think about your net worth, my pal!", "FannySneakers")
                .ChainAfter().SetHoverTextOverride("O-Oh.. Okay Fanny, you really saved me there!");

            platinumNetWorthMessage = HelperMessage.New("NetworthRich", "Oh my GYATT!!! Your net worth is off the charts bruv??? You've gyatt to get fanum taxxed for this one unc! fr fr tho no cap thats actually fire", "CrimSonDefault")
                .NeedsActivation().SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

        }


        public static void InitializeItem(Item item)
        {
            string name = itemSneakerPairs[item.type];
            item.SetNameOverride(CalRemixHelper.LocalText($"Rename.Sneakers.{name}").Value);

            //Turn demon heart into an equipable / Make sure they're all equippable
            //if (item.type == ItemID.DemonHeart)
            //{
                item.consumable = false;
                item.accessory = true;
            //}
        }

        public static void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

        }



        #region Texture changes
        public static void SaveDefaultSneakersTextures()
        {
            foreach (KeyValuePair<int, string> p in itemSneakerPairs)
            {
                OriginalItemTextures.Add(p.Key, TextureAssets.Item[p.Key]);
            }

            foreach (KeyValuePair<int, string> p in buffSneakerPairs)
            {
                OriginalBuffTextures.Add(p.Key, TextureAssets.Buff[p.Key]);
            }

            var throneData = MountLoader.GetMount(MountType<DraedonGamerChairMount>()).MountData;
            OriginalThroneBackTexture = throneData.backTexture;
            OriginalThroneTexture = throneData.frontTexture;
            OriginalThroneGlowTexture = throneData.frontTextureGlow;
        }


        public static void ApplyTextureChanges()
        {
            bool enabled = CalRemixWorld.sneakerheadMode;

            if (enabled)
            { 
                //Unanimate animated items
                UnanimateItem(ItemType<BlazingCore>());
                UnanimateItem(ItemType<DynamoStemCells>());
                UnanimateItem(ItemType<TheEvolution>());
                UnanimateItem(ItemType<Calamity>());

                foreach (KeyValuePair<int, string> p in itemSneakerPairs)
                {
                    TextureAssets.Item[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/" + p.Value);
                }
                foreach (KeyValuePair<int, string> p in buffSneakerPairs)
                {
                    TextureAssets.Buff[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/" + p.Value);
                }

                Mount.mounts[MountID.MinecartMech].frontTexture = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/MechanicalMinecartMount");
                Mount.mounts[MountID.MinecartMech].frontTextureGlow = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/MechanicalMinecartMountGlow");

                Mount.mounts[MountID.CuteFishron].backTexture = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/CuteFishronMount");
                Mount.mounts[MountID.CuteFishron].backTextureGlow = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/CuteFishronMount2");


                var throneData = MountLoader.GetMount(MountType<DraedonGamerChairMount>()).MountData;
                throneData.backTexture = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/DraedonGamerChairMount");
                throneData.frontTexture = throneData.backTexture;
                throneData.frontTextureGlow = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/DraedonGamerChairMountGlow");
            }
           
            else
            {
                //Reanimate animated items
                GetInstance<BlazingCore>().SetStaticDefaults();
                GetInstance<DynamoStemCells>().SetStaticDefaults();
                GetInstance<TheEvolution>().SetStaticDefaults();
                GetInstance<Calamity>().SetStaticDefaults();

                foreach (KeyValuePair<int, Asset<Texture2D>> p in OriginalItemTextures)
                {
                    TextureAssets.Item[p.Key] = p.Value;
                }
                foreach (KeyValuePair<int, Asset<Texture2D>> p in OriginalBuffTextures)
                {
                    TextureAssets.Buff[p.Key] = p.Value;
                }

                TextureAssets.MinecartMechMount = OriginalMechCartTexture;
                TextureAssets.CuteFishronMount = OriginalCuteFishronTexture;

                Mount.mounts[MountID.MinecartMech].frontTexture = TextureAssets.MinecartMechMount[0];
                Mount.mounts[MountID.MinecartMech].frontTextureGlow = TextureAssets.MinecartMechMount[1];

                Mount.mounts[MountID.CuteFishron].backTexture = TextureAssets.CuteFishronMount[0];
                Mount.mounts[MountID.CuteFishron].backTextureGlow = TextureAssets.CuteFishronMount[1];

                var throneData = MountLoader.GetMount(MountType<DraedonGamerChairMount>()).MountData;
                throneData.backTexture = OriginalThroneBackTexture;
                throneData.frontTexture = OriginalThroneTexture;
                throneData.frontTextureGlow = OriginalThroneGlowTexture;
            }
        }

        public static void UnanimateItem(int type)
        {
            if (Main.itemAnimationsRegistered.Contains(type))
                Main.itemAnimationsRegistered.Remove(type);
            Main.itemAnimations[type] = null;
            ItemID.Sets.AnimatesAsSoul[type] = false;
        }

        internal static Dictionary<int, string> itemSneakerPairs = new()
            {
            //Vanilla
             { ItemID.RoyalGel, "RoyalGel" },
             { ItemID.EoCShield, "ShieldOfCthulhu" },
             { ItemID.WormScarf, "WormScarf" },
             { ItemID.BrainOfConfusion, "BrainOfConfusion" },
             { ItemID.HiveBackpack, "HivePack" },
             { ItemID.BoneHelm, "BoneHelm" },
             { ItemID.BoneGlove, "BoneGlove" },
             { ItemID.DemonHeart, "DemonHeart" },

             { ItemID.VolatileGelatin, "VolatileGelatin" },
             { ItemID.MechanicalBatteryPiece, "MechanicalBatteryPiece" },
             { ItemID.MechanicalWagonPiece, "MechanicalWagonPiece" },
             { ItemID.MechanicalWheelPiece, "MechanicalWheelPiece" },
             { ItemID.MinecartMech, "MechanicalMinecart" },
             { ItemID.MinecartPowerup, "MinecartUpgradeKit" },
             { ItemID.SporeSac, "SporeSac" },
             { ItemID.ShinyStone, "ShinyStone" },
             { ItemID.ShrimpyTruffle, "ShrimpyTruffle" },
             { ItemID.EmpressFlightBooster, "SoaringInsigna" },
             { ItemID.SuspiciousLookingTentacle, "SuspiciousLookingTentacle" },

             //Calamity
             { ItemType<OceanCrest>(), "OceanCrest" },
             { ItemType<FungalClump>(), "FungalClump" },
             { ItemType<BloodyWormTooth>(), "BloodyWormTooth" },
             { ItemType<RottenBrain>(), "RottenBrain" },
             { ItemType<ManaPolarizer>(), "ManaPolarizer" },

             { ItemType<AquaticEmblem>(), "AquaticEmblem" },
             { ItemType<FlameLickedShell>(), "FlameLickedShell" },
             { ItemType<VoidofCalamity>(), "VoidOfCalamity" },
             { ItemType<LeviathanAmbergris>(), "LeviathanAmbergris" },
             { ItemType<GravistarSabaton>(), "GravistarSabaton" },
             { ItemType<ToxicHeart>(), "ToxicHeart" },
             { ItemType<HideofAstrumDeus>(), "HideOfAstrumDeus" },
             { ItemType<BloodflareCore>(), "BloodflareCore" },
             { ItemType<DynamoStemCells>(), "DynamoStemCells" },

             { ItemType<SpectralVeil>(), "SpectralVeil" },
             { ItemType<TheEvolution>(), "TheEvolution" },
             { ItemType<Affliction>(), "Affliction" },
             { ItemType<OldDukeScales>(), "OldDukesScales" },
             { ItemType<BlazingCore>(), "BlazingCore" },
             { ItemType<NebulousCore>(), "NebulousCore" },
             { ItemType<YharimsGift>(), "YharimsGift" },
             { ItemType<ExoThrone>(), "ExoBox" },
             { ItemType<Calamity>(), "Calamity" }};

        internal static Dictionary<int, string> buffSneakerPairs = new()
        {
            #region Sneakers
            { BuffID.MinecartLeftMech, "MechanicalMinecartBuff" },
            { BuffID.MinecartRightMech, "MechanicalMinecartBuff" },
            { BuffID.CuteFishronMount, "ExoBoxBuff" },
            { BuffType<DraedonGamerChairBuff>(), "ExoBoxBuff" },
            #endregion
        };


        private static Dictionary<int, Asset<Texture2D>> OriginalItemTextures = [];
        private static Dictionary<int, Asset<Texture2D>> OriginalBuffTextures = [];

        private static Asset<Texture2D>[] OriginalMechCartTexture;

        private static Asset<Texture2D>[] OriginalCuteFishronTexture;

        private static Asset<Texture2D> OriginalThroneTexture;
        private static Asset<Texture2D> OriginalThroneBackTexture;
        private static Asset<Texture2D> OriginalThroneGlowTexture;

        #endregion
    }
}
