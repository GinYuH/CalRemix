using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;

namespace CalRemix.Content.Projectiles.Accessories
{
    public class NucleateGelloMinion : ModProjectile
    {
        bool latching = false;
        NPC ntarget;
        public override string Texture => "CalRemix/Content/Items/Accessories/NucleateGello";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nucleate Gello");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.minionSlots = 0f;
            Projectile.alpha = 75;
            Projectile.aiStyle = ProjAIStyleID.Pet;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            AIType = ProjectileID.BabySlime;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override void AI()
        {
            if (latching)
            {
                if (ntarget.active && ntarget != null && !ntarget.dontTakeDamage && ntarget.chaseable)
                {
                    Projectile.position = ntarget.Center;
                    Projectile.velocity = ntarget.velocity;
                }
                else
                {
                    latching = false;
                }
            }
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bool nubert = Projectile.type == ModContent.ProjectileType<NucleateGelloMinion>();
            if (!modPlayer.nuclegel)
            {
                Projectile.active = false;
                return;
            }
            if (nubert)
            {
                if (player.dead)
                {
                    modPlayer.nuclegel = false;
                }
                if (modPlayer.nuclegel)
                {
                    Projectile.timeLeft = 2;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!latching)
            {
                ntarget = target;
                latching = true;
            }
            target.AddBuff(ModContent.BuffType<Irradiated>(), 600);
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 1200);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}
