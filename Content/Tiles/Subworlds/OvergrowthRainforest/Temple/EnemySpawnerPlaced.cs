using CalamityMod;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static AssGen.Assets;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest.Temple
{
    public class EnemySpawnerLoader : ModSystem
    {
        public static Dictionary<string, ModTileEntity> spawners = new();

        public override void Load()
        {
            AddEnemy("Chimp");
            AddEnemy("GigamothLarva");
            AddEnemy("LargeStinkbug");
        }

        public void AddEnemy(string enemyName)
        {
            EnemySpawnerPlaced newTile = new EnemySpawnerPlaced(enemyName);
            EnemySpawnerItem newItem = new EnemySpawnerItem(enemyName);
            Mod Remix = ModLoader.GetMod("CalRemix");
            newItem.designatedTile = newTile;
            Remix.AddContent(newTile);
            Remix.AddContent(newItem);
        }
    }

    [Autoload(false)]
    public class EnemySpawnerItem : ModItem
    {
        protected override bool CloneNewInstances => true;
        public override string Name => enemyName + "EnemySpawnerItem";
        public override string Texture => "CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Temple/EnemySpawnerPlaced";

        public ModTile designatedTile = null;
        public string enemyName = "";

        public EnemySpawnerItem(string name)
        {
            enemyName = name;
        }

        public override void SetDefaults() => Item.DefaultToPlaceableTile(designatedTile.Type);
    }

    [Autoload(false)]
    public class EnemySpawnerPlaced : ModTile
    {
        public override string Name => enemyName + "EnemySpawnerPlaced";
        public override string Texture => "CalRemix/Content/Tiles/Subworlds/OvergrowthRainforest/Temple/EnemySpawnerPlaced";
        public EnemySpawnerTE designatedTE = null;
        public string enemyName = "Chimp";

        public EnemySpawnerPlaced(string name)
        {
            enemyName = name;
        }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            DustType = -1;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CoordinateHeights = [16 ];
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<EnemySpawnerTE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (false)
                return;
            spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset, Main.DiscoColor);
            if (enemyName != "")
            {
                    int nme = CalRemix.instance.Find<ModNPC>(enemyName).Type;
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity TE))
                {
                    if (TE is EnemySpawnerTE eT)
                    {
                        spriteBatch.Draw(TextureAssets.Npc[nme].Value, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset, Color.White * 0.5f);
                    }
                }
            }
        }
    }

    public class EnemySpawnerTE : TempleTE
    {
        public override string Name => NPCLoader.GetNPC(enemyType).Name + "EnemySpawnerTE";
        public string EnemyName = "Observer";

        public int enemyType => ModLoader.GetMod("CalRemix").Find<ModNPC>(EnemyName)?.Type ?? NPCID.DemonEye;

        public int spawnTime = 0;

        public override bool IsTileValidForEntity(int x, int y)
        {
            return true;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            TileObjectData tileData = TileObjectData.GetTileData(type, style, alternate);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area. 
                NetMessage.SendTileSquare(Main.myPlayer, i, j, tileData.Width, tileData.Height);

                //Sync the placement of the tile entity with other clients
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);

                return -1;
            }
            int placedEntity = Place(i, j);

            return placedEntity;
        }

        public override void ObjectBehaviour()
        {
            spawnTime++;
            if (spawnTime == 300)
            {
                int enemyID = enemyType;
                Vector2 spawnPosition = new Vector2(Position.X * 16, Position.Y * 16);
                NPC.NewNPC(new EntitySource_TileEntity(this), (int)spawnPosition.X, (int)spawnPosition.Y, enemyID);
            }
            else if (spawnTime < 300)
            {
                Dust.NewDust(Position.ToVector2() * 16, 5, 5, DustID.GoldCoin);
            }
        }

        public override void Update()
        {
            if (EnemyName == "Observer")
            {
                if (TileLoader.GetTile(CalRemixHelper.ParanoidTileRetrieval(Position.X, Position.Y).TileType) is EnemySpawnerPlaced eP)
                {
                    EnemyName = eP.enemyName;
                }
            }
            base.Update();
        }

        public override void ResetObject()
        {
            spawnTime = 0;
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag.Add("enemyName", EnemyName);
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            EnemyName = tag.GetString("enemyName");
        }
    }
}
