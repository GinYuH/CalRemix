using System;
using System.Collections.Generic;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Rogue;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public abstract class ChlorophyteStormbow : AbstractStormbowClass
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
}