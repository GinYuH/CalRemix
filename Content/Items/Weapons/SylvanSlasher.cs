using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.Content.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod;
using CalamityMod.Items.Placeables.SunkenSea;

namespace CalRemix.Content.Items.Weapons;

public class SylvanSlasher: RogueWeapon
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.damage = 85;
        Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        Item.width = 90;
        Item.height = 90;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.knockBack = 2.25f;
        Item.rare = ItemRarityID.Pink;
        Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
        Item.UseSound = BetterSoundID.ItemGolfClubSwing;
        Item.autoReuse = true;
        Item.shootSpeed = 22f;

    }

    public override void HoldItem(Player player)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<SylvanSlash>()] == 0)
        {
            Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.position, new Vector2(5, 5), ModContent.ProjectileType<SylvanSlash>(), Item.damage, 0);
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<SlickCane>(1).
            AddIngredient(ItemID.LeafWand).
            AddIngredient<EssenceofBabil>(16).
            AddIngredient<SeaPrism>(10).
            AddTile(TileID.MythrilAnvil).
            Register();
    }

}

