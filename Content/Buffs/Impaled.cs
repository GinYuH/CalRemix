using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRemix.Content.Buffs
{
    public class Impaled : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Impaled");
            //Description.SetDefault("Ouch!");
            Main.debuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            //int num = npc.lifeRegenExpectedLossPerSecond;
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            int JavelinCount = 0;
            int impaleDamage = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (
                    Main.projectile[i].active && Main.projectile[i].GetGlobalProjectile<ImplaingProjectile>().CanImpale && 
                    ((Main.projectile[i].ai[0] == 1f && Main.projectile[i].ai[1] == npc.whoAmI)) 
                )
                {

                    impaleDamage += Main.projectile[i].GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler;
                    JavelinCount++;
                }
            }
            npc.lifeRegen -= impaleDamage * 2;
            npc.lifeRegenExpectedLossPerSecond = impaleDamage;
        }
    }

    public class ImplaingProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public bool CanImpale = false;
        public int damagePerImpaler = 0;
    }
}
