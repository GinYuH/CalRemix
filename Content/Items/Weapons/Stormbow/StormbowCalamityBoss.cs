using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Typeless;
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

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Pre-HM
    // desert scourge
    public class Duststorm : StormbowAbstract
    {
        public override int damage => 17;
        public override int useTime => 22;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.SandBallGun };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Green;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.MowTheLawn;
        }
    }
    #endregion
    #region HM
    // cryogen
    public class FrostedFractals : StormbowAbstract
    {
        public override int damage => 34;
        public override int crit => 12;
        public override float shootSpeed => 24;
        public override int useTime => 22;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<MistArrow>() };
        public override OverallRarity overallRarity => OverallRarity.Pink;
    }
    // aquatic scourge
    public class Rainstorm : StormbowAbstract
    {
        public override int damage => 27;
        public override int useTime => 22;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<SulphurousSandBallGun>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Pink;
    }
    // calclone
    public class RisingFire : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<BrimstoneDartMinion>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Lime;
    }
    #region Plaguebringer Goliath
    public class Alchemists3rdTrumpet : StormbowAbstract
    {
        public override int damage => 57;
        public override int crit => 12;
        public override int useTime => 14;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<AlchemistArrow>() };
        public override int arrowAmount => 7;
        public override OverallRarity overallRarity => OverallRarity.Yellow;
    }
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
    #endregion
    #endregion
    #region Post-ML
    // exo mechs
    public class ImpetusTech : StormbowAbstract
    {
        public override int damage => 7526;
        public override int useTime => 240;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<AresGaussNukeProjectile>() };
        public override int arrowAmount => 47;
        public override OverallRarity overallRarity => OverallRarity.Violet;
    }
    #endregion
}
