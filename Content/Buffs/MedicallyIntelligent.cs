using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class MedicallyIntelligent : ModBuff
    {
        public override string Texture => "CalRemix/Content/Buffs/Acid";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Medically Intelligent");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CalRemixPlayer>().phd = true;
        }
    }
}
