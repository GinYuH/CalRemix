using CalamityMod;
using CalamityMod.Items.Accessories;
using CalRemix.CrossCompatibility.OutboundCompatibility;
using CalRemix.NPCs;
using CalRemix.NPCs.Minibosses;
using CalRemix.NPCs.Bosses.Wulfwyrm;
using CalRemix.Items.Accessories;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalRemix.Backgrounds.Plague;
using Terraria.Graphics.Effects;
using CalamityMod.NPCs.HiveMind;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using CalRemix.Scenes;
using Terraria.Graphics.Shaders;
using System.Reflection;
using CalRemix.Skies;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Subworlds;
using ReLogic.Content;
using CalRemix.NPCs.Eclipse;
using Terraria.GameContent;
using System.Runtime.InteropServices;
using Terraria.Audio;
using ReLogic.Utilities;
using CalRemix.Buffs;
using CalRemix.Retheme;
using Mono.Cecil;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using System.IO;
using Terraria.Utilities;
using Terraria.ModLoader.IO;
using CalamityMod.World;
using Terraria.UI;
using CalamityMod.Systems;
using CalamityMod.NPCs.OldDuke;
using CalRemix.UI.Title;
using CalRemix.NPCs.Bosses.Carcinogen;
using CalRemix.NPCs.Bosses.Hydrogen;
using System.Linq;
using SubworldLibrary;
using CalRemix.UI;

namespace CalRemix
{
    public class CalRemix : Mod
    {
        public static CalRemix instance;
        public static Mod CalVal;

        public static int CosmiliteCoinCurrencyId;
        public static int KlepticoinCurrencyId;

        internal static Effect SlendermanShader;
        internal static Effect LeanShader;

        public static Asset<Texture2D> sunOG = null;
        public static Asset<Texture2D> sunReal = null;
        public static Asset<Texture2D> sunCreepy = null;
        public static Asset<Texture2D> sunOxy = null;
        public static Asset<Texture2D> sunOxy2 = null;

        public static readonly SoundStyle Silence = new($"{nameof(CalRemix)}/Sounds/EmptySound");

        public static List<int> oreList = new List<int>
        {
            TileID.Copper,
            TileID.Tin,
            TileID.Iron,
            TileID.Lead,
            TileID.Silver,
            TileID.Tungsten,
            TileID.Gold,
            TileID.Platinum
        };
        // Defer mod call handling to the extraneous mod call manager.
        public override object Call(params object[] args) => ModCallManager.Call(args);
        private static void RegisterMiscShader(Effect shader, string passName, string registrationName)
        {
            Ref<Effect> shaderPointer = new(shader);
            MiscShaderData passParamRegistration = new(shaderPointer, passName);
            GameShaders.Misc["CalRemix/" + registrationName] = passParamRegistration;
        }

        internal static CalRemix Instance;

        public override void Load()
        {
            instance = this;
            ModLoader.TryGetMod("CalValEX", out CalVal);

            PlagueGlobalNPC.PlagueHelper = new PlagueJungleHelper();

            CosmiliteCoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.CosmiliteCoinCurrency(ModContent.ItemType<Items.CosmiliteCoin>(), 100L, "Mods.CalRemix.Currencies.CosmiliteCoinCurrency"));
            KlepticoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.KlepticoinCurrency(ModContent.ItemType<Items.Klepticoin>(), 100L, "Mods.CalRemix.Currencies.Klepticoin"));

            if (!Main.dedServ)
            {
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:PlagueBiome"] = new Filter(new PlagueSkyData("FilterMiniTower").UseColor(Color.Green).UseOpacity(0.15f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:PlagueBiome"] = new PlagueSky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:CalamitySky"] = new Filter(new ScreenShaderData("FilterMoonLord"), EffectPriority.High);
                SkyManager.Instance["CalRemix:CalamitySky"] = new CalamitySky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Exosphere"] = new Filter(new ExosphereScreenShaderData("FilterMiniTower").UseColor(ExosphereSky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Exosphere"] = new ExosphereSky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Fanny"] = new Filter(new FannyScreenShaderData("FilterMiniTower").UseColor(FannySky.DrawColor).UseOpacity(0.25f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Fanny"] = new FannySky();

                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Slenderman"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:Asbestos"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(Color.Gray).UseOpacity(0.5f), EffectPriority.VeryHigh);
                SkyManager.Instance["CalRemix:Asbestos"] = new CarcinogenSky();
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:AcidSight"] = new Filter(new ScreenShaderData(ModContent.Request<Effect>("CalRemix/Effects/AcidSight", AssetRequestMode.ImmediateLoad), "AcidPass"), EffectPriority.VeryHigh);
                Terraria.Graphics.Effects.Filters.Scene["CalRemix:LeanVision"] = new Filter(new ScreenShaderData(ModContent.Request<Effect>("CalRemix/Effects/LeanVision", AssetRequestMode.ImmediateLoad), "LeanPass"), EffectPriority.VeryHigh);
            }
            Terraria.On_IngameOptions.DrawLeftSide += OhGod;

            AssetRepository calAss = instance.Assets;
            Effect LoadShader(string path) => calAss.Request<Effect>("Effects/" + path, AssetRequestMode.ImmediateLoad).Value;
            SlendermanShader = LoadShader("SlendermanStatic");
            RegisterMiscShader(SlendermanShader, "StaticPass", "SlendermanStaticShader");

            Terraria.On_Main.DrawDust += DrawStatic;
            Terraria.On_Main.DrawLiquid += DrawTsarBomba;
            //Terraria.Audio.On_SoundPlayer.Play += LazerSoundOverride;
            //Terraria.UI.On_UIElement.Draw += FreezeIcon;
            //Terraria.On_Player.ItemCheck_Shoot += SoldierShots;
            sunOG = TextureAssets.Sun3;
            sunReal = TextureAssets.Sun;
            sunCreepy = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Eclipse");
            sunOxy = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Oxysun");
            sunOxy2 = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Oxysun2");
        }

        // Freeze most UI elements
        public void FreezeIcon(Terraria.UI.On_UIElement.orig_Draw orig, UIElement self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
            float wid = self.GetOuterDimensions().ToRectangle().Width;
            // Elements larger than 500 pixels aren't frozen (or else you get a giant ice block covering your screen)
            // Fannies don't show up if disabled
            if (wid < 500 && !((self is Fanny || self is FannyTextbox) && !FannyManager.fannyEnabled))
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("CalamityMod/Projectiles/Magic/IceBlock", AssetRequestMode.ImmediateLoad).Value, self.GetOuterDimensions().ToRectangle(), Color.White * 0.6f * MathHelper.Lerp(0.8f, 0.2f, wid / 500));
            }
        }


        public static bool OhGod(Terraria.On_IngameOptions.orig_DrawLeftSide orig, SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale, float maxscale, float scalespeed)
        {
            bool flag = false;
            FieldInfo leftMapping = typeof(Terraria.IngameOptions).GetField("_leftSideCategoryMapping", BindingFlags.NonPublic | BindingFlags.Static);
            Dictionary<int, int> modsList = (Dictionary<int, int>)leftMapping.GetValue(null);
            if (modsList.TryGetValue(i, out var value))
            {
                flag = Terraria.IngameOptions.category == value;
            }
            Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));
            if (flag)
            {
                color = Color.Gold;
            }
            /*if (txt == "Save & Exit")
                txt = "A Fan-tastic time awaits!";*/
            if (txt == "A Fan-tastic time awaits!")
            {
                color = Color.Lerp(Color.Red, Color.Orange, (scales[i] - minscale) / (maxscale - minscale));
            }
            Vector2 vector = Utils.DrawBorderStringBig(sb, txt, anchor + offset * (1 + i), color, scales[i] * (txt == "A Fan-tastic time awaits!" ? 0.8f : 1f), 0.5f, 0.5f);
            bool flag2 = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));

            FieldInfo canConsume = typeof(Terraria.IngameOptions).GetField("_canConsumeHover", BindingFlags.NonPublic | BindingFlags.Static);
            bool consumeHover = (bool)canConsume.GetValue(null);
            if (!consumeHover)
            {
                return false;
            }
            if (flag2)
            {
                if (txt == "A Fan-tastic time awaits!")
                {
                    if (Main.mouseLeft)
                    {
                        SubworldLibrary.SubworldSystem.Enter<FannySubworld>();
                        Terraria.IngameOptions.Close();
                    }
                }
                else
                {
                    canConsume.SetValue(null, false);
                    return true;
                }
            }
            return false;
        }

        public static SlotId LazerSoundOverride(Terraria.Audio.On_SoundPlayer.orig_Play orig, SoundPlayer self, [In] ref SoundStyle style, Vector2? position, SoundUpdateCallback updateCallback)
        {
            return orig(self, ref style, position, updateCallback);
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<Scorinfestation>()))
            {
                style = SoundID.Tink;
                SoundUpdateCallback updateCallback2 = updateCallback;
                if (Main.dedServ)
                {
                    return SlotId.Invalid;
                }
                if (position.HasValue && Vector2.DistanceSquared(Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), position.Value) > 100000000f)
                {
                    return SlotId.Invalid;
                }
                if (style.PlayOnlyIfFocused && !Main.hasFocus)
                {
                    return SlotId.Invalid;
                }
                if (!Program.IsMainThread)
                {
                    SoundStyle styleCopy = style;
                    return Main.RunOnMainThread(() => PlayInerr(in styleCopy, position, updateCallback2)).GetAwaiter().GetResult();
                }
                return PlayInerr(in style, position, updateCallback2);
            }
            else
            {
                return orig(self, ref style, position, updateCallback);
            }
        }

        public static SlotId PlayInerr(in SoundStyle style, Vector2? position, SoundUpdateCallback c)
        {
            FieldInfo tracc = typeof(Terraria.Audio.SoundPlayer).GetField("_trackedSounds", BindingFlags.NonPublic | BindingFlags.Instance);
            ReLogic.Utilities.SlotVector<ActiveSound> consumeHover = (ReLogic.Utilities.SlotVector<ActiveSound>)tracc.GetValue(Terraria.Audio.SoundEngine.SoundPlayer);

            //ReLogic.Utilities.SlotVector<ActiveSound>
            int maxInstances = style.MaxInstances;
            if (maxInstances > 0)
            {
                int instanceCount = 0;
                foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)consumeHover)
                {
                    ActiveSound activeSound = item.Value;
                    if (activeSound.IsPlaying && style.IsTheSameAs(activeSound.Style) && ++instanceCount >= maxInstances)
                    {
                        if (style.SoundLimitBehavior != SoundLimitBehavior.ReplaceOldest)
                        {
                            return SlotId.Invalid;
                        }
                        activeSound.Sound?.Stop(immediate: true);
                    }
                }
            }
            SoundStyle styleCopy = style;
            ActiveSound value = new ActiveSound(styleCopy, position, c);
            return consumeHover.Add(value);
        }

        public static void SoldierShots(Terraria.On_Player.orig_ItemCheck_Shoot orig, Player self, int i, Item sItem, int weaponDamage)
        {
            //if (!sItem.channel && sItem.DamageType != DamageClass.Summon)
            for (int e = 0; e < Main.maxPlayers; e++)
                {
                    Player p = Main.player[e];
                if (p == null)
                    continue;
                if (!p.active)
                    continue;


                    if (p.whoAmI != Main.LocalPlayer.whoAmI)
                    {
                    float speed = sItem.shootSpeed;
                    float KnockBack = sItem.knockBack;
                    Vector2 pointPoisition = p.Center;
                    float dirX = (float)Main.mouseX + Main.screenPosition.X - pointPoisition.X;
                    float dirY = (float)Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
                    float dist = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                    if ((float.IsNaN(dirX) && float.IsNaN(dirY)) || (dirX == 0f && dirY == 0f))
                    {
                        dirX = p.direction;
                        dirY = 0f;
                        dist = speed;
                    }
                    else
                    {
                        dist = speed / dist;
                    }
                    dirX *= dist;
                    dirY *= dist;
                        Main.NewText("A");

                    if (sItem.ModItem?.Shoot(p, (EntitySource_ItemUse_WithAmmo)self.GetSource_ItemUse_WithPotentialAmmo(sItem, i), pointPoisition, new Vector2(dirX, dirY), sItem.shoot, weaponDamage, KnockBack) == true)
                    {
                        Projectile.NewProjectile(self.GetSource_FromThis(), pointPoisition.X, pointPoisition.Y, dirX, dirY, sItem.shoot, weaponDamage, KnockBack, i);
                    }
                        
                    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, self.whoAmI);
                }
            }
            orig(self, i, sItem, weaponDamage);
        }

        public override void Unload()
        {
            PlagueGlobalNPC.PlagueHelper = null;
            instance = null;
            CalVal = null;
        }
        public override void PostSetupContent()
        {
            // Calamity Calls
            Mod cal = ModLoader.GetMod("CalamityMod");
            cal.Call("RegisterModPopupGUIs", this);
            cal.Call("RegisterModCooldowns", this);
            cal.Call("DeclareMiniboss", ModContent.NPCType<LifeSlime>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<Clamitas>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<OnyxKinsman>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<CyberDraedon>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<PlagueEmperor>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<YggdrasilEnt>());
            cal.Call("DeclareMiniboss", ModContent.NPCType<KingMinnowsPrime>());
            cal.Call("MakeItemExhumable", ModContent.ItemType<YharimsGift>(), ModContent.ItemType<YharimsCurse>());
            //cal.Call("DeclareOneToManyRelationshipForHealthBar", ModContent.NPCType<DerellectBoss>(), ModContent.NPCType<SignalDrone>());
            //cal.Call("DeclareOneToManyRelationshipForHealthBar", ModContent.NPCType<DerellectBoss>(), ModContent.NPCType<DerellectPlug>());

            AddEnchantments(cal);
            LoadBossRushEntries(cal);

            if (Main.netMode != NetmodeID.Server)
            {
                Main.QueueMainThreadAction(() =>
                {
                    cal.Call("LoadParticleInstances", instance);
                });
            }
            AddHiveBestiary(ModContent.NPCType<DankCreeper>(), "When threatened by outside forces, chunks of the Hive Mind knocked loose in combat will animate in attempt to subdue their attacker. Each Creeper knocked loose shrinks the brain ever so slightly- though this is an inherently selfdestructive self defense mechanism, any survivors will rejoin with the main body should the threat pass.");
            AddHiveBestiary(ModContent.NPCType<HiveBlob>(), "Clustering globs ejected from the Hive Mind. The very nature of these balls of matter act as a common example of the convergent properties that the Corruption's microorganisms possess.");
            AddHiveBestiary(ModContent.NPCType<DarkHeart>(), "Flying sacs filled with large amounts of caustic liquid. The Hive Mind possesses a seemingly large amount of these hearts, adding to its strange biology.");
            RefreshBestiary();

            try
            {
                // Yes I am nullchecking all of these
                if (typeof(MenuLoader).GetMethod("OffsetModMenu", BindingFlags.Static | BindingFlags.NonPublic) is null)
                    return;
                if (typeof(MenuLoader).GetField("LastSelectedModMenu", BindingFlags.Static | BindingFlags.NonPublic) is null)
                    return;
                if (CalRemixMenu.Instance is null)
                    return;
                if (CalRemixMenu.Instance.FullName is null)
                    return;
                typeof(MenuLoader).GetMethod("OffsetModMenu", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { Main.rand.Next(-2, 3) });
                typeof(MenuLoader).GetField("LastSelectedModMenu", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, CalRemixMenu.Instance.FullName);

                if ((ModMenu)typeof(MenuLoader).GetField("switchToMenu", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) is null || CalRemixMenu.Instance is null)
                    return;
                if (((ModMenu)typeof(MenuLoader).GetField("switchToMenu", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)).FullName is null || CalRemixMenu.Instance.FullName is null)
                    return;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("CalRemixMenu");
                Console.WriteLine(e.ToString());
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n");
            }
        }

        internal void AddEnchantments(Mod cal)
        {
            LocalizedText defiant = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Defiant.Name");
            LocalizedText defiantDesc = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Defiant.Description");
            cal.Call("CreateEnchantment", defiant, defiantDesc, 150, new Predicate<Item>(DefiantEnchantable), "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneDefiant", delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().earthEnchant = true;
            });

            LocalizedText fallacious = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Name");
            LocalizedText fallaciousDesc = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Description");
            cal.Call("CreateEnchantment", fallacious, fallaciousDesc, 156, new Predicate<Item>(FallaciousEnchantable), "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneFallacious", delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().amongusEnchant = true;
            });
        }
        internal static bool DefiantEnchantable(Item item)
        {
            return item.IsEnchantable() && item.damage > 0;
        }
        internal static bool FallaciousEnchantable(Item item)
        {
            return item.IsEnchantable() && item.damage > 0 && !item.CountsAsClass<SummonDamageClass>() && !item.IsWhip();
        }
        internal static void LoadBossRushEntries(Mod cal)
        {
            List<(int, int, Action<int>, int, bool, float, int[], int[])> brEntries = (List<(int, int, Action<int>, int, bool, float, int[], int[])>)cal.Call("GetBossRushEntries");
            int[] excIDs = { ModContent.NPCType<WulfwyrmBody>(), ModContent.NPCType<WulfwyrmTail>() };
            int[] headID = { ModContent.NPCType<WulfwyrmHead>() };
            Action<int> pr = delegate (int npc)
            {
                NPC.SpawnOnPlayer(CalamityMod.Events.BossRushEvent.ClosestPlayerToWorldCenter, ModContent.NPCType<WulfwyrmHead>());
            };
            brEntries.Insert(0, (ModContent.NPCType<WulfwyrmHead>(), -1, pr, 180, false, 0f, excIDs, headID));
            foreach (var entry in brEntries)
            {
                if (entry.Item1 == ModContent.NPCType<OldDuke>())
                {
                    brEntries.Remove(entry);
                    break;
                }
            }
            cal.Call("SetBossRushEntries", brEntries);
        }

        public static void AddHiveBestiary(int id, string entryText)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers modifiers = new NPCID.Sets.NPCBestiaryDrawModifiers();
            modifiers.Hide = false;
            if (id == ModContent.NPCType<DankCreeper>())
            {
                modifiers.CustomTexturePath = "CalRemix/Retheme/HiveMind/DankCreeper";
            }
            if (id == ModContent.NPCType<HiveBlob>())
            {
                modifiers.CustomTexturePath = "CalRemix/Retheme/HiveMind/HiveBlob";
            }
            if (id == ModContent.NPCType<DarkHeart>())
            {
                modifiers.PortraitPositionXOverride = 10;
                modifiers.PortraitPositionYOverride = 20;
            }

            NPCID.Sets.NPCBestiaryDrawOffset[id] = modifiers;
            BestiaryEntry b = new BestiaryEntry();
            b.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
        new FlavorTextBestiaryInfoElement(entryText)
            });
            int associatedNPCType = ModContent.NPCType<HiveMind>();
            b.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
            ContentSamples.NpcsByNetId[id].ModNPC?.SetBestiary(Main.BestiaryDB, b);
        }

        public static void RefreshBestiary()
        {
            ContentSamples.RebuildBestiarySortingIDsByBestiaryDatabaseContents(Main.BestiaryDB);
            ItemDropDatabase itemDropDatabase = new ItemDropDatabase();
            itemDropDatabase.Populate();
            Main.ItemDropsDB = itemDropDatabase;
            Main.BestiaryDB.Merge(Main.ItemDropsDB);
            if (!Main.dedServ)
            {
                Main.BestiaryUI = new Terraria.GameContent.UI.States.UIBestiaryTest(Main.BestiaryDB);
            }
        }
        internal static void AddToShop(int type, int price, ref Chest shop, ref int nextSlot, bool condition = true, int specialMoney = 0)
        {
            if (!condition || shop is null) return;
            shop.item[nextSlot].SetDefaults(type);
            shop.item[nextSlot].shopCustomPrice = price > 0 ? price : shop.item[nextSlot].value;
            if (specialMoney == 1) shop.item[nextSlot].shopSpecialCurrency = CosmiliteCoinCurrencyId;
            else if (specialMoney == 2) shop.item[nextSlot].shopSpecialCurrency = KlepticoinCurrencyId;
            nextSlot++;
        }

        public static float extraDist = 222;
        public static void DrawStatic(Terraria.On_Main.orig_DrawDust orig, Terraria.Main self)
        {
            orig(self);
            if (NPC.AnyNPCs(ModContent.NPCType<TallMan>()))
            {
                NPC slender = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<TallMan>())];

                if (slender == null || !slender.active)
                    return;

                if (slender.ai[0] > 222)
                {
                    extraDist = 222;
                }
                else
                {
                    if (extraDist > 0 && Main.hasFocus)
                    {
                        extraDist--;
                    }
                }

                var blackTile = TextureAssets.MagicPixel;
                var shader = GameShaders.Misc["CalRemix/SlendermanStaticShader"].Shader;
                GameShaders.Misc["CalRemix/SlendermanStaticShader"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj"));
                GameShaders.Misc["CalRemix/SlendermanStaticShader"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj"), 0);
                float maxRadius = slender.ai[0] + extraDist;
                shader.Parameters["radius"].SetValue(slender.ai[0]);
                shader.Parameters["maxRadius"].SetValue(maxRadius);
                shader.Parameters["anchorPoint"].SetValue(Main.LocalPlayer.Center);
                shader.Parameters["screenPosition"].SetValue(Main.screenPosition);
                shader.Parameters["screenSize"].SetValue(Main.ScreenSize.ToVector2());
                shader.Parameters["maxOpacity"].SetValue(0.9f);
                shader.Parameters["seed"].SetValue(Main.GlobalTimeWrappedHourly);
                shader.Parameters["sizeDivisor"].SetValue(0.5f);

                Main.spriteBatch.Begin();
                Main.spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied, shader);
                Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
                Main.spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);

                shader.Parameters["maxRadius"].SetValue(slender.ai[0] + extraDist + 444);
                shader.Parameters["sizeDivisor"].SetValue(0.25f);
                Main.spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);
                Main.spriteBatch.ExitShaderRegion();

                Texture2D parasite = ModContent.Request<Texture2D>("CalRemix/NPCs/Eclipse/SlenderJumpscare" + slender.localAI[0]).Value;
                Color color = Color.White * (slender.ai[2] > 0 ? 1f : 0f);
                Vector2 scale = new Vector2(Main.screenWidth * 1.1f / parasite.Width, Main.screenHeight * 1.1f / parasite.Height);
                int shakeamt = 33;
                Vector2 screenArea = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f) + new Vector2(Main.rand.Next(-shakeamt, shakeamt), Main.rand.Next(-shakeamt, shakeamt));
                Vector2 origin = parasite.Size() * 0.5f;
                Main.spriteBatch.Draw(parasite, screenArea, null, color, 0f, origin, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.End();
            }
        }
        public static void DrawTsarBomba(Terraria.On_Main.orig_DrawLiquid orig, Terraria.Main self, bool a, int b, float c, bool d)
        {
            orig(self, a, b, c, d);
            if (NPC.AnyNPCs(ModContent.NPCType<Hydrogen>()))
            {
                NPC hydr = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Hydrogen>())];

                if (hydr == null || !hydr.active)
                    return;
                int gus = (int)(255f * hydr.localAI[0] / 100);
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth * 4, Main.screenHeight * 4), null, new Color(gus, gus, gus, (int)hydr.localAI[0]), 0f, TextureAssets.MagicPixel.Value.Size() * 0.5f, 0, 0f);
            }
        }
    }
}