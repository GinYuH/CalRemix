﻿using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Hostile;

namespace CalRemix.Content.Items.Weapons
{
    public class Mutagen : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Mutagen");
            // Tooltip.SetDefault("Releases swarms of viruses while shooting spreads of blood cells\n" + $"'A glimpse into a [c/4f1e49:Dark] and [c/d1ed98:Twisted] alternate reality...'");

        }
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.rare = RarityHelper.Pathogen;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemSlimeMountSummon;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 58;
            Item.knockBack = 2f;
            Item.mana = 8;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<FriendlyCell1>();
            Item.shootSpeed = 12;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spawnPos = player.position + velocity * 4.5f;
            for (int i = -2; i < 3; i ++)
            {
                float perShot = MathHelper.PiOver2 / 10;
                Projectile.NewProjectile(source, spawnPos, spawnPos.DirectionTo(Main.MouseWorld).RotatedBy(perShot * i) * velocity.Length(), type, (int)(damage * 0.5f), knockback, player.whoAmI);
            }
            for (int i = 0; i < 5; i++)
            {
                Projectile.NewProjectile(source, spawnPos, Main.rand.NextVector2CircularEdge(velocity.Length(), velocity.Length()), ModContent.ProjectileType<MutantSeeker>(), (int)(damage * 0.25f), knockback, player.whoAmI);
            }
            return false;
        }
    }
}
