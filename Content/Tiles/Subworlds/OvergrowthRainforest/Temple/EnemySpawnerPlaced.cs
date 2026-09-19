using CalamityMod;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.NPCs;
using CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest.Temple
{
    public class EnemySpawnerLoader : ModSystem
    {
        public override void Load()
        {
            AddEnemy(ModContent.NPCType<Chimp>());
            AddEnemy(ModContent.NPCType<LargeStinkbug>());
            AddEnemy(ModContent.NPCType<GigamothLarva>());
        }

        public void AddEnemy(int type)
        {
            string enemyName = NPCLoader.GetNPC(type).Name;
            EnemySpawnerPlaced newTile = new EnemySpawnerPlaced(enemyName);
            EnemySpawnerTE newTE = new EnemySpawnerTE(type);
            EnemySpawnerItem newItem = new EnemySpawnerItem(enemyName);
            Mod Remix = ModLoader.GetMod("CalRemix");
            newTile.designatedTE = newTE;
            newItem.designatedTile = newTile;
            Remix.AddContent(newTE);
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
        public string enemyName = "";

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
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(designatedTE.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Type t = ModContent.GetInstance < typeof(designatedTE) > ();
            spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset, Main.DiscoColor);
            if (designatedTE != null)
            if (designatedTE.type > 0)
            {
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity TE))
                {
                    if (TE is EnemySpawnerTE eT)
                    {
                        spriteBatch.Draw(TextureAssets.Npc[eT.enemyType].Value, new Vector2(i * 16, j * 16) - Main.screenPosition + CalamityUtils.TileDrawOffset, Color.White * 0.5f);
                    }
                }
            }
        }
    }

    [Autoload(false)]
    public class EnemySpawnerTE : TempleTE
    {
        public override string Name => NPCLoader.GetNPC(enemyType).Name + "EnemySpawnerTE";
        public int enemyType = NPCID.DemonEye;

        public int spawnTime = 0;

        public EnemySpawnerTE(int enemyType)
        {
            this.enemyType = enemyType;
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

        public override void ResetObject()
        {
            spawnTime = 0;
        }
    }
}
