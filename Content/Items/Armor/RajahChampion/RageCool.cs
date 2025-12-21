using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor.RajahChampion
{
    public class RageCool : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("");
            // Description.SetDefault("A champion of Terraria never backs down");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (!player.HasBuff(ModContent.BuffType<RageBuff>()))
            {
                player.statDefense -= 15;
                player.moveSpeed *= .2f;
            }
        }
    }
}