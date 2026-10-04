namespace CalRemix
{
    public enum PerlinEase
    {
        /// <summary>
        /// No easing, all of the noise is consistent
        /// </summary>
        None = 0,
        /// <summary>
        /// Top starts solid and becomes noise
        /// </summary>
        EaseInTop = 1,
        /// <summary>
        /// Top is noise, bottom is air
        /// </summary>
        EaseOutBottom = 2,
        /// <summary>
        /// Top and bottom are solid, middle is noise
        /// </summary>
        EaseInOut = 3,
        /// <summary>
        /// Top and bottom are noise, middle is solid
        /// </summary>
        EaseOutIn = 4,
        /// <summary>
        /// Top is noise, bottom is solid
        /// </summary>
        EaseInBottom = 5,
        /// <summary>
        /// Top is air, bottom is noise
        /// </summary>
        EaseOutTop = 6,
        /// <summary>
        /// Top is air, middle is noise, bottom is solid
        /// </summary>
        EaseAirTopSolidBottom = 7,
        /// <summary>
        /// Top is solid, middle is noise, bottom is air
        /// </summary>
        EaseSolidTopAirBottom = 8,
    }
}