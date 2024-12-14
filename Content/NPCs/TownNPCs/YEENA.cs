using CalamityMod;
using CalamityMod.Items.Potions;
using CalamityMod.Items.TreasureBags;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Content.Items.Placeables.MusicBoxes;
using CalRemix.Content.Items.Weapons;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class YEENA : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;
            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 2;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 3;
            NPCID.Sets.MagicAuraColor[Type] = Color.Yellow;
            NPCID.Sets.AttackTime[Type] = 12;
            NPCID.Sets.AttackAverageChance[Type] = 10;
            NPCID.Sets.HatOffsetY[Type] = 4;
            NPCID.Sets.ShimmerTownTransform[NPC.type] = false;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f, Direction = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPC.Happiness
            .SetBiomeAffection<SnowBiome>(AffectionLevel.Love) 
            .SetBiomeAffection<DesertBiome>(AffectionLevel.Like) 
            .SetBiomeAffection<OceanBiome>(AffectionLevel.Dislike)
            .SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Love)
            .SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate)
            .SetNPCAffection(ModContent.NPCType<WITCH>(), AffectionLevel.Like) 
            .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike) 
;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 200;
            NPC.defense = 25;
            NPC.lifeMax = 30000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.TaxCollector;
            
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement("A fabled general hailing from parts unknown. His subordinates can't quite agree about his personality.")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return System.DateTime.Now.Month == 12 || System.DateTime.Now.Month == 1 || System.DateTime.Now.Month == 2 || CalRemixWorld.deusDeadInSnow;
        }
        public override List<string> SetNPCNameList()
        {
            return new List<string>() { "Cheng", "Daniel", "Daxia", "Libra", "Scorpio", "Cancer", "Sagittarius", "Aquarius", "Capricorn", "Taurus", "Leo", "Gemini" };
        }
        public override string GetChat()
        {
            WeightedRandom<string> chat = new();
            chat.Add("From one war straight into the next. At least the Jungle Tyrant’s something that you can kill.");
            chat.Add("It’d be awfully stereotypical for me to start quoting Sun Tzu, so don’t even ask.");
            chat.Add("You’d make a fine mercenary, but I hesitate to say the same about becoming a soldier.");
            chat.Add("I’ve seen countless battles. Could’ve seen even more if I still had my eye.");
            chat.Add("The wind often howls during combat. That may be my fault.");
            if (NPC.GivenName == "Daniel" || NPC.GivenName == "Cheng" || NPC.GivenName == "Libra" || NPC.GivenName == "Daxia")
            {
                chat.Add("There’s no glory in needless killing. You’d do well to keep that in mind.");
                chat.Add("We all have our regrets in life. The only thing that we can do is to keep marching on.");
            }
            else if (NPC.GivenName == "Scorpio")
            {
                chat.Add("There’s no military genius greater than I am. I bet I could even take on the Tyrant’s forces myself!");
                chat.Add("I'm no braggart! Stop calling me that!");
            }
            else if (NPC.GivenName == "Cancer")
            {
                chat.Add("Are you well? Not injured? Please stay in good health. This world is doomed without you.");
                chat.Add("You’ll conquer the heavens, and I intend to stand by you every step of the way.");
                chat.Add("An unfortunate name, as I’ve been told. I’ve heard all the jokes.");
            }
            else if (NPC.GivenName == "Sagittarius")
            {
                chat.Add("This takes me back to years ago. I’d tell you stories, but we’d be here forever!");
                chat.Add("An old soldier is a good one. It means they still haven’t been killed.");
            }
            else if (NPC.GivenName == "Aquarius")
            {
                chat.Add("Defy that vile Tyrant at every opportunity that presents itself. He’s earned grudges, not respect.");
                chat.Add("Bad orders are bad orders. Don’t let fear of authority pressure you into committing war crimes.");
            }
            else if (NPC.GivenName == "Capricorn")
            {
                chat.Add("The Tyrant’s plans are much too haphazard to be any good. Knock some sense into him.");
                chat.Add("Formations have a purpose! Not that it matters to a walking army like you.");
            }
            else if (NPC.GivenName == "Taurus")
            {
                chat.Add("The iron rule of any charge is to keep moving forwards, no matter what.");
                chat.Add("Bad orders are still orders. So grit your teeth and accept what must be done.");
            }
            else if (NPC.GivenName == "Leo")
            {
                chat.Add("Don’t get too full of yourself. Not while I’m here, at least!");
                chat.Add("There’s still plenty for you to learn. The best lessons are taught on the battlefield.");
            }
            if (!Main.dayTime)
            {
                chat.Add("I’m quite familiar with nighttime ambushes. Most of them I planned myself.");
                chat.Add("There’s something quaintly romantic about the moon reflecting off of newly-polished weaponry.");
                chat.Add("Planning usually happens when the soldiers are asleep. They’ll never realize that there’s not usually a plan.");
                chat.Add("I’m no stranger to a seemingly endless slew of the undead. No stranger at all...");
            }
            if (BirthdayParty.PartyIsUp)
            {
                chat.Add("I’ve often had to wine and dine with wealthy benefactors of war, so something more casual’s a refreshing change of pace.");
            }
            if (NPC.AnyNPCs(NPCID.ArmsDealer))
            {
                int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
                chat.Add("Peh. " + Main.npc[armsDealer].GivenName + " leaves a bad taste in my mouth. He’s just an amateur in munitions, which makes him all the more dangerous.");
            }
            if (NPC.AnyNPCs(NPCID.GoblinTinkerer))
            {
                int goblinTinkerer = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
                chat.Add("It’s always important to make sure that your weapons are well-maintained. I do wish that " + Main.npc[goblinTinkerer].GivenName + " would charge less, though…");
            }
            if (NPC.AnyNPCs(NPCID.TaxCollector))
            {
                int taxCollector = NPC.FindFirstNPC(NPCID.TaxCollector);
                chat.Add("Taxes and war go hand in hand. Both are pretty thankless but necessary for the common good. Send " + Main.npc[taxCollector].GivenName + " my regards.");
            }
            if (NPC.AnyNPCs(NPCID.Pirate))
            {
                int pirate = NPC.FindFirstNPC(NPCID.Pirate);
                chat.Add("Who the hell let " +  Main.npc[pirate].GivenName + " have a cannon?! Trusting pirates with just about anything is a recipe for disaster!");
            }
            if (NPC.AnyNPCs(ModContent.NPCType<WITCH>()))
            {
                chat.Add("Nice to see that our luck’s turning around. That Calamitas reminds me an awful lot of a good friend of mine...");
            }
            if (Main.raining)
            {
                chat.Add("Rain is always terrible for morale. It’s hard to march, ruins the supplies, and you can’t tell bullets from the weather.");
            }
            if (Main.LocalPlayer.ZoneDungeon)
            {
                chat.Add("How revolting. Who’d construct a fortress this easy to invade? No wonder it’s been overrun by monsters.");
            }
            if (Main.invasionType != InvasionID.None)
            {
                chat.Add("And they call this an army? How laughable. Put me in charge, and I’ll whip them into shape!");
            }
            if (Main.invasionType == InvasionID.CachedOldOnesArmy)
            {
                chat.Add("Shouldn’t you be defending something right now? Get to it! We can’t let the enemy breach our defenses!");
            }
            if (NPC.AnyNPCs(ModContent.NPCType<AstrumDeusHead>()))
            {
                chat.Add("Splitting yourself in fragments just so that you can continue living... That hits a bit close to home. Not that you’d know.");
            }
            if (Main.LocalPlayer.Calamity().alcoholPoisonLevel > 0)
            {
                chat.Add("Inebriation on the battlefield is a one-way ticket to the pit. For you, though, I can make an exception.");
            }
            if (NPC.AnyNPCs(ModContent.NPCType<Signus>()))
            {
                chat.Add("I’m no stranger to assassins. Few were as blatant as that one.");
            }
            if (NPC.AnyNPCs(ModContent.NPCType<Draedon>()))
            {
                foreach (NPC n in Main.npc)
                {
                    if (!n.active || n == null)
                        continue;
                    if (n.type == ModContent.NPCType<Draedon>())
                    {
                        if (n.ModNPC<Draedon>().DefeatTimer > 0)
                        {
                            chat.Add("Shouldn’t that thing be retreating? Aren’t they afraid that we’d capture them for information?");
                            break;
                        }
                    }
                }
            }
            if (NPC.AnyNPCs(NPCID.Cyborg))
            {
                chat.Add("Am I seeing things, or is there a weaponized drone on the loose?");
            }
            if (!CalRemixWorld.guideHasExisted)
            {
                chat.Add("Surely you haven’t been just running amok and somehow thwarting the Tyrant without guidance? That’s both terrifying and impressive.");
            }
            if (Main.dontStarveWorld)
            {
                chat.Add("I was starting to plan logistics, but does anyone even need to eat?");
            }
            if (Main.LocalPlayer.name == "Aries" || Main.LocalPlayer.name == "kkpro")
            {
                chat.Add("I  KILL   YOU! ! !!");
            }
            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "Challenge";
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = "Yeena";
            }
            else
            {
                if (Main.LocalPlayer.name != "kkpro" && Main.LocalPlayer.name != "Aries")
                {
                    int e = Main.rand.Next(3);
                    switch (e)
                    {
                        case 0:
                            Main.npcChatText = "How spirited. But the time isn't yet ripe.";
                            break;
                        case 1:
                            Main.npcChatText = "Do you wish to follow in my footsteps? ...The world isn't ready yet.";
                            break;
                        default:
                            {
                                Main.npcChatText = "Do you wish to follow in my footsteps? ...The world isn't ready yet.";
                                if (NPC.GivenName == "Daxia" || NPC.GivenName == "Daniel" || NPC.GivenName == "Cheng" || NPC.GivenName == "Libra")
                                    Main.npcChatText = "Well, I'll be. But it's not yet the time for that.";

                                if (NPC.GivenName == "Scorpio")
                                    Main.npcChatText = "You can't seriously be challenging me! Maybe I'll let you later.";

                                if (NPC.GivenName == "Cancer")
                                    Main.npcChatText = "I can't bear the thought of hurting you. Give me some time to steel myself.";

                                if (NPC.GivenName == "Sagittarius")
                                    Main.npcChatText = "Picking on an old man, are we? Ha! Come back when you're a little older.";

                                if (NPC.GivenName == "Aquarius")
                                    Main.npcChatText = "No way.";

                                if (NPC.GivenName == "Capricorn")
                                    Main.npcChatText = "Let me come up with a battle plan first. Sit tight.";

                                if (NPC.GivenName == "Taurus")
                                    Main.npcChatText = "...Not yet.";

                                if (NPC.GivenName == "Leo")
                                    Main.npcChatText = "Very courageous of you! Hone your skills for a while longer and I'll consider it.";
                            }
                            break;
                    }
                }
                else
                {
                    int e = Main.rand.Next(3);
                    switch (e)
                    {
                        case 0:
                            Main.npcChatText = "NO KILL !!!YET";
                            break;
                        case 1:
                            Main.npcChatText = "BUKEYI!!";
                            break;
                        case 2:
                            Main.npcChatText = "YOU ARE A GNAT!";
                            break;
                    }
                }
            }
        }
        public override void AddShops()
        {
            NPCShop npcShop = new NPCShop(Type, "Yeena")
                .AddWithCustomValue<DeliciousMeat>(Item.buyPrice(copper: 20))
                .AddWithCustomValue<CryogenBag>(Item.buyPrice(gold: 50))
                .AddWithCustomValue<PinesPenetrator>(Item.buyPrice(gold: 80))
                .AddWithCustomValue<WinterBreeze>(Item.buyPrice(gold: 80))
                .AddWithCustomValue<Snowgrave>(Item.buyPrice(gold: 80))
                .AddWithCustomValue<ChristmasCarol>(Item.buyPrice(gold: 80))
                .AddWithCustomValue<WreathofBelial>(Item.buyPrice(gold: 80))
                .AddWithCustomValue<HalbardoftheHolidays>(Item.buyPrice(platinum: 50), new Condition("After The Devourer of Gods has been defeated", ()=> DownedBossSystem.downedDoG))
                .Add(new NPCShop.Entry(ModContent.ItemType<ProfanedDesertMusicBox>(), CalamityConditions.DownedProvidence));
            npcShop.Register();
        }
        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 200;
            knockback = 4f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 15;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.RubyBolt;
            attackDelay = 12;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 8f;
            randomOffset = 0f;
        }

        public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            Texture2D tex;
            Rectangle frame;
            Main.GetItemDrawFrame(ModContent.ItemType<HalbardoftheHolidays>(), out tex, out frame);
            item = tex;
            itemFrame = frame;
            itemSize = tex.Width;
            scale = 1f;
            offset = Vector2.Zero;
        }
    }
}
