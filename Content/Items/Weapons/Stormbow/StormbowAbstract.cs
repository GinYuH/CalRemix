using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.NPCs;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public abstract class StormbowAbstract : ModItem, ILocalizedModType
    {
        public virtual int damage => 1;
        public virtual int crit => 4;
        public virtual float shootSpeed => 12f;
        public virtual int useTime => 32;
        public virtual SoundStyle useSound => SoundID.Item5;
        public virtual List<int> projsToShoot => new List<int>() { ProjectileID.WoodenArrowFriendly };
        public virtual int arrowAmount => 3;
        public enum OverallRarity
        {
            Gray = -1,
            White = 0,
            Blue = 1,
            Green = 2,
            Orange = 3,
            LightRed = 4,
            Pink = 5,
            LightPurple = 6,
            Lime = 7,
            Yellow = 8,
            Cyan = 9,
            Red = 10,
            Purple = 11,
            Turquoise = 12,
            PureGreen = 13,
            DarkBlue = 14,
            Violet = 15,
            HotPink = 16,
            CalamityRed = 17

        }
        public virtual OverallRarity overallRarity => OverallRarity.White;

        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 3.5f;
            Item.shoot = projsToShoot[0];
            // i dont wanna deal with item sizes man
            Item.width = 22;
            Item.height = 22;

            Item.shootSpeed = shootSpeed;
            Item.damage = damage;
            Item.crit = crit;
            Item.useTime = useTime;
            Item.useAnimation = useTime;
            Item.UseSound = useSound;

            // dogshit
            switch (overallRarity)
            {
                case OverallRarity.Gray:
                    Item.value = 0;
                    Item.rare = ItemRarityID.Gray;
                    break;
                case OverallRarity.White:
                    Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
                    Item.rare = ItemRarityID.White;
                    break;
                case OverallRarity.Blue:
                    Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
                    Item.rare = ItemRarityID.Blue;
                    break;
                case OverallRarity.Green:
                    Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
                    Item.rare = ItemRarityID.Green;
                    break;
                case OverallRarity.Orange:
                    Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
                    Item.rare = ItemRarityID.Orange;
                    break;
                case OverallRarity.LightRed:
                    Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
                    Item.rare = ItemRarityID.LightRed;
                    break;
                case OverallRarity.Pink:
                    Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
                    Item.rare = ItemRarityID.Pink;
                    break;
                case OverallRarity.LightPurple:
                    Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
                    Item.rare = ItemRarityID.LightPurple;
                    break;
                case OverallRarity.Lime:
                    Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
                    Item.rare = ItemRarityID.Lime;
                    break;
                case OverallRarity.Yellow:
                    Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
                    Item.rare = ItemRarityID.Yellow;
                    break;
                case OverallRarity.Cyan:
                    Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
                    Item.rare = ItemRarityID.Cyan;
                    break;
                case OverallRarity.Red:
                    Item.value = CalamityGlobalItem.RarityRedBuyPrice;
                    Item.rare = ItemRarityID.Red;
                    break;
                case OverallRarity.Purple:
                    Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
                    Item.rare = ItemRarityID.Purple;
                    break;
                case OverallRarity.Turquoise:
                    Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
                    Item.rare = ModContent.RarityType<Turquoise>();
                    break;
                case OverallRarity.PureGreen:
                    Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
                    Item.rare = ModContent.RarityType<PureGreen>();
                    break;
                case OverallRarity.DarkBlue:
                    Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
                    Item.rare = ModContent.RarityType<CosmicPurple>();
                    break;
                case OverallRarity.Violet:
                    Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
                    Item.rare = ModContent.RarityType<BurnishedAuric>();
                    break;
                case OverallRarity.HotPink:
                    Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
                    Item.rare = ModContent.RarityType<HotPink>();
                    break;
                case OverallRarity.CalamityRed:
                    Item.value = CalamityGlobalItem.RarityCalamityRedBuyPrice;
                    Item.rare = ModContent.RarityType<CalamityRed>();
                    break;
                default:
                    Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
                    Item.rare = ModContent.RarityType<Turquoise>();
                    break;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // add funny noise to arrow shoot amount
            int arrowAmountNoisy = arrowAmount;
            if (Main.rand.NextBool(3))
            {
                arrowAmountNoisy++;
            }

            ShootArrowsLikeStormbow(player, source, arrowAmountNoisy, projsToShoot);

            return false;
        }

        public void ShootArrowsFromPoint(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 idealLocation, Vector2 extraSpeed)
        {
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 speed = new(Main.rand.Next(-60, 91) * 0.02f, Main.rand.Next(-60, 91) * 0.02f);
                speed += extraSpeed;

                // arrow position noise pass
                idealLocation.X += Main.rand.Next(-60, 61);
                idealLocation.Y += Main.rand.Next(-60, 61);

                int projectile = Projectile.NewProjectile(source, idealLocation.X, idealLocation.Y, speed.X, speed.Y, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
        public void ShootArrowsLikeStormbow(Player player, EntitySource_ItemUse_WithAmmo source, int arrowAmount, List<int> projToShoot)
        {
            // TODO: clean up
            for (int i = 0; i < arrowAmount; i++)
            {
                Vector2 pointPoisition = new Vector2(player.position.X + player.width * 0.5f + (Main.rand.Next(201) * -player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                pointPoisition.X = (pointPoisition.X * 10f + player.Center.X) / 11f + Main.rand.Next(-100, 101);
                pointPoisition.Y -= 150 * i;
                float num102 = Main.mouseX + Main.screenPosition.X - pointPoisition.X;
                float num113 = Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
                if (num113 < 0f)
                {
                    num113 *= -1f;
                }
                if (num113 < 20f)
                {
                    num113 = 20f;
                }
                float num124 = (float)Math.Sqrt(num102 * num102 + num113 * num113);
                num124 = shootSpeed / num124;
                num102 *= num124;
                num113 *= num124;
                float speedX = num102 + Main.rand.Next(-40, 41) * 0.03f;
                float speedY = num113 + Main.rand.Next(-40, 41) * 0.03f;
                speedX *= Main.rand.Next(75, 150) * 0.01f;
                pointPoisition.X += Main.rand.Next(-50, 51);

                int projType = projToShoot[Main.rand.Next(0, projsToShoot.Count)];
                int shotProj = Projectile.NewProjectile(source, pointPoisition.X, pointPoisition.Y, speedX, speedY, projType, damage, 3.5f, player.whoAmI);
                Main.projectile[shotProj].noDropItem = true;
            }
        }
    }
}