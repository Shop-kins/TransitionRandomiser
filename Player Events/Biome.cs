using System;

namespace TransitionRandomiser.Player_Events
{
    [Serializable]
    class Biome
    {
        private String name;
        private String[] gameIDs;

        public Biome(String name, String[] gameIDs)
        {
            this.name = name;
            this.gameIDs = gameIDs;
        }

        public String GetName()
        {
            return name;
        }

        public String[] GetGameIDs()
        {
            return gameIDs;
        }

    }

    class BiomeHandler
    {
        //public static Biome LIFEPOD = new Biome("lifepod", new string[] { "lifepod" });
        public static Biome SAFESHALLOWS = new Biome("Safe Shallows", new string[] { "safeshallows", "lifepod" });
        public static Biome GRASSYPLATEAUS = new Biome("Grassy Plateaus", new string[] { "grassyplateaus" });
        public static Biome KELPFOREST = new Biome("Kelp Forest", new string[] { "kelpforest" });
        public static Biome JELLYSHROOMCAVE = new Biome("Jellyshroom Cave", new string[] { "jellyshroomcaves" });
        public static Biome DUNES = new Biome("Dunes", new string[] { "dunes" });
        public static Biome MUSHROOMFOREST = new Biome("Mushroom Forest", new string[] { "mushroomforest" });
        public static Biome BLOODKELP = new Biome("Bloodkelp", new string[] { "bloodkelptwo" });
        public static Biome MOUNTAINS = new Biome("Mountains", new string[] { "mountains" });
        public static Biome UNDERWATERISLANDS = new Biome("Underwater Islands", new string[] { "underwaterislands" });
        public static Biome BULBZONE = new Biome("Bulbzone", new string[] { "kooshzone" });
        public static Biome CRASHZONE = new Biome("Crashzone", new string[] { "crashzone", "crashedship", "generatorroom" });
        public static Biome CRAGFIELD = new Biome("Cragfield", new string[] { "cragfield" });
        public static Biome GRANDREEF = new Biome("Grand Reef", new string[] { "grandreef", "deepgrandreef" });
        public static Biome SPARSEREEF = new Biome("Sparse Reef", new string[] { "sparsereef" });
        public static Biome SEATREADERPATH = new Biome("Sea Treaders Path", new string[] { "seatreaderpath" });
        public static Biome BLOODKELPTRENCH = new Biome("Bloodkelp Trench", new string[] { "bloodkelp" });
        public static Biome FLOATINGISLAND = new Biome("Floating Island", new string[] { "floatingisland", "floatingislandcaveteleporter" });
        public static Biome LOSTRIVER = new Biome("Lost River", new string[] { "lostriver" });
        public static Biome INACTIVELAVAZONE = new Biome("Inactive Lavazone", new string[] { "ilzcorridor", "ilzchamber", "lavapit", "ilzcastlechamber", "ilzcastletunnel" });
        public static Biome LAVALAKES = new Biome("Lava Lakes", new string[] { "lavafalls", "lavalakes" });
        public static Biome ALIENBASE = new Biome("Alien Base", new string[] { "precursorcache", "precursor", "precursorcave", "prison" });

        public static Biome[] BIOMES = new Biome[]
        {
            //LIFEPOD,
            SAFESHALLOWS,
            GRASSYPLATEAUS,
            KELPFOREST,
            JELLYSHROOMCAVE,
            DUNES,
            MUSHROOMFOREST,
            BLOODKELP,
            MOUNTAINS,
            UNDERWATERISLANDS,
            BULBZONE,
            CRASHZONE,
            CRAGFIELD,
            GRANDREEF,
            SPARSEREEF,
            SEATREADERPATH,
            BLOODKELPTRENCH,
            FLOATINGISLAND,
            LOSTRIVER,
            INACTIVELAVAZONE,
            LAVALAKES,
            ALIENBASE
        };

        public static Biome GetBiomeByName(String name)
        {
            for (int i = 0; i < BIOMES.Length; i++)
            {
                if (BIOMES[i].GetName().Equals(name))
                {
                    return BIOMES[i];
                }
            }
            return null;
        }

        // Maps an internal game biome ID to one of our valid biomes
        public static Biome GetBiomeByGameID(String gameID)
        {
            String firstPartOfGameID = gameID.ToLower().Split('_')[0];
            for (int i = 0; i < BIOMES.Length; i++)
            {
                String[] gameIDs = BIOMES[i].GetGameIDs();
                for (int j = 0; j < gameIDs.Length; j++)
                {
                    if (gameIDs[j].Equals(firstPartOfGameID))
                    {
                        return BIOMES[i];
                    }
                }
            }
            return null;
        }

    }

}
