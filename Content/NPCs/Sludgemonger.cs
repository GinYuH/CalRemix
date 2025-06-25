using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs;
using CalRemix.Content.Tiles;
using CalamityMod.Tiles;
using CalamityMod.InverseKinematics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.Collections.Generic;
using CalamityMod.Prefixes;
using CalamityMod.Items.Fishing.AstralCatches;
using CalRemix.Content.Projectiles.Hostile;
using Terraria.ModLoader.Utilities;
using CalRemix.Content.Items.Ammo;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.NPCs
{
    public class SludgeWalker : ModNPC
    {
        public List<LimbCollection> allLimbs = new List<LimbCollection>();
        public List<Vector2> legDests = new List<Vector2>();
        public override string Texture => "CalRemix/Assets/ExtraTextures/SludgeCannon";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sludge Walker");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 10;
            NPC.width = 36;
            NPC.height = 180;
            NPC.defense = 5;
            NPC.lifeMax = 150;
            NPC.knockBackResist = 0.4f;
            NPC.value = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = BetterSoundID.ItemBubbleGun3;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            NPC.ai[0]++;
            Point loc = NPC.Center.ToTileCoordinates();
            int ct = 2;
            if (Main.mouseRight || allLimbs.Count <= 0)
            {
                allLimbs.Clear();
                legDests.Clear();
                for (int i = 0; i < ct; i++)
                {
                    allLimbs.Add(new(new CyclicCoordinateDescentUpdateRule(0.07f, MathHelper.PiOver2), 100f, 100f));
                    if (i % 2 == 0)
                    {
                        allLimbs[i][0].Rotation += MathHelper.PiOver2 * 3;
                    }
                    legDests.Add(Vector2.Zero);
                }
            }
            // Update limbs.
            Vector2 connectPosition = NPC.position;
            for (int i = 0; i < ct; i++)
            {
                int moveThreshold = i % 2 == 0 ? 7 : (NPC.ai[0] > 22 ? 7 : 0);
                int whenToMove = 100;
                bool movingRight = NPC.velocity.X > 0;
                if (legDests[i] == default || legDests[i].Distance(NPC.Center) > (16 * 22) || (movingRight ? (NPC.position.X - whenToMove > legDests[i].X) : (NPC.position.X + whenToMove < legDests[i].X)))
                {
                    // bool neg = i % 2 == 0;
                    bool neg = NPC.velocity.X < 0;
                    int legNumber = i / 2;
                    if (neg)
                    {
                        bool sb = false;
                        for (int x = loc.X - moveThreshold; x < loc.X + 200; x++)
                        {
                            if (sb)
                                break;
                            for (int y = loc.Y; y < loc.Y + 100; y++)
                            {
                                Tile t = CalamityUtils.ParanoidTileRetrieval(x, y);
                                if (t.IsTileSolid())
                                {
                                    legDests[i] = new Vector2(x, y) * 16;
                                    sb = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        bool sb = false;
                        for (int x = loc.X + moveThreshold; x > loc.X - 200; x--)
                        {
                            if (sb)
                                break;
                            for (int y = loc.Y; y < loc.Y + 100; y++)
                            {
                                Tile t = CalamityUtils.ParanoidTileRetrieval(x, y);
                                if (t.IsTileSolid())
                                {
                                    legDests[i] = new Vector2(x, y) * 16;
                                    sb = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < ct; i++)
            {
                //NEG IS RED
                allLimbs[i].Limbs[0].Rotation = MathHelper.Clamp((float)allLimbs[i].Limbs[0].Rotation, -MathHelper.PiOver2, MathHelper.PiOver2);
                allLimbs[i].Update(connectPosition, legDests[i]);
            }

            NPC.netSpam = 0;
            NPC.netUpdate = true;

            float speed = 5f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Player target = Main.player[NPC.target];
            bool valid = target != null && target.active;
            NPC.direction = target.position.X - NPC.position.X > 0 ? 1 : -1;
            if (!valid)
            {
                NPC.velocity.X *= 0.9f;
                if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                {
                    NPC.velocity.X = 0f;
                }
            }
            else
            {
                if (NPC.direction > 0)
                {
                    NPC.velocity.X = (NPC.velocity.X * 20f + speed) / 21f;
                }
                if (NPC.direction < 0)
                {
                    NPC.velocity.X = (NPC.velocity.X * 20f - speed) / 21f;
                }
            }
            int width = 80;
            int height = 20;
            Vector2 collisionTile = new Vector2(NPC.Center.X - (float)(width / 2), NPC.position.Y + (float)NPC.height - (float)height);
            bool fall = false;
            if (valid && NPC.position.X < target.position.X && NPC.position.X + (float)NPC.width > target.position.X + (float)target.width && NPC.position.Y + (float)NPC.height < target.position.Y + (float)target.height - 16f)
            {
                fall = true;
            }
            if (fall)
            {
                NPC.velocity.Y += 0.5f;
            }
            else if (Collision.SolidCollision(collisionTile, width, height))
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if ((double)NPC.velocity.Y > -0.2)
                {
                    NPC.velocity.Y -= 0.025f;
                }
                else
                {
                    NPC.velocity.Y -= 0.2f;
                }
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
            }
            else
            {
                if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if ((double)NPC.velocity.Y < 0.1)
                {
                    NPC.velocity.Y += 0.025f;
                }
                else
                {
                    NPC.velocity.Y += 0.5f;
                }
            }
            if (NPC.velocity.Y > 10f)
            {
                NPC.velocity.Y = 10f;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
            new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            if (spawnInfo.Player.ZoneCorrupt)
                return SpawnCondition.Corruption.Chance * 0.04f;
            else if(spawnInfo.Player.ZoneCrimson)
                return SpawnCondition.Crimson.Chance * 0.04f;
            return 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int k = 0; k < allLimbs.Count; k++)
            {
                LimbCollection l = allLimbs[k];
                for (int i = 0; i < l.Limbs.Length; i++)
                {
                    Texture2D tex = TextureAssets.Npc[Type].Value;
                    Limb lim = l[i];
                    Vector2 scale = new Vector2(NPC.scale);
                    float dist = lim.ConnectPoint.Distance(lim.EndPoint);
                    scale = new Vector2(dist / tex.Width, 1);
                    bool neg = k % 2 == 0;
                    spriteBatch.Draw(tex, lim.ConnectPoint - screenPos, null, neg ? Color.Red : Color.Blue, (float)lim.Rotation, new Vector2(0, tex.Height / 2), scale, 0, 0);
                }
            }
            Texture2D sprime = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(sprime, new Vector2(NPC.Center.X - NPC.width, NPC.position.Y) - screenPos, null, drawColor, 0, new Vector2(0, 0), NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Ice, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Ice, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<BlightedGel>(), 1, 10, 26);
        }
    }
}
