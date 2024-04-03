using CalamityMod;
using CalamityMod.Items.LoreItems;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Magic;
using CalRemix.Items;
using CalRemix.Items.Weapons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.NPCs.TownNPCs
{
    [AutoloadHead]
	public class ZER0 : ModNPC
	{
		private static List<int> Fish = new()
		{
			ItemID.Bass,
			ItemID.Salmon,
			ItemID.Penguin,
			ItemID.AtlanticCod,
			ItemID.Trout,
			ItemID.Tuna,
			ItemID.RedSnapper,
			ItemID.BlueJellyfish,
            ItemID.GreenJellyfish,
            ItemID.PinkJellyfish
        };
		public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = 21;
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 2;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
            NPCID.Sets.MagicAuraColor[Type] = Color.Yellow;
            NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 10;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false;
			NPCID.Sets.ShimmerTownTransform[Type] = false;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f, Direction = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() 
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 22;
			NPC.defense = 90;
			NPC.lifeMax = 2000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = NPCID.Dryad;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) 
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("There is a rumor that this bird is really a dragon, but rumors are just rumors.")
			});
		}
		public override void AI()
		{
			if (NPC.ai[0] == 14f && NPC.localAI[3] == 30 && Main.netMode != NetmodeID.MultiplayerClient)
				SoundEngine.PlaySound(Yharon.RoarSound with { Pitch = 1.0f }, NPC.Center);
        }
		public override bool CanTownNPCSpawn(int numTownNPCs) 
		{
			for (int k = 0; k < Main.maxPlayers; k++) 
			{
				Player player = Main.player[k];
				if (!player.active) 
				{
					continue;
				}
				if (NPC.downedMoonlord && player.inventory.Any(item => item.type == ModContent.ItemType<Ogscule>()))
					return true;
			}
			return false;
		}
		public override List<string> SetNPCNameList() 
		{
			return new List<string>() { "Zeratros" };
		}
		public override string GetChat() 
		{
			Player player = Main.player[Main.myPlayer];
            if (player.HasItem(ModContent.ItemType<ProfanedVocalizer>()))
            {
                WeightedRandom<string> chat = new();
				if (DownedBossSystem.downedProvidence)
                {
                    if (player.HasItems(Fish))
                    {
                        chat.Add("May I have some of your food? I'm not hungry but I may save it for later.", 2);
                        chat.Add("That food you have smells delicious. It's alright if you're not sharing with me, I understand.", 2);
                    }
                    chat.Add("Oh hello. My feathers are nice and clean, and you look well kept too.");
                    chat.Add("It is an honor to stand before a legend such as yourself. My days are in the past, but this is your time to paint our world's future.");
                    chat.Add("Watch your back, you may never know when someone will attack you. I'll help defend you if anything comes your way");
                    chat.Add("Nice to see you, " + player.name + ". You could take a rest with me if you want. You deserve it.");
                    chat.Add("This world used to be filled with such much war. The Tyrant's Reign was not a good time. I have hope that we can keep this peace.");
                    chat.Add("My sister was always very rash and overconfident. I guess her weakness was the flames of her ego.");
                    chat.Add("I do miss my sister. I wish she chose a better path, one not filled with such misplaced zeal.");
                    if (DownedBossSystem.startedBossRushAtLeastOnce)
                    {
                        chat.Add("I sensed the presence of my father when you used that artifact. You best be careful, stronger foes may come after you.");
                        chat.Add("The air now feels cleaner, yet also heavier. A strange but welcome change.");
                    }
                    if (Main.dayTime)
                    {
                        chat.Add("The sun is shining bright. Reminds me of myself in my younger days.");
                        chat.Add("What a lovely day, isn't it " + player.name + "?");
                        chat.Add("This is a good time for a fish picnic. I may be small, but I have a large appetite.");
                    }
					else
                    {
                        chat.Add("The moon is lively tonight. Reminds me of my sister whenever she was calm.");
                        chat.Add("What a lovely night, isn't it " + player.name + "?");
                        chat.Add("This is a good time to sleep. I don't have to though, but it's always an option.");
                    }
                    WeightedRandom<string> happy = new();
                    happy.Add("Why do you want to know how I am? I'm fine.");
                    happy.Add("My situation is mediocre, but it can't get any better or worse.");
                    happy.Add("I am content. That is all you need to know.");
                    happy.Add("This place is alright. Nothing else to it.");
                    happy.Add("I have no interest in letting you know how I truly feel.");
                    player.currentShoppingSettings.HappinessReport = happy;
                }
				else
                {
                    if (player.HasItems(Fish))
                    {
                        chat.Add("What do you got there, human? Food?! Holding something delicious will not change my view of you!", 2);
                        chat.Add("Seafood! Give it to me! This is an order, not a plea!", 2);
                    }
                    chat.Add("I can't even breathe fire anymore! This is absurd!");
                    chat.Add("Human! You stand before me! The great " + NPC.GivenName + "!");
                    chat.Add("Watch your back human, you may never know when I decide to attack you while you're asleep");
                    chat.Add("You're lucky I'm in this form, or else I'll burn out your eyes by staring at you!");
                    chat.Add("These creatures think they can best me, the great " + NPC.GivenName + "?");
                    WeightedRandom<string> happy = new();
                    happy.Add("Why do you want to know how I am? I'm the best! Remember that!");
                    happy.Add("My situation is none of your business, human!");
                    happy.Add("All you need to know, human, is that my happiness is something you can NEVER understand!");
                    happy.Add("This place is not worthy of my presence!");
                    happy.Add("I don't have to share anything. Go away.");
                    player.currentShoppingSettings.HappinessReport = happy;
                }
                if (DownedBossSystem.downedBoomerDuke && !DownedBossSystem.downedProvidence)
                {
                    chat.Add("I'm SO glad that abomination of a creature was killed. You have no idea what it's like being a dragon and living with one of \"those\" claiming to be just like me!");
                }
                return chat;
            }
			else
            {
                string squawk = "SQUAWK";
                for (int i = 0; i < Main.rand.Next(50); i++)
                {
                    squawk += " SQUAWK";
                }
                WeightedRandom<string> chat = new();
                chat.Add(squawk);
                player.currentShoppingSettings.HappinessReport = squawk;
                return chat;
            }
		}

		public override void SetChatButtons(ref string button, ref string button2) 
		{
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = "Pet";
		}
		public override void OnChatButtonClicked(bool firstButton, ref string shop) 
		{
			Player player = Main.LocalPlayer;
			bool pet = player.Distance(NPC.Center) < 56 && player.velocity.Y == 0 && player.Bottom.Y <= NPC.Bottom.Y + 24 && player.Bottom.Y >= NPC.Bottom.Y - 24 && !player.CCed;
            if (firstButton)
                shop = "Zeratros";
			else if (!firstButton && pet)
				Main.LocalPlayer.PetAnimal(NPC.whoAmI);
		}
		public override void AddShops() 
		{
			NPCShop npcShop = new NPCShop(Type, "Zeratros")
				.AddWithCustomValue<LorePrelude>(Item.buyPrice(gold: 20))
				.AddWithCustomValue<LoreRequiem>(Item.buyPrice(gold: 50));
            npcShop.Register();
		}
		public override bool CanGoToStatue(bool toKingStatue) => true;
		public override void TownNPCAttackStrength(ref int damage, ref float knockback) 
		{
			damage = 22;
			knockback = 4f;
		}
		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) 
		{
			cooldown = 30;
			randExtraCooldown = 15;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) 
		{
			projType = ModContent.ProjectileType<EidolicWailSoundwave>();
			attackDelay = 30;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) 
		{
			multiplier = 8f;
			randomOffset = 0f;
		}
	}
}