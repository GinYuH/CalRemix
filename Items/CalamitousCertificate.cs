using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using System.IO;
using System;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityMod.Rarities;
using Steamworks;

namespace CalRemix.Items
{
	public class CalamitousCertificate : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Calamitous Certificate");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 0;
			Item.rare = ModContent.RarityType<CalamityRed>();
            Item.useAnimation = 30;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted)
			{
				string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/Trial Award for {SteamFriends.GetPersonaName()}.png";
				if (!File.Exists(path))
				{
                    Main.NewText("You have been mailed your certificate.", Color.Red);
                    Texture2D texture = ModContent.Request<Texture2D>($"{Mod.Name}/Items/CalamitousCertificate_Full", AssetRequestMode.ImmediateLoad).Value;
					FileStream stream = new(path, FileMode.Create);
					texture.SaveAsPng(stream, texture.Width, texture.Height);
                }
				else
					CombatText.NewText(player.getRect(), Color.Red, "Certificate has already been mailed.", true);
            }
            return true;
        }
	}
}