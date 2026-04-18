using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalRemix.Core.Biomes;
using CalRemix.Core.Biomes.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class Gigamoth : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 270;
            NPC.width = 100;
            NPC.height = 60;
            NPC.defense = 20;
            NPC.lifeMax = 30000;
            NPC.value = Item.buyPrice(silver: 20);
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = BetterSoundID.HitMothron with { Pitch = 0.2f };
            NPC.DeathSound = BetterSoundID.DeathMothron with { Pitch = 0.2f };
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type, ModContent.GetInstance<BigOlBranchesBiome>().Type];
        }

        public override void AI()
        {
            NPC.direction = System.Math.Sign(NPC.velocity.X);
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest();
            if (NPC.HasPlayerTarget)
            {
                Vector2 mauth = NPC.Center + Vector2.UnitX * NPC.spriteDirection * 80;
                Vector2 dist = Main.player[NPC.target].Center - mauth;
                dist.Normalize();
                NPC.velocity = dist * 8f;
                NPC.ai[1]++;
                if (NPC.ai[1] > 51 && NPC.ai[1] % 50 == 0)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemInfernoFork with { Pitch = 0.4f }, NPC.Center);
                    SoundEngine.PlaySound(BetterSoundID.DeathSharkron with { Pitch = 0.4f }, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int ct = 3;
                        for (int i = 0; i < ct; i++)
                        {
                            Vector2 velocity = dist * 16;
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4 / (float)ct, MathHelper.PiOver4 / (float)ct, i / (float)(ct - 1)));

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), mauth, perturbedSpeed, ModContent.ProjectileType<Projectiles.Hostile.RedstoneFireball>(), CalRemixHelper.ProjectileDamage(240, 400), 0f);
                        }
                    }
                }
            }
            else
            {
                NPC.velocity *= 0.98f;
            }
            NPC.velocity.Y -= 3 + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly) * 1;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;

            DrawWing(spriteBatch, screenPos, drawColor, new Vector2(8 * NPC.spriteDirection, -10));
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
            DrawWing(spriteBatch, screenPos, drawColor, new Vector2(-8 * NPC.spriteDirection, -10));
            return false;
        }

        public void DrawWing(SpriteBatch sb, Vector2 screenPos, Color drawColor, Vector2 wingPos)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture + "_Wing").Value;

            Vector2 scale = new Vector2(1, 0.5f + (0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly * 20)));

            sb.Draw(tex, new Vector2(NPC.Center.X + wingPos.X, NPC.Center.Y + wingPos.Y) - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height), NPC.scale * scale, NPC.FlippedEffects(), 0);
        }
    }
}
