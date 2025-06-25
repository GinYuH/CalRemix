using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class ILoveLean : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("I LOVE LEAN!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.13f;
            player.moveSpeed += 0.35f;
            player.endurance += 0.15f;
            player.statDefense += 22;

            if (!Filters.Scene["CalRemix:LeanVision"].IsActive())
            {
                Filters.Scene.Activate("CalRemix:LeanVision", Main.player[Main.myPlayer].position);
                Filters.Scene["CalRemix:LeanVision"].GetShader().UseColor(80f, 0f, 160f);
            }
        }
    }
}
