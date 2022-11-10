using CalamityMod.CalPlayer;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using CalRemix.Buffs;
using Terraria.ModLoader;
using CalamityMod.Particles;
using Terraria.Audio;

namespace CalRemix.Projectiles
{
    public class CosmicElementalMinion : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/NormalNPCs/CosmicElemental";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Elemental");
            Main.projFrames[Projectile.type] = 11;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
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
            Projectile.penetrate = -1;
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            player.AddBuff(ModContent.BuffType<CosmicElementalBuff>(), 3600);
            bool flag64 = Projectile.type == ModContent.ProjectileType<CosmicElementalMinion>();
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.cosmele = false;
                }
                if (modPlayer.cosmele)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 120 && Projectile.localAI[0] % 120 == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30)), ModContent.ProjectileType<CalamityMod.Projectiles.Melee.GalaxyStar>(), Projectile.damage, 0f, Projectile.owner);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = Projectile.originalDamage;
                    Main.projectile[p].DamageType = DamageClass.Summon;
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 6)
                Projectile.frame = 0;

            Projectile.ChargingMinionAI(600f, 800f, 1200f, 150f, 0, 40f, 8f, 4f, new Vector2(0f, -60f), 40f, 9.5f, true, true);
            Projectile.rotation = Projectile.velocity.X * 0.1f;
        }
    }
}
