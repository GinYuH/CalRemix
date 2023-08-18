using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.UI.CalamitasEnchants;
using CalRemix.NPCs;
using CalRemix.NPCs.Bosses;
using CalRemix.Items.Accessories;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.Localization;
using ReLogic.Content;
using Terraria.GameContent;
using ReLogic;
using Terraria.ID;

namespace CalRemix
{
	public class CalRemix : Mod
	{
		public static CalRemix instance;
		public static int CosmiliteCoinCurrencyId;
		public static int KlepticoinCurrencyId;

		public static List<int> oreList = new List<int>
		{
			TileID.Copper,
			TileID.Tin,
			TileID.Iron,
			TileID.Lead,
			TileID.Silver,
			TileID.Tungsten,
			TileID.Gold,
			TileID.Platinum
		};

		public override void PostSetupContent()
		{
			Mod cal = ModLoader.GetMod("CalamityMod");
			cal.Call("RegisterModCooldowns", this);
			cal.Call("DeclareMiniboss", ModContent.NPCType<LifeSlime>());
			cal.Call("MakeItemExhumable", ModContent.ItemType<YharimsGift>(), ModContent.ItemType<YharimsCurse>());
			/*cal.Call("DeclareOneToManyRelationshipForHealthBar", ModContent.NPCType<DerellectBoss>(), ModContent.NPCType<SignalDrone>());
            cal.Call("DeclareOneToManyRelationshipForHealthBar", ModContent.NPCType<DerellectBoss>(), ModContent.NPCType<DerellectPlug>());
			{
				Mod bossChecklist;
				ModLoader.TryGetMod("BossChecklist", out bossChecklist);
				if (bossChecklist != null)
				{
					bossChecklist.Call(new object[12]
				{
				"AddBoss",
				12.5f,
				ModContent.NPCType<NPCs.Bosses.DerellectBoss>(),
				this,
				"The Derellect",
				(Func<bool>)(() => CalRemixWorld.downedDerellect),
				ModContent.ItemType<CalamityMod.Items.Pets.BloodyVein>(),
				null,
				new List<int>
				{
					ModLoader.GetMod("CalamityMod").Find<ModItem>("BloodyVein").Type
				},
				$"Damage the Mechanical Worm using a [i:{ModContent.ItemType<CalamityMod.Items.Pets.BloodyVein>()}]. But why would you do that?",
				"The Derellect returns to the scrap heap...",
				null
				});
				}
			}*/
			{
				Mod bossChecklist;
				ModLoader.TryGetMod("BossChecklist", out bossChecklist);
				//private static void AddBoss(Mod bossChecklist, Mod hostMod, string name, float difficulty, List<int> npcTypes, Func<bool> downed, object summon,
				//List<int> collection, string instructions, string despawn, Func< bool > available, Action<SpriteBatch, Rectangle, Color> portrait = null, string bossHeadTex = null)
				if (bossChecklist != null)
				{
					bossChecklist.Call(
					"AddBoss",
					0.22f,
					ModContent.NPCType<WulfwyrmHead>(),
					this,
					"Wulfrum Excavator",
					(Func<bool>)(() => CalRemixWorld.downedExcavator),
					ModContent.ItemType<CalamityMod.Items.Materials.EnergyCore>(),
					new List<int>() { ModContent.ItemType<Items.KnowledgeExcavator>() },
					new List<int>() { ItemID.LesserHealingPotion },
					$"Place an [i:{ModContent.ItemType<CalamityMod.Items.Materials.EnergyCore>()}] inside of an empty [i:{ItemID.LivingWoodChest}]",
				"The old excavator has successfully harvested every player.",
					CalamityUtils.ColorMessage("The old excavator has successfully harvested every player.", new Microsoft.Xna.Framework.Color(108, 245, 133)),
					"CalamityMod/NPCs/Boss/WulfwyrmBossChecklist",
					null,
					null);
				}
			}
			LocalizedText fallacious = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Name");
			LocalizedText fallaciousDesc = Language.GetOrRegister($"Mods.{nameof(CalRemix)}.Enchantments.Fallacious.Description");
			cal.Call("CreateEnchantment", fallacious, fallaciousDesc, 156, new Predicate<Item>(Enchantable), "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneFallacious", delegate (Player player)
			{
				player.GetModPlayer<CalRemixPlayer>().amongusEnchant = true;
			});
			List<(int, int, Action<int>, int, bool, float, int[], int[])> brEntries = (List<(int, int, Action<int>, int, bool, float, int[], int[])>)cal.Call("GetBossRushEntries");
			int[] excIDs = { ModContent.NPCType<WulfwyrmBody>(), ModContent.NPCType<WulfwyrmTail>() };
			int[] headID = { ModContent.NPCType<WulfwyrmHead>() };
			Action<int> pr = delegate (int npc) 
			{
				NPC.SpawnOnPlayer(CalamityMod.Events.BossRushEvent.ClosestPlayerToWorldCenter, ModContent.NPCType<WulfwyrmHead>()); 
			};
			brEntries.Insert(0, (ModContent.NPCType<WulfwyrmHead>(), -1, pr, 180, false, 0f, excIDs, headID));
			cal.Call("SetBossRushEntries", brEntries);

			/* I hate enchantments
            EnchantmentManager.EnchantmentList.Add(new Enchantment("Fallacious", "Greatly increases critical strike damage but critical strike chance is reduced. Critical hits also hurt you.\nDoes nothing for now.", 156, "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneFallacious", null, delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().amongusEnchant = true;
            }, (Item item) => item.IsEnchantable() && item.damage > 0 && !item.CountsAsClass<SummonDamageClass>() && !item.IsWhip()));
            EnchantmentManager.EnchantmentList.Add(new Enchantment("Defiant", "Dealing damage increases defense and damage but defense damage taken is increased.\nDoes nothing for now.", 157, "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneDefiant", null, delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().earthEnchant = true;
            }, (Item item) => item.IsEnchantable() && item.damage > 0 && !item.CountsAsClass<SummonDamageClass>() && !item.IsWhip()));
			*/
		}

        public override void Load()
        {
			instance = this;
			CosmiliteCoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.CosmiliteCoinCurrency(ModContent.ItemType<Items.CosmiliteCoin>(), 100L, "Mods.CalRemix.Currencies.CosmiliteCoinCurrency"));
			KlepticoinCurrencyId = CustomCurrencyManager.RegisterCurrency(new Items.KlepticoinCurrency(ModContent.ItemType<Items.Klepticoin>(), 100L, "Mods.CalRemix.Currencies.Klepticoin"));
		}
		public override void Unload()
		{
			instance = null;
		}

		public static bool Enchantable(Item item)
        {
            return item.IsEnchantable() && item.damage > 0 && !item.CountsAsClass<SummonDamageClass>() && !item.IsWhip();
        }
		public static void AddToShop(int type, int price, ref Chest shop, ref int nextSlot, bool condition = true, int specialMoney = 0)
        {
			if (!condition || shop is null) return;
			shop.item[nextSlot].SetDefaults(type);
			shop.item[nextSlot].shopCustomPrice = price > 0 ? price : shop.item[nextSlot].value;
			if (specialMoney == 1) shop.item[nextSlot].shopSpecialCurrency = CosmiliteCoinCurrencyId;
			else if (specialMoney == 2) shop.item[nextSlot].shopSpecialCurrency = KlepticoinCurrencyId;
			nextSlot++;
		}
    }
}