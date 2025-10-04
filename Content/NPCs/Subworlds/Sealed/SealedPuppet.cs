using CalamityMod;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.Biomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;
//using CalamityMod.CalPlayer;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class SealedPuppet : ModNPC
    {
        public static List<int> infighting = new()
        {
            NPCType<SealedPuppet>(),
            NPCType<SealedCitizen>(),
            NPCType<EvilSealedPuppet>(),
            NPCType<SealedPorswine>(),
            NPCType<TemporalAbomination>(),
            NPCType<DoUHead>()
        };

        public static SoundStyle SealedSound = new SoundStyle("CalRemix/Assets/Sounds/SealedIdle") { PitchVariance = 1f, MaxInstances = 0 };

        public override void SetStaticDefaults()
        {
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.UsesNewTargetting[Type] = true;
            Main.npcFrameCount[NPC.type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Squirrel);
            NPC.width = 30;
            NPC.height = 54;
            NPC.friendly = false;
            NPC.npcSlots = 1;
            NPC.HitSound = new SoundStyle("CalRemix/Assets/Sounds/SealedHurt");
            NPC.DeathSound = new SoundStyle("CalRemix/Assets/Sounds/SealedDeath");
            NPC.lifeMax = 1000;
            NPC.defense = 10;
            NPC.damage = 40;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
            SpawnModBiomes = [GetInstance<SealedFieldsBiome>().Type];
        }

        public override void AI()
        {
            NPCUtils.TargetSearchResults results = NPCUtils.SearchForTarget(NPC, NPCUtils.TargetSearchFlag.NPCs, npcFilter: (NPC n) => Collision.CanHitLine(NPC.Top, 1, 1, n.Center, 1, 1) && NPC.Distance(n.Center) < 920 && infighting.Contains(n.type) && n.type != Type);
            NPC.TargetClosest();
            if (NPC.type == NPCType<TemporalAbomination>() && Main.player[NPC.target].Distance(NPC.Center) < 2000)
            {
                NPC.aiStyle = -1;
                Player target = Main.player[NPC.target];
                int direction = NPC.Center.X < target.Center.X ? 1 : -1;

                float acc = 0.3f;
                float speed = 5.5f;

                if (direction == 1 && NPC.velocity.X < speed)
                {
                    NPC.velocity.X += acc;
                }
                else if (direction == -1 && NPC.velocity.X > -speed)
                {
                    NPC.velocity.X -= acc;
                }
                NPC.direction = direction;

                Vector2 frontTilePos = NPC.Bottom + new Vector2(NPC.direction * NPC.width / 2 + 1, -4);
                Point tileCoords = frontTilePos.ToTileCoordinates();

                Tile tile = Framing.GetTileSafely(tileCoords.X, tileCoords.Y);
                Tile above = Framing.GetTileSafely(tileCoords.X, tileCoords.Y - 1);
                if (Main.rand.NextBool(200))
                {
                    int spawnType = Main.rand.NextBool() ? NPCID.GiantFlyingFox : NPCID.DesertScorpionWalk;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, spawnType);
                    }
                }

                if ((tile.HasTile && Main.tileSolid[tile.TileType] || Main.rand.NextBool(50)) && NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -5f;
                }
            }
            else if (results.NearestNPCIndex != -1)
            {
                NPC target = Main.npc[results.NearestNPCIndex];
                NPC.aiStyle = -1;
                int direction = NPC.Center.X < target.Center.X ? 1 : -1;

                float acc = 0.1f;
                float speed = 1.5f;

                if (direction == 1 && NPC.velocity.X < speed)
                {
                    NPC.velocity.X += acc;
                }
                else if (direction == -1 && NPC.velocity.X > -speed)
                {
                    NPC.velocity.X -= acc;
                }
                NPC.direction = direction;

                Vector2 frontTilePos = NPC.Bottom + new Vector2(NPC.direction * NPC.width / 2 + 1, -4);
                Point tileCoords = frontTilePos.ToTileCoordinates();

                Tile tile = Framing.GetTileSafely(tileCoords.X, tileCoords.Y);
                Tile above = Framing.GetTileSafely(tileCoords.X, tileCoords.Y - 1);

                if ((tile.HasTile && Main.tileSolid[tile.TileType] || Main.rand.NextBool(50)) && NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -5f;
                }

                if (Main.rand.NextBool(120) && NPC.Distance(target.Center) > 300)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemBow with { Volume = 2f }, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.SafeDirectionTo(target.Center, Vector2.UnitY) * 20, ProjectileType<SealTokenProj>(), CalRemixHelper.ProjectileDamage(60, 100), 1f, ai1: NPC.whoAmI);
                    }
                }
            }
            else
            {
                NPC.aiStyle = NPCAIStyleID.Passive;
            }
            if (Main.rand.NextBool(1200))
            {
                SoundStyle top = NPC.type == ModContent.NPCType<TemporalAbomination>() ? TemporalAbomination.musiq : SealedSound;
                SoundEngine.PlaySound(top, NPC.Center);
            }
            NPC.spriteDirection = NPC.direction;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X != 0)
            {
                NPC.frameCounter += 0.125f;
                NPC.frameCounter %= Main.npcFrameCount[NPC.type];
                int frame = (int)NPC.frameCounter;
                NPC.frame.Y = frame * frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new Terraria.GameContent.Bestiary.IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool CanHitNPC(NPC target)
        {
            return infighting.Contains(target.type) && target.type != Type;
        }

        public override bool CanBeHitByNPC(NPC attacker)
        {
            return infighting.Contains(attacker.type) && attacker.type != Type;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (NPC.type != NPCType<EvilSealedPuppet>())
                modifiers.SourceDamage *= 3;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<SealToken>(), 1, 1, 3);
        }
    }

    public class SealedCitizen : SealedPuppet
    {
        public override string Texture => "CalRemix/Content/NPCs/Subworlds/Sealed/SealedPuppet";

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.damage = 30;
            SpawnModBiomes = [GetInstance<TurnipBiome>().Type];
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D hat = Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/Sealed/SealedCitizen").Value;
            spriteBatch.Draw(hat, NPC.Top - screenPos + Vector2.UnitY * NPC.gfxOffY, null, NPC.GetAlpha(drawColor), NPC.rotation, hat.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<SealToken>(), 1, 1, 2);
        }
    }
    public class EvilSealedPuppet : SealedPuppet
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.lifeMax = 1400;
            NPC.defense = 20;
            NPC.damage = 100;
            SpawnModBiomes = [GetInstance<BadlandsBiome>().Type];
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<SealToken>(), 1, 3, 5);
        }
    }
    public class TemporalAbomination : SealedPuppet
    {
        public static SoundStyle musiq = new SoundStyle("CalRemix/Assets/Music/Misc/Menu2") { MaxInstances = 22, PitchVariance = 1f, PauseBehavior = PauseBehavior.StopWhenGamePaused };
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.damage = 200;
            NPC.lifeMax = 20000;
            SpawnModBiomes = [GetInstance<TurnipBiome>().Type];
            Music = CalRemixMusic.Menu2;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemType<AbnormalSample>());
            npcLoot.Add(ItemType<AbnormalEye>());
            npcLoot.Add(ItemType<AbnormalRecord>(), 10);
            npcLoot.Add(ItemType<SealToken>(), 1, 30, 60);
        }
    }
}