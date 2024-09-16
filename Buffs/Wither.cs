using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class Wither : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wither");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<CalRemixGlobalNPC>().witherDebuff = true;
            if (Main.rand.NextBool(3))
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ash, Main.rand.Next(-5, 6), Main.rand.Next(1, 6));
        }
    }
}
