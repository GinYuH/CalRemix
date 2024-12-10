using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using System;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityMod.Rarities;
using Steamworks;
using Terraria.Localization;
using CalamityMod;

namespace CalRemix.Content.Items.Misc
{
    public class CalamitousCertificate : ModItem
    {
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
                    CalamityUtils.DisplayLocalizedText("Mods.CalRemix.StatusText.CertificateSent", Color.Brown);
                    Texture2D texture = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Items/Misc/CalamitousCertificate_Full").Value;
                    FileStream stream = new(path, FileMode.Create);
                    texture.SaveAsPng(stream, texture.Width, texture.Height);
                }
                else
                    CombatText.NewText(player.getRect(), Color.Red, Language.GetOrRegister("Mods.CalRemix.StatusText.CertificateOwned").Value, true);
            }
            return true;
        }
    }
}