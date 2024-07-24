using Terraria;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;
using CalRemix.Buffs;
using CalRemix.Subworlds;
using CalRemix.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using ReLogic.Utilities;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.UI;
using CalamityMod;
using CalRemix.NPCs.Bosses.Hydrogen;
using CalRemix.NPCs.Eclipse;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using CalRemix.World;
using Terraria.GameContent.UI.States;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using CalRemix.Items.Lore;
using System.IO;
using CalRemix.UI.Anomaly109;
using CalRemix.Items.Placeables;

namespace CalRemix
{
    internal class CalRemixHooks : ModSystem
    {
        public override void Load()
        {
            IL.CalamityMod.Events.AcidRainEvent.TryStartEvent += AcidsighterToggle;
            IL.CalamityMod.Events.AcidRainEvent.TryToStartEventNaturally += AcidsighterToggle2;
            On.CalamityMod.Systems.ExoMechsMusicScene.AdditionalCheck += ExoMusicDeath;
            //IL_Player.ItemCheck_UseBossSpawners += HookDerellectSpawn;
            Terraria.On_Main.DrawDust += DrawStatic;
            Terraria.On_Main.DrawLiquid += DrawTsarBomba;
            Terraria.Audio.On_SoundPlayer.Play += LazerSoundOverride;
            //Terraria.UI.On_UIElement.Draw += FreezeIcon;
            //Terraria.On_Player.ItemCheck_Shoot += SoldierShots;
            Terraria.On_IngameOptions.DrawLeftSide += SendToFannyDimension;
            Terraria.GameContent.UI.States.On_UIWorldCreation.BuildPage += AddRemixOption;
        }

        public static void AddRemixOption(Terraria.GameContent.UI.States.On_UIWorldCreation.orig_BuildPage orig, UIWorldCreation self)
        {
            orig(self);
            if (File.Exists(Anomaly109UI.a109path))
            {
                UIElement mainElement = new UIElement
                {
                    Width = StyleDimension.FromPixels(46),
                    Height = StyleDimension.FromPixels(46),
                    Left = StyleDimension.FromPixels(280f),
                    Top = StyleDimension.FromPixels(450f),
                    HAlign = 0.5f,
                    VAlign = 0f
                };

                UIPanel backgroundPanel = new UIPanel
                {
                    Width = new StyleDimension(46f, 0f),
                    Height = new StyleDimension(46, 0f)

                }.WithFadedMouseOver();

                RemixButton flame = new RemixButton
                {
                    Width = new StyleDimension(40f, 0f),
                    Height = new StyleDimension(40, 0f),
                    Left = new StyleDimension(-8f, 0f),
                    Top = new StyleDimension(-8f, 0f),
                    PaddingLeft = 4f,
                    PaddingRight = 4f
                };

                mainElement.OnLeftClick += ToggleStratus;
                mainElement.OnMouseOver += MausHover;
                mainElement.Append(backgroundPanel);
                backgroundPanel.Append(flame);
                self.Append(mainElement);
            }
            CalRemixWorld.stratusDungeonDisabled = false;
        }

        public static void MausHover(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public static void ToggleStratus(UIMouseEvent evt, UIElement listeningElement)
        {
            CalRemixWorld.stratusDungeonDisabled = !CalRemixWorld.stratusDungeonDisabled;
            SoundStyle toplay = !CalRemixWorld.stratusDungeonDisabled ? SoundID.MenuOpen : SoundID.MenuClose;
            SoundEngine.PlaySound(toplay);
        }

        private static void AcidsighterToggle(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(NPC).GetField("downedBoss1", BindingFlags.Public | BindingFlags.Static);
            if (c.TryGotoNext(i => i.MatchLdsfld(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : typeof(CalRemixWorld).GetField("downedAcidsighter", BindingFlags.Public | BindingFlags.Static));
            }
        }
        private static void AcidsighterToggle2(ILContext il)
        {
            var c = new ILCursor(il);
            var d = typeof(NPC).GetField("downedBoss1", BindingFlags.Public | BindingFlags.Static);
            if (c.TryGotoNext(i => i.MatchLdsfld(d)))
            {
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.EmitDelegate(() => !CalRemixWorld.npcChanges ? d : typeof(CalRemixWorld).GetField("downedAcidsighter", BindingFlags.Public | BindingFlags.Static));
            }
        }
        private static bool ExoMusicDeath(On.CalamityMod.Systems.ExoMechsMusicScene.orig_AdditionalCheck orig, CalamityMod.Systems.ExoMechsMusicScene self) => false;
        /*
        private static void HookDerellectSpawn(ILContext il)
        {
            // Hey, uh, Purified here. As of writing, all the code in this file is for IL editing, and is thus very sensitive.
            // If you touch this stuff without knowing what you're doing it will cause the game to hard crash.
            // Keep that in mind.
            var c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(134)))
                return;
            c.Index++;
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<int, Player, int>>((id, player) =>
            {
                if (player.HasBuff(ModContent.BuffType<BloodBound>()))
                return ModContent.NPCType<DerellectBoss>();
                return id;
            });

            if (!c.TryGotoNext(i => i.MatchLdcR4(134f)))
                return;
            c.Index++;
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.EmitDelegate<Func<float, Player, float>>((id, player) =>
            {
                if (player.HasBuff(ModContent.BuffType<BloodBound>()))
                return ModContent.NPCType<DerellectBoss>();
                return id;
            });
        }
        */

        // Freeze most UI elements
        public static void FreezeIcon(Terraria.UI.On_UIElement.orig_Draw orig, UIElement self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
            float wid = self.GetOuterDimensions().Width;
            // Elements larger than 500 pixels aren't frozen (or else you get a giant ice block covering your screen)
            // Fannies don't show up if disabled
            if (wid < 500 && !((self is ScreenHelper || self is ScreenHelperTextbox) && !ScreenHelperManager.screenHelpersEnabled) && (CalRemix.CalVal != null && self.GetType() != CalRemix.calvalFanny && self.GetType() != CalRemix.calvalFannyBox))
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("CalamityMod/Projectiles/Magic/IceBlock", AssetRequestMode.ImmediateLoad).Value, self.GetOuterDimensions().ToRectangle(), Color.White * MathHelper.Lerp(0.8f, 0.2f, wid / 500));
            }
        }


        public static bool SendToFannyDimension(Terraria.On_IngameOptions.orig_DrawLeftSide orig, SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale, float maxscale, float scalespeed)
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
            if (Main.LocalPlayer.TryGetModPlayer(out CalRemixPlayer crp))
            {
                if (crp.oxygenSoul)
                {
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
                    SoundStyle newst = new SoundStyle();
                    newst = style with { Pitch = 1f };
                    return PlayInerr(in newst, position, updateCallback2);
                }
                return orig(self, ref style, position, updateCallback);
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

    public class RemixButton : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = new Vector2(dimensions.X + 4f, dimensions.Y + 4f);
            Color color = !CalRemixWorld.stratusDungeonDisabled ? Color.White : Color.Gray;
            spriteBatch.Draw(ModContent.Request<Texture2D>("CalRemix/icon_small").Value, position, color);
            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 1, 1);
            if (maus.Intersects(dimensions.ToRectangle() with { Height = (int)(dimensions.Height * 2), Width = (int)(dimensions.Width * 2) }))
            {
                Vector2 startpos = Main.MouseScreen + new Vector2(22, 22);
                string desc = "Stratus Dungeon generation: ";
                float descWidth = FontAssets.MouseText.Value.MeasureString(desc).X;
                Utils.DrawBorderString(spriteBatch, desc, startpos, Color.White);
                Utils.DrawBorderString(spriteBatch, !CalRemixWorld.stratusDungeonDisabled ? "Enabled" : "Disabled", startpos + Vector2.UnitX * descWidth, !CalRemixWorld.stratusDungeonDisabled ? Color.Green : Color.Red);
            }
        }
    }
}
