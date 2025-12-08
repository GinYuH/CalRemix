using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRemix.Content.Items.Weapons
{
    public class Grablerscalibur : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 26;
            Item.knockBack = 0;
            Item.crit = 3;
            Item.shoot = ModContent.ProjectileType<GrablerscaliburProj>();

            Item.value = Item.buyPrice(silver:3,copper:33);
            Item.UseSound = SoundID.NPCDeath13;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 100);
        }
        
    }
}