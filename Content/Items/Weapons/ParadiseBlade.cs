using CalamityMod.Particles;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class ParadiseBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 3;
            Item.DamageType = DamageClass.Generic;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 28;
            Item.useTurn = true;
            Item.knockBack = 2f;
            Item.UseSound = BetterSoundID.ItemBell;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(100);
            Item.rare = ItemRarityID.Cyan;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                SoundEngine.PlaySound(CalamityMod.Projectiles.Melee.PwnagehammerProj.UseSoundFunny with { Pitch = -1 }, player.Center);
                Projectile.NewProjectile(Item.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<LightOrbGuiding>(), 0, 0, player.whoAmI);
            }
            return true;
        }

        public override void UseItemFrame(Player player)
        {
            if (player.ItemAnimationJustStarted)
                player.itemLocation = (Vector2)player.HandPosition;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Mikado>())
                .AddIngredient(ModContent.ItemType<GildedShard>(), 2)
                .AddIngredient(ItemID.GoldWatch)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Mikado>())
                .AddIngredient(ModContent.ItemType<GildedShard>(), 2)
                .AddIngredient(ItemID.PlatinumWatch)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}