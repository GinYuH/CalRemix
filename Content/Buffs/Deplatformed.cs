using CalRemix.Content.NPCs.Bosses.Pyrogen;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace CalRemix.Content.Buffs
{
    public class Deplatformed : ModBuff
    {
        NPC pyro;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deplatformed");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            static bool PlatformCheck(int tileType)
            {
                return tileType == TileID.Platforms || tileType == TileID.PlanterBox;
            }

            Tile beneathMe = Framing.GetTileSafely(player.Bottom);
            Tile beneathTile = Framing.GetTileSafely(player.Bottom + Vector2.UnitY * 8);

            if (!Collision.SolidCollision(player.BottomLeft, player.width, 16))
            {
                if (player.velocity.Y >= 0 && (PlatformCheck(beneathMe.TileType) || PlatformCheck(beneathTile.TileType)))
                {
                    player.position.Y += 2;
                }
                if (player.velocity.Y == 0)
                {
                    player.position.Y += 16;
                }

            }
            //kill the debuff when pyro stops storming, no matter how he does
            if (!NPC.AnyNPCs(ModContent.NPCType<Pyrogen>()))
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
         }
    }
}
