using CalRemix.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class ValfreyBurn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Valfrey Burn");
            // Description.SetDefault("Your power is being absorbed");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextBool(60))
                Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<ValfreyDust>(), Main.rand.Next(-5,6), Main.rand.Next(-5, 6));
            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Flare, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6));
        }
    }
}
