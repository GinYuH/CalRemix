using System.Collections.Generic;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Noxus;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Minibosses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles;

namespace CalRemix.Content.Items.Tools
{
    public class KetchupSqueezie : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 34;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<KetchupSqueezieProj>()] < 1;
    }
}
