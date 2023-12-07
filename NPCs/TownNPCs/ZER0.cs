using CalamityMod;
using CalamityMod.Items.LoreItems;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Magic;
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
			string squawk = "SQUAWK";
			for (int i = 0; i < Main.rand.Next(50); i++)
			{
				squawk += " SQUAWK";
			}
            WeightedRandom<string> chat = new();
			chat.Add(squawk);
            Main.player[Main.myPlayer].currentShoppingSettings.HappinessReport = squawk;
            return chat;
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