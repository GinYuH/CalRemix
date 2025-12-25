using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Terraria.Audio;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Materials;
using CalamityMod.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Weapons
{
    public class ShadesBane : ModItem
    {
        int currentDir = 1;

        public static SoundStyle WetSlash = new SoundStyle("CalRemix/Assets/Sounds/WetSlash");

        public static SoundStyle WetSlashBig = new SoundStyle("CalRemix/Assets/Sounds/WetSlashBig");

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 66;
            Item.damage = 310;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.shoot = ModContent.ProjectileType<ShadesBaneHoldout>();
            Item.shootSpeed = 60f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (currentDir == 0)
                currentDir = 1;
            int aivar = currentDir == 5 ? 3 : currentDir == 1 || currentDir == 3 ? 1 : 2;
            int p = Projectile.NewProjectile(source, position, velocity, type, aivar == 3 ? damage * 5 : damage, knockback, player.whoAmI, ai2: aivar);
            if (aivar == 3)
            {
                Main.projectile[p].timeLeft = 120;
            }
            currentDir++;
            if (currentDir >= 6)
                currentDir = 1;

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Mikado>())
                .AddIngredient(ModContent.ItemType<ArchRaptierre>())
                .AddIngredient(ModContent.ItemType<CarnelianWoodSword>())
                .AddIngredient(ModContent.ItemType<VirisiteTear>(), 10)
                .AddIngredient(ModContent.ItemType<VoidInfusedTurnipFruit>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
