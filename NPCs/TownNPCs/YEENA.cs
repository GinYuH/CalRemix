using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;

namespace CalRemix.NPCs.TownNPCs
{
    [AutoloadHead]
    public class YEENA : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("General");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = -1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 25;
            NPC.lifeMax = 30000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            AnimationType = NPCID.Guide;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("An icy man with an icy demeanor, this retired soldier has won many battles. However, he is most known for inventing a popular preserved meat recipe, a fact which he resents.")
            });
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() 
            {
                "Cheng",
                "Daniel",
                "Daxia",
                "Libra",
                "Scorpio",
                "Cancer",
                "Sagittarius",
                "Aquarius",
                "Capricorn",
                "Taurus",
                "Leo",
                "Gemini"
            };
        }
        public override bool CanTownNPCSpawn(int numNPCs, int money)
        {
            return CalamityMod.DownedBossSystem.downedAstrumDeus;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 100;
            knockback = 4f;
        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<CalamityMod.Projectiles.Melee.Spears.AstralPikeProj>(); // placeholder because no halberd
            attackDelay = 1;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
        }
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return toKingStatue;
        }
        public override string GetChat()
        {
            List<string> chat = new List<string>
            {
                "From one war straight into the next. At least the Jungle Tyrant’s something that you can kill.",
                "It’d be awfully stereotypical for me to start quoting Sun Tzu, so don’t even ask.",
                "You’d make a fine mercenary, but I hesitate to say the same about becoming a soldier.",
                "I’ve seen countless battles. Could’ve seen even more if I still had my eye.",
                "The wind often howls during combat. That may be my fault.",
            };
            if(NPC.GivenName == "Daxia" || NPC.GivenName == "Daniel" || NPC.GivenName == "Cheng" || NPC.GivenName == "Libra")
            {
                chat.Add("There’s no glory in needless killing. You’d do well to keep that in mind.");
                chat.Add("We all have our regrets in life. The only thing that we can do is to keep marching on.");
            }
            if (NPC.GivenName == "Scorpio")
            {
                chat.Add("There’s no military genius greater than I am. I bet I could even take on the Tyrant’s forces myself!");
                chat.Add("I’m no braggart! Stop calling me that!");
            }
            if (NPC.GivenName == "Cancer")
            {
                chat.Add("Are you well? Not injured? Please stay in good health. This world is doomed without you.");
                chat.Add("You’ll conquer the heavens, and I intend to stand by you every step of the way.");
                chat.Add("An unfortunate name, as I’ve been told. I’ve heard all the jokes.");
            }
            if (NPC.GivenName == "Sagittarius")
            {
                chat.Add("This takes me back to years ago. I’d tell you stories, but we’d be here forever!");
                chat.Add("An old soldier is a good one. It means they still haven’t been killed.");
            }
            if (NPC.GivenName == "Aquarius")
            {
                chat.Add("Defy that vile Tyrant at every opportunity that presents itself. He’s earned grudges, not respect.");
                chat.Add("Bad orders are bad orders. Don’t let fear of authority pressure you into committing war crimes.");
            }
            if (NPC.GivenName == "Capricorn")
            {
                chat.Add("The Tyrant’s plans are much too haphazard to be any good. Knock some sense into him.");
                chat.Add("Formations have a purpose! Not that it matters to a walking army like you.");
            }
            if (NPC.GivenName == "Taurus")
            {
                chat.Add("The iron rule of any charge is to keep moving forwards, no matter what.");
                chat.Add("Bad orders are still orders. So grit your teeth and accept what must be done.");
            }
            if (NPC.GivenName == "Leo")
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
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp) chat.Add("I’ve often had to wine and dine with wealthy benefactors of war, so something more casual’s a refreshing change of pace.");
            if (NPC.AnyNPCs(NPCID.ArmsDealer)) chat.Add($"Peh. {Main.npc[NPC.FindFirstNPC(NPCID.ArmsDealer)].GivenName} leaves a bad taste in my mouth. He’s just an amateur in munitions, which makes him all the more dangerous.");
            if (NPC.AnyNPCs(NPCID.GoblinTinkerer)) chat.Add($"It’s always important to make sure that your weapons are well-maintained. I do wish that {Main.npc[NPC.FindFirstNPC(NPCID.GoblinTinkerer)].GivenName} would charge less, though...");
            if (NPC.AnyNPCs(NPCID.TaxCollector)) chat.Add($"Taxes and war go hand in hand. Both are pretty thankless but necessary for the common good. Send {Main.npc[NPC.FindFirstNPC(NPCID.TaxCollector)].GivenName} my regards.");
            if (NPC.AnyNPCs(NPCID.Pirate)) chat.Add($"Who the hell let {Main.npc[NPC.FindFirstNPC(NPCID.Pirate)].GivenName} have a cannon?! Trusting pirates with just about anything is a recipe for disaster!");
            if (NPC.AnyNPCs(ModContent.NPCType<CalamityMod.NPCs.TownNPCs.WITCH>())) chat.Add($"Nice to see that our luck’s turning around. That {Main.npc[NPC.FindFirstNPC(ModContent.NPCType<CalamityMod.NPCs.TownNPCs.WITCH>())].GivenName} reminds me an awful lot of a good friend of mine...");
            if (Main.IsItRaining) chat.Add("Rain is always terrible for morale. It’s hard to march, ruins the supplies, and you can’t tell bullets from the weather.");
            if (Main.LocalPlayer.ZoneDungeon) chat.Add("How revolting. Who’d construct a fortress this easy to invade? No wonder it’s been overrun by monsters.");
            if (Main.invasionProgress >= 0 && Main.invasionProgress <= 100) chat.Add("And they call this an army? How laughable. Put me in charge, and I’ll whip them into shape!");
            if (Terraria.GameContent.Events.DD2Event.Ongoing) chat.Add("Shouldn’t you be defending something right now? Get to it! We can’t let the enemy breach our defenses!");
            if (NPC.AnyNPCs(ModContent.NPCType<CalamityMod.NPCs.AstrumDeus.AstrumDeusHead>())) chat.Add("Splitting yourself in fragments just so that you can continue living… That hits a bit close to home. Not that you’d know.");
            if (Main.LocalPlayer.GetModPlayer<CalamityMod.CalPlayer.CalamityPlayer>().alcoholPoisonLevel > 0) chat.Add("Inebriation on the battlefield is a one-way ticket to the pit. For you, though, I can make an exception.");
            if (CalamityMod.DownedBossSystem.downedSignus) chat.Add("I’m no stranger to assassins. Few were as blatant as that one.");
            if (NPC.AnyNPCs(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Draedon>())
                && !NPC.AnyNPCs(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Apollo.Apollo>())
                && !NPC.AnyNPCs(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Ares.AresBody>())
                && !NPC.AnyNPCs(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Artemis.Artemis>())
                && !NPC.AnyNPCs(ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosHead>())) chat.Add("Shouldn’t that thing be retreating? Aren’t they afraid that we’d capture them for information?");
            if (NPC.AnyNPCs(NPCID.Cyborg)) chat.Add("Am I seeing things, or is there a weaponized drone on the loose?");
            if (!Main.dontStarveWorld) chat.Add("I was starting to plan logistics, but does anyone even need to eat?");
            if (Main.LocalPlayer.name == "Aries" || Main.LocalPlayer.name == "kkpro") return "I KILL YOU !  !!!";

            return Main.rand.Next(chat);
        }
        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.Potions.DeliciousMeat>(), Item.buyPrice(0, 0, 0, 20), ref shop, ref nextSlot);
            CalRemix.AddToShop(ModContent.ItemType<CalamityMod.Items.TreasureBags.CryogenBag>(), Item.buyPrice(0, 50), ref shop, ref nextSlot);
            // no other items exist yet, add them once they do
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
        }
    }
}
