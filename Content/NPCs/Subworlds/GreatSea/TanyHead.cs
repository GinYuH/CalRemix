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

namespace CalRemix.Content.NPCs.Subworlds.GreatSea
{
    public class TanyHead : ModNPC
    {
        public ref float SegmentType => ref NPC.Calamity().newAI[3];

        public bool Purple => NPC.Calamity().newAI[2] == 1;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = -1;
            NPC.damage = 110;
            NPC.width = 40; //324
            NPC.height = 40; //216
            NPC.defense = 20;
            NPC.lifeMax = 20000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 40, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = CalamityMod.NPCs.Ravager.RavagerBody.LimbLossSound with { Pitch = 0.2f };
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PrimordialCavesBiome>().Type };
        }

        public override void AI()
        {
            NPC.TargetClosest();
            if (SegmentType == 0)
            {
                if (NPC.Calamity().newAI[2] == 0)
                {
                    if (Main.rand.NextBool(30))
                        NPC.Calamity().newAI[2] = 1;
                    else
                        NPC.Calamity().newAI[2] = -1;
                }
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[3] = NPC.whoAmI;
                    NPC.realLife = NPC.whoAmI;
                    int num4 = 0;
                    int num5 = NPC.whoAmI;
                    int ct = 30;
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
                            if (m == 16)
                            {
                                n.width = n.height = 80;
                                n.Calamity().newAI[3] = 2;
                            }
                            if (m == ct - 1)
                            {
                                n.Calamity().newAI[3] = 3;
                            }
                            if (m == 5)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    NPC.NewNPC(n.GetSource_FromThis(), (int)n.Center.X, (int)n.Center.Y, ModContent.NPCType<TanyParasite>(), ai0: n.whoAmI + 1);
                                }
                            }
                        }).whoAmI;
                        num5 = num4;
                    }
                }
                CalRemixNPC.WormAI(NPC, 10, 0.22f, Main.player[NPC.target], Vector2.Zero, segmentType: SegmentType == 0 ? 0 : 1, canFlyByDefault: true);
            }
            else
            {
                CalRemixNPC.WormAI(NPC, 32, 0.7f, Main.player[NPC.target], Vector2.Zero, segmentType: 1, canFlyByDefault: true);
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

            string purpleSuffix = Purple ? "_Purple" : "";
            Texture2D texture = SegmentType switch
            {
                1 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/TanySegment" + purpleSuffix).Value,
                2 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/TanyBody" + purpleSuffix).Value,
                3 => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/TanyTail" + purpleSuffix).Value,
                _ => ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/GreatSea/TanyHead" + purpleSuffix).Value
            };

            Rectangle? frame = SegmentType == 2 ? texture.Frame(1, 4, 0, NPC.frame.Y) : null;
            Vector2 origin = SegmentType == 2 ? new Vector2(texture.Width / 2, texture.Height / 8) : texture.Size() / 2;
            Vector2 bodyOffset = SegmentType != 2 ? Vector2.Zero : NPC.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver2) + NPC.rotation.ToRotationVector2() * -2;
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos + bodyOffset * 12, frame, NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.Pi, origin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
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
        public override void FindFrame(int frameHeight)
        {
            if (SegmentType == 2)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 6)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y++;
                }
                if (NPC.frame.Y >= 4)
                {
                    NPC.frame.Y = 0;
                }
            }
        }
        public override bool CheckActive() => SegmentType == 0;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => SegmentType == 0;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(DropHelper.If(info => info.npc.type == ModContent.NPCType<TanyHead>() && info.npc.ModNPC<TanyHead>().Purple && info.npc.ModNPC<TanyHead>().SegmentType == 0), ItemID.GrapeJuice));
        }
    }
}
