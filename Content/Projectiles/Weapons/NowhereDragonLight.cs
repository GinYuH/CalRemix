using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Buffs;
using Terraria.Audio;
using System;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class NowhereDragonLight : ModProjectile
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
            Projectile.minionSlots = 0.5f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            player.AddBuff(ModContent.BuffType<NowhereDragonBuff>(), 3600);
            bool flag64 = Projectile.type == ModContent.ProjectileType<NowhereDragonLight>();
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.nowhereDragons = false;
                }
                if (modPlayer.nowhereDragons)
                {
                    Projectile.timeLeft = 2;
                }
            }

            NPC targ = CalamityUtils.MinionHoming(Projectile.Center, 2300, Main.player[Projectile.owner]);
            if (targ != null && targ.active)
            {
                NPC npc = targ;
                Projectile.direction = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.spriteDirection = npc.Center.X - Projectile.Center.X >= 0 ? -1 : 1;
                Projectile.localAI[1]++;
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.Center = Vector2.Lerp(Projectile.Center, targ.Center + new Vector2(-300, -300), 0.1f);
                    if (Projectile.localAI[1] % 20 == 0)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemShadowbeamStaff with { Pitch = 0.4f }, Projectile.Center);
                        Vector2 dest = npc.Center - Projectile.Center;
                        dest.Normalize();
                        Vector2 laserVel = dest * 20;
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, laserVel, ModContent.ProjectileType<AngelicBeam>(), Projectile.damage, 0f, Projectile.owner);
                        Projectile.netUpdate = true;
                    }
                }
            }
            else
            {
                Projectile.FloatingPetAI(false, 0.02f);
                Projectile.localAI[0] = 0;
            }
            CalamityUtils.MinionAntiClump(Projectile);

            if (Projectile.frameCounter++ % 8 == 0)
            {
                Projectile.frame++;
            }
            if (Projectile.frame > 1)
                Projectile.frame = 0;
        }
    }
}
