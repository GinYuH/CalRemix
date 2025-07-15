using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class OmegaPlumLaser : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BrainScramblerBolt;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BrainScramblerBolt);
            AIType = ProjectileID.BrainScramblerBolt;
            Projectile.penetrate = -1;
            Main.projFrames[Type] = 4;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
    }
}