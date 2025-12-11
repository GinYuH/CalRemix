using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;
using Terraria.GameContent;

namespace CalRemix.Content.NPCs.Subworlds.ClownWorld
{
    public class BananaMan : ModNPC
    {
        public ref float unsure => ref NPC.ai[0];
        public ref float IsOnWall => ref NPC.ai[1];
        public ref float ShouldRandomlyFall => ref NPC.ai[2];
        public ref float CurrentMode => ref NPC.ai[3];
        public ref float FrameColumn => ref NPC.localAI[0];
        private int VisualTimer = 0;
        private int FrameToUse = 0;
        public static readonly SoundStyle BananamanHurt = new("CalRemix/Assets/Sounds/BananamanOw")
        {
            PitchVariance = 0.6f,
            MaxInstances = 0
        };
        public static readonly SoundStyle BananamanDie = new("CalRemix/Assets/Sounds/BananamanDie")
        {
            PitchVariance = 0.6f,
            MaxInstances = 0
        };
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Evil Eye");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GlowingSnail);
            NPC.aiStyle = -1;
            NPC.width = 16;
            NPC.height = 16;
            NPC.life = 10000;
            NPC.lifeMax = 10000;
            NPC.defense = 2; // one for each lens of his sunglasses
            NPC.knockBackResist = 0;
            NPC.HitSound = BananamanHurt;
            NPC.DeathSound = BananamanDie;
            NPC.frame = new Rectangle(0, 0, 34, 56);
        }

        public override void AI()
        {
            // this is messy and ugly but it works goddamnit
            float speed = 0.6f;

            if (Main.rand.NextBool(500) && NPC.noGravity == true && CurrentMode != 2)
            {
                if (CurrentMode == 0)
                    CurrentMode = 1;
                else
                    CurrentMode = 0;
            }

            #region frame stuff
            // frametouse and visualtimer are flipped sorrey :(( but im too lazy to fix it 
            FrameToUse++;
            if (FrameToUse % 4 == 0)
            {
                VisualTimer++;
                FrameToUse = 0;
            }

            if (CurrentMode == 0)
            {
                FrameColumn = 1;
                if (VisualTimer > 2) 
                    VisualTimer = 0;
            }
            else
            {
                if (CurrentMode == 1)
                {
                    FrameColumn = 0;
                    VisualTimer = 0;
                }
                else
                {
                    FrameColumn = 2;
                    if (VisualTimer > 29)
                    {
                        VisualTimer = 0;
                        CurrentMode = 1;
                    }
                }

                if (Main.rand.NextBool(240) && CurrentMode == 1)
                {
                    CurrentMode = 2;
                }
            }
            #endregion

            // do movement if mode is 1, otherwise stand still
            if (CurrentMode == 0)
            {
                if (NPC.ai[0] == 0f)
                {
                    NPC.TargetClosest();
                    NPC.directionY = 1;
                    NPC.ai[0] = 1f;
                    if (NPC.direction > 0)
                    {
                        NPC.spriteDirection = 1;
                    }
                }
                bool flag61 = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // randomly decide to fall sometimes
                    if (ShouldRandomlyFall == 0f && Main.rand.NextBool(7200))
                    {
                        //ShouldRandomlyFall = 2f;
                        NPC.netUpdate = true;
                    }

                    if (!NPC.collideX && !NPC.collideY)
                    {
                        NPC.localAI[3] += 1f;
                        if (NPC.localAI[3] > 5f)
                        {
                            ShouldRandomlyFall = 2f;
                            NPC.netUpdate = true;
                        }
                    }
                    else
                    {
                        NPC.localAI[3] = 0f;
                    }
                }

                // handle falling
                if (ShouldRandomlyFall > 0f)
                {
                    IsOnWall = 0f;
                    NPC.ai[0] = 1f;
                    NPC.directionY = 1;
                    if (NPC.velocity.Y > speed)
                    {
                        NPC.rotation += (float)NPC.direction * 0.1f;
                    }
                    else
                    {
                        NPC.rotation = 0f;
                    }
                    NPC.spriteDirection = NPC.direction;
                    NPC.velocity.X = speed * (float)NPC.direction;
                    NPC.noGravity = false;

                    int num50 = (int)(NPC.Center.X + (float)(NPC.width / 2 * -NPC.direction)) / 16;
                    int num51 = (int)(NPC.position.Y + (float)NPC.height + 8f) / 16;
                    if (Main.tile[num50, num51] != null && !(Main.tile[num50, num51].Slope == SlopeType.SlopeUpRight || Main.tile[num50, num51].Slope == SlopeType.SlopeUpLeft) && NPC.collideY)
                    {
                        ShouldRandomlyFall -= 1f;
                    }
                    num51 = (int)(NPC.position.Y + (float)NPC.height - 4f) / 16;
                    num50 = (int)(NPC.Center.X + (float)(NPC.width / 2 * NPC.direction)) / 16;
                    if (Main.tile[num50, num51] != null && (Main.tile[num50, num51].Slope == SlopeType.SlopeDownRight || Main.tile[num50, num51].Slope == SlopeType.SlopeDownLeft))
                    {
                        NPC.direction *= -1;
                    }

                    if (NPC.collideX && NPC.velocity.Y == 0f)
                    {
                        flag61 = true;
                        ShouldRandomlyFall = 0f;
                        NPC.directionY = -1;
                        IsOnWall = 1f;
                    }
                    if (NPC.velocity.Y == 0f)
                    {
                        if (NPC.localAI[1] == NPC.position.X)
                        {
                            NPC.localAI[2] += 1f;
                            if (NPC.localAI[2] > 10f)
                            {
                                NPC.direction = 1;
                                NPC.velocity.X = (float)NPC.direction * speed;
                                NPC.localAI[2] = 0f;
                            }
                        }
                        else
                        {
                            NPC.localAI[2] = 0f;
                            NPC.localAI[1] = NPC.position.X;
                        }
                    }
                }
                if (ShouldRandomlyFall != 0f)
                {
                    return;
                }
                NPC.noGravity = true;

                if (IsOnWall == 0f)
                {
                    if (NPC.collideY)
                    {
                        NPC.ai[0] = 2f;
                    }
                    if (!NPC.collideY && NPC.ai[0] == 2f)
                    {
                        NPC.direction = -NPC.direction;
                        IsOnWall = 1f;
                        NPC.ai[0] = 1f;
                    }
                    if (NPC.collideX)
                    {
                        NPC.directionY = -NPC.directionY;
                        IsOnWall = 1f;
                    }
                }
                else
                {
                    if (NPC.collideX)
                    {
                        NPC.ai[0] = 2f;
                    }
                    if (!NPC.collideX && NPC.ai[0] == 2f)
                    {
                        NPC.directionY = -NPC.directionY;
                        IsOnWall = 0f;
                        NPC.ai[0] = 1f;
                    }
                    if (NPC.collideY)
                    {
                        NPC.direction = -NPC.direction;
                        IsOnWall = 0f;
                    }
                }

                if (!flag61)
                {
                    float num52 = NPC.rotation;
                    if (NPC.directionY < 0)
                    {
                        if (NPC.direction < 0)
                        {
                            if (NPC.collideX)
                            {
                                NPC.rotation = 1.57f;
                                NPC.spriteDirection = -1;
                            }
                            else if (NPC.collideY)
                            {
                                NPC.rotation = 3.14f;
                                NPC.spriteDirection = 1;
                            }
                        }
                        else if (NPC.collideY)
                        {
                            NPC.rotation = 3.14f;
                            NPC.spriteDirection = -1;
                        }
                        else if (NPC.collideX)
                        {
                            NPC.rotation = 4.71f;
                            NPC.spriteDirection = 1;
                        }
                    }
                    else if (NPC.direction < 0)
                    {
                        if (NPC.collideY)
                        {
                            NPC.rotation = 0f;
                            NPC.spriteDirection = -1;
                        }
                        else if (NPC.collideX)
                        {
                            NPC.rotation = 1.57f;
                            NPC.spriteDirection = 1;
                        }
                    }
                    else if (NPC.collideX)
                    {
                        NPC.rotation = 4.71f;
                        NPC.spriteDirection = -1;
                    }
                    else if (NPC.collideY)
                    {
                        NPC.rotation = 0f;
                        NPC.spriteDirection = 1;
                    }
                    float num53 = NPC.rotation;
                    NPC.rotation = num52;

                    if ((double)NPC.rotation > 6.28)
                    {
                        NPC.rotation -= 6.28f;
                    }
                    if (NPC.rotation < 0f)
                    {
                        NPC.rotation += 6.28f;
                    }
                    float num54 = Math.Abs(NPC.rotation - num53);
                    float num55 = 0.1f;

                    if (NPC.rotation > num53)
                    {
                        if ((double)num54 > 3.14)
                        {
                            NPC.rotation += num55;
                        }
                        else
                        {
                            NPC.rotation -= num55;
                            if (NPC.rotation < num53)
                            {
                                NPC.rotation = num53;
                            }
                        }
                    }
                    if (NPC.rotation < num53)
                    {
                        if ((double)num54 > 3.14)
                        {
                            NPC.rotation -= num55;
                        }
                        else
                        {
                            NPC.rotation += num55;
                            if (NPC.rotation > num53)
                            {
                                NPC.rotation = num53;
                            }
                        }
                    }
                }
                NPC.velocity.X = speed * NPC.direction;
                NPC.velocity.Y = speed * NPC.directionY;
            }
            else
            {
                NPC.noGravity = true;
                NPC.velocity = Vector2.Zero;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            SpriteEffects flip = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 pos = NPC.Center - screenPos + Vector2.UnitY * NPC.gfxOffY - (Vector2.UnitY * 18).RotatedBy(NPC.rotation);
            spriteBatch.Draw(texture, pos, texture.Frame(3, 30, (int)FrameColumn, (int)VisualTimer), drawColor, NPC.rotation, new Vector2(texture.Width / 6, texture.Height / 60), NPC.scale, flip, 0f);

            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return 0f;
            return SpawnCondition.OverworldNight.Chance * 0.02f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ItemID.Nazar, 1);
        }
    }
}
