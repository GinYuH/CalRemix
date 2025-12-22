using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.World;
using CalRemix.Content.NPCs.Subworlds.GreatSea;
using CalRemix.Content.NPCs.Subworlds.SingularPoint;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class AnomalyOrbuleBoss : ModProjectile
    {
        public override string Texture => "CalRemix/Content/NPCs/Subworlds/SingularPoint/AnomalyThree_Segment";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 420;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0;
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            Lighting.AddLight(Projectile.Center, 0, 1, 1);
            NPC hostNPC = Main.npc[(int)Projectile.ai[2]];
            if (!hostNPC.active || hostNPC.type != ModContent.NPCType<AnomalyThree>())
            {
                Projectile.Kill();
                return;
            }
            switch (Projectile.ai[0])
            {
                case 0:
                    {
                        float degPerSecond = MathHelper.Lerp(MathHelper.ToRadians(20), MathHelper.ToRadians(90), Utils.GetLerpValue(0, 180, Projectile.ai[1], true)) / 60f;
                        float dist = MathHelper.Lerp(0, Projectile.localAI[2], Utils.GetLerpValue(0, 120, Projectile.ai[1], true));
                        Projectile.localAI[1] += degPerSecond * Projectile.direction;
                        Projectile.Center = hostNPC.Center + Vector2.UnitY.RotatedBy(Projectile.localAI[1]) * dist;   
                        if (Projectile.timeLeft > 60)
                        {
                            Projectile.Opacity = Utils.GetLerpValue(0, 60, Projectile.ai[1], true);
                        }
                        else
                        {
                            Projectile.Opacity = Utils.GetLerpValue(0, 60, Projectile.timeLeft, true);
                        }
                    }
                    break;
                case 1:
                    {
                        if (Projectile.localAI[1] <= 0)
                        {
                            if (Projectile.Center.Y > SPSky.SafeArea.Bottom || Projectile.Center.Y < SPSky.SafeArea.Top || CalamityUtils.ParanoidTileRetrieval((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).HasTile)
                            {
                                SoundEngine.PlaySound(AnomalyDisciple3.OrbuleSound with { Pitch = 0.5f }, Projectile.Center);
                                Projectile.velocity.Y *= -1;
                                if (CalamityWorld.revenge)
                                {
                                    Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
                                }
                                GeneralParticleHandler.SpawnParticle(new PulseRing(Projectile.Center, Vector2.Zero, Color.SeaGreen * 0.6f, 0, 0.4f, 20));
                                Projectile.localAI[1] = 10;
                            }
                            if (Projectile.Center.X > SPSky.SafeArea.Right || Projectile.Center.X < SPSky.SafeArea.Left)
                            {
                                SoundEngine.PlaySound(AnomalyDisciple3.OrbuleSound with { Pitch = 0.5f }, Projectile.Center);
                                Projectile.velocity.X *= -1;
                                if (CalamityWorld.revenge)
                                {
                                    Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
                                }
                                GeneralParticleHandler.SpawnParticle(new PulseRing(Projectile.Center, Vector2.Zero, Color.SeaGreen * 0.6f, 0, 0.4f, 20));
                                Projectile.localAI[1] = 10;
                            }
                        }
                        else
                        {
                            Projectile.localAI[1]--;
                        }
                    }
                    break;
            }
        }

        public override void OnKill(int timeLeft)
        {
            GeneralParticleHandler.SpawnParticle(new BloomParticle(Projectile.Center, Vector2.Zero, Color.SeaGreen, 0.2f, 0.8f, 30));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, Color.SeaGreen * 0.6f, 0, bloom.Size() / 2, Projectile.scale * 0.6f, 0);
            Main.spriteBatch.ExitShaderRegion();
            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, TextureAssets.Projectile[Type].Value.Size() / 2, Projectile.scale, 0);
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return Projectile.Opacity >= 1;
        }
    }
}