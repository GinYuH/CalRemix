using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
        public override SoundStyle useSound => SoundID.NPCHit8;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.SandBallGun };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Green;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.MowTheLawn;
        }
    }
    // Crabulon
    public class Mutation : StormbowAbstract
    {
        public override int damage => 20;
        public override int useTime => 22;
        public override SoundStyle useSound => SoundID.Item97;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<BouncingMushroom>() };
        public override int arrowAmount => 2;
        public override OverallRarity overallRarity => OverallRarity.Green;
    }
    // evil2s
    public class AmalgamatedRain : StormbowAbstract
    {
        public override int damage => 26;
        public override int useTime => 22;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<Shaderain>(), ModContent.ProjectileType<IchorSpark>() };
        public override int arrowAmount => 2;
        public override OverallRarity overallRarity => OverallRarity.Green;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RottenMatter>(25).
                AddIngredient<BloodSample>(25).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    // acidsighter
    public class Scatteract : StormbowAbstract
    {
        public override int damage => 15;
        public override int useTime => 5;
        public override SoundStyle useSound => SoundID.Item13;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<AcidGunStream>() };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Green;
    }
    #endregion
    #region HM
    // GiantclamHM
    public class ClamSlamSupremeSpecial : StormbowAbstract
    {
        public override int damage => 40;
        public override int useTime => 30;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<ClamCrusherFlail>() };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Pink;
    }
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
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<BrimstoneHomer>() };
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
    // livyatan
    public class Pigeon : StormbowAbstract
    {
        public override int damage => 230;
        public override int useTime => 12;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.SpectreWrath };
        public override int arrowAmount => 3;
        public override OverallRarity overallRarity => OverallRarity.Turquoise;
    }
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
