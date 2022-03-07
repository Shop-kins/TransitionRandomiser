using System;

namespace TransitionRandomiser.Player_Events
{
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
        public static Biome LIFEPOD = new Biome("lifepod", new string[] { "lifepod" });
        public static Biome SAFESHALLOWS = new Biome("safeshallows", new string[] { "safeshallows" });
        public static Biome GRASSYPLATEAUS = new Biome("grassyplateaus", new string[] { "grassyplateaus" });
        public static Biome KELPFOREST = new Biome("kelpforest", new string[] { "kelpforest" });
        public static Biome JELLYSHROOMCAVE = new Biome("jellyshroomcaves", new string[] { "jellyshroomcaves" });
        public static Biome DUNES = new Biome("dunes", new string[] { "dunes" });
        public static Biome MUSHROOMFOREST = new Biome("mushroomforest", new string[] { "mushroomforest" });
        public static Biome BLOODKELP = new Biome("bloodkelp", new string[] { "bloodkelptwo" });
        public static Biome MOUNTAINS = new Biome("mountains", new string[] { "mountains" });
        public static Biome UNDERWATERISLANDS = new Biome("underwaterislands", new string[] { "underwaterislands" });
        public static Biome BULBZONE = new Biome("bulbzone", new string[] { "kooshzone" });
        public static Biome CRASHZONE = new Biome("crashzone", new string[] { "crashzone", "crashedship", "generatorroom" });
        public static Biome CRAGFIELD = new Biome("cragfield", new string[] { "cragfield" });
        public static Biome GRANDREEF = new Biome("grandreef", new string[] { "grandreef", "deepgrandreef" });
        public static Biome SPARSEREEF = new Biome("sparsereef", new string[] { "sparsereef" });
        public static Biome SEATREADERPATH = new Biome("seatreaderpath", new string[] { "seatreaderpath" });
        public static Biome BLOODKELPTRENCH = new Biome("bloodkelptrench", new string[] { "bloodkelp" });
        public static Biome FLOATINGISLAND = new Biome("floatingisland", new string[] { "floatingisland", "floatingislandcaveteleporter" });
        public static Biome LOSTRIVER = new Biome("lostriver", new string[] { "lostriver" });
        public static Biome INACTIVELAVAZONE = new Biome("inactivelavazone", new string[] { "ilzcorridor", "ilzchamber", "lavapit", "ilzcastlechamber", "ilzcastletunnel" });
        public static Biome LAVALAKES = new Biome("lavalakes", new string[] { "lavafalls", "lavalakes" });
        public static Biome ALIENBASE = new Biome("alienbase", new string[] { "precursorcache", "precursor", "precursorcave", "prison" });

        public static Biome[] BIOMES = new Biome[]
        {
            LIFEPOD,
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
