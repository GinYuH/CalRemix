using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SlimeGod;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.NPCs.Providence;
using CalamityMod.Events;
using System;
using Terraria.GameContent;
using System.IO;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod.Projectiles.Melee;
using Terraria.Graphics.Shaders;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Resprites
{
    public class RespriteMaster : GlobalNPC
    {
        private bool useDefenseFrames;
        private int frameUsed;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCType<SlimeGodCore>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/SlimeGodCore");
                TextureAssets.NpcHeadBoss[npc.GetBossHeadTextureIndex()] = Request<Texture2D>("CalRemix/Resprites/SlimeGod/SlimeGodCore_Head_Boss");
            }
            else if (npc.type == NPCType<Eidolist>())
            {
                TextureAssets.Npc[npc.type] = Request<Texture2D>("CalRemix/Resprites/Eidolist");
            }
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (npc.type == NPCType<Providence>() && !Main.dayTime)
            {
                binaryWriter.Write(useDefenseFrames);
                binaryWriter.Write(frameUsed);
                for (int i = 0; i < 4; i++)
                {
                    binaryWriter.Write(npc.Calamity().newAI[i]);
                }
            }
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (npc.type == NPCType<Providence>() && !Main.dayTime)
            {
                useDefenseFrames = binaryReader.ReadBoolean();
                frameUsed = binaryReader.ReadInt32();
                for (int i = 0; i < 4; i++)
                {
                    npc.Calamity().newAI[i] = binaryReader.ReadSingle();
                }
            }
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.type == NPCType<Providence>() && !Main.dayTime)
            {
                if (npc.ai[0] == 2f && npc.ai[0] == 5f)
                {
                    if (!useDefenseFrames)
                        useDefenseFrames = true;
                }
                else
                {
                    if (useDefenseFrames)
                        useDefenseFrames = false;
                    if (frameUsed > 3)
                        frameUsed = 0;
                }
      
            }
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return !(npc.type == NPCType<Providence>() && !Main.dayTime);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCType<Providence>() && (!Main.dayTime || BossRushEvent.BossRushActive))
            {
                Providence prov = npc.ModNPC as Providence;
                float lerpValue = Utils.GetLerpValue(0f, 45f, prov.DeathAnimationTimer, true);
                int lerps = (int)MathHelper.Lerp(1f, 30f, lerpValue);
                for (int i = 0; i < lerps; i++)
                {
                    float lerps2 = MathF.PI * 2f * i * 2f / lerps;
                    Vector2 drawOffset = lerps2.ToRotationVector2() * (float)Math.Sin(lerps2 * 6f + Main.GlobalTimeWrappedHourly * MathF.PI) * ((float)Math.Pow(lerpValue, 3.0) * 50f);
                    Color value = Color.Lerp(Color.White, Color.White * (MathHelper.Lerp(0.4f, 0.8f, lerpValue) / lerps * 1.5f), lerpValue);
                    value.A = 0;
                    drawProvidenceInstance(drawOffset, lerps==1 ? null : new Color?(value));
                }
                void drawProvidenceInstance(Vector2 drawOffset, Color? colorOverride)
                {
                    string textBase = "CalRemix/Resprites/Providence/Providence";
                    string textWings = "CalRemix/Resprites/Providence/Glowmasks/Providence";
                    string textSpike = "CalRemix/Resprites/Providence/Glowmasks/Providence";
                    if (npc.ai[0] == 2f || npc.ai[0] == 5f)
                    {
                        if (useDefenseFrames)
                        {
                            textBase += "DefenseNight";
                            textWings += "DefenseGlowNight";
                            textSpike += "DefenseGlow2Night";
                        }
                        else
                        {
                            textBase += "DefenseAltNight";
                            textWings += "DefenseAltGlowNight";
                            textSpike += "DefenseAltGlow2Night";
                        }
                    }
                    else if (frameUsed == 0)
                    {
                        textWings += "GlowNight";
                        textSpike += "Glow2Night";
                    }
                    else if (frameUsed == 1)
                    {
                        textBase += "AltNight";
                        textWings += "AltGlowNight";
                        textSpike += "AltGlow2Night";
                    }
                    else if (frameUsed == 2)
                    {
                        textBase += "AttackNight";
                        textWings += "AttackGlowNight";
                        textSpike += "AttackGlow2Night";
                    }
                    else
                    {
                        textBase += "AttackAltNight";
                        textWings += "AttackAltGlowNight";
                        textSpike += "AttackAltGlow2Night";
                    }
                    Texture2D body = Request<Texture2D>(textBase).Value;
                    Texture2D wings = Request<Texture2D>(textWings).Value;
                    Texture2D spikes = Request<Texture2D>(textSpike).Value;
                    SpriteEffects effects = npc.spriteDirection==1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                    Vector2 origin = new Vector2(TextureAssets.Npc[npc.type].Value.Width / 2, TextureAssets.Npc[npc.type].Value.Height / Main.npcFrameCount[npc.type] / 2);
                    Vector2 textPos = (npc.Center - screenPos) - (new Vector2(body.Width, body.Height / Main.npcFrameCount[npc.type]) * npc.scale / 2f) + (origin * npc.scale + new Vector2(0f, npc.gfxOffY) + drawOffset);
                    spriteBatch.Draw(body, textPos, npc.frame, colorOverride ?? npc.GetAlpha(drawColor), npc.rotation, origin, npc.scale, effects, 0f);

                    Color color = Color.Lerp(Color.White, Color.Purple, 0.5f) * npc.Opacity;
                    Color color2 = Color.Lerp(Color.White, Color.LightGreen, 0.5f) * npc.Opacity;
                    if (colorOverride.HasValue)
                    {
                        color = colorOverride.Value;
                        color2 = colorOverride.Value;
                    }
                    if (CalamityConfig.Instance.Afterimages)
                    {
                        for (int k = 1; k < 5; k++)
                        {
                            Color color3 = npc.GetAlpha(Color.Lerp(color, Color.White, 0.5f)) * ((5 - k) / 15f);
                            Color color4 = npc.GetAlpha(Color.Lerp(color2, Color.White, 0.5f)) * ((5 - k) / 15f);
                            if (colorOverride.HasValue)
                            {
                                color3 = colorOverride.Value;
                                color4 = colorOverride.Value;
                            }
                            Vector2 position3 = (npc.oldPos[k] + new Vector2(npc.width, npc.height) / 2f - screenPos) - (new Vector2(wings.Width, wings.Height / Main.npcFrameCount[npc.type]) * npc.scale / 2f) + (origin * npc.scale + new Vector2(0f, npc.gfxOffY) + drawOffset);
                            spriteBatch.Draw(wings, position3, npc.frame, color3, npc.rotation, origin, npc.scale, effects, 0f);
                            spriteBatch.Draw(spikes, position3, npc.frame, color4, npc.rotation, origin, npc.scale, effects, 0f);
                        }
                    }
                    spriteBatch.Draw(wings, textPos, npc.frame, color, npc.rotation, origin, npc.scale, effects, 0f);
                    spriteBatch.Draw(spikes, textPos, npc.frame, color2, npc.rotation, origin, npc.scale, effects, 0f);
                }
            }
        }
    }
    public class RespriteItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemType<PearlShard>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/PearlShard");
            }
            else if (item.type == ItemType<Nadir>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Nadir");
            }
            else if (item.type == ItemType<Violence>())
            {
                TextureAssets.Item[item.type] = Request<Texture2D>("CalRemix/Resprites/Violence");
            }
        }
    }
    public class RespriteProj : GlobalProjectile
    {
        private int frameX;
        private int frameY;
        private int MurasamaFrame
        {
            get
            {
                return frameX * 7 + frameY;
            }
            set
            {
                frameX = value / 7;
                frameY = value % 7;
            }
        }

        internal PrimitiveTrail StreakDrawer;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileType<NadirSpear>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/NadirSpear");
            }
            else if (projectile.type == ProjectileType<VoidEssence>())
            {
                TextureAssets.Projectile[projectile.type] = Request<Texture2D>("CalRemix/Resprites/VoidEssence");
            }
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.type == ProjectileType<MurasamaSlash>())
            {
                if (projectile.frameCounter % 3 == 0)
                {
                    MurasamaFrame++;
                    if (frameX >= 2)
                        MurasamaFrame = 0;
                }
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileType<ViolenceThrownProjectile>())
                return false;
            if (projectile.type == ProjectileType<MurasamaSlash>())
                return false;
            return true;
        }
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            if (projectile.type == ProjectileType<MurasamaSlash>())
            {
                Texture2D texture = Request<Texture2D>("CalRemix/Resprites/MurasamaSlash").Value;
                Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, texture.Frame(2, 7, frameX, frameY), Color.White, projectile.rotation, texture.Size() / new Vector2(2f, 7f) * 0.5f, projectile.scale, (projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            if (projectile.type == ProjectileType<ViolenceThrownProjectile>())
            {
                if (StreakDrawer == null)
                    StreakDrawer = new PrimitiveTrail(PWF, PCF, null, GameShaders.Misc["CalamityMod:TrailStreak"]);
                GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(Request<Texture2D>("CalamityMod/ExtraTextures/Trails/FabstaffStreak"));
                Texture2D texture = Request<Texture2D>("CalRemix/Resprites/Violence").Value;
                Vector2[] array = (Vector2[])projectile.oldPos.Clone();
                if (Main.player[projectile.owner].channel)
                {
                    array[0] += (projectile.rotation - MathF.PI / 2f).ToRotationVector2() * -12f;
                    array[1] = array[0] - (projectile.rotation + MathF.PI / 4f).ToRotationVector2() * Vector2.Distance(array[0], array[1]);
                }
                for (int i = 0; i < array.Length; i++)
                    array[i] -= (projectile.oldRot[i] + MathF.PI / 4f).ToRotationVector2() * projectile.height * 0.5f;
                if (projectile.ai[0] > projectile.oldPos.Length)
                    StreakDrawer.Draw(array, projectile.Size * 0.5f - Main.screenPosition, 88);
                for (int j = 0; j < 6; j++)
                {
                    float num = projectile.oldRot[j] - MathF.PI / 2f;
                    if (Main.player[projectile.owner].channel)
                        num += 0.2f;
                    Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.Transparent, 1f - (float)Math.Pow(Utils.GetLerpValue(0f, 6f, j), 1.4)) * projectile.Opacity, num, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0);
                }
            }
        }
        internal float PWF(float ratio)
        {
            return (float)Math.Pow(MathHelper.SmoothStep(0f, 1f, Utils.GetLerpValue(0.01f, 0.04f, ratio)) * (float)Math.Pow(Utils.GetLerpValue(1f, 0.04f, ratio), 0.9), 0.1) * 30f;
        }
        internal Color PCF(float ratio)
        {
            Color color = new Color(255, 145, 115);
            return Color.Lerp(Color.Lerp(Color.Lerp(color, new Color(113, 0, 159), MathHelper.Lerp(0.15f, 0.75f, (float)Math.Cos(Main.GlobalTimeWrappedHourly * -9f + ratio * 6f + 2f) * 0.5f + 0.5f)), Color.DarkRed, 0.5f), color, (float)Math.Pow(ratio, 1.2)) * (float)Math.Pow(1f - ratio, 1.1);
        }
    }
}
