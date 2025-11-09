using CalamityMod;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SubworldLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
    public abstract class SubworldDoorPlaced : ModTile
    {
        public Asset<Texture2D> PreviewTex;

        public virtual string PreviewTexName => null;
        public virtual Subworld BoundSubworld => null;

        public virtual Color DoorColor => Color.White;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                // Cache the extra texture displayed on the pedestal
                PreviewTex = ModContent.Request<Texture2D>(PreviewTexName);
            }
        }

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };

            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
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

        public override bool RightClick(int i, int j)
        {
            if (Main.tile[i, j].TileFrameY >= AnimationFrameHeight)
            {
                SubworldSystem.Enter(BoundSubworld.FullName);
                SoundEngine.PlaySound(BetterSoundID.ItemTeleportMirror);
            }
            else
            {
                HitWire(i, j);
            }
            return true;
        }

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
                            Main.tile[l, m].TileFrameY += 54;
                        }
                        else
                        {
                            Main.tile[l, m].TileFrameY -= 54;
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

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Main.tile[i, j];
            if (t.TileFrameX % 36 == 0 && t.TileFrameY == 54)
            {
                Texture2D tex = PreviewTex.Value;
                Vector2 tileSize = new Vector2(32, 54);
                Main.EntitySpriteDraw(tex, new Vector2(i, j) * 16 - Main.screenPosition + CalamityUtils.TileDrawOffset, null, Color.White, 0, Vector2.Zero, tileSize / tex.Size(), 0);
            }
            Main.EntitySpriteDraw(TextureAssets.Tile[Type].Value, new Vector2(i, j) * 16 - Main.screenPosition + CalamityUtils.TileDrawOffset, new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Lighting.GetColor(i, j, DoorColor), 0, Vector2.Zero, 1, 0);

            return false;
        }
    }

    public class ExosphereDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Assets/ExtraTextures/SubworldPreviews/ExospherePreview";
        public override Subworld BoundSubworld => ModContent.GetInstance<ExosphereSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";

        public override Color DoorColor => Color.DarkGray;
    }

    public class BaronDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Assets/ExtraTextures/SubworldPreviews/BaronPreview";
        public override Subworld BoundSubworld => ModContent.GetInstance<BaronSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.DarkCyan;
    }

    public class NormalDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Content/Items/Accessories/Baroclaw";
        public override Subworld BoundSubworld => ModContent.GetInstance<NormalSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.Brown;
    }

    public class ScreamDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Assets/ExtraTextures/SubworldPreviews/ScreamingFacePreview";
        public override Subworld BoundSubworld => ModContent.GetInstance<ScreamingSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.GhostWhite;
    }

    public class GrandSeaDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Assets/ExtraTextures/SubworldPreviews/GrandSeaPreview";
        public override Subworld BoundSubworld => ModContent.GetInstance<GreatSeaSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.RoyalBlue;
    }

    public class AntDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Assets/ExtraTextures/SubworldPreviews/AntPreview";
        public override Subworld BoundSubworld => ModContent.GetInstance<AntSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.WhiteSmoke;
    }

    public class PiggyDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalamityMod/Items/Critters/PiggyItem";
        public override Subworld BoundSubworld => ModContent.GetInstance<PiggySubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.LightPink;
    }

    public class SealedDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Content/Items/Placeables/Subworlds/Sealed/SealedStone";
        public override Subworld BoundSubworld => ModContent.GetInstance<SealedSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.Purple;
    }

    public class HorizonDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Content/Tiles/Subworlds/Horizon/HorizonGrass";
        public override Subworld BoundSubworld => ModContent.GetInstance<HorizonSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.PaleGoldenrod;
    }

    public class DeformityDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "Terraria/Images/Item_" + ItemID.DarkCelestialBrick;
        public override Subworld BoundSubworld => ModContent.GetInstance<DeformitySubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.CadetBlue;
    }


    // this is my special door for testing so it goes at the bottom always
    public class TestDoor : SubworldDoorPlaced
    {
        public override string PreviewTexName => "CalRemix/Content/Items/Weapons/AergianTechnistaff";
        public override Subworld BoundSubworld => ModContent.GetInstance<IllKillThisLaterSubworld>();

        public override string Texture => "CalRemix/Content/Tiles/SubworldDoorPlaced";
        public override Color DoorColor => Color.AliceBlue;
    }
}