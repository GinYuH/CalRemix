using CalamityMod.Projectiles.Rogue;
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
    public class CopperStormbow : StormbowAbstract
    {
        public override int damage => 6;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CopperBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Amethyst, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class TinStormbow : StormbowAbstract
    {
        public override int damage => 7;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TinBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Topaz, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class IronStormbow : StormbowAbstract
    {
        public override int damage => 5;
        public override int crit => 20;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IronBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class LeadStormbow : StormbowAbstract
    {
        public override int damage => 6;
        public override int crit => 22;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LeadBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class SilverStormbow : StormbowAbstract
    {
        public override int damage => 8;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SilverBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Sapphire, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class TungstenStormbow : StormbowAbstract
    {
        public override int damage => 9;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TungstenBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Emerald, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class GoldStormbow : StormbowAbstract
    {
        public override int damage => 10;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GoldBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Ruby, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class PlatinumStormbow : StormbowAbstract
    {
        public override int damage => 11;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PlatinumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.Diamond, 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }

    public class SpacePinball : StormbowAbstract
    {
        public override int damage => 34;
        public override int useTime => 12;
        public override SoundStyle useSound => SoundID.Item12;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.GreenLaser };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Blue;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.ManaCrystal, 2).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class WorldFeeder : StormbowAbstract
    {
        public override int damage => 32;
        public override int useTime => 12;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.UnholyArrow };
        public override int arrowAmount => 1;
        public override OverallRarity overallRarity => OverallRarity.Blue;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DemoniteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class OfVericourse : StormbowAbstract
    {
        public override int damage => 46;
        public override int useTime => 22;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.UnholyArrow };
        public override int arrowAmount => 4;
        public override OverallRarity overallRarity => OverallRarity.Blue;
    }
    public class Fruminous : StormbowAbstract
    {
        public override int damage => 57;
        public override int crit => 16;
        public override int useTime => 28;
        public override SoundStyle useSound => SoundID.Item5;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.Hellwing };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Orange;
        // these dont use my awesome system bcuz hellwing bats are fucking weird and i hate them
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 cursorPos = player.Center; // lol
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
                cursorPos.Y += Main.rand.Next(-60, 61);

                int ai1 = -10;

                // if to right of player, right direct all projectiles. else, left
                if (Main.MouseWorld.X - player.Center.X > 0)
                {
                    cursorPos.X -= 1500;
                    speedX += 50;
                    ai1 = 10;
                }
                else
                {
                    cursorPos.X += 1500;
                    speedX -= 50;
                }

                int batOffset = Main.rand.Next(-3, 3);

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, ai1, batOffset);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MoltenFury, 2).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.HellstoneBar, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    #endregion
    #region HM
    public class CobaltStormbow : StormbowAbstract
    {
        public override int damage =>54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<CobaltKunaiProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CobaltBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class PalladiumStormbow : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<PalladiumJavelinProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PalladiumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class MythrilStormbow : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<MythrilKnifeProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MythrilBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class OrichalcumStormbow : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<OrichalcumSpikedGemstoneProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.OrichalcumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class AdamantiteStormbow : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<AdamantiteThrowingAxeProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TitaniumBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class TitaniumStormbow : StormbowAbstract
    {
        public override int damage => 54;
        public override int crit => 12;
        public override int useTime => 18;
        public override List<int> projsToShoot => new List<int>() { ModContent.ProjectileType<TitaniumShurikenProjectile>() };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.LightRed;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AdamantiteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }

    public class ExcaliburStormbow : StormbowAbstract
    {
        public override int damage => 100;
        public override int useTime => 62;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.HolyArrow };
        public override int arrowAmount => 12;
        public override OverallRarity overallRarity => OverallRarity.Pink;
    }
    #region Chlorophyte
    public abstract class ChlorophyteStormbow : StormbowAbstract
    {
        public override int damage => 74;
        public override int crit => 12;
        public override int useTime => 14;
        public override List<int> projsToShoot => new List<int>() { ProjectileID.ChlorophyteArrow, ProjectileID.ChlorophyteBullet, ProjectileID.ChlorophyteOrb, ProjectileID.SporeCloud };
        public override int arrowAmount => 5;
        public override OverallRarity overallRarity => OverallRarity.Lime;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ChlorophyteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public abstract class ChlorophyteStormbowSword : ChlorophyteStormbow
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.Swing;
        }
    }

    // sprited by split
    public class ChlorophyteStormbowTheFirst : ChlorophyteStormbow { }
    // sprited by split
    public class ChlorophyteStormbowTheSecond : ChlorophyteStormbowSword { }
    // sprited by mochi
    public class ChlorophyteStormbowTheThird : ChlorophyteStormbow { }
    // sprited by moonbee
    public class ChlorophyteStormbowTheFourth : ChlorophyteStormbow { }
    // sprited by me!!!! caligulasaquarium. so its the best. yep
    public class ChlorophyteStormbowTheFifth : ChlorophyteStormbow { }
    // sprited by the pooper
    public class ChlorophyteStormbowTheSixth : ChlorophyteStormbow { }
    // sprited by yuh
    public class ChlorophyteStormbowTheSeventh : ChlorophyteStormbow { }
    // sprited by spoop
    public class ChlorophyteStormbowTheEighth : ChlorophyteStormbow { }
    // sprited by babybluesheep
    public class ChlorophyteStormbowTheNineth : ChlorophyteStormbow { }
    // sprited by willowmaine
    public class ChlorophyteStormbowTheTenth : ChlorophyteStormbow { }
    // sprited by ibanplay
    public class ChlorophyteStormbowTheEleventh : ChlorophyteStormbowSword { }
    // sprited by delly
    public class ChlorophyteStormbowTheTwelvth : ChlorophyteStormbowSword { }
    // sprited by crimsoncb
    public class ChlorophyteStormbowTheThirteenth : ChlorophyteStormbow { }
    #endregion
    #endregion
}
