using CalamityMod.Buffs.StatDebuffs;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class Cultacean : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 60;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 3f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            player.AddBuff(ModContent.BuffType<CultaceanBuff>(), 3600);
            bool flag64 = Projectile.type == ModContent.ProjectileType<Cultacean>();
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.cultacean = false;
                }
                if (modPlayer.cultacean)
                {
                    Projectile.timeLeft = 2;
                }
            }

            NPC targ = CalamityUtils.MinionHoming(Projectile.Center, 3000, Main.player[Projectile.owner]);
            if (targ != null && targ.active)
            {
                NPC npc = targ;
                Projectile.direction = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.spriteDirection = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.localAI[1]++;
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.Center = Vector2.Lerp(Projectile.Center, targ.Center - Vector2.UnitX.RotatedBy(Projectile.whoAmI % 4 + MathF.Sin(Projectile.localAI[1] * 0.1f + Projectile.whoAmI) * 0.1f) * 400, 0.1f);
                    if (Projectile.localAI[1] % 20 == 0)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemCultistUnused, Projectile.Center);
                        Vector2 dest = npc.Center - Projectile.Center;
                        dest.Normalize();
                        Vector2 laserVel = dest * 20;
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, laserVel, ModContent.ProjectileType<Livyatanado>(), Projectile.damage, 0f, Projectile.owner);
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.frame = 1;
            }
            else
            {
                Projectile.FloatingPetAI(false, 0.02f);
                Projectile.localAI[0] = 0;
                Projectile.frame = 0;
            }
            CalamityUtils.MinionAntiClump(Projectile);
        }
    }
}
