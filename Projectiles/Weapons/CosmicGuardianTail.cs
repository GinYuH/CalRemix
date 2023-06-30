using CalamityMod.Buffs.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
namespace CalRemix.Projectiles.Weapons
{
    // HUGE credit to Dozezoze for lending his worm projectile code
    public class CosmicGuardianTail : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/DevourerofGods/CosmicGuardianTail";
        public int segmentIndex = 1;

        public override void SetDefaults()
        {
            Projectile.timeLeft = 10;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.localNPCHitCooldown = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.netImportant = true;

        }
        internal void SegmentMove()
        {
            Player player = Main.player[Projectile.owner];
            var live = false;
            Projectile nextSegment = new Projectile();
            CosmicGuardianHead head = new CosmicGuardianHead();

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                var projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<CosmicGuardianBody>() && projectile.owner == Projectile.owner && projectile.active && Projectile.ai[2] == projectile.ai[2])
                {
                    if (projectile.ModProjectile<CosmicGuardianBody>().segmentIndex == segmentIndex - 1)
                    {
                        live = true;
                        nextSegment = projectile;
                    }
                }
                if (projectile.type == ModContent.ProjectileType<CosmicGuardianHead>() && projectile.owner == Projectile.owner && projectile.active && Projectile.ai[2] == projectile.whoAmI)
                {
                    if (segmentIndex == 1)
                    {
                        live = true;
                        nextSegment = projectile;
                    }
                    head = projectile.ModProjectile<CosmicGuardianHead>();
                }
            }
            if (!live) Projectile.Kill();
            Vector2 destinationOffset = nextSegment.Center - Projectile.Center;
            if (nextSegment.rotation != Projectile.rotation)
            {
                float angle = MathHelper.WrapAngle(nextSegment.rotation - Projectile.rotation);
                //destinationOffset = Projectile.Center.DirectionTo(nextSegment.Center);
                destinationOffset = destinationOffset.RotatedBy(angle * 0.1f);
            }
            Projectile.rotation = destinationOffset.ToRotation();
            if (destinationOffset != Vector2.Zero)
            {
                Projectile.Center = nextSegment.Center - destinationOffset.SafeNormalize(Vector2.Zero) * 20f;
            }
            Projectile.velocity = Vector2.Zero;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<CosmicGuardianHead>()] > 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    var projectile = Main.projectile[i];
                    if (projectile.type == ModContent.ProjectileType<CosmicGuardianHead>() && projectile.owner == Projectile.owner && projectile.active)
                    {
                        projectile.Kill();
                    }
                }
            }
            if (Main.netMode != NetmodeID.Server)
            {
                float randomSpread = Main.rand.Next(-200, 201) / 100f;
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * randomSpread * Main.rand.NextFloat(), ModLoader.GetMod("CalamityMod").Find<ModGore>("DoT2").Type, 1f);
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * randomSpread * Main.rand.NextFloat(), ModLoader.GetMod("CalamityMod").Find<ModGore>("DoT3").Type, 1f);
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * randomSpread * Main.rand.NextFloat(), ModLoader.GetMod("CalamityMod").Find<ModGore>("DoT4").Type, 1f);
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * randomSpread * Main.rand.NextFloat(), ModLoader.GetMod("CalamityMod").Find<ModGore>("DoT5").Type, 1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<CalamityMod.Buffs.DamageOverTime.GodSlayerInferno>(), 180);
    }
}
