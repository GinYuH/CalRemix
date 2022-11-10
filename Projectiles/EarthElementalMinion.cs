using CalamityMod.CalPlayer;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;
using CalRemix.Buffs;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace CalRemix.Projectiles
{
    public class EarthElementalMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Earth Elemental");
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 240;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            player.AddBuff(ModContent.BuffType<EarthElementalBuff>(), 3600);
            bool flag64 = Projectile.type == ModContent.ProjectileType<EarthElementalMinion>();
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.earthele = false;
                }
                if (modPlayer.earthele)
                {
                    Projectile.timeLeft = 2;
                }
            }
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                Projectile.direction = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.spriteDirection = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] < 300)
                {
                    Projectile.ChargingMinionAI(1600f, 1800f, 2500f, 400f, 1, 30f, 24f, 12f, new Vector2(0f, -60f), 30f, 10f, true, true);
                }
                else if (Projectile.localAI[0] >= 300 && Projectile.localAI[0] < 600)
                {
                    Projectile.localAI[1]++;
                    if (Main.myPlayer == Projectile.owner && Projectile.localAI[1] <= 250)
                    {
                        float lerpx = MathHelper.Lerp(Projectile.position.X, npc.position.X + 200 * npc.direction, 0.2f);
                        float lerpY = MathHelper.Lerp(Projectile.position.Y, npc.position.Y - 100, 0.2f);
                        Projectile.position = new Vector2(lerpx, lerpY);
                        if (Projectile.localAI[1] == 240)
                        {
                            SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
                            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                            Vector2 dest = npc.Center - Projectile.Center;
                            dest.Normalize();
                            Vector2 laserVel = dest * 10;
                            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, laserVel, ModContent.ProjectileType<CalamityMod.Projectiles.Enemy.EarthRockBig>(), Projectile.damage * 8, 0f, Projectile.owner);
                            if (Main.projectile.IndexInRange(p))
                                Main.projectile[p].originalDamage = Projectile.originalDamage;
                            Main.projectile[p].friendly = true;
                            Main.projectile[p].hostile = false;
                            Main.projectile[p].DamageType = DamageClass.Summon;
                            Main.projectile[p].penetrate = 1;
                            Projectile.netUpdate = true;
                        }
                        else if (Projectile.localAI[1] % 20 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
                            Vector2 dest = npc.Center - Projectile.Center;
                            dest.Normalize();
                            Vector2 laserVel = dest * 10;
                            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, laserVel, ModContent.ProjectileType<CalamityMod.Projectiles.Enemy.EarthRockSmall>(), Projectile.damage, 0f, Projectile.owner);
                            if (Main.projectile.IndexInRange(p))
                                Main.projectile[p].originalDamage = Projectile.originalDamage;
                            Main.projectile[p].friendly = true;
                            Main.projectile[p].hostile = false;
                            Main.projectile[p].DamageType = DamageClass.Summon;
                            Main.projectile[p].tileCollide = false;
                            Projectile.netUpdate = true;
                        }
                    }

                }
                else if (Projectile.localAI[0] >= 600 && Projectile.localAI[0] < 900)
                {
                    Projectile.localAI[1] = 0;
                    int num412 = 1;
                    float num413 = 25f;
                    float num414 = 1.2f;
                    float distanceX = 120f;
                    float yoffset = 0f;

                    Vector2 vector40 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    float num415 = npc.position.X + (float)(npc.width / 2) + (float)(num412 * distanceX) - vector40.X;
                    float num416 = npc.position.Y + (float)(npc.height / 2) + yoffset - vector40.Y;
                    float num417 = (float)System.Math.Sqrt(num415 * num415 + num416 * num416);
                    num417 = num413 / num417;
                    num415 *= num417;
                    num416 *= num417;
                    if (Projectile.velocity.X < num415)
                    {
                        Projectile.velocity.X += num414;
                        if (Projectile.velocity.X < 0f && num415 > 0f)
                        {
                            Projectile.velocity.X += num414;
                        }
                    }
                    else if (Projectile.velocity.X > num415)
                    {
                        Projectile.velocity.X -= num414;
                        if (Projectile.velocity.X > 0f && num415 < 0f)
                        {
                            Projectile.velocity.X -= num414;
                        }
                    }
                    if (Projectile.velocity.Y < num416)
                    {
                        Projectile.velocity.Y += num414;
                        if (Projectile.velocity.Y < 0f && num416 > 0f)
                        {
                            Projectile.velocity.Y += num414;
                        }
                    }
                    else if (Projectile.velocity.Y > num416)
                    {
                        Projectile.velocity.Y -= num414;
                        if (Projectile.velocity.Y > 0f && num416 < 0f)
                        {
                            Projectile.velocity.Y -= num414;
                        }
                    }
                }
                else
                {
                    Projectile.localAI[0] = 0;
                }
            }
            else
            {
                Projectile.FloatingPetAI(false, 0.02f);
                Projectile.localAI[0] = 0;
            }
            
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 12)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 5)
                Projectile.frame = 0;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D deusheadsprite;
            deusheadsprite = (ModContent.Request<Texture2D>("CalRemix/Projectiles/EarthElementalArm").Value);
            Rectangle deusheadsquare = new Rectangle(0, 0, deusheadsprite.Width, deusheadsprite.Height);
            Color deusheadalpha = Projectile.GetAlpha(lightColor);
            float rotcounter = Projectile.localAI[0] >= 600 && Projectile.localAI[0] < 900 ? Projectile.localAI[0] * 0.2f : 0;
            Main.EntitySpriteDraw(deusheadsprite, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY - 30), deusheadsquare, deusheadalpha, Projectile.rotation + rotcounter, Utils.Size(deusheadsquare) / 2f, Projectile.scale, Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.localAI[0] >= 600 && Projectile.localAI[0] < 900)
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 120);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Projectile.localAI[0] >= 600 && Projectile.localAI[0] < 900)
                damage *= 6;
        }
    }
}
