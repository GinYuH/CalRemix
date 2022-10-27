using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using CalamityMod.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalRemix
{
	public class CalRemixItem : GlobalItem
	{
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            if (modPlayer.roguebox && item.CountsAsClass<RogueDamageClass>())
            {
                int p = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y - 400), new Vector2(0, 20), type, (int)(damage * 0.33f), knockback);
                Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().rogueclone = true;
            }
            return true;
        }
	}
}
