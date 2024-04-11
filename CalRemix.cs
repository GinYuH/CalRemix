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
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ModLoader;
using System.Runtime.InteropServices;
using Terraria.Audio;
using ReLogic.Utilities;
using CalRemix.Buffs;

namespace CalRemix
{
    public class CalRemix : Mod
    {
        public static CalRemix instance;
        public static Mod CalVal;

        public static int CosmiliteCoinCurrencyId;
        public static int KlepticoinCurrencyId;
        internal static Effect SlendermanShader;
        public static Asset<Texture2D> sunOG = null;
        public static Asset<Texture2D> sunCreepy = null;

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
            }
            Terraria.On_IngameOptions.DrawLeftSide += OhGod;

            AssetRepository calAss = instance.Assets;
            Effect LoadShader(string path) => calAss.Request<Effect>("Effects/" + path, AssetRequestMode.ImmediateLoad).Value;
            SlendermanShader = LoadShader("SlendermanStatic");
            RegisterMiscShader(SlendermanShader, "StaticPass", "SlendermanStaticShader");
            Terraria.On_Main.DrawGore += DrawStatic;
            Terraria.Audio.On_SoundPlayer.Play += LazerSoundOverride;
            sunOG = TextureAssets.Sun3;
            sunCreepy = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Eclipse");
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
            if (txt == "Save & Exit")
                txt = "A Fan-tastic time awaits!";
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
        public static void DrawStatic(Terraria.On_Main.orig_DrawGore orig, Terraria.Main self)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<TallMan>()))
                return;
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
            float maxRadius = slender.ai[0] + extraDist;
            shader.Parameters["radius"].SetValue(slender.ai[0]);
            shader.Parameters["maxRadius"].SetValue(maxRadius);
            shader.Parameters["anchorPoint"].SetValue(Main.LocalPlayer.Center);
            shader.Parameters["screenPosition"].SetValue(Main.screenPosition);
            shader.Parameters["screenSize"].SetValue(Main.ScreenSize.ToVector2());
            shader.Parameters["maxOpacity"].SetValue(0.9f);
            shader.Parameters["seed"].SetValue(Main.GlobalTimeWrappedHourly);

            Main.spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied, shader);
            Rectangle rekt = new(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth, Main.screenHeight);
            Main.spriteBatch.Draw(blackTile.Value, rekt, null, default, 0f, blackTile.Value.Size() * 0.5f, 0, 0f);
            Main.spriteBatch.ExitShaderRegion();

            Texture2D parasite = ModContent.Request<Texture2D>("CalRemix/NPCs/Eclipse/SlenderJumpscare").Value;
            Vector2 scale = new Vector2(Main.screenWidth * 1.1f / parasite.Width, Main.screenHeight * 1.1f / parasite.Height);
            Vector2 screenArea = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
            Vector2 origin = parasite.Size() * 0.5f;
            Main.spriteBatch.Draw(parasite, screenArea, null, Color.Red * slender.ai[2], 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}