using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Cooldowns;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Accessories
{
    public class MagnaCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Magna Core");
            /* Tooltip.SetDefault("30% increased magic damage, 25% increased magic critical strike chance, and 100% decreased mana usage\n"+
            "+ 250 max mana\n"+
            "Increases pickup range for mana stars\n" +
            "Using a Mana Potion causes a burst of granite energy to assault enemies"); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 5));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.GetModPlayer<CalRemixPlayer>().marnite = true;
            ModContent.GetModItem(ModContent.ItemType<EtherealTalisman>()).UpdateAccessory(player, hideVisual);
            ModContent.GetModItem(ModContent.ItemType<UnstableGraniteCore>()).UpdateAccessory(player, hideVisual);

            player.statManaMax2 += 250;
            player.GetDamage<MagicDamageClass>() += 0.3f;
            player.manaCost *= 0f;
            player.GetCritChance<MagicDamageClass>() += 25;


            if (Main.LocalPlayer.HasBuff(BuffID.ManaSickness) && !player.GetModPlayer<CalRemixPlayer>().godfather)
            {
                if (!Main.LocalPlayer.HasCooldown(MagnaCoreCooldown.ID) && player.active)
                {
                    float variance = MathHelper.TwoPi / 8;
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 velocity = new Vector2(0f, 10f);
                        velocity = velocity.RotatedBy(variance * i);
                        int p = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, velocity, ModContent.ProjectileType<MarniteOrb>(), 20000, 0, player.whoAmI);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = 20000;
                    }
                    Main.LocalPlayer.AddCooldown(MagnaCoreCooldown.ID, CalamityUtils.SecondsToFrames(20));
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EtherealTalisman>(1).
                AddIngredient<UnstableGraniteCore>(1).
                AddIngredient<SupremeManaPotion>(20).
                AddIngredient(ItemID.GraniteBlock, 50).
                AddIngredient(ItemID.MarbleBlock, 50).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
