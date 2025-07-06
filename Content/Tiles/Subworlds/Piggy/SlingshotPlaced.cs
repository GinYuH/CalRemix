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
using CalamityMod.Projectiles.Melee;
using SubworldLibrary;
using CalRemix.Core.Subworlds;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;

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

        public override void NearbyEffects(int i, int j, bool closer)
        {
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
            return false;
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
                    Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
                    Vector2 worldPos = SlingshotSystem.slingPosition + CalamityUtils.TileDrawOffset;
                    float rotation = SlingshotSystem.dragOffset.ToRotation() - MathHelper.PiOver2;
                    if (SlingshotSystem.dragOffset == default)
                        rotation = MathHelper.PiOver2;
                    else
                    {
                        int ptAmt = 16; // How many points to draw
                        int lineStrength = 6; // The multiplier put on the line's distance. The base distance is the distance from the bird to the slingshot. A value of 6 makes the line 6 times longer.
                        int gravity = 4; // How much to decrement each node's vertical position. !!! This value must be updated if the above is because I'm too lazy to math.
                        spriteBatch.EnterShaderRegion(BlendState.Additive);
                        for (int k = 0; k < ptAmt; k++)
                        {
                            Vector2 dotPos = Vector2.Lerp(worldPos, worldPos - SlingshotSystem.dragOffset * lineStrength + Vector2.UnitY * k * gravity, k / (float)ptAmt);
                            float bloomScale = MathHelper.Lerp(0.05f, 0.01f, k / (float)ptAmt);
                            spriteBatch.Draw(bloom, dotPos - Main.screenPosition, null, Color.White, 0, bloom.Size() / 2, bloomScale, 0, 0);
                        }
                        spriteBatch.ExitShaderRegion();
                    }
                    spriteBatch.Draw(tex, worldPos - Main.screenPosition + SlingshotSystem.dragOffset, null, Lighting.GetColor(i, j), rotation, tex.Size() / 2, 1, 0, 0);

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
                            if (effectiveNess <= 20)
                            {
                                WorldGen.KillTile(i, j);
                                p.penetrate -= effectiveNess;
                            }
                            else
                            {
                                p.Kill();
                            }
                        }
                        if (p.penetrate <= 0)
                            p.Kill();
                    }
                }
            }
        }
    }

    public class APPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (SubworldSystem.IsActive<PiggySubworld>())
            {
                Player.chaosState = true;
                Point pt = Main.MouseWorld.ToTileCoordinates();
                Tile t = CalamityUtils.ParanoidTileRetrieval(pt.X, pt.Y);
                if (t.TileType == ModContent.TileType<SlingshotPlaced>())
                {
                    Point tileFrame = new Point(t.TileFrameX / 18, t.TileFrameY / 18);
                    SlingshotSystem.slingPosition = new Vector2(pt.X - tileFrame.X + 2, pt.Y - tileFrame.Y + 1) * 16;
                    if (SlingshotSystem.LoadedBird == null)
                    {
                        SlingshotSystem.LoadedBird = SlingshotSystem.birdData[Main.rand.Next(0, 3)];
                    }
                    if (Player.controlUseItem)
                    {
                        SlingshotSystem.wasHolding = true;
                    }
                }
                if (SlingshotSystem.slingPosition != default)
                {
                    if (SlingshotSystem.wasHolding && Player.controlUseItem)
                    {
                        SlingshotSystem.dragOffset = SlingshotSystem.slingPosition.DirectionTo(Main.MouseWorld) * MathHelper.Clamp(SlingshotSystem.slingPosition.Distance(Main.MouseWorld), 0.1f, 100f);
                    }
                    if (!Player.controlUseItem && SlingshotSystem.wasHolding)
                    {
                        SlingshotSystem.wasHolding = false;
                        int extraUpdates = 10;
                        float strength = SlingshotSystem.dragOffset.Length() / ((float)extraUpdates - 2);
                        int p = Projectile.NewProjectile(new EntitySource_WorldEvent(), SlingshotSystem.slingPosition, -SlingshotSystem.dragOffset.SafeNormalize(Vector2.UnitY) * strength, SlingshotSystem.LoadedBird.ProjType, 2222222, 1, Player.whoAmI);
                        Main.projectile[p].extraUpdates = extraUpdates;
                        Main.projectile[p].penetrate = 20;
                        SoundEngine.PlaySound(SlingshotSystem.LoadedBird.sound);
                        SlingshotSystem.LoadedBird = null;
                        SlingshotSystem.dragOffset = default;
                    }
                }
            }
            else
            {
                SlingshotSystem.slingPosition = default;
            }
        }
    }

    public class SlingshotSystem : ModSystem
    {
        public static Vector2 slingPosition = default;

        public static SlingshotBird LoadedBird = null;

        public static List<SlingshotBird> birdData = new();

        public static Vector2 dragOffset = default;

        public static bool wasHolding = false;

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
            birdData.Add(new SlingshotBird(ProjectileID.WoodenArrowFriendly, new int[5] { 6, 8, 15, 20, 21 }, BetterSoundID.ItemLaserMachinegun));
            birdData.Add(new SlingshotBird(ProjectileID.FireArrow, new int[5] { 6, 4, 15, 20, 20 }, BetterSoundID.ItemInfernoFork));
            birdData.Add(new SlingshotBird(ProjectileID.HellfireArrow, new int[5] { 6, 8, 8, 17, 20 }, BetterSoundID.ItemExplosion));
        }
    }

    public class SlingshotBird(int projType, int[] materialEffectivness, SoundStyle sound)
    {
        public int ProjType = projType;
        public int[] MaterialEffectiveness = materialEffectivness;
        public SoundStyle sound = sound;
    }
}