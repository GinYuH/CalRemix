using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeExcavator : RemixLoreItem
    {
        public override string LoreText => Language.GetOrRegister($"Mods.CalRemix.Items.{Name}.LoreText").Value;
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
        }
    }
}
