using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Rarities;
using CalRemix.Content.NPCs;
using CalRemix.Core.World;
using CalRemix.UI;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools
{
    public class ShadeSingularity : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.useAnimation = 30;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<DialoguePlayer>().readDialogue.Clear();
            CalRemixWorld.shadeQuestLevel = 0;
            ItemQuestSystem.cultistLevel = 0;
            ItemQuestSystem.rubyLevel = 0;
            ItemQuestSystem.brainLevel = 0;
            ItemQuestSystem.draedonLevel = 0;
            CalRemixWorld.UpdateWorldBool();
            return true;
        }
    }
}
