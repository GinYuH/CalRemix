using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalRemix.Content.Projectiles
{
    public class ChloriumDrillProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Prevents jitter when stepping up and down blocks and half blocks
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = 20;
            Projectile.hide = true; // Hides the projectile, so it will draw in the player's hand when we set the player's heldProj to this one.
        }
    }
}
