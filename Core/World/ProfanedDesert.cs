using CalamityMod.NPCs.Providence;
using System;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using CalamityMod;
using CalRemix.Core.World;
using CalRemix.Content.Tiles;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace CalRemix.World
{
    public class ProfanedDesert : ModSystem
    {
        public static readonly object IOLock = new object(); //Shouldn't be necessary but just in case
        public static readonly FieldInfo worldFilePathInfo = typeof(FileData).GetField("_path", BindingFlags.Instance | BindingFlags.NonPublic);

        public enum FileStatus
        {
            Default,
            Requested,
            Validated
        }

        public static FileStatus saveWorldStatus = FileStatus.Default; //The status on saving the original file before modifying anything further
        public static FileStatus duplicateWorldStatus = FileStatus.Default; //The status on duplicating the original file before modifying anything further


        public static bool lockRespawn = false;   //Prevents the player from respawning while the worldgen is taking place
        public static bool gotJumpscared = false; //If the world already got turned into a desert once (saved on the world file which was spared, not the desert world file)
        public static bool scorchedWorld = false; //If the world is a profaned desert world and should get a special icon
        public static int flashTimer = -1;
        public const int flashPeak = 60;
        public const int flashPause = flashPeak + 120;
        public const int flashTotal = flashPause + 240;

        //Only generates in singleplayer in non cloud save worlds, and only if the player's world didn't already get desert-ed once
        bool CanGenerate => !gotJumpscared &&!Main.ActiveWorldFileData.IsCloudSave && Main.netMode == NetmodeID.SinglePlayer;

        public override void Load()
        {
            On_AWorldListItem.GetIcon += On_AWorldListItem_GetIcon;
            if (!Main.dedServ)
            {
                ScorchedWorldIcon = ModContent.Request<Texture2D>("CalRemix/Core/World/ScorchedWorldIcon", AssetRequestMode.ImmediateLoad);
            }
        }

        public static Asset<Texture2D> ScorchedWorldIcon;
        private Asset<Texture2D> On_AWorldListItem_GetIcon(On_AWorldListItem.orig_GetIcon orig, AWorldListItem self)
        {
            if (self.Data.TryGetHeaderData<ProfanedDesert>(out TagCompound tag) && tag.ContainsKey("ScorchedWorld"))
                return ScorchedWorldIcon;

            return orig(self);
        }

        public override void PostUpdateEverything()
        {
            if (lockRespawn && Main.LocalPlayer.dead)
            {
                Main.LocalPlayer.respawnTimer = Math.Max(Main.LocalPlayer.respawnTimer, 5 * 60);
            }

            CheckForDesertificationStart();
            if (scorchedWorld)
                WorldEffects();

            if (flashTimer >= 0 && flashTimer < flashTotal && !(Main.LocalPlayer.dead && flashTimer > (flashPause - 1)))
            {
                flashTimer++;
            }

            //After requesting the save of the world, and it got confirmed saved, we can proceed
            if (saveWorldStatus == FileStatus.Validated)
            {
                saveWorldStatus = FileStatus.Default;
                lockRespawn = true;

                ThreadPool.QueueUserWorkItem(DuplicateOriginalWorldCallback, 1);
            }
        }

        #region Generation
        public void CheckForDesertificationStart()
        {
            if (!CanGenerate)
                return;

            if (Main.LocalPlayer.dead && NPC.AnyNPCs(ModContent.NPCType<Providence>()) && CalRemixWorld.profanedDesert)
            {
                gotJumpscared = true;

                flashTimer = 0;

                SoundEngine.PlaySound(Providence.HolyRaySound);

                //Set the request and then save the world with the gotjumpscared change
                //SaveAndPlay is multithreaded, so we can't do our changes immediately
                saveWorldStatus = FileStatus.Requested;
                WorldGen.saveAndPlay();
            }
        }

        public static void DuplicateOriginalWorldCallback(object threadContext)
        {
            if (!Monitor.TryEnter(IOLock))
            {
                lockRespawn = false;
                return;
            }
            try
            {
                duplicateWorldStatus = FileStatus.Requested;
                FileUtilities.ProtectedInvoke(ChangeFilePath);
            }
            finally
            {
                Monitor.Exit(IOLock);

                //Stop the respawn lock if we couldnt duplicate the world
                if (duplicateWorldStatus != FileStatus.Validated)
                    lockRespawn = false;
            }
        }

        //Changes the world's file path
        public static void ChangeFilePath()
        {
            string worldPath = Main.ActiveWorldFileData.Path;
            string savePath = FileUtilities.GetParentFolderPath(worldPath, false);
            string fileName = FileUtilities.GetFileName(worldPath, true);


            Main.ActiveWorldFileData.UniqueId = new Guid();
            worldFilePathInfo.SetValue(Main.ActiveWorldFileData, savePath + "scorched_" + fileName);
            ThreadPool.QueueUserWorkItem(ConvertWorld, 1);
        }

        //Duplicates the world files without changing the world file path
        public static void DuplicateOriginalWorld()
        {
            string worldPath = Main.ActiveWorldFileData.Path;
            string savePath = FileUtilities.GetParentFolderPath(worldPath, false);
            string fileName = FileUtilities.GetFileName(worldPath, false);

            if (FileUtilities.Exists(worldPath, false) && !FileUtilities.Exists(savePath + fileName + "_spared.wld", false))
                FileUtilities.Copy(worldPath, savePath + fileName + "_spared.wld", false);
            else
                return;

            worldPath = Path.ChangeExtension(worldPath, ".twld"); //Tmod data
            if (FileUtilities.Exists(worldPath, false) && !FileUtilities.Exists(savePath + fileName + "_spared.twld", false))
                FileUtilities.Copy(worldPath, savePath + fileName + "_spared.twld", false);
            else
                return;

            duplicateWorldStatus = FileStatus.Validated; 
            ThreadPool.QueueUserWorkItem(ConvertWorld, 1);
        }

        public static void ConvertWorld(object threadContext)
        {

            //Ideally it would be "Scorched X" but then it throws the alphabetical sort out of wack, so instead its gonna be X (Scorched)
            Main.ActiveWorldFileData.Name = Main.ActiveWorldFileData.Name + " (Scorched)";
            Main.worldName =Main.worldName + " (Scorched)";

            scorchedWorld = true;


            int noiseSeed = WorldGen.genRand.Next(0, int.MaxValue);
            int baseHeight = (int)Main.worldSurface - 50;
            ushort groundType = (ushort)ModContent.TileType<TorrefiedTephraPlaced>();
            ushort surfaceType = (ushort)ModContent.TileType<TorrefiedTephraPlaced>();
            ushort crystalType = (ushort)ModContent.TileType<CalamityMod.Tiles.FurnitureProfaned.ProfanedCrystal>();

            Main.spawnTileX = Main.maxTilesX / 2;
            Main.LocalPlayer.SpawnX = -1;
            Main.LocalPlayer.SpawnY = -1;

            int proviType = ModContent.NPCType<Providence>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type != proviType )
                {
                    Main.npc[i].active = false;
                }
            }

            for (int i = 0; i < Main.maxItems; i++)
            {
                Main.item[i].active = false;
            }


            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float height = CalamityUtils.PerlinNoise2D(i / 380f, 0, 3, noiseSeed);

                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = Main.tile[i, j];
                    t.WallType = 0;
                    t.LiquidAmount = 0;
                    t.IsActuated = false;
                    t.Slope = SlopeType.Solid;
                    t.TileColor = 0;
                    t.IsTileInvisible = false;
                    t.IsTileFullbright = false;
                    t.HasActuator = false;
                    t.IsHalfBlock = false;
                    t.TileType = j > baseHeight + 2 + (int)(height * 20) ? groundType : surfaceType;
                    t.HasTile = j > baseHeight + (int)(height * 20);

                    t.TileFrameY = 18;
                    t.TileFrameX = (short)(18 * WorldGen.genRand.Next(1, 4));
                }

                if (i == Main.maxTilesX / 2)
                    Main.spawnTileY = baseHeight + (int)(height * 20) - 2;
            }

            //Frame the tiles at the surface
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = (int)MathHelper.Max(2, baseHeight - 50); j < baseHeight + 150; j++)
                {
                    WorldGen.TileFrame(i, j, true);

                    if (i > 2 && i < Main.maxTilesX - 3 )
                        Tile.SmoothSlope(i, j, applyToNeighbors: false);
                }
            }

            for (int i = 0; i < Main.maxTilesX / 30; i++)
            {
                int x = WorldGen.genRand.Next(5, Main.maxTilesX - 5);

                for (int j = (int)MathHelper.Max(2, baseHeight - 50); j < baseHeight + 150; j++)
                {
                    if (Main.tile[x, j].HasTile)
                    {
                        j++;
                        int width = 2;
                        int height = 2;

                        if (WorldGen.genRand.NextBool(8))
                        {
                            width = 3;
                            height = 5;
                        }

                        for (int cx = x; cx < x + width; cx++)
                        {
                            for (int cy = j - height; cy < j; cy++)
                            {
                                Tile t = Main.tile[cx, cy];
                                t.HasTile = true;
                                t.TileType = crystalType;
                                t.IsHalfBlock = false;
                                t.Slope = 0;

                                if (cy == j - height)
                                {
                                    if (width == 2)
                                        t.HasTile = !WorldGen.genRand.NextBool(6);

                                    else if (cx != x + 1)
                                        t.HasTile = false;
                                }
                            }
                        }

                        for (int cx = x; cx < x + width; cx++)
                            for (int cy = j - height; cy < j; cy++)
                                WorldGen.TileFrame(cx, cy, true);
                        break;
                    }
                }
            }

            for (int i = 0; i < Main.maxTilesX / 20; i++)
            {
                int x = WorldGen.genRand.Next(5, Main.maxTilesX - 5);

                for (int j = (int)MathHelper.Max(2, baseHeight - 50); j < baseHeight + 150; j++)
                {
                    if (Main.tile[x, j].HasTile && Main.tile[x, j].TileType == (ushort)ModContent.TileType<TorrefiedTephraPlaced>() && !Main.tile[x, j - 1].HasTile)
                    {
                        j--;
                        WorldGen.PlaceTile(x, j, TileID.ExposedGems, true, false, -1, 6);
                        break;
                    }
                }
            }

            WorldGen.setBG(6, 4);

            //Doesnt clear the map properly fsr idk why
            Main.Map.Clear();
            Main.clearMap = true;
            lockRespawn = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (saveWorldStatus == FileStatus.Requested)
                saveWorldStatus = FileStatus.Validated; //Validate the world save if it was requested
            tag["GotProfanedDesertJumpscare"] = gotJumpscared;
            tag["ScorchedWorld"] = scorchedWorld;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("GotProfanedDesertJumpscare", out bool jumpscared))
                gotJumpscared = jumpscared;
            if (tag.TryGet("ScorchedWorld", out bool scrch))
                scorchedWorld = scrch;
        }

        public override void SaveWorldHeader(TagCompound tag)
        {
            if (scorchedWorld)
                tag["ScorchedWorld"] = true;
        }

        public override void ClearWorld()
        {
            duplicateWorldStatus = default;
            flashTimer = -1;
            gotJumpscared = false;
            scorchedWorld = false;
            //saveWorldStatus = default;
        }
        #endregion

        #region Behavior
        public static void WorldEffects()
        {
            //Permanently noon
            Main.dayTime = true;
            Main.time = Main.dayLength / 2;

            Main.LocalPlayer.ZoneDesert = true;

            //Remove any seeds
            Main.remixWorld = false;
            Main.zenithWorld = false;
            Main.notTheBeesWorld = false;
            Main.drunkWorld = false;
            Main.noTrapsWorld = false;
        }


        #endregion
    }
}
