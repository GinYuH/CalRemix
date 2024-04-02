using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod.Buffs.DamageOverTime;
using CalRemix.Biomes;
using CalRemix.Projectiles.Hostile;
using Microsoft.Xna.Framework.Graphics;

namespace CalRemix.NPCs
{
    public class PlaguedSpidrone : ModNPC
    {
        public float[] GreenAI = new float[4];
        int xColumn = 0;
        int yMin = 0;
        int yMax = 1;
        int currentFrame = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plagued Spidrone");
            Main.npcFrameCount[NPC.type] = 12;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 70;
            NPC.width = 46;
            NPC.height = 46;
            NPC.defense = 23;
            NPC.lifeMax = 900;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 40);
            NPC.noGravity = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = CalamityMod.Sounds.CommonCalamitySounds.PlagueBoomSound;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            GreenAI[0] = 0;
            GreenAI[1] = 0;
        }

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest();
            switch (GreenAI[0])
            {
                case 0:
                    {
                        xColumn = 0;
                        yMin = 0;
                        yMax = 11;
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                        GreenAI[1]++;
                        CalamityMod.NPCs.CalamityGlobalAI.BuffedHerplingAI(NPC, Mod);
                        if (GreenAI[1] >= 120)
                        {
                            SwitchPhase(1);
                        }
                    }
                    break;
                case 1:
                    {
                        xColumn = 1;
                        yMin = 4;
                        yMax = 4;
                        NPC.velocity.X *= 0.95f;
                        NPC.noTileCollide = false;
                        GreenAI[1]++;
                        if (NPC.HasPlayerTarget)
                        {
                            if (GreenAI[1] % 20 == 0)
                            {
                                Vector2 dist = Main.player[NPC.target].position - NPC.position;
                                dist.Normalize();
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 20 * NPC.direction, NPC.Center.Y), dist * 15, ModContent.ProjectileType<PlagueSpit>(), NPC.damage, 0, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item17, NPC.Center);
                            }
                            NPC.direction = NPC.position.X > Main.player[NPC.target].position.X ? -1 : 1;
                        }
                        if (GreenAI[1] % 90 == 0)
                        {
                            SwitchPhase(2);
                        }
                        break;
                    }
                case 2:
                    {
                        xColumn = 1;
                        yMin = 0;
                        yMax = 3;
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        Vector2 dist = Main.player[NPC.target].Center - Vector2.UnitY * 160 - NPC.Center;
                        dist.Normalize();
                        NPC.velocity = dist * 4f;
                        GreenAI[1]++;
                        if (GreenAI[1] % (20 + Main.rand.Next(0, 8)) == 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-8, 9), Main.rand.Next(8, 12)), ProjectileID.GreekFire1 + Main.rand.Next(0, 3), NPC.damage, 0, Main.myPlayer);
                            SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                        }
                        if (GreenAI[1] >= 180)
                        {
                            SwitchPhase(0);
                        }
                        break;
                    }
            }
        }

        public void SwitchPhase(int ai0 = 0, int ai1 = 0, int ai2 = 0, int ai3 = 0)
        {
            GreenAI[0] = ai0;
            GreenAI[1] = ai1;
            GreenAI[2] = ai2;
            GreenAI[3] = ai3;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
        new FlavorTextBestiaryInfoElement("A common fungus that has grown angry from being infected by the plague.")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y != 0 && GreenAI[1] != 0)
            {
                NPC.frameCounter += 1.0;
                if (NPC.frameCounter > 6.0)
                {
                    NPC.frameCounter = 0.0;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 2)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = frameHeight * 3;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.InModBiome<PlagueBiome>())
                return 0f;

            return SpawnCondition.HardmodeJungle.Chance * 0.1f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<PlagueCellCanister>(), 1, 5, 8);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frameUsed = texture.Frame(2, 12, 0, 0);
            Rectangle squintFrame = texture.Frame(2, 12, 0, 0);

            Vector2 origin = new Vector2((float)(texture.Width / (2)), (float)(texture.Height / 14));
            Vector2 npcOffset = NPC.Center - screenPos;
            npcOffset -= new Vector2((float)texture.Width / 4, (float)texture.Height / 12) * NPC.scale / 2f;
            npcOffset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);

            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frameCounter = 0;
                currentFrame++;
            }
            if (currentFrame > yMax || currentFrame < yMin)
            {
                currentFrame = yMin;
            }
            frameUsed = texture.Frame(2, 12, xColumn, currentFrame);
            spriteBatch.Draw(texture, npcOffset, frameUsed, drawColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            return false;
        }
    }
}
