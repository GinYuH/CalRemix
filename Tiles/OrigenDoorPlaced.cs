using CalamityMod.NPCs.Cryogen;
using CalRemix;
using CalRemix.NPCs.Bosses.Pathogen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Tiles
{
    public class OrigenDoorPlaced : ModTile
    {
        private bool spawnjon = false;
        private bool sound = false;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(75, 139, 166));
            DustType = 1;
            AnimationFrameHeight = 54;
            TileID.Sets.DisableSmartCursor[Type] = true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

       /*public override void NearbyEffects(int i, int j, bool closer)
        {
            int x = i - Main.tile[i, j].frameX / 18 % 2;
            int y = j - Main.tile[i, j].frameY / 18 % 3;
            for (int l = x; l < x + 3; l++)
            {
                for (int m = y; m < y + 3; m++)
                {
                    if (Main.tile[l, m] == null)
                    {
                        Main.tile[l, m] = new Tile();
                    }
                    if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.JohnWulfrum.JohnWulfrumSentry>()) && CalValPlusWorld.downedJohnWulfrum)
                        {
                            Main.tile[l, m].frameY = 0;
                        }
                    }
                }
            }
        }*/

        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
            for (int l = x; l < x + 3; l++)
            {
                for (int m = y; m < y + 3; m++)
                {
                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameY < 54)
                        {
                            int spawnX = i * 16 + 16;
                            int spawnY = j * 16 + 6;
                            if (!spawnjon)
                            {
                                if (!NPC.AnyNPCs(ModContent.NPCType<Cryogen>()) && !RemixDowned.downedPathogen)
                                {
                                    NPC.NewNPC(new EntitySource_WorldEvent(), spawnX, spawnY - 12, ModContent.NPCType<Cryogen>());
                                    spawnjon = true;
                                }
                                if (!NPC.AnyNPCs(ModContent.NPCType<Pathogen>()) && RemixDowned.downedPathogen)
                                {
                                    NPC.NewNPC(new EntitySource_WorldEvent(), spawnX, spawnY - 12, ModContent.NPCType<Pathogen>());
                                    spawnjon = true;
                                }
                            }
                            if (!sound)
                            {
                                SoundEngine.PlaySound(SoundID.DoorOpen);
                                sound = true;
                            }
                            Main.tile[l, m].TileFrameY += 54;
                        }
                        else
                        {
                            if (sound)
                            {
                                SoundEngine.PlaySound(SoundID.DoorClosed);
                                sound = false;
                            }
                            Main.tile[l, m].TileFrameY -= 54;
                            spawnjon = false;
                        }
                    }
                }
            }
            if (Wiring.running)
            {
                Wiring.SkipWire(x, y);
                Wiring.SkipWire(x, y + 1);
                Wiring.SkipWire(x, y + 2);
                Wiring.SkipWire(x + 1, y);
                Wiring.SkipWire(x + 1, y + 1);
                Wiring.SkipWire(x + 1, y + 2);
            }
            NetMessage.SendTileSquare(-1, x, y + 1, 3);
        }
    }
}