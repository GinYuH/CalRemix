using CalamityMod;
using CalamityMod.NPCs.Ravager;
using CalRemix.Content.Items.Weapons.Stormbow;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.Glamour
{
    public class FreeTree : ModTile
    {
        public static Asset<Texture2D> full = null;

        public static int timer = 0;

        public override void Load()
        {
            full = ModContent.Request<Texture2D>(Texture + "_Full");
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.Width = 15;
            TileObjectData.newTile.Height = 14;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(6, 13);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 15, 0);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 120, 0), name);
            RegisterItemDrop(ModContent.ItemType<BigEater>());
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
            if (t.TileFrameX == 0 && t.TileFrameY == 0)
            {
                if (closer)
                {
                    if (Main.LocalPlayer.Distance(new Microsoft.Xna.Framework.Vector2(i + 6, j + 13) * 16) < 200)
                    {
                        timer++;
                        if (timer > CalamityUtils.SecondsToFrames(1) && timer % (int)(MathHelper.Lerp(22, 2, Utils.GetLerpValue(CalamityUtils.SecondsToFrames(1), CalamityUtils.SecondsToFrames(5), timer, true))) == 0)
                        {
                            SoundEngine.PlaySound(RavagerBody.HitSound with { Pitch = 1 }, new Vector2(i, j) * 16);
                        }
                    }
                }
                if (timer >= CalamityUtils.SecondsToFrames(5))
                {
                    SoundEngine.PlaySound(RavagerBody.LimbLossSound with { Pitch = -1f }, new Vector2(i + 8, j) * 16);
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = 20;
                    WorldGen.KillTile(i, j);
                    timer = 0;
                }
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 2;
            g = 2;
            b = 2;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
            if (t.TileFrameX == 0 && t.TileFrameY == 0)
            {
                Vector2 extraOff = Main.rand.NextVector2Circular(1, 1) * Utils.GetLerpValue(CalamityUtils.SecondsToFrames(0.5f), CalamityUtils.SecondsToFrames(5), timer, true) * 22;
                spriteBatch.Draw(full.Value, new Vector2(i + 6, j + 14) * 16 + CalamityUtils.TileDrawOffset - Main.screenPosition + extraOff, null, Lighting.GetColor(i, j), 0, new Vector2(full.Value.Width / 2, full.Value.Height), 1, 0, 0);
            }
            return false;
        }
    }
}