using CalamityMod.Projectiles.Rogue;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    #region Pre-HM
    // queen bee
    public class TheQueensKnees : StormbowAbstract
    {
        public override int damage => 10;

        public override int crit => 12;

        public override int useTime => 22;

        public override SoundStyle useSound => SoundID.Zombie125;

        public override List<int> projsToShoot => [ProjectileID.Bee];

        public override int arrowAmount => 22;

        public override OverallRarity overallRarity => OverallRarity.Green;
    }
    // deerclops
    public class DeerdalusStormclops : StormbowAbstract
    {
        public override int damage => 22;
        public override int crit => 12;
        public override int useTime => 44;
        public override SoundStyle useSound => SoundID.DeerclopsScream;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.Blizzard };
        public override int arrowAmount => 20;
        public override OverallRarity overallRarity => OverallRarity.Green;
        // stormclops retains the older stormbow code to retain its intended effect
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X + (Main.MouseWorld.X - player.Center.X);
                cursorPos.Y = player.Center.Y - 800 - (100 * (i * 0.75f));
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;
                speedY += 15;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-100, 101);
                cursorPos.Y += Main.rand.Next(-60, 61);

                // if to right of player, right direct all projectiles. else, left
                if (Main.MouseWorld.X - player.Center.X > 0)
                {
                    cursorPos.X -= 200;
                    speedX += 5;
                }
                else
                {
                    cursorPos.X += 200;
                    speedX -= 5;
                }

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }
    }
    // skeletron
    public class SkullStormbow : StormbowAbstract
    {
        public override int damage => 22;

        public override int crit => 12;

        public override int useTime => 44;

        public override SoundStyle useSound => SoundID.Item8;

        public override List<int> projsToShoot => [837, 270, ProjectileID.Bone, ProjectileID.BoneArrow, ProjectileID.BoneDagger, ProjectileID.BoneGloveProj];

        public override int arrowAmount => 20;

        public override OverallRarity overallRarity => OverallRarity.Green;
    }
    // wall of flesh
    #endregion
    #region HM
    // ogre
    public class BoringStormbow : StormbowAbstract
    {
        public override int damage => 57;
        public override int crit => 87;
        public override int useTime => 22;
        public override SoundStyle useSound => SoundID.NPCHit1;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.DD2OgreSpit };
        public override int arrowAmount => 6;
        public override OverallRarity overallRarity => OverallRarity.Pink;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 idealLocation = player.Center;
                idealLocation.Y -= 800 - (100 * (i * 0.75f));
                float speedX = Main.rand.Next(-90, 121) * 0.02f;
                float speedY = Main.rand.Next(-90, 121) * 0.02f;
                speedY += 15;

                // arrow position noise pass
                idealLocation.X += Main.rand.Next(-90, 91);
                idealLocation.Y += Main.rand.Next(-90, 91);

                int projectile = Projectile.NewProjectile(source, idealLocation.X, idealLocation.Y, speedX, speedY, type, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }
    }
    // plantera
    public class AprilShowers : StormbowAbstract
    {
        public override int damage => 110;

        public override int useTime => 40;

        public override SoundStyle useSound => SoundID.Item17;

        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<TbcProj>() };

        public override int arrowAmount => 5;

        public override OverallRarity overallRarity => OverallRarity.Green;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
        }
    }
    // golem
    public class LihzahrdianSunMask : StormbowAbstract
    {
        public override int damage => 107;

        public override int useTime => 27;

        public override SoundStyle useSound => SoundID.Item12;

        public override List<int> projsToShoot => new List<int>() { ProjectileID.BallofFire };

        public override int arrowAmount => 1;

        public override OverallRarity overallRarity => OverallRarity.Green;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
    }
    // empress of light
    // duke fishron
    public class DukeFishrod : StormbowAbstract
    {
        public override int damage => 200;

        public override int useTime => 30;

        public override SoundStyle useSound => SoundID.Item39;

        public override List<int> projsToShoot => new List<int>() { ProjectileID.MiniSharkron };

        public override int arrowAmount => 3;

        public override OverallRarity overallRarity => OverallRarity.Green;

        public override void SetDefaults()
        {
        base.SetDefaults();
        Item.useStyle = ItemUseStyleID.Swing;
        }
    }
    // Martian Saucer
    public class LastStand : StormbowAbstract
    {
        public override int useTime => 15;

        public override int damage => 100;

        public override SoundStyle useSound => SoundID.Item15;

        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<UFOSmall>(), ModContent.ProjectileType<UFOLarge>() };

        public override int arrowAmount => 1;

        public override OverallRarity overallRarity => OverallRarity.Green;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
    }

    // moon lord 1
    public class Moonfall : StormbowAbstract
    {
        public override int damage => 1000;

        public override int useTime => 150;

        public override SoundStyle useSound => SoundID.Item12;

        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<FallingMoon>() };

        public override int arrowAmount => 1;

        public override OverallRarity overallRarity => OverallRarity.Green;
    }
    // moon lord 2
    #endregion
}
