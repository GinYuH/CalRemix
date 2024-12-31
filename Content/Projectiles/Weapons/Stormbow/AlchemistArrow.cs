using CalRemix.Content.Items.ZAccessories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons.Stormbow
{
    public class AlchemistArrow : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Ranged/PlagueArrow";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.VenomArrow);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int awesomeRandomNumber = Main.rand.Next(0, BuffLoader.BuffCount);
            target.AddBuff(awesomeRandomNumber, 180);
        }
        public override Color? GetAlpha(Color lightColor) => new Color(Main.rand.Next(0, 255), Main.rand.Next(0, 255), Main.rand.Next(0, 255), 0);
    }
}
