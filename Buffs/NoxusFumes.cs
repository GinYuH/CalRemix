using CalRemix;
using CalRemix.Gores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Buffs
{
	public class NoxusFumes : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Noxus Fumes");
			Description.SetDefault("A cosmic affliction spreads throughout your body");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.blackout = true;
            player.GetModPlayer<CalRemixPlayer>().noxusFumes = true;
			if (Main.rand.NextBool(4))
			{
                Gore.NewGore(null, player.position, new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(0, 1)), ModContent.GoreType<Gores.NoxCloud>());
            }
            if (Main.rand.NextBool(30))
            {
				int poof = Main.rand.Next(3);
                if (poof == 0)
                {
                    Gore.NewGore(null, player.position, new Vector2(0, 0), ModContent.GoreType<NoxYha>());
                }
				else if (poof == 1)
				{
                    Gore.NewGore(null, player.position, new Vector2(0, 0), ModContent.GoreType<NoxDra>());
                }
				else
				{
                    Gore.NewGore(null, player.position, new Vector2(0, 0), ModContent.GoreType<NoxCal>());
                }
            }
        }
	}
}
