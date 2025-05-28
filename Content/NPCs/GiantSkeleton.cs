using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.Biomes;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.Linq;
using CalRemix.Content.NPCs.Minibosses;
using CalRemix.Core.World;
using Terraria.Audio;
using System.Security.Cryptography.X509Certificates;
using CalRemix.Content.NPCs.Bosses.BossChanges.SupremeCalamitas;
using Terraria.WorldBuilding;
using CalamityMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;

namespace CalRemix.Content.NPCs
{
    public class GiantSkeleton : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float Mode => ref NPC.ai[1];
        public ref float TeleLocationX => ref NPC.ai[2];
        public ref float TeleLocationY => ref NPC.ai[3];

        private int VisualTimer = 0;
        private static int amountOfSpineSegments = 10;
        private static int amountOfSegmentFrames = 3;
        private int?[] segmentFrame = new int?[amountOfSpineSegments];

        public enum AttackTypes
        {
            None = -1,
            Spawn = 0,
            DigDown = 1,
            AttemptTeleport = 2,
            DigUp = 3,
            ShootProjsAtTarget = 4,
            TelegraphDig = 5
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.width = 46;
            NPC.height = 64;
            NPC.defense = 3;
            NPC.lifeMax = 660;
            NPC.value = 200;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit2 with { Pitch = 1, Volume = 2 };
            NPC.DeathSound = SoundID.NPCDeath2 with { Pitch = 1, Volume = 2 };
            NPC.rarity = 2;
            NPC.behindTiles = true;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToCold = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BrimstoneCragsBiome>().Type };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.velocity.X *= 0;

            #region Variables
            int aboveHeadOffset = 24;

            Vector2 spotAboveHead = new Vector2(NPC.Center.X, NPC.position.Y - aboveHeadOffset);
            Vector2 spotAtPelvis = new Vector2(NPC.Center.X, NPC.position.Y + (amountOfSpineSegments * 22) + 27);
            Vector2 spotAtChest = new Vector2(NPC.Center.X, NPC.position.Y + (amountOfSpineSegments / 2 * 22));
            Vector2 spotAtHalfHead = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height);
            Vector2 spotAbovePelvis = new Vector2(spotAtPelvis.X, spotAtPelvis.Y - 30);

            Vector2 spotAboveHeadTileCoords = new Vector2((int)spotAboveHead.X / 16, (int)spotAboveHead.Y / 16);
            //Vector2 spotAtChestTileCoords = new Vector2((int)spotAtChest.X / 16, (int)spotAtChest.Y / 16);
            Vector2 spotAtPelvisTileCoords = new Vector2((int)spotAtPelvis.X / 16, (int)spotAtPelvis.Y / 16);

            //Dust.NewDustPerfect(spotAboveHead, DustID.BlueFairy, Vector2.Zero);
            //Dust.NewDustPerfect(spotAtChest, DustID.PinkFairy, Vector2.Zero);
            //Dust.NewDustPerfect(spotAtPelvis, DustID.BlueFairy, Vector2.Zero);
            //Dust.NewDustPerfect(spotAtHalfHead, DustID.GreenFairy, Vector2.Zero);
            //Dust.NewDustPerfect(spotAbovePelvis, DustID.GreenFairy, Vector2.Zero);

            //Dust.NewDustPerfect(new Vector2(NPC.Center.X, TeleLocationY), DustID.BlueFairy, Vector2.Zero);
            //Dust.NewDustPerfect(new Vector2((int)NPC.Center.X, (int)NPC.position.Y), DustID.CrimsonSpray, Vector2.Zero);
            #endregion

            switch (Mode)
            {
                case (int)AttackTypes.None:
                    break;
                case (int)AttackTypes.Spawn:
                    TeleLocationY = NPC.position.Y;
                    Mode = (int)AttackTypes.AttemptTeleport;
                    break;
                case (int)AttackTypes.DigDown:
                    // move downwards slowly
                    float movementRateLeaving = Math.Clamp((float)Math.Pow(Timer / 15, 2), 0, 15);
                    NPC.position.Y += movementRateLeaving;

                    // if you go below the og starting pos, LEAVE!!!
                    bool isBelowStartingPosition = TeleLocationY < NPC.position.Y;
                    if (isBelowStartingPosition)
                    {
                        Timer = 0;
                        TeleLocationY = 0;
                        Mode = (int)AttackTypes.AttemptTeleport;
                    }
                    break;
                case (int)AttackTypes.AttemptTeleport:
                    // adapted from caster ai xd
                    Timer = 0;
                    
                    // using vanillas method to find a spot to tp to because it just works
                    int targetTileX = (int)Target.Center.X / 16;
                    int targetTileY = (int)Target.Center.Y / 16;
                    Vector2 chosenTile = Vector2.Zero;
                    if (NPC.AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY))
                    {
                        TeleLocationX = chosenTile.X;
                        TeleLocationY = chosenTile.Y;
                    }

                    // if the previous if statement succeeds, go throguh with the teleport
                    if (TeleLocationX != 0f && TeleLocationY != 0f)
                    {
                        NPC.position.X = TeleLocationX * 16f - (NPC.width / 2) + 8f;
                        NPC.position.Y = TeleLocationY * 16f + aboveHeadOffset;
                        NPC.velocity.X = 0f;
                        NPC.velocity.Y = 0f;
                        TeleLocationX = 0f;
                        TeleLocationY = 0f;

                        Timer = 0;
                        Mode = (int)AttackTypes.TelegraphDig;
                    }
                    break;
                case (int)AttackTypes.DigUp:
                    // epic emerge visual
                    if (Timer == 2)
                    {
                        SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = -1, Volume = 2f, Pitch = -1.2f }, NPC.position);
                        for (int i = 0; i < 5; i++)
                        {
                            if (CalamityUtils.ParanoidTileRetrieval((int)(NPC.Center.X) / 16, (int)(TeleLocationY / 16) - 1).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval((int)(NPC.Center.X) / 16, (int)(TeleLocationY / 16) - 1).IsTileSolidGround())
                            {
                                WorldGen.KillTile((int)(NPC.Center.X) / 16, ((int)TeleLocationY / 16) - 1, true, true);
                            }
                            if (CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) - 1, (int)(TeleLocationY / 16) - 1).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) - 1, (int)(TeleLocationY / 16) - 1).IsTileSolidGround())
                            {
                                WorldGen.KillTile(((int)(NPC.Center.X) / 16) - 1, ((int)TeleLocationY / 16) - 1, true, true);
                            }
                            if (CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) + 1, (int)(TeleLocationY / 16) - 1).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) + 1, (int)(TeleLocationY / 16) - 1).IsTileSolidGround())
                            {
                                WorldGen.KillTile(((int)(NPC.Center.X) / 16) + 1, ((int)TeleLocationY / 16) - 1, true, true);
                            }
                        }
                    }

                    // move upwards slowly
                    // start w a big burst of speed, then slow down
                    float movementRateEntering = Math.Clamp(15 - (float)Math.Pow(Timer / 6, 2), 2, 15);
                    NPC.position.Y -= movementRateEntering;

                    // check if the tile at the pelvis point ISNT a tile
                    bool checkPelvisTileIsSolid = CalamityUtils.ParanoidTileRetrieval((int)spotAtPelvisTileCoords.X, (int)spotAtPelvisTileCoords.Y).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval((int)spotAtPelvisTileCoords.X, (int)spotAtPelvisTileCoords.Y).IsTileSolidGround();
                    bool checkPelvisIsInLava = Collision.LavaCollision(spotAtPelvis, 8, 8);
                    bool checkPelvisIsInRightSpot = spotAtPelvis.Y < TeleLocationY;
                    // if we take too long to find a nice place to stop at, then reduce the radius we check for tiles
                    Vector2 spotToRaycastTo = Timer >= 40 ? spotAbovePelvis : spotAtChest;
                    // if the head hits the ceiling prematurely then FUCK!!!!!!!
                    // we also make sure that there are tiles between them, so this cant trigger while burrowing out of tiles
                    bool checkTopOfHeadIsSolid = CalamityUtils.ParanoidTileRetrieval((int)NPC.Center.X / 16, (int)NPC.position.Y / 16).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval((int)NPC.Center.X / 16, (int)NPC.position.Y / 16).IsTileSolidGround();
                    bool headHittingCeiling = Timer >= 10 && !Collision.CanHitLine(spotAboveHead, 2, 2, spotAtHalfHead, 2, 2) && checkPelvisTileIsSolid && checkTopOfHeadIsSolid;
                    if (headHittingCeiling || !checkPelvisTileIsSolid && checkPelvisIsInRightSpot && Collision.CanHitLine(spotAtPelvis, 2, 2, spotToRaycastTo, 2, 2))
                    {
                        if (checkPelvisIsInLava)
                            return;
                        // if it is, enter attack mode
                        if (headHittingCeiling)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit2 with { MaxInstances = -1, Volume = 2f }, NPC.position);
                            WorldGen.KillTile((int)NPC.Center.X / 16, (int)NPC.position.Y / 16, true, true);
                        }
                        Timer = 0;
                        Mode = (int)AttackTypes.ShootProjsAtTarget;
                    }
                    break;
                case (int)AttackTypes.ShootProjsAtTarget:
                    int timerOffset = (int)Timer + 60;
                    
                    if (Timer >- timerOffset && timerOffset % 50 == 0 && timerOffset <= 200)
                    {
                        SoundEngine.PlaySound(SoundID.Item73 with { MaxInstances = -1, Volume = 2f }, NPC.position);
                        for (int i = 0; i < 12; i++)
                        {
                            Vector2 dustVelocity = new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-5, 5));
                            Dust.NewDustPerfect(NPC.Center, ModContent.DustType<BrimstoneFireDustMatte>(), dustVelocity);
                        }
                        Vector2 velocity = NPC.DirectionTo(Target.Center) * 4;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<BrimstoneBall>(), 25, 0, Main.myPlayer);
                    }

                    
                    if (timerOffset >= 300)
                    {
                        Timer = 0;
                        Mode = (int)AttackTypes.DigDown;
                    }
                    break;
                case (int)AttackTypes.TelegraphDig:
                    // saving this value for later
                    if (Timer == 1)
                    {
                        TeleLocationY = NPC.position.Y;
                    }

                    int timerInterval = 30;
                    int timerDelay = 20;
                    int timerWithDelay = (int)Timer - timerDelay;
                    int knockOnWoodCount = 2;
                    if (Timer >= timerDelay && timerWithDelay % timerInterval == 0 && Timer - timerDelay != (timerInterval * knockOnWoodCount))
                    {
                        SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = -1, Volume = 2f }, NPC.position);

                        // originally i was going to get the dust and make it bigger, but                     .
                        for (int i = 0; i < 3; i++)
                        {
                            if (CalamityUtils.ParanoidTileRetrieval((int)(NPC.Center.X) / 16, (int)(TeleLocationY / 16) - 1).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval((int)(NPC.Center.X) / 16, (int)(TeleLocationY / 16) - 1).IsTileSolidGround())
                            {
                                WorldGen.KillTile((int)(NPC.Center.X) / 16, ((int)TeleLocationY / 16) - 1, true, true);
                            }
                            if (CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) - 1, (int)(TeleLocationY / 16) - 1).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) - 1, (int)(TeleLocationY / 16) - 1).IsTileSolidGround())
                            {
                                WorldGen.KillTile(((int)(NPC.Center.X) / 16) - 1, ((int)TeleLocationY / 16) - 1, true, true);
                            }
                            if (CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) + 1, (int)(TeleLocationY / 16) - 1).IsTileSolid() && CalamityUtils.ParanoidTileRetrieval(((int)(NPC.Center.X) / 16) + 1, (int)(TeleLocationY / 16) - 1).IsTileSolidGround())
                            {
                                WorldGen.KillTile(((int)(NPC.Center.X) / 16) + 1, ((int)TeleLocationY / 16) - 1, true, true);
                            }
                        }
                    }

                    if (timerWithDelay >= timerInterval * knockOnWoodCount)
                    {
                        Timer = 0;
                        Mode = (int)AttackTypes.DigUp;
                    }
                    break;
            }

            Timer++;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.InModBiome<AstralInfectionBiome>() || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea)
                return 0f;
            if (spawnInfo.Player.Calamity().ZoneCalamity)
                return 0.05f;
            else if (spawnInfo.Player.position.Y / 16 > GenVars.lavaLine)
                return SpawnCondition.Cavern.Chance * 0.025f;
            else
                return SpawnCondition.Underworld.Chance * 0.025f;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Crimslime, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Crimslime, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<AncientBoneDust>(), 2);
            npcLoot.Add(ModContent.ItemType<DemonicBoneAsh>(), 2);

            LeadingConditionRule hardmode = npcLoot.DefineConditionalDropSet(DropHelper.Hardmode());
            LeadingConditionRule postProv = npcLoot.DefineConditionalDropSet(DropHelper.PostProv());
            hardmode.Add(ModContent.ItemType<EssenceofHavoc>(), 2);
            postProv.Add(ModContent.ItemType<Bloodstone>(), 4);
            // Copy over all loot from Dungeon Guardians since theyre dead
            List<IItemDropRule> gardRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.DungeonGuardian, false);
            foreach (var v in gardRules)
            {
                npcLoot.Add(v);
            }
        }

        private struct BoneSpur
        {
            public int width;
            public int height;
            /// <summary>
            /// How far right from the true origin you want to start the render origin at.
            /// </summary>
            public int offsetX;
            /// <summary>
            /// How far down from the true origin you want to start the render origin at.
            /// </summary>
            public int offsetY;
            public Vector2 startingPoint;

            #region Constructors
            public BoneSpur(int width, int height, int offsetX = 0, int offsetY = 0)
            {
                this.width = width;
                this.height = height;
                this.offsetX = offsetX;
                this.offsetY = offsetY;
                startingPoint = Vector2.Zero;
            }
            public BoneSpur(int width, int height, BoneSpur previousSpur, int offsetX = 0, int offsetY = 0)
            {
                this.width = width;
                this.height = height;
                this.offsetX = offsetX;
                this.offsetY = offsetY;
                int startingPointX = 0;
                int startingPointY = (int)previousSpur.startingPoint.Y + previousSpur.height + 2;
                startingPoint = new Vector2(startingPointX, startingPointY);
            }
            #endregion

            public Rectangle GetLeftRectangle()
            {
                Rectangle leftRect = new Rectangle((int)startingPoint.X, (int)startingPoint.Y, (int)width, (int)height);
                return leftRect;
            }
            public Rectangle GetRightRectangle()
            {
                Rectangle rightRect = new Rectangle((int)startingPoint.X + (int)width + 2, (int)startingPoint.Y, (int)width, (int)height);
                return rightRect;
            }
            public Vector2 GetLeftOrigin()
            {
                Vector2 leftOrigin = new Vector2(offsetX + width, offsetY);
                return leftOrigin;
            }
            public Vector2 GetRightOrigin()
            {
                Vector2 rightOrigin = new Vector2(offsetX, offsetY);
                return rightOrigin;
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                // sometimes when hes teleporting or starting digup you can see him for a split second
                // this fixes that by not having him render during those times
                if (Mode == (int)AttackTypes.Spawn || Mode == (int)AttackTypes.AttemptTeleport || Mode == (int)AttackTypes.TelegraphDig || Mode == (int)AttackTypes.DigUp && Timer == 1)
                    return false;
            }
            
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Texture2D extras = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/GiantSkeletonExtras").Value;
            Texture2D spine = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/GiantSkeletonSpine").Value;
            Vector2 pelvis = new Vector2(52, 38);
            Vector2 ribs = new Vector2(68, 56);
            BoneSpur spur5 = new BoneSpur(28, 18, 0, 4);
            BoneSpur spur4 = new BoneSpur(24, 16, spur5, 0, 6);
            BoneSpur spur3 = new BoneSpur(16, 14, spur4, 0, 8);
            BoneSpur spur2 = new BoneSpur(18, 12, spur3, 0, 6);
            BoneSpur spur1 = new BoneSpur(12, 10, spur2, 0, 6);
            BoneSpur[] spurList = { spur5, spur4, spur2, spur3, spur1 };
            Color trueDrawColor = NPC.IsABestiaryIconDummy ? Color.White : drawColor;

            RasterizerState rasterizer = Main.Rasterizer;
            // execute NONE of this in the bestiary 
            if (!NPC.IsABestiaryIconDummy)
            {
                // this rectangle contains the entirety of bro. it also contains everything above the og tp location
                int cullingHeight = (int)(TeleLocationY - screenPos.Y);
                Rectangle entireSkeleton = new Rectangle(0, 0, Main.screenWidth, cullingHeight);
                // and we use that to do cull everything underneath our awesome arbitrary position
                rasterizer.ScissorTestEnable = true;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, Main.Transform);
                spriteBatch.GraphicsDevice.ScissorRectangle = entireSkeleton;
            }

            // a bunch of spine calculation stuff 
            // we won't render until near the end so that everything else can be rendered underneath it,
            // but we need the info 
            int initialOffset = 15;
            float[] horizOffsets = new float[amountOfSpineSegments];
            for (int i = 0; i < amountOfSpineSegments; i++)
            {
                horizOffsets[i] = (float)Math.Sin((VisualTimer + (i * 15)) / 54f) * 5f;
                if (segmentFrame[i] == null)
                {
                    segmentFrame[i] = Main.rand.Next(0, 3);
                }
            }

            // drawing the pelvis
            spriteBatch.Draw(extras, new Vector2(NPC.Center.X + horizOffsets[amountOfSpineSegments - 1], NPC.Center.Y + ((spine.Height / amountOfSegmentFrames) - 2) * amountOfSpineSegments) - screenPos, new Rectangle(0, 80, (int)pelvis.X, (int)pelvis.Y), trueDrawColor, NPC.rotation, new Vector2(pelvis.X / 2, pelvis.Y / 2), NPC.scale, SpriteEffects.None, 0f);

            // drawing the background ribs
            spriteBatch.Draw(extras, new Vector2(NPC.Center.X + horizOffsets[3], NPC.Center.Y + 60) - screenPos, new Rectangle(54, 20 + (int)ribs.Y + 2, (int)ribs.X, (int)ribs.Y), trueDrawColor, NPC.rotation, new Vector2(ribs.X / 2, ribs.Y / 2), NPC.scale, SpriteEffects.None, 0f);

            // drawing the side bone spurs
            for (int i = 0; i <= spurList.Length - 1; i++)
            {
                float independantRotationOffset = i * 25;
                float rotate = (float)Math.Sin((VisualTimer + independantRotationOffset) / 64f) * 0.5f;
                float rotateInverted = rotate * -1;
                float posOffsetY = 18 * i;

                spriteBatch.Draw(extras, new Vector2(NPC.Center.X + 0 + horizOffsets[i + 3], NPC.Center.Y + 95 + posOffsetY) - screenPos, spurList[i].GetLeftRectangle(), trueDrawColor, NPC.rotation + rotate, spurList[i].GetLeftOrigin(), NPC.scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(extras, new Vector2(NPC.Center.X - 0 + horizOffsets[i + 3], NPC.Center.Y + 95 + posOffsetY) - screenPos, spurList[i].GetRightRectangle(), trueDrawColor, NPC.rotation + rotateInverted, spurList[i].GetRightOrigin(), NPC.scale, SpriteEffects.None, 0f);
            }

            // drawing the spine
            for (int i = 0; i < amountOfSpineSegments; i++)
            {
                int extraOffset = ((spine.Height / amountOfSegmentFrames) - 2) * i;
                spriteBatch.Draw(spine, new Vector2(NPC.Center.X + horizOffsets[i], NPC.Center.Y + initialOffset + extraOffset) - screenPos, spine.Frame(1, 3, 0, (int)segmentFrame[i]), trueDrawColor, NPC.rotation, new Vector2(spine.Width / 2, spine.Height / 6), NPC.scale, SpriteEffects.None, 0f);
            }

            // drawing the ribs
            spriteBatch.Draw(extras, new Vector2(NPC.Center.X + horizOffsets[3], NPC.Center.Y + 60) - screenPos, new Rectangle(54, 20, (int)ribs.X, (int)ribs.Y), trueDrawColor, NPC.rotation, new Vector2(ribs.X / 2, ribs.Y / 2), NPC.scale, SpriteEffects.None, 0f);

            // and finally, drawing the skull
            spriteBatch.Draw(texture, new Vector2(NPC.Center.X + horizOffsets[0], NPC.Center.Y) - screenPos, null, trueDrawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 2), NPC.scale, SpriteEffects.None, 0f);

            if (!NPC.IsABestiaryIconDummy)
            {
                // end that spritebatch fuckery we started earlier. feel free not to end it for epic fail
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, Main.Transform);
                spriteBatch.ReleaseCutoffRegion(Main.Transform);
            }

            VisualTimer++;
            return false;
        }
    }
}
