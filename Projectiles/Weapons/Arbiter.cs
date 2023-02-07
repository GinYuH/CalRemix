using CalamityMod.Buffs.StatDebuffs;
using CalRemix.Buffs;
using CalRemix.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class Arbiter : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arbiter of Judgement");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 88;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.minionSlots = 0;
            Projectile.scale = 0.75f;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, ModContent.DustType<ValfreyDust>(), Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }
        public override void AI()
        {
            if (Projectile.type != ModContent.ProjectileType<Arbiter>())
                return;
            if (Owner.dead || Owner.HeldItem.DamageType != DamageClass.Summon && Owner.HeldItem.DamageType != DamageClass.SummonMeleeSpeed)
                Projectile.Kill();
            else
                Projectile.timeLeft = 2;
            float num460 = Projectile.position.X;
            float num461 = Projectile.position.Y;
            float num462 = 900f;
            bool flag19 = false;
            int num463 = 500;
            if (Math.Abs(Projectile.Center.X - Main.player[Projectile.owner].Center.X) + Math.Abs(Projectile.Center.Y - Main.player[Projectile.owner].Center.Y) > (float)num463)
            {
                Projectile.ai[0] = 1f;
            }
            if (Projectile.ai[0] == 0f)
            {
                NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
                if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(this))
                {
                    float num464 = ownerMinionAttackTargetNPC2.position.X + (float)(ownerMinionAttackTargetNPC2.width / 2);
                    float num465 = ownerMinionAttackTargetNPC2.position.Y + (float)(ownerMinionAttackTargetNPC2.height / 2);
                    float num466 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num464) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num465);
                    if (num466 < num462 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
                    {
                        num462 = num466;
                        num460 = num464;
                        num461 = num465;
                        flag19 = true;
                    }
                }
                if (!flag19)
                {
                    for (int num467 = 0; num467 < 200; num467++)
                    {
                        if (Main.npc[num467].CanBeChasedBy(this))
                        {
                            float num468 = Main.npc[num467].position.X + (float)(Main.npc[num467].width / 2);
                            float num469 = Main.npc[num467].position.Y + (float)(Main.npc[num467].height / 2);
                            float num470 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num468) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num469);
                            if (num470 < num462 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num467].position, Main.npc[num467].width, Main.npc[num467].height))
                            {
                                num462 = num470;
                                num460 = num468;
                                num461 = num469;
                                flag19 = true;
                            }
                        }
                    }
                }
            }
            if (!flag19)
            {
                float num471 = 8f;
                if (Projectile.ai[0] == 1f)
                {
                    num471 = 12f;
                }
                Vector2 vector39 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num472 = Main.player[Projectile.owner].Center.X - vector39.X;
                float num473 = Main.player[Projectile.owner].Center.Y - vector39.Y - 60f;
                float num474 = (float)Math.Sqrt(num472 * num472 + num473 * num473);
                float num475 = num474;
                if (num474 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                }
                if (num474 > 2000f)
                {
                    Projectile.position.X = Main.player[Projectile.owner].Center.X - (float)(Projectile.width / 2);
                    Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (float)(Projectile.width / 2);
                }
                if (num474 > 70f)
                {
                    num474 = num471 / num474;
                    num472 *= num474;
                    num473 *= num474;
                    Projectile.velocity.X = (Projectile.velocity.X * 20f + num472) / 21f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num473) / 21f;
                }
                else
                {
                    if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                    {
                        Projectile.velocity.X = -0.15f;
                        Projectile.velocity.Y = -0.05f;
                    }
                    Projectile.velocity *= 1.01f;
                }
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 4)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
                if ((double)Math.Abs(Projectile.velocity.X) > 0.2)
                {
                    Projectile.spriteDirection = -Projectile.direction;
                }
                return;
            }
            if (Projectile.ai[1] == -1f)
            {
                Projectile.ai[1] = 17f;
            }
            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] -= 1f;
            }
            if (Projectile.ai[1] == 0f)
            {
                float num476 = 16f;
                Vector2 vector40 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num477 = num460 - vector40.X;
                float num478 = num461 - vector40.Y;
                float num479 = (float)Math.Sqrt(num477 * num477 + num478 * num478);
                if (num479 < 100f)
                {
                    num476 = 10f;
                }
                num479 = num476 / num479;
                num477 *= num479;
                num478 *= num479;
                Projectile.velocity.X = (Projectile.velocity.X * 14f + num477) / 15f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 14f + num478) / 15f;
            }
            else
            {
                if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 10f)
                {
                    Projectile.velocity *= 1.05f;
                }
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
            if (Projectile.frame < 4)
            {
                Projectile.frame = 4;
            }
            if (Projectile.frame > 7)
            {
                Projectile.frame = 4;
            }
            if ((double)Math.Abs(Projectile.velocity.X) > 0.2)
            {
                Projectile.spriteDirection = -Projectile.direction;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_BetsySummon, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, ModContent.DustType<ValfreyDust>(), Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 120);
            target.AddBuff(ModContent.BuffType<ValfreyBurn>(), 120);
        }
    }
}
