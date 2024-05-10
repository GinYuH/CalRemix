using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.NPCs.Eclipse
{
    public class Corruption : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corruption");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 24;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 128;
            NPC.height = 128;
            NPC.lifeMax = 5;
            NPC.damage = 50;
            NPC.defense = 30;
            NPC.knockBackResist = 0f;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
        }

        public override void AI()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 24;
            if (!NPC.AnyNPCs(ModContent.NPCType<Glitch>()))
            {
                NPC.ai[1] = 1;
            }
            NPC.TargetClosest(false);
            if (NPC.HasPlayerTarget)
            {
                NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * 5;
            }
            else
            {
                NPC.ai[1] = 1;
            }
            if (NPC.ai[1] == 1)
            {
                NPC.damage = 0;
                NPC.velocity *= 0.9f;
                NPC.alpha += 5;
                if (NPC.alpha >= 255)
                    NPC.active = false;
            }
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC otherProj = Main.npc[k];
                if (!otherProj.active || k == NPC.whoAmI || otherProj.type != NPC.type)
                    continue;
                float pushForce = 22f;
                float dist = Math.Abs(NPC.position.X - otherProj.position.X) + Math.Abs(NPC.position.Y - otherProj.position.Y);
                if (dist < NPC.width)
                {
                    if (NPC.position.X < otherProj.position.X)
                        NPC.velocity.X -= pushForce;
                    else
                        NPC.velocity.X += pushForce;

                    if (NPC.position.Y < otherProj.position.Y)
                        NPC.velocity.Y -= pushForce;
                    else
                        NPC.velocity.Y += pushForce;
                }
            }
            if (Main.rand.NextBool(300))
            {
                if (Main.rand.NextBool())
                {
                    SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Zombie_" + Main.rand.Next(1, 131)) with { PitchRange = (-1, 1) }, NPC.Center);
                }
                else
                {
                    SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_" + Main.rand.Next(1, 179)) with { PitchRange = (-1, 1) }, NPC.Center);
                }
                int radius = 222;
                NPC.position += new Vector2(Main.rand.Next(-radius, radius), Main.rand.Next(-radius, radius));
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter > 6.0)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement("EXULHG DOLYH.")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = TextureAssets.Npc[Type].Value;
            Vector2 origin = new Vector2((float)(sprite.Width / 2), (float)(sprite.Height / Main.npcFrameCount[NPC.type] / 2));
            if (NPC.ai[0] > 0)
            {
                sprite = ModContent.Request<Texture2D>("CalRemix/NPCs/Eclipse/Corruption" + (NPC.ai[0] + 1)).Value;
            }

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < 22; i += 4)
                {
                    
                    Color extraAfterimageColor = Color.Red;
                    int colorChoice = Main.rand.Next(6);

                    switch (colorChoice)
                    {
                        case 0:
                            extraAfterimageColor = Color.Red;
                            break;
                        case 1:
                            extraAfterimageColor = Color.Green;
                            break;
                        case 2:
                            extraAfterimageColor = Color.Blue;
                            break;
                        case 3:
                            extraAfterimageColor = Color.Yellow;
                            break;
                        case 4:
                            extraAfterimageColor = Color.Orange;
                            break;
                        case 5:
                            extraAfterimageColor = Color.Purple;
                            break;
                    }
                    extraAfterimageColor = Color.Lerp(extraAfterimageColor, Color.White, 0.5f);
                    extraAfterimageColor *= (float)(22 - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
                    offset -= new Vector2((float)sprite.Width, (float)(sprite.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(sprite, offset, NPC.frame, extraAfterimageColor * NPC.Opacity, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
                }
            }
            Vector2 npcOffset = NPC.Center - screenPos;
            spriteBatch.Draw(sprite, npcOffset, NPC.frame, NPC.GetAlpha(Color.White), 0f, origin, 1f, SpriteEffects.None, 1f);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
            target.AddBuff(BuffID.Cursed, CalamityUtils.SecondsToFrames(25));
            target.AddBuff(ModContent.BuffType<Vaporfied>(), CalamityUtils.SecondsToFrames(4));
        }
    }
}