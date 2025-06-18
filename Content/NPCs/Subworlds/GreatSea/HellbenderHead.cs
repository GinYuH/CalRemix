using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Core.Biomes;
using CalamityMod;
using CalamityMod.Items.Potions;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.NPCs.SupremeCalamitas;
using System.Collections.Generic;
using CalamityMod.Graphics.Primitives;

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class HellbenderHead : ModNPC
    {
        public ref float SegmentType => ref NPC.Calamity().newAI[3];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 15;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.width = 254; 
            NPC.height = 254; 
            NPC.defense = 200;
            NPC.lifeMax = 10000000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 40, 0);
            NPC.HitSound = SupremeCalamitas.HurtSound;
            NPC.DeathSound = SupremeCalamitas.SepulcherSummonSound with { Pitch = 1 };
            SpawnModBiomes = new int[1] { ModContent.GetInstance<GrandSeaBiome>().Type };
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
            NPC.TargetClosest(false);
            if (Main.player[NPC.target].Distance(NPC.Center) < 500)
            {
                NPC.Calamity().newAI[0]++;
            }
            if (NPC.localAI[2] == 0)
            {
                NPC.localAI[2] = NPC.Center.X;
                NPC.localAI[3] = NPC.Center.Y;
            }
            if (SegmentType == 0)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[3] = NPC.whoAmI;
                    NPC.realLife = NPC.whoAmI;
                    int num4 = 0;
                    int num5 = NPC.whoAmI;
                    int ct = 2;
                    for (int m = 0; m < ct; m++)
                    {
                        num4 = CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), Type, NPC.whoAmI, npcTasks: (NPC n) =>
                        {
                            n.ai[3] = NPC.whoAmI;
                            n.realLife = NPC.whoAmI;
                            n.ai[1] = num5;
                            n.npcSlots = 0;
                            n.dontCountMe = true;
                            Main.npc[num5].ai[0] = n.whoAmI;
                            n.width = 20;
                            n.width = 20;
                            n.Calamity().newAI[3] = 1;
                            n.Calamity().newAI[2] = NPC.Calamity().newAI[2];
                            if (m == 0)
                            {
                                n.width = n.height = 230;
                                n.Calamity().newAI[3] = 1;
                            }
                            if (m == ct - 1)
                            {
                                n.width = n.height = 150;
                                n.Calamity().newAI[3] = 2;
                            }
                        }).whoAmI;
                        num5 = num4;
                    }
                }
                CalRemixNPC.WormAI(NPC, 10, 0.05f, null, new Vector2(NPC.localAI[2], NPC.localAI[3]), segmentType: SegmentType == 0 ? 0 : 1, canFlyByDefault: true);
            }
            else
            {
                CalRemixNPC.WormAI(NPC, 32, 0.7f, null, new Vector2(NPC.localAI[2], NPC.localAI[3]), segmentType: 1, canFlyByDefault: true);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            Texture2D texture = SegmentType switch
            {
                1 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderBody").Value,
                2 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderTail").Value,
                _ => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderHead").Value
            };

            if (SegmentType == 2 && !NPC.IsABestiaryIconDummy)
            {

                spriteBatch.ExitShaderRegion();

                float correctedRotation = NPC.rotation - MathHelper.PiOver2;

                Vector2 ribbonOffset = -Vector2.UnitY.RotatedBy(correctedRotation);
                //ribbonOffset += Vector2.UnitX.RotatedBy(correctedRotation) * 90f;

                float currentSegmentRotation = correctedRotation;
                List<Vector2> ribbonDrawPositions = new List<Vector2>();
                int ct = 50;
                for (int i = 0; i < ct; i++)
                {
                    float ribbonCompletionRatio = i / 12f;
                    float wrappedAngularOffset = MathHelper.WrapAngle(NPC.oldRot[i + 1] - currentSegmentRotation - MathHelper.PiOver2) * 0.3f;

                    Vector2 ribbonSegmentOffset = Vector2.UnitY.RotatedBy(currentSegmentRotation) * ribbonCompletionRatio * 470f;
                    ribbonDrawPositions.Add(NPC.Center + ribbonSegmentOffset + ribbonOffset);

                    currentSegmentRotation += wrappedAngularOffset;


                    if (i % 5  == 0 && i > 2)
                    {
                        int texType = i % 3;
                        bool right = i % 2 == 0;
                        Texture2D ornament = texType switch
                        {
                            1 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderTailOrnament1").Value,
                            2 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderTailOrnament2").Value,
                            _ => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderTailOrnament3").Value
                        };
                        Vector2 ornamentPosition = NPC.Center + (ribbonSegmentOffset + ribbonOffset) + (Vector2.UnitX.RotatedBy((i % 2 == 0).ToDirectionInt() * MathHelper.PiOver2) * MathHelper.Lerp(90, 50, i / (float)(ct - 1))).RotatedBy(NPC.rotation);

                        float dirOff = right ? MathHelper.Pi : 0;
                        Main.EntitySpriteDraw(ornament, ornamentPosition - screenPos, null, Lighting.GetColor(ornamentPosition.ToTileCoordinates()), (right.ToDirectionInt() * -MathHelper.PiOver4) + NPC.rotation + right.ToDirectionInt() * 0.2f * MathF.Sin(Main.GlobalTimeWrappedHourly) - dirOff, !right ? new Vector2(0, ornament.Height) : new Vector2(ornament.Width, ornament.Height), NPC.scale, !right ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                    }
                }
                PrimitiveRenderer.RenderTrail(ribbonDrawPositions, new((float f) => 50 * (1 - f), (float f) => Color.Lerp(Color.PaleGoldenrod, Color.Tan * 0.1f, f)));

                Texture2D ornamental = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderTailOrnamentFinal").Value;
                Main.EntitySpriteDraw(ornamental, ribbonDrawPositions[^1] - screenPos, null, Lighting.GetColor(ribbonDrawPositions[^1].ToTileCoordinates()), NPC.rotation - MathHelper.PiOver2, new Vector2(ornamental.Width / 2, 0), NPC.scale, SpriteEffects.None);

            }

            Rectangle? frame = SegmentType == 2 ? texture.Frame(1, 4, 0, NPC.frame.Y) : null;
            Vector2 origin = SegmentType == 2 ? new Vector2(texture.Width / 2, texture.Height / 8) : texture.Size() / 2;
            Vector2 bodyOffset = SegmentType != 2 ? Vector2.Zero : NPC.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver2) + NPC.rotation.ToRotationVector2() * -2;
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos + bodyOffset * 12, null, NPC.GetAlpha(drawColor), NPC.rotation - MathHelper.PiOver2, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);


            if (SegmentType == 1)
            {
                int ct = 13;
                for (int i = 0; i < ct; i++)
                {
                    int texType = i % 3;
                    bool right = i < (ct * 0.5f);
                    Texture2D ornament = texType switch
                    {
                        1 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderOrnament1").Value,
                        2 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderOrnament2").Value,
                        _ => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/HellbenderOrnament3").Value
                    };
                    float rotOff = (right ? MathHelper.PiOver2 : MathHelper.PiOver4) + right.ToDirectionInt() * MathF.Sin(Main.GlobalTimeWrappedHourly) * MathHelper.Lerp(0.75f, 0.05f, Utils.Turn01ToCyclic010(Utils.GetLerpValue(0, ct, i))); ;
                    Vector2 ornamentBaseOffset = Vector2.UnitY.RotatedBy(NPC.rotation - MathHelper.PiOver2) * MathHelper.Lerp(180, 120, Utils.Turn01ToCyclic010(Utils.GetLerpValue(0, ct, i)));
                    Vector2 ornamentPosition = NPC.Center + Vector2.Lerp(ornamentBaseOffset.RotatedBy(-MathHelper.PiOver4 * 0.5f), ornamentBaseOffset.RotatedBy(MathHelper.PiOver4 * 0.5f), i / (float)(ct - 2));
                    Main.EntitySpriteDraw(ornament, ornamentPosition - screenPos, null, Lighting.GetColor(ornamentPosition.ToTileCoordinates()), ornamentPosition.DirectionTo(NPC.Center).ToRotation() + rotOff, right ? Vector2.Zero : new Vector2(ornament.Width, 0), NPC.scale, !right ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                }
            }
            return false;
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
        public override bool CheckActive() => NPC.Calamity().newAI[0] > 300 && SegmentType == 0;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => SegmentType == 0;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

        }
    }
}
