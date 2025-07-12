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
using CalamityMod.NPCs.Cryogen;
using CalamityMod.Particles;
using System.Collections.Generic;
using CalamityMod.Graphics.Primitives;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class WinterWitch : ModNPC
    {
        public ref float Timer => ref NPC.Calamity().newAI[0];

        public ref float State => ref NPC.Calamity().newAI[1];

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 140;
            NPC.width = 30;
            NPC.height = 60;
            NPC.defense = 20;
            NPC.lifeMax = 10000;
            NPC.value = 2000;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = Cryogen.HitSound;
            NPC.DeathSound = BetterSoundID.ItemBubblePop;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<CarnelianForestBiome>().Type };
            NPC.alpha = 50;
            NPC.rarity = 1;
            NPC.chaseable = false;
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, 0, 0.6f, 0.8f);
            NPC.TargetClosest(true);
            if (State == 0)
            {
                Timer++;
                if ((Timer % 150 == 0))
                {
                    NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.2f, 1.1f);
                }
                if (NPC.justHit && State == 0)
                {
                    State = 1;
                    Timer = 0;
                }
                NPC.spriteDirection = NPC.direction = -NPC.velocity.X.DirectionalSign();
            }
            else if (State == 1)
            {
                NPC.chaseable = true;
                Timer++;
                NPC.aiStyle = NPCAIStyleID.HoveringFighter;
                if (Timer % 160 == 0 && !NPC.AnyNPCs(ModContent.NPCType<EnchantedTome>()))
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemCast, NPC.Center);
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X - 40, (int)NPC.Center.Y, ModContent.NPCType<EnchantedTome>());
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X + 40, (int)NPC.Center.Y, ModContent.NPCType<EnchantedTome>());
                }

                if (Timer % 40 == 0)
                {
                    if (Timer != 0)
                    {
                        SoundEngine.PlaySound(BetterSoundID.ItemMagicIceBlock, NPC.Center);

                        int mod = (int)Timer % 360;
                        int projCount = mod / 40 + 2;
                        for (int i = 0; i < projCount; i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.Center.DirectionTo(Main.player[NPC.target].Center).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / MathHelper.Max(1, (float)(projCount - 1)))) * 14, ProjectileID.FrostBlastHostile, CalRemixHelper.ProjectileDamage(140, 280), 1);
                        }
                    }
                }
                // This was originally gonna be normal, but i forgot dendritus existed
                if (Main.zenithWorld)
                {
                    if (Timer == 1200)
                    {
                        int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Cryogen>());
                        NPC ne = Main.npc[n];
                        ne.boss = false;
                        ne.scale = 0.22f;
                        ne.lifeMax = ne.life = (int)(ne.lifeMax * 0.1f);
                        ne.width = ne.height = (int)(ne.width * ne.scale);
                    }
                }
                NPC.spriteDirection = -NPC.direction;
            }

            if (Main.rand.NextBool(3))
            {
                Vector2 pos = Main.rand.NextVector2FromRectangle(NPC.getRect());
                GeneralParticleHandler.SpawnParticle(new SnowflakeSparkle(pos, NPC.Center.DirectionTo(pos) * Main.rand.NextFloat(2, 4), Color.Cyan, Color.White, Main.rand.NextFloat(0.2f, 0.5f), 20, Main.rand.NextFloatDirection() * 0.1f));
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            //spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, SpriteEffects.None, 0);

            int pts = 60;
            List<Vector2> points = new();
            for (int i = 0; i < pts; i++)
            {
                Vector2 pos = Vector2.Lerp(NPC.Bottom, NPC.Bottom + Vector2.UnitY * 60, i / (float)pts);
                pos.X += MathHelper.Lerp(0, MathF.Sin(Main.GlobalTimeWrappedHourly * 2 + i * 0.1f + NPC.whoAmI) * 10, i / (float)pts); 
                points.Add(pos);
            }
            PrimitiveRenderer.RenderTrail(points, new PrimitiveSettings(new PrimitiveSettings.VertexWidthFunction((float f) => (1 - f) * 6), new PrimitiveSettings.VertexColorFunction((float f) => Color.Cyan * (1 - f))));
            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<ArcticRapier>());
            npcLoot.Add(ModContent.ItemType<SealedIce>(), 1, 32, 100);
        }
    }
}
