using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using System.Collections.Generic;
using CalamityMod;
using Terraria.GameContent;
using Terraria.DataStructures;
using System.Linq;

namespace CalRemix.Content.Tiles.Subworlds.Piggy
{
    public class SlingshotPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileID.Sets.DisableSmartCursor[Type] = false;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 18 }; //
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(255, 255, 255), name);
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            if (SlingshotSystem.LoadedBird == null)
            {
                SlingshotSystem.LoadedBird = SlingshotSystem.birdData[Main.rand.Next(0, 3)];
            }
            else
            {
                Vector2 pos = new Vector2(i, j) * 16 + new Vector2(1, 3) * 16;
                float strength = MathHelper.Clamp(pos.Distance(Main.MouseWorld) / 16f, 0.1f, 10f) * 5;
                int p = Projectile.NewProjectile(new EntitySource_WorldEvent(), Main.LocalPlayer.position, Main.LocalPlayer.position.DirectionTo(Main.MouseWorld) * strength, SlingshotSystem.LoadedBird.ProjType, 10, 1, Main.LocalPlayer.whoAmI);
                Main.projectile[p].extraUpdates = 1;
                SlingshotSystem.LoadedBird = null;
            }

            return true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Main.tile[i, j];
            if (t.TileFrameX == 0 && t.TileFrameY == 0)
            {
                if (SlingshotSystem.LoadedBird != null)
                {
                    SlingshotBird loadedBird = SlingshotSystem.LoadedBird;
                    Texture2D tex = TextureAssets.Projectile[loadedBird.ProjType].Value;
                    Vector2 worldPos = new Vector2(i, j) * 16 + CalamityUtils.TileDrawOffset;
                    spriteBatch.Draw(tex, worldPos - Main.screenPosition, null, Lighting.GetColor(i, j), 0, tex.Size() / 2, 1, 0, 0);
                }
            }
            return true;
        }
    }

    public class BirdProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile p)
        {
            int b = SlingshotSystem.birdData.FindIndex((SlingshotBird b) => b.ProjType == p.type);
            if (b > -1)
            {
                SlingshotBird birdType = SlingshotSystem.birdData[b];
                Point tileCords = p.Center.ToSafeTileCoordinates();
                int rad = 1;
                for (int i = tileCords.X - rad; i < tileCords.X + 2; i++)
                {
                    for (int j = tileCords.Y - rad; j < tileCords.Y + 2; j++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);

                        int effectiveNess = 0;

                        if (SlingshotSystem.StoneTiles.Contains(t.TileType))
                        {
                            effectiveNess = birdType.MaterialEffectiveness[(int)SlingshotSystem.MaterialType.Stone];
                        }
                        else if (SlingshotSystem.WoodTiles.Contains(t.TileType))
                        {
                            effectiveNess = birdType.MaterialEffectiveness[(int)SlingshotSystem.MaterialType.Wood];
                        }
                        else if (SlingshotSystem.GlassTiles.Contains(t.TileType))
                        {
                            effectiveNess = birdType.MaterialEffectiveness[(int)SlingshotSystem.MaterialType.Glass];
                        }
                        else if (SlingshotSystem.MetalTiles.Contains(t.TileType))
                        {
                            effectiveNess = birdType.MaterialEffectiveness[(int)SlingshotSystem.MaterialType.Metal];
                        }
                        else if (SlingshotSystem.ObsidianTiles.Contains(t.TileType))
                        {
                            effectiveNess = birdType.MaterialEffectiveness[(int)SlingshotSystem.MaterialType.Obsidian];
                        }

                        if (effectiveNess > 0)
                        {
                            WorldGen.KillTile(i, j);
                            p.penetrate--;
                        }
                    }
                }
            }
        }
    }

    public class SlingshotSystem : ModSystem
    {
        public static BirdType LoadedBirdType => (BirdType)LoadedBird.ProjType;

        public static SlingshotBird LoadedBird = null;

        public static List<SlingshotBird> birdData = new();

        public enum BirdType
        {
            None = 0,
            Basic = 1,
            Fast = 2,
            Explode = 3
        }

        public enum MaterialType
        {
            Glass = 0,
            Wood = 1,
            Stone = 2,
            Metal = 3,
            Obsidian = 4
        }

        public static List<int> GlassTiles = new List<int> { TileID.Glass };

        public static List<int> WoodTiles = new List<int> { TileID.WoodBlock, ModContent.TileType<StickBlockPlaced>() };

        public static List<int> StoneTiles = new List<int> { TileID.StoneSlab, TileID.GrayBrick, TileID.RedBrick };

        public static List<int> MetalTiles = new List<int> { };

        public static List<int> ObsidianTiles = new List<int> { TileID.ObsidianBrick };

        public override void PostSetupContent()
        {
            birdData.Add(new SlingshotBird(ProjectileID.WoodenArrowFriendly, new int[5] { 6, 5, 2, 1, 0 }, BetterSoundID.ItemLaserMachinegun));
            birdData.Add(new SlingshotBird(ProjectileID.FireArrow, new int[5] { 6, 12, 3, 2, 1 }, BetterSoundID.ItemInfernoFork));
            birdData.Add(new SlingshotBird(ProjectileID.HellfireArrow, new int[5] { 6, 5, 8, 4, 1 }, BetterSoundID.ItemExplosion));
        }
    }

    public class SlingshotBird(int projType, int[] materialEffectivness, SoundStyle sound)
    {
        public int ProjType = projType;
        public int[] MaterialEffectiveness = materialEffectivness;
        public SoundStyle sound = sound;
    }
}