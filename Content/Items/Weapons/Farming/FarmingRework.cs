using System.Collections.Generic;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee;
using CalRemix.Content.DamageClasses;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class FarmingReworkItem : GlobalItem
    {
        private static readonly List<int> ReworkedItems =
        [
            ItemID.StaffofRegrowth,
            ItemID.AcornAxe,
            ItemID.Sickle,
            ItemID.IceSickle,
            ItemID.DeathSickle,
            ItemID.DemonScythe,
            ItemType<AstralScythe>(),
            ItemType<LifehuntScythe>(),
            ItemType<ChronomancersScythe>(),
        ];
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
                TextureAssets.Item[ItemID.DemonScythe] = Request<Texture2D>("CalRemix/Content/Items/Weapons/Farming/DemonScythe");
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.DemonScythe)
            {
                item.useStyle = ItemUseStyleID.Swing;
                item.noMelee = false;
            }
            if (ReworkedItems.Contains(item.type))
                item.DamageType = GetInstance<FarmingDamageClass>();
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string key = "Items.Tooltips.";
            if (item.DamageType == GetInstance<FarmingDamageClass>())
            {
                foreach (TooltipLine t in tooltips)
                {
                    t.Text = t.Text.Replace(CalRemixHelper.LocalText($"{key}Crit").Value, CalRemixHelper.LocalText($"{key}Harvest").Value);
                    t.Text = t.Text.Replace(CalRemixHelper.LocalText($"{key}Knockback").Value, CalRemixHelper.LocalText($"{key}Cultivation").Value);
                    if (t.Name == "Speed")
                        t.Text = t.Text.Replace(CalRemixHelper.LocalText($"{key}Speed").Value, CalRemixHelper.LocalText($"{key}Efficiency").Value);

                }
            }
        }
    }
    public class FarmingReworkProjectile : GlobalProjectile
    {
        private static readonly List<int> ReworkedProjectiles =
        [
            ProjectileID.IceSickle,
            ProjectileID.DeathSickle,
            ProjectileID.DemonScythe,
            ProjectileType<AstralScytheProjectile>(),
            ProjectileType<LifeScythe>(),
            ProjectileType<ChronomancersScytheHoldout>(),
            ProjectileType<ChronomancersScytheSwing>(),
            ProjectileType<ChronoIcicleSmall>(),
            ProjectileType<ChronoIcicleLarge>()
        ];
        public override void SetDefaults(Projectile projectile)
        {
            if (ReworkedProjectiles.Contains(projectile.type))
                projectile.DamageType = GetInstance<FarmingDamageClass>();
        }
        public override void AI(Projectile projectile)
        {
            if (ReworkedProjectiles.Contains(projectile.type))
            {
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (projectile.Hitbox.Intersects(npc.Hitbox) && !npc.GetGlobalNPC<CalRemixNPC>().farmNPC)
                        projectile.Kill();
                }
            }
        }
    }
}