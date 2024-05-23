using CalamityMod.Items;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class DeterioratingLens : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Deteriorating Lens");
        }

        public override void SetDefaults()
        {
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.scope = true;
            player.GetCritChance(DamageClass.Generic) += 16;
            if (!Filters.Scene["CalRemix:AcidSight"].IsActive())
            {
                Filters.Scene.Activate("CalRemix:AcidSight", Main.player[Main.myPlayer].position);
                Filters.Scene["CalRemix:AcidSight"].GetShader().UseImage("Images/Misc/Perlin");
            }
        }
    }
}
