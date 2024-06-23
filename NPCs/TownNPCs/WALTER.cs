using System;
using System.Collections.Generic;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Projectiles.Magic;
using CalRemix.World;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.NPCs.TownNPCs
{
    [AutoloadHead]
    public class WALTER : ModNPC
    {
        int currentShop = 0;
        public static int total = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archmagus");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<MushroomBiome>(AffectionLevel.Like)
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Dislike)
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
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
                new FlavorTextBestiaryInfoElement("A master of the shadows, this powerful magus dominated the battlefield from behind the scenes by conjuring up plagues and diseases to weaken the onslaught of enemies. She struggled against Yharim's mechanical forces, however, so she resorted to crafting antidotes and strategizing to contribute to the battle.")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => RemixDowned.downedPathogen;

        public override List<string> SetNPCNameList() => new List<string>() { "Mona" };

        public override string GetChat()
        {
            WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add("Shadow and substance, I bend them both to my will.");
            dialogue.Add("In the darkness, I find my strength. Beware what you cannot see.");
            dialogue.Add("Plagues and potions, curses and cures. All tools in my arsenal!");

            if (!Main.dayTime)
            {
                dialogue.Add("The night is my ally. Under its cover, my magic thrives.");
                dialogue.Add("In the veil of darkness, my true power is unleashed. Stay vigilant.");
            }

            if (BirthdayParty.PartyIsUp)
                dialogue.Add("A celebration? Perhaps I can slip in a few new recipes. Strictly for research, of course.");

            if (Main.bloodMoon)
            {
                dialogue.Add("A Blood Moon? Perfect for brewing something particularly nasty.");
                dialogue.Add("The Blood Moon’s curse is a potent ingredient. Use it wisely, or perish.");
            }

            if (Main.LocalPlayer.ZoneDesert)
            {
                dialogue.Add("The desert's harshness hides potent secrets. Perfect for my more volatile concoctions.");
            }

            if (Main.LocalPlayer.ZoneSnow)
            {
                dialogue.Add("The frigid cold preserves the most delicate of ingredients. I can work wonders here.");
            }

            if (Main.LocalPlayer.ZoneJungle)
            {
                dialogue.Add("The jungle's murky waters teem with life...and death. A perfect place for gathering toxins.");
            }

            if (Main.LocalPlayer.ZonePurity)
            {
                dialogue.Add("The forest is rich with ingredients. Nature provides the most potent toxins.");
            }

            if (Main.LocalPlayer.InModBiome<SunkenSeaBiome>() && !RemixDowned.downedHydrogen)
            {
                dialogue.Add("The Sunken Sea teems with vibrant life. Its waters are a treasure trove of rare and potent ingredients.");
            }

            if (Main.LocalPlayer.ZoneUndergroundDesert && RemixDowned.downedHydrogen)
            {
                dialogue.Add("The Sunken Sea's void is a haunting reminder of lost potential. Worthless.");
            }

            if (Main.LocalPlayer.ZoneUnderworldHeight)
            {
                dialogue.Add("Hell's fiery depths conceal the most powerful reagents. The flames themselves seem to whisper secrets.");
            }

            if (Main.LocalPlayer.ZoneRockLayerHeight)
            {
                dialogue.Add("The underground is a labyrinth of secrets. In its depths, I find the most elusive and powerful ingredients.");
            }

            if (Main.LocalPlayer.ZoneGlowshroom)
            {
                dialogue.Add("The mushroom biome is a wonder. Its bioluminescent fungi hold the key to some of my most potent brews.");
            }

            if (Main.LocalPlayer.InModBiome<BrimstoneCragsBiome>())
            {
                dialogue.Add("The Brimstone Crag, an abandoned city of flames and ash. It’s here I feel truly at home, surrounded by forgotten power.");
            }

            if (Main.LocalPlayer.InModBiome<SulphurousSeaBiome>())
            {
                dialogue.Add("The Sulphurous Sea’s volcanic beach is a cauldron of toxicity. Perfect for brewing the deadliest concoctions.");
            }

            if (Main.LocalPlayer.InModBiome<AstralInfectionBiome>())
            {
                dialogue.Add("The Astral Infection is a fascinating blend of biology and machinery. Its biomechanical essence provides unique components for my plagues.");
            }

            return dialogue;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop" + (currentShop == 0 ? "" : " " + (currentShop + 1));
            button2 = "Switch";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Mona" + currentShop;
            }
            else
            {
                if (currentShop < total)
                    currentShop++;
                else
                    currentShop = 0;
            }
        }

        public override void AddShops()
        {
            List<Item> potions = new List<Item>();
            int vodka = ModContent.ItemType<FabsolsVodka>();
            for (int i = 0; i < ContentSamples.ItemsByType.Count; i++)
            {
                Item item = ContentSamples.ItemsByType[i];
                if (item.type != ItemID.NebulaPickup1 && item.type != ItemID.NebulaPickup2 && item.type != ItemID.NebulaPickup3 && item.type != vodka && item.buffType > 0 && !BuffID.Sets.IsWellFed[item.buffType] && !Main.buffNoTimeDisplay[item.buffType] && !Main.vanityPet[item.buffType] && !Main.lightPet[item.buffType])
                {
                    potions.Add(item);
                }
            }
            int potsPerPage = 36;
            int totalShops = (int)Math.Ceiling((double)(potions.Count / potsPerPage));
            if (potions.Count % potsPerPage == 0)
            {
                potsPerPage = (int)((float)potions.Count / (float)potsPerPage);
            }
            total = totalShops; // im not risking it after Jharim
            int iter = 0;
            for (int i = 0; i <= totalShops; i++)
            {
                NPCShop npcShop = new NPCShop(Type, "Mona" + i);
                for (int j = iter; j < potsPerPage * (i + 1); j++)
                {
                    if (j < potions.Count)
                    {
                        if (potions[j] != null && !potions[j].IsAir)
                            npcShop.Add(potions[j]);
                        iter++;
                    }
                }
                npcShop.Register();
            }
        }

        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

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
            projType = ModContent.ProjectileType<MadAlchemistsCocktailBlue>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 2f;
        }
    }
}
