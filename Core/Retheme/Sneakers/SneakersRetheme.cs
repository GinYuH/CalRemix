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
using ReLogic.Utilities;
using Terraria.UI;
using Terraria.DataStructures;
using CalRemix.Core.Retheme.Sneakers;

namespace CalRemix.Core.Retheme
{
    public class SneakersRetheme
    {
        public static HelperMessage sneakerIntroMessage;
        public static HelperMessage platinumNetWorthMessage;
        public static HelperMessage platinumNetWorthLossMessage;
        public static HelperMessage exoBoxWrongSlotMessage;
        public static HelperMessage exoBoxWrongSlotRefundMessage;
        public static HelperMessage ultraRichMessage;

        public static Asset<Texture2D> originalSocTexture;
        public static Asset<Texture2D> originalHivePackTexture;
        public static Asset<Texture2D> originalBoneHelmTexture;
        public static Asset<Texture2D> originalWormScarfTexture;
        public static Asset<Texture2D> originalBoneGloveFrontTexture;
        public static Asset<Texture2D> originalBoneGloveBackTexture;
        public static Asset<Texture2D> invisibleSprite;

        public static List<Asset<Texture2D>> BrandLogos = new();

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
            On_ItemSlot.SelectEquipPage += MountsAndPetsIntoMainPage;

            if (!Main.dedServ)
            {
                BrandLogos = new()
                {
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoNike"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoAdidas"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoAsics"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoBalenciaga"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoYeezy"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoReebok"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoVans"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoPuma"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoNewBalance"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoSkechers"),
                    Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/LogoFandom"),
                };

                invisibleSprite = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/Invisible");
            }
        }

        private static void MountsAndPetsIntoMainPage(On_ItemSlot.orig_SelectEquipPage orig, Item item)
        {
            orig(item);
            //Forces main equip page instead of pets/mounts page
            if (IsASneaker(item.type))
                Main.EquipPage = 0;
        }

        private static bool HideDemonHeartSlot(On_Player.orig_IsItemSlotUnlockedAndUsable orig, Player self, int slot)
        {
            if (slot == 8 && CalRemixWorld.sneakerheadMode)
                return false;
            return orig(self, slot);
        }

        public static bool IsASneaker(int type)
        {
            return CalRemixWorld.sneakerheadMode && SneakerList.Length > type && SneakerList[type];
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
                .NeedsActivation().SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon)
                .SetHoverTextOverride("Rated triple Boom by the costco guys, check me out!");

            platinumNetWorthLossMessage = HelperMessage.New("NetworthLoseMoney", "Shiiiiii dude you've gyatt to be more careful w ur jordans bruh! ur networth lowkey on the down low cuz of ur antics", "CrimSonLostSoul")
                .NeedsActivation().SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon)
                .SetHoverTextOverride("1 2 buckle my shoes lil bro, no one gaf");

            exoBoxWrongSlotMessage = HelperMessage.New("ExoBoxWrongSlot", "Wait a second! You're not WEARING this custom pair of sneakers are you??? I know the mount may look useful, but by not equipping them in your sick kicks slot, you're missing out on its net worth benefit! If you really want to wear a pair, I'll grab you a copy of my uncreasable kicks if you reach 999 platinum net worth, kay?", "FannySneakers")
                .NeedsActivation().SetHoverTextOverride("I'm so infinitely sorry and grateful for your boundless generosity when it comes to this");

            exoBoxWrongSlotRefundMessage = HelperMessage.New("ExoBoxWrongSlotRefund", "pssst unc, Fanny doesn't know these Js are mass produced by big D uptop. Ive snatched an extra pair because of how GOATed they are, so there you go fam", "CrimSonDefault")
                .NeedsActivation().SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).AddStartEvent(()=>Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ItemType<ExoThrone>()))
                .SetHoverTextOverride("You're really a lifesaver my g, i owe you one");

            ultraRichMessage = HelperMessage.New("FinalNetWorth", "You did it! You finally achieved maximum net worth! Through the fire and the fans, we made it here... I'm so proud of you.. here, you can have my uncreasable jordans", "FannyBarefoot")
                .NeedsActivation().SetHoverTextOverride("I didn't want to see this").AddStartEvent(() => Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ItemType<UncreasableKicks>()));

            HelperMessage.New("FinalNetWorth2", "Fanny lil bro put socks on for the love of gyatt, ur not rizzing any1 like this", "CrimSonHeadless")
                .ChainAfter(delay : 3f, startTimerOnMessageSpoken: true).SetHoverTextOverride("youre only spitting str8 fax bruh")
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon); ;
                

            HelperMessage.New("AquaSneakers", "Daaaaamn bro, you got them real Jordans bro! Better collect more of them, then we'll phonk it out, my fellow commie!", "CrimSonDefault",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemType<AquaticEmblem>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).SetHoverTextOverride("Blessed creatures made with love and care in mind");

            HelperMessage.New("VolatileSneakers", "Oh look, another pathetic schmuck who just wants to collect those branded sneakers to increase their net worth. Just know that someone, somewhere in was paid FAR less than these shoes base worth. While your pointless net worth increases, the poor stays poor, and there is no changing that. But fine, flex those blasted sneakers to your easily impressionable \"friends\" and not care about the poor workers who were paid only 3 copper coins per shoe.", "EvilFannyDisgusted",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.VolatileGelatin))
                .SpokenByAnotherHelper(ScreenHelpersUIState.EvilFanny);

            HelperMessage.New("MinecartSneakers", "damn bruh I didn't know you were a sneaker head too! P sure that Jordan's not factory laced, tho", "CrimSonDefault",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.MinecartPowerup))
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("InsignaSneakers", "'Tower hitters'? Those who nose:", "CrimSonNose",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.EmpressFlightBooster))
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).SetSoundOverride(ScreenHelperManager.ThoseWhoNose).SetHoverTextOverride("I don't nose what youre talking about buddy");

            HelperMessage.New("SporeSneakers", "ooohhhh that sneaker stank like garbage, guh", "CrimSonDefault",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.SporeSac))
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("BrimmySneakers", "ooohh I don't like that Jordan bruh. Reminds me of that dare I made with my friends at the sleepover.", "CrimSonLostSoul",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemType<FlameLickedShell>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("ClonecalSneakers", "oh my gyatt! A calamitous Jordan! Bro got the Calamitas-approved Jordans y'all!!!!", "CrimSonDefault",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemType<VoidofCalamity>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("ClonecalSneakersFake", " ...ooh shit, I just realized, that calamitas approved Jordan? That Jordan's A MFING FAKE Js!!!!!!!!!", "CrimSonDefault",
               (ScreenHelperSceneMetrics m) => HelperMessage.ByID("ClonecalSneakers").alreadySeen && Main.rand.NextBool(10000))
               .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).SetHoverTextOverride("damn we got scammed bruv");


            HelperMessage.New("AfflictionSneakers", " wow that Js look like something trapper bulb chan would wear on god fr fr", "CrimSonDefault",
                (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemType<Affliction>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

            HelperMessage.New("AfflictionSneakers2", "umm, no... -_- I don't like wearing those garish branded sneakers, nya... I just prefer wearing the ones that fits me the most, desu~! OWO", "TrapperDisgust")
                .ChainAfter().SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

        }


        public static void InitializeItem(Item item)
        {
            string name = itemSneakerPairs[item.type];
            item.SetNameOverride(CalRemixHelper.LocalText($"Rename.Sneakers.{name}").Value);

            //Turn demon heart into an equipable / Make sure they're all equippable


            //Minecart powerup gives you mech minecart so its fine for it to be consumable
            if (item.type != ItemID.MinecartPowerup)
                item.consumable = false;

            item.accessory = true;
        }

        public static void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string netWorthValue = "";
            int platinumCount = 0;
            int goldCount = 0;
            int silverCount = 0;
            int copperCount = 0;

            int monerz = NetWorthPlayer.netWorthCapPerSneaker[item.type];
            if (monerz < 1)
                monerz = 1;

            if (monerz >= 1000000)
            {
                platinumCount = monerz / 1000000;
                monerz -= platinumCount * 1000000;
            }

            if (monerz >= 10000)
            {
                goldCount = monerz / 10000;
                monerz -= goldCount * 10000;
            }

            if (monerz >= 100)
            {
                silverCount = monerz / 100;
                monerz -= silverCount * 100;
            }

            if (monerz >= 1)
                copperCount = monerz;

            if (platinumCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.PlatinumCoin}][c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + platinumCount + "]";

            if (goldCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.GoldCoin}][c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + goldCount + "]";

            if (silverCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.SilverCoin}][c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + silverCount +  "]";

            if (copperCount > 0)
                netWorthValue = netWorthValue + $"[i:{ItemID.CopperCoin}][c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + copperCount + "]";

            tooltips.Add(new TooltipLine(CalRemix.instance, "Brand", "logo"));
            tooltips.Add(new TooltipLine(CalRemix.instance, "NetWorth", $"Total net worth:{netWorthValue}"));
        }

        public static bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Mod == "CalRemix" && line.Name == "Brand")
            {
                Texture2D brandLogo = BrandLogos[(int)sneakerBrands[item.type]].Value;

                for (int l = 0; l < 5; l++)
                {
                    int drawX = line.X;
                    int drawY = line.Y;

                    switch (l)
                    {
                        case 0:
                            drawX--;
                            break;
                        case 1:
                            drawX++;
                            break;
                        case 2:
                            drawY--;
                            break;
                        case 3:
                            drawY++;
                            break;
                    }

                    Main.spriteBatch.Draw(brandLogo, new Vector2(drawX, drawY), null, l == 4 ? line.Color : Color.Black, line.Rotation, line.Origin, (line.BaseScale.X + line.BaseScale.Y) / 2f, SpriteEffects.None, 0f);
                }


                return false;
            }


            return true;
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

            originalSocTexture = TextureAssets.AccShield[ArmorIDs.Shield.ShieldofCthulhu];
            originalWormScarfTexture = TextureAssets.AccNeck[ArmorIDs.Neck.WormScarf];
            originalHivePackTexture = TextureAssets.AccBack[ArmorIDs.Back.HivePack];
            originalBoneHelmTexture = TextureAssets.AccFace[ArmorIDs.Face.BoneHelm];
            originalBoneGloveFrontTexture = TextureAssets.AccHandsOn[ArmorIDs.HandOn.BoneGlove];
            originalBoneGloveBackTexture = TextureAssets.AccHandsOff[ArmorIDs.HandOff.BoneGlove];
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

                Mount.mounts[MountID.MinecartMech].frontTexture = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/MechanicalMinecartMount", AssetRequestMode.ImmediateLoad);
                Mount.mounts[MountID.MinecartMech].frontTextureGlow = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/MechanicalMinecartMountGlow");
                Mount.mounts[MountID.MinecartMech].textureWidth = Mount.mounts[MountID.MinecartMech].frontTexture.Width();

                Mount.mounts[MountID.CuteFishron].backTexture = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/CuteFishronMount");
                Mount.mounts[MountID.CuteFishron].backTextureGlow = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/CuteFishronMount2");
                Mount.mounts[MountID.CuteFishron].frontTexture = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/CuteFishronMount", AssetRequestMode.ImmediateLoad);
                Mount.mounts[MountID.CuteFishron].frontTextureGlow = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/CuteFishronMount2");
                Mount.mounts[MountID.CuteFishron].textureWidth = Mount.mounts[MountID.CuteFishron].frontTexture.Width();


                var throneData = MountLoader.GetMount(MountType<DraedonGamerChairMount>()).MountData;
                throneData.backTexture = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/DraedonGamerChairMount", AssetRequestMode.ImmediateLoad);
                throneData.frontTexture = throneData.backTexture;
                throneData.frontTextureGlow = Request<Texture2D>("CalRemix/Core/Retheme/Sneakers/DraedonGamerChairMountGlow");
                throneData.textureWidth = throneData.frontTexture.Width();

                TextureAssets.AccShield[ArmorIDs.Shield.ShieldofCthulhu] = invisibleSprite;
                TextureAssets.AccNeck[ArmorIDs.Neck.WormScarf] = invisibleSprite;
                TextureAssets.AccBack[ArmorIDs.Back.HivePack] = invisibleSprite;
                TextureAssets.AccFace[ArmorIDs.Face.BoneHelm] = invisibleSprite;
                TextureAssets.AccHandsOn[ArmorIDs.HandOn.BoneGlove] = invisibleSprite;
                TextureAssets.AccHandsOff[ArmorIDs.HandOff.BoneGlove] = invisibleSprite;

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
                Mount.mounts[MountID.MinecartMech].textureWidth = Mount.mounts[MountID.MinecartMech].frontTexture.Width();

                Mount.mounts[MountID.CuteFishron].backTexture = TextureAssets.CuteFishronMount[0];
                Mount.mounts[MountID.CuteFishron].backTextureGlow = TextureAssets.CuteFishronMount[1];
                Mount.mounts[MountID.CuteFishron].frontTexture = Asset<Texture2D>.Empty;
                Mount.mounts[MountID.CuteFishron].frontTextureGlow = Asset<Texture2D>.Empty;
                Mount.mounts[MountID.CuteFishron].textureWidth = Mount.mounts[MountID.CuteFishron].backTexture.Width();

                var throneData = MountLoader.GetMount(MountType<DraedonGamerChairMount>()).MountData;
                throneData.backTexture = OriginalThroneBackTexture;
                throneData.frontTexture = OriginalThroneTexture;
                throneData.frontTextureGlow = OriginalThroneGlowTexture;
                throneData.textureWidth = throneData.frontTexture.Width();

                TextureAssets.AccShield[ArmorIDs.Shield.ShieldofCthulhu] = originalSocTexture;
                TextureAssets.AccNeck[ArmorIDs.Neck.WormScarf] = originalWormScarfTexture;
                TextureAssets.AccBack[ArmorIDs.Back.HivePack] = originalHivePackTexture;
                TextureAssets.AccFace[ArmorIDs.Face.BoneHelm] = originalBoneHelmTexture;
                TextureAssets.AccHandsOn[ArmorIDs.HandOn.BoneGlove] = originalBoneGloveFrontTexture;
                TextureAssets.AccHandsOff[ArmorIDs.HandOff.BoneGlove] = originalBoneGloveBackTexture;

                //Set textures back to the regular modified items
                if (CalRemixWorld.itemChanges)
                {
                    foreach (KeyValuePair<int, string> p in RethemeList.Items)
                    {
                        if (IsASneaker(p.Key))
                            TextureAssets.Item[p.Key] = Request<Texture2D>("CalRemix/Core/Retheme/" + p.Value);
                    }
                }
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

        public enum ShoeBrand
        {
            Nike,
            Adidas,
            Asics,
            Balenciaga,
            Yeezy,
            Reebok,
            Vans,
            Puma,
            NewBalance,
            Skechers,
            Fandom
        }
        internal static Dictionary<int, ShoeBrand> sneakerBrands = new()
            {
            //Vanilla
             { ItemID.RoyalGel, ShoeBrand.Nike },
             { ItemID.EoCShield, ShoeBrand.Nike },
             { ItemID.WormScarf, ShoeBrand.Nike },
             { ItemID.BrainOfConfusion, ShoeBrand.Nike },
             { ItemID.HiveBackpack,  ShoeBrand.Asics },
             { ItemID.BoneHelm,  ShoeBrand.Nike },
             { ItemID.BoneGlove,  ShoeBrand.Balenciaga },
             { ItemID.DemonHeart,  ShoeBrand.Adidas },

             { ItemID.VolatileGelatin, ShoeBrand.Nike  },
             { ItemID.MechanicalBatteryPiece, ShoeBrand.Nike  },
             { ItemID.MechanicalWagonPiece, ShoeBrand.Nike  },
             { ItemID.MechanicalWheelPiece, ShoeBrand.Nike  },
             { ItemID.MinecartMech, ShoeBrand.Nike  },
             { ItemID.MinecartPowerup, ShoeBrand.Nike  },
             { ItemID.SporeSac, ShoeBrand.Yeezy },
             { ItemID.ShinyStone, ShoeBrand.Adidas },
             { ItemID.ShrimpyTruffle, ShoeBrand.Reebok },
             { ItemID.EmpressFlightBooster, ShoeBrand.Adidas },
             { ItemID.SuspiciousLookingTentacle, ShoeBrand.Nike },

             //Calamity
             { ItemType<OceanCrest>(), ShoeBrand.Nike },
             { ItemType<FungalClump>(), ShoeBrand.Yeezy },
             { ItemType<BloodyWormTooth>(), ShoeBrand.Asics },
             { ItemType<RottenBrain>(),  ShoeBrand.Vans },
             { ItemType<ManaPolarizer>(), ShoeBrand.Vans },

             { ItemType<AquaticEmblem>(), ShoeBrand.Nike },
             { ItemType<FlameLickedShell>(),  ShoeBrand.Adidas },
             { ItemType<VoidofCalamity>(), ShoeBrand.Puma },
             { ItemType<LeviathanAmbergris>(), ShoeBrand.Vans },
             { ItemType<GravistarSabaton>(), ShoeBrand.Nike },
             { ItemType<ToxicHeart>(), ShoeBrand.NewBalance },
             { ItemType<HideofAstrumDeus>(),  ShoeBrand.Adidas },
             { ItemType<BloodflareCore>(),  ShoeBrand.Adidas },
             { ItemType<DynamoStemCells>(),  ShoeBrand.Nike  },

             { ItemType<SpectralVeil>(),  ShoeBrand.Nike  },
             { ItemType<TheEvolution>(),  ShoeBrand.Yeezy  },
             { ItemType<Affliction>(),  ShoeBrand.Yeezy  },
             { ItemType<OldDukeScales>(),  ShoeBrand.NewBalance  },
             { ItemType<BlazingCore>(),  ShoeBrand.Skechers  },
             { ItemType<NebulousCore>(), ShoeBrand.NewBalance  },
             { ItemType<YharimsGift>(),  ShoeBrand.Nike  },
             { ItemType<ExoThrone>(), ShoeBrand.Nike  },
             { ItemType<Calamity>(), ShoeBrand.NewBalance }
        };


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
