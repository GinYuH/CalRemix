using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
    public class WarBolt : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warbolt");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.AmethystBolt);
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone)];
            d.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 600);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 600);
        }
    }
}