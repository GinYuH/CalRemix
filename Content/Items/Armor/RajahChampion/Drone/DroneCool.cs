using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor.RajahChampion.Drone
{
    public class DroneCool : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("R.A.B.I.T. Unit Reload Protocol");
            // Description.SetDefault("RELOADING. DAMAGE COMPENSATION PROVIDED.");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetDamage(DamageClass.Ranged) += .15f;
        }
    }
}