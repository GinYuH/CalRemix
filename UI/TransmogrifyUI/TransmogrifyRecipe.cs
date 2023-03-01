using System;
using Terraria;

namespace CalRemix.UI.TransmogrifyUI
{
    public struct TransmogrifyRecipe
    {
        public int ingredient;
        public int catalyst;
        public int catalystAmount;
        public int time;
        public int result;
        public int resultAmount;

        public TransmogrifyRecipe(int ingredient, int catalyst, int catalystAmount, int time, int result, int resultAmount = 1)
        {
            this.ingredient = ingredient;
            this.catalyst = catalyst;
            this.catalystAmount = catalystAmount;
            this.time = time;
            this.result = result;
            this.resultAmount = resultAmount;
        }
    }
}
