using CalRemix.Content.Items.Placeables.MusicBoxes.ExoMechs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Placeables.MusicBoxes
{
    public abstract class RemixMusicBox : ModItem
    {
        public static readonly int[] ExoMechMusicBoxes =
        [
            ItemType<ApingasMusicBox>(),
            ItemType<LarryMusicBox>(),
            ItemType<ThanosMusicBox>(),
            ItemType<ApingasLarryMusicBox>(),
            ItemType<ApingasThanosMusicBox>(),
            ItemType<ThanosLarryMusicBox>(),
            ItemType<XOMusicBox>()
        ];
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => (this != null);
    }
    public class AcidsighterMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Acidsighter, Type, TileType<Tiles.MusicBoxes.AcidsighterMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.AcidsighterMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class AcidRainTier2MusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.AcidRainTier2, Type, TileType<Tiles.MusicBoxes.AcidRainTier2MusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.AcidRainTier2MusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(CalRemix.CalMusic.Find<ModItem>("AcidRainTier3MusicBox").Type).
                AddIngredient(CalRemix.CalMusic.Find<ModItem>("SulphurousSeaDayMusicBox").Type).
                AddTile(TileID.TinkerersWorkbench).
                Register();
            CreateRecipe().
                AddIngredient(CalRemix.CalMusic.Find<ModItem>("AcidRainTier3MusicBox").Type).
                AddIngredient(CalRemix.CalMusic.Find<ModItem>("SulphurousSeaNightMusicBox").Type).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
    public class CarcinogenMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Carcinogen, Type, TileType<Tiles.MusicBoxes.CarcinogenMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.CarcinogenMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class CryoSlimeMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.CryoSlime, Type, TileType<Tiles.MusicBoxes.CryoSlimeMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.CryoSlimeMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class DerellectMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Derellect, Type, TileType<Tiles.MusicBoxes.DerellectMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.DerellectMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class EmpressofLightDayMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.EmpressofLightDay, Type, TileType<Tiles.MusicBoxes.EmpressofLightDayMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.EmpressofLightDayMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ExcavatorMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.WulfrumExcavator, Type, TileType<Tiles.MusicBoxes.ExcavatorMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExcavatorMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class HypnosMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Hypnos, Type, TileType<Tiles.MusicBoxes.HypnosMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.HypnosMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class HydrogenMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Hydrogen, Type, TileType<Tiles.MusicBoxes.HydrogenMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.HydrogenMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class IonogenMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Ionogen, Type, TileType<Tiles.MusicBoxes.IonogenMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.IonogenMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class LaRugaMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.LaRuga, Type, TileType<Tiles.MusicBoxes.LaRugaMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.LaRugaMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PlaguedJungleMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.PlaguedJungle, Type, TileType<Tiles.MusicBoxes.PlaguedJungleMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PlaguedJungleMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PolyphemalusMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Polyphemalus, Type, TileType<Tiles.MusicBoxes.PolyphemalusMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PolyphemalusMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PolyphemalusAltMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.PolyphemalusAlt, Type, TileType<Tiles.MusicBoxes.PolyphemalusAltMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PolyphemalusAltMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class TheCalamityMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.TheCalamity, Type, TileType<Tiles.MusicBoxes.TheCalamityMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.TheCalamityMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class MenuMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Menu, Type, TileType<Tiles.MusicBoxes.MenuMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.MenuMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AcidsighterMusicBox>().
                AddIngredient<EmpressofLightDayMusicBox>().
                AddIngredient<HypnosMusicBox>().
                AddIngredient<PlaguedJungleMusicBox>().
                AddIngredient<PolyphemalusAltMusicBox>().
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
    public class VernalPassMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.VernalPass, Type, TileType<Tiles.MusicBoxes.VernalPassMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.VernalPassMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class OxygenMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Oxygen, Type, TileType<Tiles.MusicBoxes.OxygenMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.OxygenMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PyrogenMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Pyrogen, Type, TileType<Tiles.MusicBoxes.PyrogenMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PyrogenMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PhytogenMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Phytogen, Type, TileType<Tiles.MusicBoxes.PhytogenMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PhytogenMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PathogenMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Pathogen, Type, TileType<Tiles.MusicBoxes.PathogenMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PathogenMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class BaronStraitMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.BaronStrait, Type, TileType<Tiles.MusicBoxes.BaronStraitMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.BaronStraitMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class AsbestosMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.AsbestosCaves, Type, TileType<Tiles.MusicBoxes.AsbestosMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.AsbestosMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class PlasticOracleMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.PlasticOracle, Type, TileType<Tiles.MusicBoxes.PlasticOracleMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.PlasticOracleMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ProfanedDesertMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.ProfanedDesert, Type, TileType<Tiles.MusicBoxes.ProfanedDesertMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ProfanedDesertMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
}
