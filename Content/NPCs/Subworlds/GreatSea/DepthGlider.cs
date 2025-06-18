using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Audio;
using System;
using CalamityMod.Items.Accessories;
using CalRemix.Core.Biomes;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.DesertScourge;
using CalRemix.Content.Buffs;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class DepthGlider : ModNPC
    {
        public ref float Timer => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 120;
            NPC.width = 70;
            NPC.height = 40;
            NPC.defense = 80;
            NPC.lifeMax = 50000;
            NPC.value = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.HitSound = Leviathan.RoarChargeSound;
            NPC.DeathSound = DesertScourgeHead.RoarSound with { Pitch = -1 };
            NPC.GravityIgnoresLiquid = true;
            NPC.waterMovementSpeed = 1f;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            foreach (Player p in Main.ActivePlayers)
            {
                if (p.getRect().Intersects(NPC.getRect()))
                {
                    Main.BestiaryTracker.Kills.RegisterKill(NPC);
                }
            }
            if (NPC.ai[1] == 0)
            {
                NPC.TargetClosest(false);
                if (Timer % 100 == 0 || NPC.collideX || NPC.collideY)
                {
                    if (NPC.velocity.Length() < 1)
                    {
                        NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3, 5f);
                    }
                    else
                    {
                        NPC.velocity = NPC.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.5f);
                    }
                }
                Timer++;
                NPC.spriteDirection = NPC.direction = NPC.velocity.X.DirectionalSign();
                NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.velocity.ToRotation() - (NPC.direction == 1 ? 0 : MathHelper.Pi), 0.1f);

                Rectangle maus = Utils.CenteredRectangle(Main.MouseWorld, new Vector2(10));
                if (Main.player[NPC.target].getRect().Intersects(NPC.getRect()) && Timer > 120)
                {
                    if (Main.player[NPC.target].controlMount)
                    {
                        NPC.active = false;
                        Main.player[NPC.target].AddBuff(ModContent.BuffType<DepthGliderBuff>(), 60);
                        for (int i = 0; i < 60; i ++)
                        {
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Clentaminator_Cyan);
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(tex.Width / 2, tex.Height / Main.npcFrameCount[Type] / 2), NPC.scale, fx, 0);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}
