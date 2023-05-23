using CalRemix.Projectiles;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.Items
{
    public class KnowledgeDerellect : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("The Derellect");
            Tooltip.SetDefault("A twisted mother computer created with lab-grown souls.\n" +
            "It showed much more promise than its predecessors. I do not know why it was abandoned so soon.");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = 6;
        }
    }
}
