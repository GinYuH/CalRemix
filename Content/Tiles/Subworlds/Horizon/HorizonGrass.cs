using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalRemix.Content.NPCs.Bosses.SealedOne.OrbitingOrb;

namespace CalRemix.Content.Tiles.Subworlds.Horizon
{
    public class HorizonGrass : ModTile
    {
        public static Asset<Texture2D> MainBlock;

        public static Asset<Texture2D> GrassBlade;

        public override void Load()
        {
            MainBlock = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Horizon/HorizonFloor");
            GrassBlade = ModContent.Request<Texture2D>("CalRemix/Content/Tiles/Subworlds/Horizon/HorizonGrass");
        }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(255, 255, 255));
            HitSound = SoundID.Grass;
            DustType = DustID.WhiteTorch;
            Main.tileBlendAll[Type] = true;
            MinPick = 99999;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float strength = Utils.MultiLerp(CalamityUtils.SineInEasing(1 - Utils.GetLerpValue((int)(Main.maxTilesY * 0.902f), (int)(Main.maxTilesY * 0.93f), j, true), 1), 0, 0.4f, 1f);
            r = strength * 2f;
            g = strength * 1.5f;
            b = strength;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
            Texture2D block = MainBlock.Value;
            Texture2D blade = GrassBlade.Value;
            int possibleX = 111;
            int possibleY = 12;
            bool left = ((i * 7 + j * 13) % 1000 / 1000f) == 0;
            Rectangle frame = block.Frame(possibleX, possibleY, i % possibleX, j % possibleY);
            //if (!Main.tile[i, j - 1].HasTile)
            {
                float grassAmt = 5;
                for (int l = 0; l < grassAmt; l++)
                    spriteBatch.Draw(blade, new Vector2(i, j) * 16 + new Vector2(MathHelper.Lerp(0, 16, l / grassAmt), (i * 3 + j * 7 + l * 5) % 8) + CalamityUtils.TileDrawOffset - Main.screenPosition, blade.Frame(12, 1, ((i * 7 + j * 3 + l * 5) % 12) + 1, 0), (Color.White * 0.8f).MultiplyRGB(Lighting.GetColor(i, j)) with { A = 255 }, ((i * 5 + j * 13 + l * 3) % 100 / 100f * MathHelper.PiOver2 - MathHelper.PiOver4​) * (0.5f + 0.5f * MathF.Sin(Main.GlobalTimeWrappedHourly + i % 7 + l % 3)), new Vector2(blade.Width / 24f, blade.Height), (i * 7 + j * 13 + l * 5) % 1000 / 1000f * 0.4f + 0.8f, left ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            spriteBatch.Draw(block, new Vector2(i, j) * 16 + CalamityUtils.TileDrawOffset - Main.screenPosition, frame, Lighting.GetColor(i, j), 0, Vector2.Zero, 1, 0, 0);

            return false;
        }
    }
}