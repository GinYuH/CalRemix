using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.Content.Items.Weapons
{
    public class Delirious : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.UseSound = BetterSoundID.ItemSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 570;
            Item.knockBack = 10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<NauseatingPowder>(), 4).
                AddIngredient(ModContent.ItemType<VoidInfusedStone>(), 12).
                //AddIngredient(ModContent.ItemType<SealedToken>(), 1).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

        public override void HoldItem(Player player)
        {
            player.AddBuff(BuffID.Obstructed, 600);
            player.AddBuff(BuffID.Blackout, 600);
        }
    }
}
