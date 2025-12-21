using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalRemix.Content.Items.RajahItems
{
	public class CarrotFarmer : ModItem
	{
		public override void SetStaticDefaults()
		{
			//crossoverModName = "ThoriumMod";
            //DisplayName.SetDefault("Carrot Farmer");
            //Tooltip.SetDefault(@"Spins a Carrot Scythe around you that shreds through enemies\nScythes fire off carrots while spun\nGrants 1 soul essence on direct hit");			
		}

		public override void SetDefaults()
		{
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.rare = 8;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.useStyle = 1;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.UseSound = SoundID.Item1;
            Item.damage = 80;
            Item.knockBack = 9;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CarrotFarmerProj>();
            Item.shootSpeed = 0.1f;
		}
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int k = 0; k < 2; k++)
			{
				Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<CarrotFarmerEffect>(), damage, knockback, player.whoAmI, k, 0f);
			}
			return true;
		}

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            if (!thorium.TryFind("HealerDamage", out DamageClass healer))
                return;

            player.GetDamage(healer).ApplyTo(damage.Base);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            if (!thorium.TryFind("HealerDamage", out DamageClass healer))
                return;

            if (Main.rand.Next(100) <= player.GetCritChance(healer))
            {
                modifiers.SetCrit();
			}
		}

        public override void UpdateInventory(Player player)
        {
            if (!ModLoader.HasMod("ThoriumMod"))
            {
                Item.TurnToAir();
            }
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            int index = -1, index2 = -1;
            for (int m = 0; m < list.Count; m++)
            {
                if (list[m].Name.Equals("Damage")) { index = m; continue; }
                if (list[m].Name.Equals("Tooltip0")) { index2 = m; continue; }		
				if(index > -1 && index2 > -1) break;
            }
            string oldTooltip = list[index].Text;
            string[] split = oldTooltip.Split(' '); 
            list.RemoveAt(index);
            list.Insert(index, new TooltipLine(Mod, "Damage", split[0] + " radiant damage"));
            TooltipLine colorLine = new TooltipLine(Mod, "Healer", "-Healer Class-")
            {
                OverrideColor = new Color(255, 255, 91)
            };
            list.Insert(index2, colorLine);
			base.ModifyTooltips(list);
        }
    }
}