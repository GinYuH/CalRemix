using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Items.Materials;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace CalRemix.Content.Items.Tools.Chlorium
{
    public class ChloriumPaintbrush : ModItem
    {
        public override void Load()
        {
            On_Main.TryGetAmmo += GetPaintAmmo;
            On_SmartCursorHelper.SmartCursorLookup += SpoofPaintbrush;
        }

        private void SpoofPaintbrush(On_SmartCursorHelper.orig_SmartCursorLookup orig, Player player)
        {
            //Its annoying to edit the individual methods for smart cursor stuff because it uses private classes, so instead we just turn the paintbrush into a real paintbrush
            bool spoofed = false;
            if (player.inventory[player.selectedItem].type == Type)
            {
                player.inventory[player.selectedItem].type = ItemID.Paintbrush;
                spoofed = true;
            }

            orig(player);

            //If we turned the paintbrush into a vanilla one, undo that
            if (spoofed)
            {
                player.inventory[player.selectedItem].type = Type;
            }
        }

        public static readonly MethodInfo TryPaintMethod = typeof(Player).GetMethod("TryPainting", BindingFlags.Instance | BindingFlags.NonPublic);

        //Litterally only used to draw the remaining paint ammo on the ui
        private bool GetPaintAmmo(On_Main.orig_TryGetAmmo orig, Main self, Item sourceItem, out Item ammoItem, out Microsoft.Xna.Framework.Color ammoColor, out float ammoScale, out Microsoft.Xna.Framework.Vector2 ammoOffset)
        {
            if (sourceItem.type == Type)
            {
                ammoScale = 0.8f;
                ammoColor = Color.White;
                ammoOffset = new Vector2(22, 22);
                ammoItem = Main.LocalPlayer.FindPaintOrCoating();
                return ammoItem != null;
            }

            return orig(self, sourceItem, out ammoItem, out  ammoColor, out  ammoScale, out ammoOffset);
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityPainting[Type] = 1;
            ItemID.Sets.AlsoABuildingItem[Type] = true;
            ItemID.Sets.DuplicationMenuToolsFilter[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Paintbrush);
            Item.tileBoost = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(8).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.InventorySlotsTotal; i++)
            {
                if (inventory[i].PaintOrCoating)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool? UseItem(Player player)
        {
            int xpos = Player.tileTargetX;
            int ypos = Player.tileTargetY;
            if (Main.tile[xpos, ypos] != null && Main.tile[xpos, ypos].HasTile)
            {
                player.cursorItemIconEnabled = true;
                if (player.ItemTimeIsZero && player.itemAnimation > 0 && player.controlUseItem)
                {
                    TryPaintMethod.Invoke(player, xpos, ypos, false, true);
                }
            }
            return true;
        }
    }
}
