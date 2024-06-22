using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Tiles.Abyss;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.NPCs.TownNPCs
{
    [AutoloadHead]
    public class SIIVA : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archdruid");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike);
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
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("Unlike her many allies during the war with Yharim, Silva was not trained by the ancient scholars, but rather by nature itself. Despite her differences, she was instrumental for the war as she had kept the armies fed by sprouting crops from seemingly thin air.")
            });
        }

        public override void AI()
        {
            Point p = NPC.Bottom.ToSafeTileCoordinates();
            Tile t = CalamityUtils.ParanoidTileRetrieval(p.X, p.Y);
            if (t.HasTile && (TileID.Sets.Grass[t.TileType] || TileID.Sets.Dirt[t.TileType] || TileID.Sets.Mud[t.TileType] || TileID.Sets.Stone[t.TileType]))
            {
                t.ResetToType((ushort)ModContent.TileType<PlantyMush>());
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => RemixDowned.downedPhytogen;

        public override List<string> SetNPCNameList() => new List<string>() { "SiIva" };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Nature speaks to those who listen. Can you hear its wisdom?");
            dialogue.Add("Every leaf, every branch has a story. The forest is my library.");
            dialogue.Add("With a touch, I can summon life from the soil. Respect the power of nature.");

            if (!Main.dayTime)
            {
                dialogue.Add("The jungle is alive even at night. Listen closely, and you'll feel its heartbeat.");
                dialogue.Add("In the darkness, the forest protects its own. Stay close, and you won't be lost.");
            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("Even nature celebrates in its own way. Let us enjoy this moment of peace and harmony.");

            if (Main.bloodMoon)
            {
                dialogue.Add("The Blood Moon's curse taints even the purest soil. We must withstand its corruption.");
                dialogue.Add("The crimson glow unnerves the wildlife. Stay vigilant; danger lurks in every shadow.");
            }

            if (Main.LocalPlayer.ZoneJungle)
            {
                dialogue.Add("The jungle's embrace is both fierce and gentle. Here, we find balance.");
                dialogue.Add("Amidst the foliage and fauna, I am at home. Let the jungle guide you.");
            }

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Get Blessing (1 gold)";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                if (Main.LocalPlayer.BuyItem(Item.buyPrice(gold: 1)))
                {
                    Main.LocalPlayer.AddBuff(BuffID.DryadsWard, CalamityUtils.SecondsToFrames(600));
                    SoundEngine.PlaySound(SoundID.CoinPickup);
                }
            }
        }

        public override bool CanGoToStatue(bool toKingStatue) => false;

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
            projType = ModContent.ProjectileType<LeafArrow>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }
    }
}
