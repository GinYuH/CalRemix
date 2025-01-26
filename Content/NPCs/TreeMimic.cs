using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.UI;
using System.Linq;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs
{
    public class TreeMimic : ModNPC
    {

        public override string Texture => "CalamityMod/Projectiles/Summon/Umbrella/TreeForest";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tree");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 88;
            NPC.height = 418;
            NPC.defense = 4900;
            NPC.lifeMax = 49000000;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.dontTakeDamage = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = null;
            NPC.DeathSound = null;
        }

        public override void AI()
        {
            if (NPC.ai[1] < 2222)
            {
                NPC.ai[1]++;
            }
            if (NPC.ai[1] > 2 && NPC.ai[1] < 2222)
            {
                // check if there are any interrupting tiles nearby that will break the illusion
                Point bottom = NPC.Bottom.ToTileCoordinates();
                int checkX = 2;
                int checkY = 2;
                bool clearSpot = true;
                for (int i = bottom.X - checkX; i < bottom.X + checkX; i++)
                {
                    for (int j = bottom.Y - checkY; j < bottom.Y + 2; j++)
                    {
                        Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                        if ((t.TileType != TileID.Grass && t.TileType != TileID.Dirt && t.TileType != TileID.Plants && t.TileType != TileID.Plants2 && t.HasTile && j < bottom.Y + 1) || (j == bottom.Y + 1 && (!t.HasTile || t.Slope > 0)))
                        {
                            clearSpot = false;
                        }
                    }
                }
                if (!clearSpot)
                {
                    NPC.active = false;
                }
                NPC.ai[1] = 2222;
            }
            // deal defense damage when touched
            if (NPC.ai[0] <= 0)
            {
                foreach (Player p in Main.ActivePlayers)
                {
                    if (p.getRect().Intersects(NPC.getRect()))
                    {
                        p.Calamity().DealDefenseDamage(p.statDefense);
                        NPC.ai[0] = 180;
                        Main.BestiaryTracker.Kills.RegisterKill(NPC);
                    }
                }
            }
            NPC.ai[0]--;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value),                
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (SpawnCondition.OverworldDay.Active && spawnInfo.SpawnTileType == TileID.Grass)
            return SpawnCondition.OverworldDay.Chance * 0.22f;
            return 0;
        }
        // todo: add texture pack support? (reconstructing the entire tree)
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;
            Color color = Color.SkyBlue * NPC.Opacity;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, null, color, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, null, Color.White, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            return false;*/
            return true;
        }
    }
}
