using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Potions;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.NPCs
{
    public class Shaggy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.damage = 150;
            NPC.width = 50;
            NPC.height = 80;
            NPC.defense = 8;
            NPC.lifeMax = 2000;
            NPC.knockBackResist = 0.9f;
            NPC.value = Item.buyPrice(silver: 2);
            NPC.noGravity = false;
            NPC.HitSound = CalamityMod.NPCs.NormalNPCs.Rimehound.HitSound with { Pitch = 1 };
            NPC.DeathSound = SoundID.NPCDeath27 with { Pitch = 1 };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player target = Main.player[NPC.target];
            NPC.spriteDirection = -NPC.velocity.X.DirectionalSign();

            NPC.velocity.Y += 0.03f;
            int dirTarget = NPC.Center.X > target.Center.X ? -1 : 1;
            NPC.velocity.X += 0.05f * dirTarget;
            if ((NPC.velocity.X > 0 && dirTarget < 0) || (NPC.velocity.X < 0 && dirTarget > 0))
            {
                NPC.velocity.X += 0.12f * dirTarget;
            }

            int posX = dirTarget == -1 ? (int)(NPC.position.X - 10) / 16 : (int)(NPC.position.X + NPC.width + 10) / 16;
            int posY = (int)(NPC.position.Y + NPC.height - 10) / 16;
            if ((Main.tile[posX, posY].IsTileSolid() || Main.tile[posX - dirTarget, posY].IsTileSolid()) && NPC.ai[0] <= 0 && NPC.velocity.Length() >= 2)
            {
                NPC.position.Y -= 80;
                NPC.ai[0] = 10;
            }
            // clip thru wall 
            if ((Main.tile[posX, posY].IsTileSolid() || Main.tile[posX - dirTarget, posY].IsTileSolid()) && NPC.ai[0] <= 0 && NPC.velocity.Length() <= 2)
            {
                NPC.ai[0] = Main.rand.Next(2, 5);
                NPC.position.X += 10 * dirTarget;
            }
            //DUMBFUCK JUMP
            if (Main.tile[posX - dirTarget, posY + 1].IsTileSolid() && Main.tile[posX - dirTarget, posY + 1].IsHalfBlock)
            {
                NPC.position.Y -= 80;
            }

            if (Main.rand.NextBool(700))
                NPC.velocity = Vector2.Zero;

            if (Main.rand.NextBool(400))
                NPC.velocity.Y = -10;

            if (Main.rand.NextBool(400))
                NPC.scale = Main.rand.NextFloat(0.8f, 1.2f);

            NPC.ai[0]--;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X >= 2f || NPC.velocity.X <= -2f)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 4)
                {
                    NPC.frame.Y += 104;
                    NPC.frameCounter = 0;
                }
                if (NPC.frame.Y > 104 * 8)
                    NPC.frame.Y = 104;
            }
            else
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (SpawnCondition.OverworldNight.Active && Main.hardMode)
                return SpawnCondition.OverworldNightMonster.Chance * 0.05f;
            return 0;
        }
    }
}
