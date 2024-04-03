using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CalRemix.Projectiles.Weapons;

namespace CalRemix.Items.Weapons
{

    public class ChainSaw : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chain-Saw");
			Tooltip.SetDefault("Hitting enemies stores charge, which progressively blinds you, but increases your crit chance.\nRight-Click with charge to fire a projectile and consume it all\nDoes not benefit from Critical Strike Chance boosts.\n'Chain-saw! Get it?'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

		}

		public override void SetDefaults()
		{
			Item.width = 81;
			Item.height = 30;
			Item.value = 100000;
			Item.rare = 8;
			Item.damage = 125;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.consumable = false;
			Item.autoReuse = true;
			Item.scale = 1.25f;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.knockBack = 6.5f;
			Item.shoot = ModContent.ProjectileType<ChainSawProjectile>();
			Item.shootSpeed = 40f;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.UseSound = SoundID.Item22;
        }

		public override bool CanUseItem(Player player)
		{
            CalRemixPlayer pplayer = player.GetModPlayer<CalRemixPlayer>();
            if (player.altFunctionUse == 2 && pplayer.chainSawCharge <= 0)
			{
				return false;
			}
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

        public override void UpdateInventory(Player player)
        {
            CalRemixPlayer pplayer = player.GetModPlayer<CalRemixPlayer>();
            Item.SetNameOverride("Chain-Saw (" + Math.Round(100f * pplayer.chainSawCharge / pplayer.chainSawChargeMax) + "%)");
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{

            CalRemixPlayer pplayer = player.GetModPlayer<CalRemixPlayer>();
            if (player.altFunctionUse == 2)
			{
				if (pplayer.chainSawCharge >= pplayer.chainSawLevel1)
				{
					ChargeAtkCount += 3;

				}
				if (pplayer.chainSawCharge >= pplayer.chainSawLevel2)
				{
					ChargeAtkCount += 6;

				}
				if (pplayer.chainSawCharge >= pplayer.chainSawLevel3)
				{
					ChargeAtkCount += 9;

				}
				if (pplayer.chainSawCharge >= pplayer.chainSawChargeCritMax)
				{
					ChargeAtkCount += 9;

				}
                if (pplayer.chainSawCharge >= 30*19)
                {
                    ChargeAtkCount += 5;

                }
                if (ChargeAtkCount > 0)
				{
					pplayer.chainSawCharge = 0;
					for (int i = 0; i < ChargeAtkCount; i++)
					{
                        List<int> list = new List<int>() { ModContent.ProjectileType<NystagmusProjectileRed>(), ModContent.ProjectileType<NystagmusProjectileBlue>(), ModContent.ProjectileType<NystagmusProjectileGreen>(), ModContent.ProjectileType<NystagmusProjectileGray>() };
                        Projectile.NewProjectile(source, position, 10 * velocity.SafeNormalize(Vector2.One).RotatedBy(Math.PI/180 * i * (360/ChargeAtkCount)), list[Main.rand.Next(0, 4)], (int)(damage * 1.5f), knockback, player.whoAmI,2,2);
					}
					ChargeAtkCount = 0;
					player.ClearBuff(BuffID.Obstructed);
                    player.ClearBuff(BuffID.Blackout);
                    player.ClearBuff(BuffID.Darkness);
                    return false;
				}


			}
			return true;
		}

		private int ChargeAtkCount = 0;

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            CalRemixPlayer pplayer = player.GetModPlayer<CalRemixPlayer>();
			crit = (float)Math.Pow(pplayer.chainSawCharge+1, 2) / (8*pplayer.chainSawChargeCritMax);
        }
    }
}