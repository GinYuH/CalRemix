﻿using System;
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
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public abstract class ChlorophyteStormbow : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            Item.width = 20;
            Item.height = 46;
            Item.damage = 74;
            Item.crit = 12;
            Item.useTime = 14;
            Item.useAnimation = 14;

            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileID.ChlorophyteArrow;

            Item.SetNameOverride("Chlorophyte Sporebow");
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // big govt secret: this is actually just a really edited undines retribution. but dont tell anyone that
            // u can edit the i < whatever for extra arrows lool. lol. haha lol
            for (int i = 0; i < 5; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X + (Main.MouseWorld.X - player.Center.X);
                cursorPos.Y = player.Center.Y - 800 - (100 * (i * 0.75f));
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;
                speedY += 15;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
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

                int projToShoot = ProjectileID.ChlorophyteArrow;
                int awesomeRandomNumber = Main.rand.Next(0, 4);
                // if 0, chloro arrow
                if (awesomeRandomNumber == 1)
                {
                    projToShoot = ProjectileID.ChlorophyteBullet;
                }
                else if (awesomeRandomNumber == 2)
                {
                    projToShoot = ProjectileID.ChlorophyteOrb;
                }
                else if (awesomeRandomNumber == 3)
                {
                    projToShoot = ProjectileID.SporeCloud;
                }

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, projToShoot, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }

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
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            Item.width = 20;
            Item.height = 46;
            Item.damage = 74;
            Item.crit = 12;
            Item.useTime = 14;
            Item.useAnimation = 14;

            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileID.ChlorophyteArrow;

            Item.SetNameOverride("Chlorophyte Sporebow");

            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
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
}