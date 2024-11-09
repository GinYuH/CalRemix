using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class FocusedConvergence : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(" ");
        }
        public override void UpdateInventory(Player player)
        {
            Item.stack = 0;
            Item.active = false;
        }
        public override void PostUpdate()
        {
            Item.active = false;
        }
    }
}