using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Content.Buffs;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class SickCell : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, TorchID.Green);
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bool isMinion = Projectile.type == ModContent.ProjectileType<SickCell>();
            player.AddBuff(ModContent.BuffType<SickCellBuff>(), 3600);
            if (isMinion)
            {
                if (player.dead)
                {
                    modPlayer.sickcell = false;
                }
                if (modPlayer.sickcell)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Projectile.rotation += 0.075f;

            if (Projectile.ai[0] <= -1)
            {
                Projectile.localAI[0] += 0.03f;
                NPC n = CalamityUtils.MinionHoming(Projectile.Center, 2100, player);
                Projectile.scale = 1 + Math.Abs(MathF.Sin(Projectile.localAI[0])) * 0.3f;
                if (n != null && n.active && n.life > 0)
                {
                    Projectile.velocity = Projectile.DirectionTo(n.Center) * 10;
                }
                else
                {
                    Projectile.position = Vector2.Lerp(Projectile.position, player.Top - Vector2.UnitY * 22, 0.3f);
                    Projectile.velocity = Vector2.Zero;
                }
            }
            else
            {
                Projectile p = Main.projectile[(int)Projectile.ai[0]];
                if (p != null && p.active && p.type == Type)
                {
                    Projectile.damage = (int)(Projectile.Distance(p.Center) * 0.4f * player.GetDamage<SummonDamageClass>().Multiplicative);
                    Projectile.position = new Vector2(p.position.X - Projectile.ai[1], p.position.Y - Projectile.ai[2]);


                    NPC n = CalamityUtils.MinionHoming(Projectile.Center, 2100, player);
                    if (n != null)
                    {
                        Projectile.localAI[0]++;
                        if (Projectile.localAI[0] % 30 == 0)
                        {
                            int pe = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.DirectionTo(n.Center) * 10, Projectile.DirectionTo(n.Center) * 10, ProjectileID.BloodRain, Projectile.damage, Projectile.knockBack, Projectile.owner);
                            Main.projectile[pe].DamageType = DamageClass.Summon;
                            Main.projectile[pe].rotation = Main.projectile[pe].velocity.ToRotation() - MathHelper.PiOver2;
                            Main.projectile[pe].timeLeft = 22;
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrainRot>(), 60);
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 60);
        }
    }
}
