using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;

namespace CalRemix.Items.Weapons
{
	public class PungentBomber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pungent Bomber");
            Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.useTime = 33; 
			Item.useAnimation = 33;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 27;
			Item.knockBack = 5.5f;
            Item.useTurn = true;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == player.whoAmI)
                Projectile.NewProjectile(Item.GetSource_OnHit(target), new Vector2(player.Center.X + (player.direction > 0 ? 32f : -32f) * Item.scale, player.Center.Y), Vector2.Zero, ModContent.ProjectileType<PungentBomb>(), Item.damage / 2, 0, player.whoAmI);
        }
    }
}
