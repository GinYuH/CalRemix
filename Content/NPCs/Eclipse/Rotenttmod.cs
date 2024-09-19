using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class Rodenttmod : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rodent.tmod");
            Main.npcFrameCount[NPC.type] = 6;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 900;
            NPC.damage = 100;
            NPC.defense = 15;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(gold: 6, silver: 66);
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noTileCollide = true;
        }
        public override void AI()
        {
            NPC.TargetClosest(false);

            if (NPC.HasPlayerTarget)
            {
                Player target = Main.player[NPC.target];
                float speed = 0.75f;
                int xBuffer = 60;
                int yBuffer = 60;
                int maxX = 14;
                int maxY = 14;
                if (NPC.position.X > target.position.X + xBuffer)
                {
                    NPC.velocity.X -= speed;
                    if (NPC.velocity.X <= -maxX)
                    {
                        NPC.velocity.X = -maxX;
                    }
                }
                else if (NPC.position.X < target.position.X - xBuffer)
                {
                    NPC.velocity.X += speed;
                    if (NPC.velocity.X > maxX)
                    {
                        NPC.velocity.X = maxX;
                    }
                }
                else
                {
                    NPC.velocity.X *= 0.998f;
                }
                if (NPC.position.Y > target.position.Y + yBuffer)
                {
                    NPC.velocity.Y -= speed;
                    if (NPC.velocity.Y <= -maxY)
                    {
                        NPC.velocity.Y = -maxY;
                    }
                }
                else if (NPC.position.Y < target.position.Y - yBuffer)
                {
                    NPC.velocity.Y += speed;
                    if (NPC.velocity.Y > maxY)
                    {
                        NPC.velocity.Y = maxY;
                    }
                }
                else
                {
                    NPC.velocity.Y *= 0.998f;
                }
            }
            else
            {
                NPC.velocity.Y -= 1;
                if (NPC.velocity.Y <= -30)
                {
                    NPC.velocity.Y = -30;
                }
            }
            if (NPC.velocity.Length() <= 1)
            {
                if (NPC.velocity == Vector2.Zero)
                {
                    NPC.velocity = Vector2.One;
                }
                NPC.velocity *= 4f;
            }

            NPC.rotation = NPC.velocity.ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0);
            if (NPC.velocity.X > 0)
            {
                NPC.direction = 1;
            }
            else
            {
                NPC.direction = -1;
            }
            NPC.spriteDirection = -NPC.direction;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement("What happens when a hedgehog falls into a         ? The answer:")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter > 6.0)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > frameHeight * 5)
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG)
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.5f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), new Fraction(3, 4), 1, 2);
        }
    }
}