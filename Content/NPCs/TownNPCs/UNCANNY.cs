using System.Collections.Generic;
using CalamityMod;
using CalRemix.Content.Items.Placeables;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class UNCANNY : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archwizard");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Nurse, AffectionLevel.Dislike);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 20000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.8f;
            AnimationType = NPCID.Guide;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Core.Biomes.AsbestosBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("This powerful wizard is feared across the land for his ability to manipulate the cells in the bodies of his enemies to destroy them from the inside out. Though his magics can be used for violence, they can be used for healing too which made him the leading medic against the war with Yharim.")
            });
        }

        public override void AI()
        {
            Vector2 firePos = NPC.Center + Vector2.UnitY * -22 + Vector2.UnitX * 22 * NPC.direction;
            Dust d = Dust.NewDustPerfect(firePos, DustID.Torch, Vector2.UnitY * -5, Scale: Main.rand.NextFloat(1.2f, 2.2f));
            d.noGravity = true;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => RemixDowned.downedCarcinogen;

        public override List<string> SetNPCNameList() => new List<string>() { "Carcinoma" };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Life and death, two sides of the same coin. Care for a wager?");
            dialogue.Add("Don't fear the cards you're dealt. Fear the hand that deals them.");
            dialogue.Add("Healing or harm, it’s all in the flick of a wrist. Choose wisely.");

            if (!Main.dayTime)
            {
                dialogue.Add("Night's my favorite time. Shadows hide the smokes and secrets.");
                dialogue.Add("The darkness? It’s comforting. Just like my cards and my smokes.");
            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("A party, you say? Let’s see how the odds play out tonight.");

            if (Main.bloodMoon)
            {
                dialogue.Add("Blood Moon or not, my magic never falters. Keep your guard up.");
                dialogue.Add("A crimson night calls for a heavier smoke. Danger’s in the air.");
            }

            if (Main.LocalPlayer.InModBiome<Core.Biomes.AsbestosBiome>())
            {
                dialogue.Add("Ah, the asbestos cave. It’s not for everyone, but it’s home to me.");
                dialogue.Add("Surrounded by asbestos, I feel at ease. The risk is part of the thrill.");
            }

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Carcinoma";
            }
        }

        public override void AddShops()
        {
            NPCShop npcShop = new NPCShop(Type, "Carcinoma");
            for (int i = 0; i < 32; i++)
            {
                int cointype = Main.rand.Next(4);
                int maxPrice = 2;
                switch (cointype)
                {
                    case 0:
                        maxPrice = Item.buyPrice(copper: 99);
                        break;
                    case 1:
                        maxPrice = Item.buyPrice(silver: 99);
                        break;
                    case 2:
                        maxPrice = Item.buyPrice(gold: 99);
                        break;
                    case 3:
                        maxPrice = Item.buyPrice(platinum: 20);
                        break;
                }
                npcShop.AddWithCustomValue(ModContent.ItemType<Asbestos>(), Main.rand.Next(1, maxPrice));
            }
            npcShop.Register();
        }

        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 9f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 50;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.SmokeBomb;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Microsoft.Xna.Framework.Color drawColor)
        {
            Vector2 npcOffset = NPC.Center - screenPos + Vector2.UnitY * 12 - new Vector2(0, NPC.gfxOffY);
            Texture2D balloons = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/Cigar").Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(balloons, npcOffset - Vector2.UnitY * 26 - Vector2.UnitX * NPC.spriteDirection * -10, null, NPC.GetAlpha(drawColor), 0f, balloons.Size() / 2, 1f, fx, 0);
            return true;
        }
    }
}
