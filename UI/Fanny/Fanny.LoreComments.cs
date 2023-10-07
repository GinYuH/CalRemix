using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.Social.Base;
using Terraria.ModLoader.IO;
using System.IO;
using CalRemix.Projectiles.Weapons;
using System;
using CalamityMod;
using static System.Net.Mime.MediaTypeNames;
using Terraria.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReLogic.Graphics;
using ReLogic.Content;
using System.Linq;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Systems;
using CalamityMod.Items.LoreItems;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        internal static int previousHoveredItem;
        internal static int hoverTime;
        internal static int previousHoverTime;
        internal static bool ReadLoreItem => previousHoverTime >= 2 * 60 && hoverTime == 0;

        public static void UpdateLoreCommentTracking()
        {
            previousHoverTime = hoverTime;
            //Reset hover time if the player changes items theyre hovering over
            if (Main.HoverItem.type != previousHoveredItem || Main.HoverItem.ModItem == null || Main.HoverItem.ModItem is not LoreItem)
                hoverTime = 0;

            //Hover time should go up if were hovering a lore item
            else if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                hoverTime++;

            //If the player stops holding shift after holding shift for long enough to have been considered as "having read the lore", set hovertime to 0 so that fanny speaks
            else if (hoverTime > 2 * 60)
                hoverTime = 0;
        }

        public static void LoadLoreComments()
        {
            int pbgLoreItemType = ModContent.ItemType<LorePlaguebringerGoliath>();
            FannyMessage pbgLore = new FannyMessage("LorePBG", "I have the feeling this guy likes bees! What is he, an apiarist or something? It's fan-tastic to see people following their passion!",
                "Idle", (FannySceneMetrics scene) => ReadLoreItem && previousHoveredItem == pbgLoreItemType, 5, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves:false).AddDelay(0.4f);

            fannyMessages.Add(pbgLore);

            FannyMessage pbgEvilLore = new FannyMessage("LoreEvilPBG", "Boo hoo wahh wahhh wahhh the bees wahhh so cruel wahh wahhh wahh Draedon is heartless wahhhhh vile and despicable",
                "EvilIdle", FannyMessage.AlwaysShow, 5, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false)
                .NeedsActivation(1.5f).SpokenByEvilFanny();

            pbgLore.AddStartEvent(() => pbgEvilLore.ActivateMessage());

            fannyMessages.Add(pbgEvilLore);
        }

    }
}