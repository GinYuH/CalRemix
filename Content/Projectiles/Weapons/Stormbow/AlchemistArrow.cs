using CalRemix.Content.Items.ZAccessories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
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
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Main.rand.Next(0, 323)); // 323 is highest dust in vanilla too lazy to figure out how to make it dynamitcally :((
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.scale *= 1.1f;
            }
        }
        public override Color? GetAlpha(Color lightColor) => new Color(Main.rand.Next(0, 255), Main.rand.Next(0, 255), Main.rand.Next(0, 255), 127);
    }
}
