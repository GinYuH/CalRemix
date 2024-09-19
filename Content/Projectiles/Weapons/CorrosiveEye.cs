using CalamityMod;
using CalamityMod.Dusts;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class CorrosiveEye : ModProjectile
    {
        public ref float Time => ref Projectile.ai[0];
        public ref float Attack => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrosive Eye");
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            CheckActive();
            Time++;
            Projectile.Center = Owner.Center - new Vector2((float)Math.Cos(MathHelper.ToRadians(Time)) * 48, (float)Math.Sin(MathHelper.ToRadians(Time)) * 48);
            if (Time > 360)
                Time = 0;
            int index = Projectile.FindTargetWithLineOfSight();
            bool hasTarget = false;
            if (index.WithinBounds(Main.maxNPCs))
            {
                if (Main.npc[index] != null)
                {
                    Projectile.rotation = Projectile.DirectionTo(Main.npc[Projectile.FindTargetWithLineOfSight()].Center).ToRotation();
                    hasTarget = true;
                }
            }
            if (!hasTarget)
            {
                Projectile.rotation = Projectile.DirectionFrom(Owner.Center).ToRotation();
                Attack = 0;
            }
            else
            {
                Attack++;
                if (Attack > 90)
                {
                    Attack = 0;
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, (Main.npc[index].Center - Projectile.Center) / 10, ModContent.ProjectileType<PungentShot>(), Projectile.damage / 2, Projectile.knockBack);
                    proj.DamageType = DamageClass.Summon;
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.SulphurousSeaAcid, Scale: 1f + Main.rand.NextFloat());
                    dust.velocity = Projectile.rotation.ToRotationVector2() * Main.rand.Next(2, 6);
                    dust.noGravity = false;
                }
            }
        }
        private void CheckActive()
        {
            Owner.AddBuff(ModContent.BuffType<CorrosiveEyeBuff>(), 3600);
            if (Projectile.type != ModContent.ProjectileType<CorrosiveEye>())
                return;
            if (Owner.dead)
                Owner.GetModPlayer<CalRemixPlayer>().corrosiveEye = false;
            if (Owner.GetModPlayer<CalRemixPlayer>().corrosiveEye)
                Projectile.timeLeft = 2;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}
