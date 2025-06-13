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
using Microsoft.Xna.Framework;
using CalRemix.Content.NPCs.Bosses.Origen;
using Newtonsoft.Json.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using System;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis.Host.Mef;
using Terraria.DataStructures;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class BullShark : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 90;
            NPC.width = 116;
            NPC.height = 74;
            NPC.defense = 40;
            NPC.lifeMax = 20000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath4 with { Pitch = 0.6f };
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < Main.rand.Next(0, 5); i++)
            {
                int n = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Remora>(), 0, ai2: NPC.whoAmI + 1);
                Main.npc[n].rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }
        }

        public override void AI()
        {
            if (NPC.ai[1] == 0)
            {
                NPC.TargetClosest(false);
                if (Timer % 150 == 0 || NPC.collideX || NPC.collideY)
                {
                    NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1, 3);
                }
                Timer++;
                NPC.rotation = NPC.velocity.ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi);
                NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();
                if (Main.player[NPC.target].Distance(NPC.Center) < 600 && Timer > 120)
                {
                    NPC.ai[1] = 1;
                    Timer = 0;
                }
            }
            else if (NPC.ai[1] == 1)
            {
                Timer++;
                NPC.noTileCollide = true;
                if (Timer == 1)
                {
                    NPC.velocity = Main.player[NPC.target].DirectionTo(NPC.Center) * 4;
                }
                NPC.velocity *= 0.99f;
                if (Timer > 30)
                {
                    NPC.ai[1] = 2;
                    Timer = 0;
                }
                NPC.spriteDirection = Main.player[NPC.target].Center.X < NPC.Center.X ? -1 : 1;
                NPC.rotation = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() + (NPC.spriteDirection == 1 ? 0 : MathHelper.Pi);
            }
            else if (NPC.ai[1] == 2)
            {
                if (Timer == 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, NPC.Center);
                    NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * 20;
                }
                else
                {
                    NPC.rotation = NPC.velocity.ToRotation() + (NPC.spriteDirection == 1 ? 0 : MathHelper.Pi);
                }

                if (!Collision.SolidTiles(NPC.position, NPC.width, NPC.height))
                {
                    NPC.ai[3] = 1;
                }

                if ((NPC.ai[3] == 1 && Collision.SolidTiles(NPC.position, NPC.width, NPC.height)) || Timer > 60)
                {
                    if (Timer <= 60)
                    {
                        Main.LocalPlayer.Calamity().GeneralScreenShakePower = 20;
                        SoundEngine.PlaySound(BetterSoundID.ItemMeteorImpact with { Pitch = -0.4f }, NPC.Center);
                        NPC.ai[2] = 1;
                        NPC.velocity *= -1;
                        NPC.velocity = NPC.velocity.SafeNormalize(NPC.velocity) * 2;
                    }
                    NPC.ai[1] = 3;
                    Timer = 0;
                    NPC.ai[3] = 0;
                }
                NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();
                Timer++;
            }
            else if (NPC.ai[1] == 3)
            {
                NPC.velocity *= 0.96f;
                Timer++;
                if (Timer == 30)
                    SoundEngine.PlaySound(SoundID.Zombie97, NPC.Center);
                if (Timer > 120)
                {
                    NPC.ai[1] = 1;
                    NPC.ai[2] = 0;
                    Timer = 0;
                }
                NPC.spriteDirection = NPC.direction = -NPC.velocity.X.DirectionalSign();
                if (NPC.ai[2] == 0)
                {
                    NPC.spriteDirection = Main.player[NPC.target].Center.X < NPC.Center.X ? -1 : 1;
                    NPC.rotation = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation() + (NPC.spriteDirection == 1 ? 0 : MathHelper.Pi);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 5)
            {
                NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D head = ModContent.Request<Texture2D>(Texture + "Head").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            int frame = NPC.ai[1] > 0 ? 1 : 0;
            spriteBatch.Draw(head, NPC.Center - screenPos + (NPC.spriteDirection == -1 ? NPC.rotation - MathHelper.Pi : NPC.rotation).ToRotationVector2() * (NPC.spriteDirection == -1 ? 76 : 176), head.Frame(1, 2, 0, frame), NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.type == ModContent.NPCType<Remora>() && n.ai[2] == NPC.whoAmI + 1)
                {
                    Texture2D rem = TextureAssets.Npc[ModContent.NPCType<Remora>()].Value;
                    spriteBatch.Draw(rem, n.Center - screenPos, n.frame, n.GetAlpha(drawColor), n.rotation, new Vector2(rem.Width / 2, rem.Height / 4), n.scale, fx, 0);
                }
            }
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
    }
}
