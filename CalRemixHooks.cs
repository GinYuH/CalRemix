using Terraria;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;
using CalRemix.NPCs.Bosses.Pyrogen;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalRemix.Scenes;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;

namespace CalRemix
{
    internal class CalRemixHooks : ModSystem
    {
        public override void Load()
        {
            IL.CalamityMod.Events.AcidRainEvent.TryStartEvent += AcidsighterToggle;
            IL.CalamityMod.Events.AcidRainEvent.TryToStartEventNaturally += AcidsighterToggle2;
            Terraria.On_Main.DrawLiquid += DrawTsarBomba;
            //IL_Player.ItemCheck_UseBossSpawners += HookDerellectSpawn;
        }

        public static void DrawTsarBomba(Terraria.On_Main.orig_DrawLiquid orig, Terraria.Main self, bool a, int b, float c, bool d)
        {
            orig(self, a, b, c, d);
            if (CalRemixWorld.roachDuration > 0)
            {
                float duration = CalRemixWorld.ROACHDURATIONSECONDS;

                string bf = "BLACK FRIDAY";
                string mayhem = "ROACH MAYHEM";

                float bfWidth = FontAssets.MouseText.Value.MeasureString(bf).X;
                float mayhemWidth = FontAssets.MouseText.Value.MeasureString(bf).X;

                float bfY = Main.screenHeight * 0.4f;
                float mayhemY = Main.screenHeight * 0.6f;

                Vector2 bfOff = new Vector2(-3000, bfY);
                Vector2 mayhemOff = new Vector2(3000, mayhemY);

                Vector2 bfLocation = new Vector2(Main.screenWidth * 0.34f, bfY) + Main.rand.NextVector2Square(-10, 10);
                Vector2 mayhemLocation = new Vector2(Main.screenWidth * 0.32f, mayhemY) + Main.rand.NextVector2Square(-10, 10);

                float bfCompletion = Utils.GetLerpValue(CalamityUtils.SecondsToFrames(duration - 2), CalamityUtils.SecondsToFrames(duration - 4), CalRemixWorld.roachDuration, true);
                float mayhemCompletion = Utils.GetLerpValue(CalamityUtils.SecondsToFrames(duration - 4), CalamityUtils.SecondsToFrames(duration - 6), CalRemixWorld.roachDuration, true);

                float textOpacity = Utils.GetLerpValue(CalamityUtils.SecondsToFrames(duration - 12), CalamityUtils.SecondsToFrames(duration - 10), CalRemixWorld.roachDuration, true);

                Utils.DrawBorderString(Main.spriteBatch, bf, Vector2.Lerp(bfOff, bfLocation, bfCompletion), Color.Red * textOpacity, (Main.screenWidth / 2 / bfWidth) + 0.1f * (float)Math.Cos(Main.GlobalTimeWrappedHourly * 22));
                Utils.DrawBorderString(Main.spriteBatch, mayhem, Vector2.Lerp(mayhemOff, mayhemLocation, mayhemCompletion), Color.Red * textOpacity, (Main.screenWidth / 2 / mayhemWidth) + 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 22));

                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth * 4, Main.screenHeight * 4), null, Color.Red * 0.22f * Utils.GetLerpValue(CalamityUtils.SecondsToFrames(CalRemixWorld.ROACHDURATIONSECONDS), CalamityUtils.SecondsToFrames(CalRemixWorld.ROACHDURATIONSECONDS - 3), CalRemixWorld.roachDuration, true), 0f, TextureAssets.MagicPixel.Value.Size() * 0.5f, 0, 0f);
                Texture2D explosion = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/RealisticExplosion").Value;
                for (int i = 0; i < RoachScene.explosions.Count; i++)
                {
                    RealisticExplosion v = RoachScene.explosions[i];
                    RoachScene.explosions[i].frameCounter = v.frameCounter + 1;
                    if (v.frameCounter > 3)
                    {
                        v.frameCounter = 0;
                        v.frameX++;
                        if (v.frameX > 5)
                        {
                            v.frameX = 0;
                            v.frameY++;
                        }
                    }
                    if (v.frameY < 3)
                        Main.spriteBatch.Draw(explosion, v.position, explosion.Frame(6, 3, v.frameX, v.frameY), Color.White, 0f, new Vector2(0, explosion.Height * 0.2f), 12f, SpriteEffects.None, 0);
                }
            }
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
    }
}
