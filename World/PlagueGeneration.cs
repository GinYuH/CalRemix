using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.World;
using CalRemix.Backgrounds.Plague;
using CalRemix.Projectiles;

namespace CalRemix.World
{
    public class PlagueGeneration : ModSystem
    {
        public static void GeneratePlague()
        {
            bool gennedplague = false;
            int plagueX = 0;
            int plagueY = 0;
            int plagueY2 = 0;
            if (CalamityWorld.JungleLabCenter != Vector2.Zero)
            {
                PlaguedSpray.Convert((int)(CalamityWorld.JungleLabCenter.X / 16), (int)(CalamityWorld.JungleLabCenter.Y / 16), 222 * (WorldGen.GetWorldSize() + 1));
                plagueX = (int)(CalamityWorld.JungleLabCenter.X / 16);
                plagueY = (int)(CalamityWorld.JungleLabCenter.Y / 16);
                gennedplague = true;
            }
            if (!gennedplague)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (Main.tile[i, j].TileType == TileID.Mud && Main.rand.NextBool(2222))
                        {
                            PlaguedSpray.Convert(i, j, 222 * (WorldGen.GetWorldSize() + 1));
                            plagueX = i;
                            plagueY = j;
                            gennedplague = true;
                            break;
                        }
                        if (gennedplague)
                        {
                            break;
                        }
                    }
                    if (gennedplague)
                    {
                        break;
                    }
                }
            }
            if (gennedplague)
            {
                for (int j = 100; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[plagueX, j].HasTile && !Main.tile[plagueX, j - 1].HasTile)
                    {
                        PlaguedSpray.Convert(plagueX, j, 111 * (WorldGen.GetWorldSize() + 1));
                        plagueY2 = j;
                        break;
                    }
                }
                for (int i = plagueX - 33 * (WorldGen.GetWorldSize() + 1); i < 33 * (WorldGen.GetWorldSize() + 1) + plagueX; i++)
                {
                    for (int j = plagueY2; j < plagueY; j++)
                    {
                        PlaguedSpray.Convert(i, j, 2);
                    }
                }
                CalRemixWorld.generatedPlague = true;
            }
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (Main.gameMenu)
            {
                return;
            }
            var player = Main.LocalPlayer;
            var pPlayer = player.GetModPlayer<CalRemixPlayer>();
            if (pPlayer.ZonePlague || pPlayer.ZonePlagueDesert)
            {
                float amount = 0.2f;
                if (PlagueSky.Intensity < 1f)
                {
                    float r = backgroundColor.R / 255f;
                    float g = backgroundColor.G / 255f;
                    float b = backgroundColor.B / 255f;
                    r = MathHelper.Lerp(r, amount, PlagueSky.Intensity);
                    g = MathHelper.Lerp(g, amount, PlagueSky.Intensity);
                    b = MathHelper.Lerp(b, amount, PlagueSky.Intensity);
                    backgroundColor.R = (byte)(int)(r * 255f);
                    backgroundColor.G = (byte)(int)(g * 255f);
                    backgroundColor.B = (byte)(int)(b * 255f);
                }
                else
                {
                    byte a = (byte)(int)(amount * 255f);
                    backgroundColor.R = 40;
                    backgroundColor.G = 40;
                    backgroundColor.B = 40;
                }
            }
            if (CalRemixWorld.oxydayTime > 0 && player.position.Y < Main.worldSurface * 16)
            {
                backgroundColor = Color.Lerp(backgroundColor, Color.LightSkyBlue, 0.2f);
                backgroundColor = Color.Lerp(backgroundColor, Color.Cyan, 0.2f);
            }
        }
    }
}