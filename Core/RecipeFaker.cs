using System.Linq;
using System.Reflection;

using CalRemix.Content.Items.Placeables;

using MonoMod.Cil;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core;

internal sealed class RecipeFaker : ModSystem
{
    public override void PostSetupContent()
    {
        base.PostSetupContent();

        if (!ModLoader.TryGetMod("RecipeBrowser", out var mod))
        {
            // shouldn't be possible
            return;
        }

        var asm = mod.Code;
        if (asm.GetType("RecipeBrowser.CraftUI") is not { } craftUiType)
        {
            return;
        }

        var setItemMethod = craftUiType.GetMethod("SetItem", BindingFlags.NonPublic | BindingFlags.Instance);
        if (setItemMethod is null)
        {
            return;
        }

        MonoModHooks.Modify(
            setItemMethod,
            il =>
            {
                var c = new ILCursor(il);

                c.GotoNext(MoveType.After, x => x.MatchLdarg1());
                c.EmitDelegate(ReplaceItem);
            }
        );

        if (asm.GetType("RecipeBrowser.RecipeCatalogueUI") is not { } recipeCatalogueUiType)
        {
            return;
        }

        if (recipeCatalogueUiType.GetMethod("UpdateGrid", BindingFlags.NonPublic | BindingFlags.Instance) is not { } updateGrid)
        {
            return;
        }

        /*
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            ReplaceWithFake(item.type);
            RecipeCatalogueUI.instance.queryLootItem = (item.type == 0) ? null : item;
            RecipeCatalogueUI.instance.updateNeeded = true;
            SharedUI.instance.SelectedCategory = SharedUI.instance.categories[0];
        }
         */

        MonoModHooks.Modify(
            updateGrid,
            il =>
            {
                var c = new ILCursor(il);

                while (c.TryGotoNext(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type))))
                {
                    c.EmitDelegate(ReplaceItem);
                }
            }
        );
        
        if (recipeCatalogueUiType.GetMethod("PassRecipeFilters", BindingFlags.NonPublic | BindingFlags.Instance) is not { } passRecipeFilters)
        {
            return;
        }
        
        MonoModHooks.Modify(
            passRecipeFilters,
            il =>
            {
                var c = new ILCursor(il);

                while (c.TryGotoNext(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.type))))
                {
                    c.EmitDelegate(ReplaceItem);
                }
            }
        );
    }

    private static int ReplaceItem(int itemId)
    {
        var fakeStones = new[]
        {
            ModContent.ItemType<Andesite>(),
            ModContent.ItemType<Diorite>(),
            ModContent.ItemType<Granite>(),
        };

        return fakeStones.Contains(itemId) ? ItemID.StoneBlock : itemId;
    }
}