using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TransitionRandomiser.Player_Events
{

    [Serializable]
    internal class TeleportLocation
    {
        private Biome origin;
        private Biome biome;
        private SerialisableVector3 position;
        private SerialisableVector3 rotation;
        private Boolean knownToPlayer;
        private String marker;

        public TeleportLocation(Biome biome, SerialisableVector3 position, SerialisableVector3 rotation, Biome origin, Boolean knownToPlayer, String marker)
        {
            this.biome = biome;
            this.position = position;
            this.rotation = rotation;
            this.origin = origin;
            this.knownToPlayer = knownToPlayer;
            this.marker = marker;
        }

        public String getMarker()
        {
            return marker;
        }

        public void SetMarker(String m)
        {
            marker = m;
        }

        public Boolean isKnownToPlayer()
        {
            return knownToPlayer;
        }

        public void SetKnownToPlayer(Boolean k)
        {
            knownToPlayer = k;
        }

        public Biome GetOrigin()
        {
            return origin;
        }

        public Biome GetBiome()
        {
            return biome;
        }

        public SerialisableVector3 GetPosition()
        {
            return position;
        }

        public SerialisableVector3 GetRotation()
        {
            return rotation;
        }

    }

    [Serializable]
    internal class Transition
    {
        private Biome from, to;

        private KeyValuePair<SerialisableVector3, SerialisableVector3>[] playerStates;

        private String[] markers;

        public Transition(Biome from, Biome to, Vector3 targetPosition, Vector3 targetRotation, String marker)
        {
            this.from = from;
            this.to = to;
            playerStates = new KeyValuePair<SerialisableVector3, SerialisableVector3>[]
            {
                new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(targetPosition), new SerialisableVector3(targetRotation))
            };
            markers = new String[] { marker };
        }

        public Transition(Biome from, Biome to, KeyValuePair<SerialisableVector3, SerialisableVector3>[] playerStates, String[] markers)
        {
            this.from = from;
            this.to = to;
            this.playerStates = playerStates;
            this.markers = markers;
        }

        public String[] getMarkers()
        {
            return markers;
        }

        public Biome GetFrom()
        {
            return from;
        }

        public Biome GetTo()
        {
            return to;
        }

        public KeyValuePair<SerialisableVector3, SerialisableVector3>[] GetPlayerStates()
        {
            return playerStates;
        }

    }

    [Serializable]
    internal class SerialisableVector3
    {
        public int x, y, z;

        public SerialisableVector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SerialisableVector3(Vector3 v)
        {
            x = (int)v.x;
            y = (int)v.y;
            z = (int)v.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

    }

    internal class TransitionHandler
    {

        static Transition DUNES_BLOODKELP = new Transition(BiomeHandler.DUNES, BiomeHandler.BLOODKELP, new Vector3(-881, -270, 1375), new Vector3(40, 48, 0), "A");
        static Transition DUNES_MUSHROOMFOREST = new Transition(BiomeHandler.DUNES, BiomeHandler.MUSHROOMFOREST, new Vector3(-1220, -217, 682), new Vector3(1, 101, 0), "B");
        static Transition DUNES_KELPFOREST = new Transition(BiomeHandler.DUNES, BiomeHandler.KELPFOREST, new Vector3(-964, -66, 166), new Vector3(5, 87, 0), "C");
        static Transition DUNES_GRASSYPLATEAUS = new Transition(BiomeHandler.DUNES, BiomeHandler.GRASSYPLATEAUS, new Vector3(-940, -128, -55), new Vector3(346, 80, 0), "D");
        static Transition DUNES_BLOODKELPTRENCH = new Transition(BiomeHandler.DUNES, BiomeHandler.BLOODKELPTRENCH, new Vector3(-1286, -238, -320), new Vector3(87, 187, 0), "E");
        static Transition DUNES_SEATREADERSPATH = new Transition(BiomeHandler.DUNES, BiomeHandler.SEATREADERPATH, new Vector3(-1542, -335, -133), new Vector3(8, 180, 0), "F");

        static Transition BLOODKELP_DUNES = new Transition(BiomeHandler.BLOODKELP, BiomeHandler.DUNES, new Vector3(-936, -227, 1333), new Vector3(20, 226, 0), "A");
        static Transition BLOODKELP_MUSHROOMFOREST = new Transition(BiomeHandler.BLOODKELP, BiomeHandler.MUSHROOMFOREST, new Vector3(-736, -180, 875), new Vector3(11, 160, 0), "B");
        static Transition BLOODKELP_UNDERWATERISLANDS = new Transition(BiomeHandler.BLOODKELP, BiomeHandler.UNDERWATERISLANDS, new Vector3(-345, -181, 1108), new Vector3(22, 116, 0), "C");
        static Transition BLOODKELP_MOUNTAINS = new Transition(BiomeHandler.BLOODKELP, BiomeHandler.MOUNTAINS, new Vector3(-129, -368, 1482), new Vector3(12, 84, 0), "D");

        static Transition MOUNTAINS_BLOODKELP = new Transition(BiomeHandler.MOUNTAINS, BiomeHandler.BLOODKELP, new Vector3(-222, -368, 1495), new Vector3(17, 247, 0), "A");
        static Transition MOUNTAINS_UNDERWATERISLANDS = new Transition(BiomeHandler.MOUNTAINS, BiomeHandler.UNDERWATERISLANDS, new Vector3(75, -147, 959), new Vector3(25, 258, 0), "B");
        static Transition MOUNTAINS_KELPFOREST = new Transition(BiomeHandler.MOUNTAINS, BiomeHandler.KELPFOREST, new Vector3(135, -29, 804), new Vector3(7, 210, 0), "C");
        static Transition MOUNTAINS_MUSHROOMFOREST = new Transition(BiomeHandler.MOUNTAINS, BiomeHandler.MUSHROOMFOREST, new Vector3(524, -144, 924), new Vector3(33, 165, 0), "D");
        static Transition MOUNTAINS_BULBZONE = new Transition(BiomeHandler.MOUNTAINS, BiomeHandler.BULBZONE, new Vector3(980, -186, 954), new Vector3(35, 122, 0), "E");

        static Transition UNDERWATERISLANDS_BLOODKELP = new Transition(BiomeHandler.UNDERWATERISLANDS, BiomeHandler.BLOODKELP, new Vector3(-384, -235, 1142), new Vector3(35, 287, 0), "A");
        static Transition UNDERWATERISLANDS_MOUNTAINS = new Transition(BiomeHandler.UNDERWATERISLANDS, BiomeHandler.MOUNTAINS, new Vector3(159, -85, 980), new Vector3(349, 59, 0), "B");
        static Transition UNDERWATERISLANDS_KELPFOREST = new Transition(BiomeHandler.UNDERWATERISLANDS, BiomeHandler.KELPFOREST, new Vector3(-10, -16, 502), new Vector3(6, 149, 0), "C");
        static Transition UNDERWATERISLANDS_GRASSYPLATEAUS = new Transition(BiomeHandler.UNDERWATERISLANDS, BiomeHandler.GRASSYPLATEAUS, new Vector3(-232, -49, 665), new Vector3(24, 236, 0), "D");
        static Transition UNDERWATERISLANDS_MUSHROOMFOREST = new Transition(BiomeHandler.UNDERWATERISLANDS, BiomeHandler.MUSHROOMFOREST, new Vector3(-605, -95, 860), new Vector3(18, 221, 0), "E");

        static Transition BULBZONE_MOUNTAINS = new Transition(BiomeHandler.BULBZONE, BiomeHandler.MOUNTAINS, new Vector3(866, -187, 1004), new Vector3(42, 289, 0), "A");
        static Transition BULBZONE_MUSHROOMFOREST = new Transition(BiomeHandler.BULBZONE, BiomeHandler.MUSHROOMFOREST, new Vector3(723, -154, 665), new Vector3(5, 203, 0), "B");
        static Transition BULBZONE_CRASHZONE = new Transition(BiomeHandler.BULBZONE, BiomeHandler.CRASHZONE, new Vector3(1131, -88, 236), new Vector3(350, 203, 0), "C");

        static Transition MUSHROOMFOREST_BLOODKELP = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.BLOODKELP, new Vector3(-726, -202, 921), new Vector3(30, 354, 0), "A");
        static Transition MUSHROOMFOREST_DUNES = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.DUNES, new Vector3(-1272, -218, 702), new Vector3(47, 275, 0), "B");
        static Transition MUSHROOMFOREST_KELPFOREST = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.KELPFOREST, new Vector3(525, -68, 598), new Vector3(16, 262, 0), "C");
        static Transition MUSHROOMFOREST_GRASSYPLATEAUS = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.GRASSYPLATEAUS, new Vector3(475, -132, 373), new Vector3(353, 252, 0), "D");
        static Transition MUSHROOMFOREST_UNDERWATERISLANDS = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.UNDERWATERISLANDS, new Vector3(-498, -66, 872), new Vector3(23, 44, 0), "E");
        static Transition MUSHROOMFOREST_MOUNTAINS = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.MOUNTAINS, new Vector3(575, -173, 960), new Vector3(39, 35, 0), "F");
        static Transition MUSHROOMFOREST_BULBZONE = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.BULBZONE, new Vector3(865, -171, 637), new Vector3(27, 95, 0), "G");
        static Transition MUSHROOMFOREST_CRASHZONE = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.CRASHZONE, new Vector3(650, -38, 140), new Vector3(350, 171, 0), "H");
        static Transition MUSHROOMFOREST_SAFESHALLOWS = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.SAFESHALLOWS, new Vector3(523, -42, 34), new Vector3(350, 152, 0), "I");

        static Transition KELPFOREST_MOUNTAINS = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.MOUNTAINS, new Vector3(163, -32, 850), new Vector3(25, 32, 0), "A");
        static Transition KELPFOREST_MUSHROOMFOREST = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.MUSHROOMFOREST, new Vector3(561, -109, 581), new Vector3(31, 107, 0), "B");
        static Transition KELPFOREST_GRASSYPLATEAUS = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.GRASSYPLATEAUS, new Vector3(205, -61, 313), new Vector3(23, 80, 0), "C");
        static Transition KELPFOREST_SAFESHALLOWS = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.SAFESHALLOWS, new Vector3(-110, -7, -178), new Vector3(12, 329, 0), "D");
        static Transition KELPFOREST_UNDERWATERISLANDS = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.UNDERWATERISLANDS, new Vector3(-25, -39, 586), new Vector3(19, 344, 0), "E");
        static Transition KELPFOREST_CRASHZONE = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.CRASHZONE, new Vector3(382, -11, 369), new Vector3(347, 83, 0), "F");
        static Transition KELPFOREST_CRAGFIELD = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.CRAGFIELD, new Vector3(-189, -50, -660), new Vector3(31, 173, 0), "G");
        static Transition KELPFOREST_GRANDREEF = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.GRANDREEF, new Vector3(-275, -98, -696), new Vector3(58, 228, 0), "H");
        static Transition KELPFOREST_SPARSEREEF = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.SPARSEREEF, new Vector3(-388, -26, -640), new Vector3(41, 202, 0), "I");
        static Transition KELPFOREST_DUNES = new Transition(BiomeHandler.KELPFOREST, BiomeHandler.DUNES, new Vector3(-991, -76, 175), new Vector3(43, 293, 0), "J");

        static Transition GRASSYPLATEAUS_UNDERWATERISLANDS = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.UNDERWATERISLANDS, new Vector3(-113, -51, 670), new Vector3(40, 57, 0), "A");
        static Transition GRASSYPLATEAUS_MUSHROOMFOREST = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.MUSHROOMFOREST, new Vector3(533, -160, 382), new Vector3(0, 61, 0), "B");
        static Transition GRASSYPLATEAUS_KELPFOREST = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.KELPFOREST, new Vector3(153, -58, 283), new Vector3(6, 227, 0), "C");
        static Transition GRASSYPLATEAUS_SAFESHALLOWS = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.SAFESHALLOWS, new Vector3(125, -48, 157), new Vector3(343, 240, 0), "D");
        static Transition GRASSYPLATEAUS_CRAGFIELD = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.CRAGFIELD, new Vector3(-116, -119, -831), new Vector3(33, 204, 0), "E");
        static Transition GRASSYPLATEAUS_CRASHZONE = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.CRASHZONE, new Vector3(252, -15, -640), new Vector3(17, 115, 0), "F");
        static Transition GRASSYPLATEAUS_DUNES = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.DUNES, new Vector3(-1040, -171, -14), new Vector3(23, 230, 0), "G");
        static Transition GRASSYPLATEAUS_BLOODKELPTRENCH = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.BLOODKELPTRENCH, new Vector3(-758, -124, -300), new Vector3(77, 209, 0), "H");
        static Transition GRASSYPLATEAUS_SPARSEREEF = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.SPARSEREEF, new Vector3(-542, -41, -565), new Vector3(55, 214, 0), "I");

        static Transition CRASHZONE_BULBZONE = new Transition(BiomeHandler.CRASHZONE, BiomeHandler.BULBZONE, new Vector3(1131, -131, 296), new Vector3(45, 16, 0), "A");
        static Transition CRASHZONE_MUSHROOMFOREST = new Transition(BiomeHandler.CRASHZONE, BiomeHandler.MUSHROOMFOREST, new Vector3(658, -71, 204), new Vector3(41, 356, 0), "B");
        static Transition CRASHZONE_SAFESHALLOWS = new Transition(BiomeHandler.CRASHZONE, BiomeHandler.SAFESHALLOWS, new Vector3(388, -11, -219), new Vector3(2, 289, 0), "C");
        static Transition CRASHZONE_KELPFOREST = new Transition(BiomeHandler.CRASHZONE, BiomeHandler.KELPFOREST, new Vector3(308, -22, -378), new Vector3(10, 281, 0), "D");
        static Transition CRASHZONE_GRASSYPLATEAUS = new Transition(BiomeHandler.CRASHZONE, BiomeHandler.GRASSYPLATEAUS, new Vector3(173, -42, -607), new Vector3(37, 285, 0), "E");
        static Transition CRASHZONE_CRAGFIELD = new Transition(BiomeHandler.CRASHZONE, BiomeHandler.CRAGFIELD, new Vector3(22, -126, -1025), new Vector3(34, 214, 0), "F");

        static Transition CRAGFIELD_CRASHZONE = new Transition(BiomeHandler.CRAGFIELD, BiomeHandler.CRASHZONE, new Vector3(143, -61, -934), new Vector3(358, 40, 0), "A");
        static Transition CRAGFIELD_GRASSYPLATEAUS = new Transition(BiomeHandler.CRAGFIELD, BiomeHandler.GRASSYPLATEAUS, new Vector3(-65, -112, -751), new Vector3(354, 26, 0), "B");
        static Transition CRAGFIELD_KELPFOREST = new Transition(BiomeHandler.CRAGFIELD, BiomeHandler.KELPFOREST, new Vector3(-250, -33, -630), new Vector3(1, 1, 0), "C");
        static Transition CRAGFIELD_GRANDREEF = new Transition(BiomeHandler.CRAGFIELD, BiomeHandler.GRANDREEF, new Vector3(-250, -189, -961), new Vector3(50, 290, 0), "D");

        static Transition GRANDREEF_CRAGFIELD = new Transition(BiomeHandler.GRANDREEF, BiomeHandler.CRAGFIELD, new Vector3(-178, -140, -948), new Vector3(12, 60, 0), "A");
        static Transition GRANDREEF_KELPFOREST = new Transition(BiomeHandler.GRANDREEF, BiomeHandler.KELPFOREST, new Vector3(-261, -51, -654), new Vector3(3, 27, 0), "B");
        static Transition GRANDREEF_SPARSEREEF = new Transition(BiomeHandler.GRANDREEF, BiomeHandler.SPARSEREEF, new Vector3(-632, -170, -896), new Vector3(18, 338, 0), "C");
        static Transition GRANDREEF_SEATREADERSPATH = new Transition(BiomeHandler.GRANDREEF, BiomeHandler.SEATREADERPATH, new Vector3(-1221, -200, -907), new Vector3(4, 10, 0), "D");

        static Transition SPARSEREEF_GRASSYPLATEAUS = new Transition(BiomeHandler.SPARSEREEF, BiomeHandler.GRASSYPLATEAUS, new Vector3(-441, -50, -432), new Vector3(15, 35, 0), "A");
        static Transition SPARSEREEF_KELPFOREST = new Transition(BiomeHandler.SPARSEREEF, BiomeHandler.KELPFOREST, new Vector3(-346, -25, -566), new Vector3(10, 40, 0), "B");
        static Transition SPARSEREEF_GRANDREEF = new Transition(BiomeHandler.SPARSEREEF, BiomeHandler.GRANDREEF, new Vector3(-615, -210, -973), new Vector3(49, 205, 0), "C");
        static Transition SPARSEREEF_SEATREADERSPATH = new Transition(BiomeHandler.SPARSEREEF, BiomeHandler.SEATREADERPATH, new Vector3(-1087, -180, -733), new Vector3(348, 277, 0), "D");
        static Transition SPARSEREEF_BLOODKELPTRENCH = new Transition(BiomeHandler.SPARSEREEF, BiomeHandler.BLOODKELPTRENCH, new Vector3(-943, -214, -606), new Vector3(81, 326, 0), "E");

        static Transition SEATREADERSPATH_DUNES = new Transition(BiomeHandler.SEATREADERPATH, BiomeHandler.DUNES, new Vector3(-1532, -336, -58), new Vector3(359, 342, 0), "A");
        static Transition SEATREADERSPATH_BLOODKELPTRENCH = new Transition(BiomeHandler.SEATREADERPATH, BiomeHandler.BLOODKELPTRENCH, new Vector3(-1091, -239, -527), new Vector3(67, 70, 0), "B");
        static Transition SEATREADERSPATH_SPARSEREEF = new Transition(BiomeHandler.SEATREADERPATH, BiomeHandler.SPARSEREEF, new Vector3(-1024, -178, -706), new Vector3(43, 66, 0), "C");
        static Transition SEATREADERSPATH_GRANDREEF = new Transition(BiomeHandler.SEATREADERPATH, BiomeHandler.GRANDREEF, new Vector3(-1252, -255, -971), new Vector3(35, 199, 0), "D");

        static Transition BLOODKELPTRENCH_GRASSYPLATEAUS = new Transition(BiomeHandler.BLOODKELPTRENCH, BiomeHandler.GRASSYPLATEAUS, new Vector3(-730, -93, -239), new Vector3(358, 52, 0), "A");
        static Transition BLOODKELPTRENCH_SPARSEREEF = new Transition(BiomeHandler.BLOODKELPTRENCH, BiomeHandler.SPARSEREEF, new Vector3(-858, -178, -647), new Vector3(23, 129, 0), "B");
        static Transition BLOODKELPTRENCH_SEATREADERSPATH = new Transition(BiomeHandler.BLOODKELPTRENCH, BiomeHandler.SEATREADERPATH, new Vector3(-1120, -187, -577), new Vector3(0, 193, 0), "C");
        static Transition BLOODKELPTRENCH_DUNES = new Transition(BiomeHandler.BLOODKELPTRENCH, BiomeHandler.DUNES, new Vector3(-1250, -232, -280), new Vector3(1, 35, 0), "D");

        static Transition SAFESHALLOWS_GRASSYPLATEAUS = new Transition(BiomeHandler.SAFESHALLOWS, BiomeHandler.GRASSYPLATEAUS, new Vector3(172, -78, 178), new Vector3(24, 72, 0), "A");
        static Transition SAFESHALLOWS_KELPFOREST = new Transition(BiomeHandler.SAFESHALLOWS, BiomeHandler.KELPFOREST, new Vector3(-80, -12, -198), new Vector3(14, 166, 0), "B");
        static Transition SAFESHALLOWS_MUSHROOMFOREST = new Transition(BiomeHandler.SAFESHALLOWS, BiomeHandler.MUSHROOMFOREST, new Vector3(542, -90, 121), new Vector3(24, 18, 0), "C");
        static Transition SAFESHALLOWS_CRASHZONE = new Transition(BiomeHandler.SAFESHALLOWS, BiomeHandler.CRASHZONE, new Vector3(406, -9, -290), new Vector3(0, 110, 0), "D");

        static Transition GRANDREEF_FLOATINGISLAND = new Transition(BiomeHandler.GRANDREEF, BiomeHandler.FLOATINGISLAND, new Vector3(-729, 2, -1062), new Vector3(355, 324, 0), "E");
        static Transition SPARSEREEF_FLOATINGISLAND = new Transition(BiomeHandler.SPARSEREEF, BiomeHandler.FLOATINGISLAND, new Vector3(-765, 6, -950), new Vector3(336, 146, 0), "F");
        static Transition FLOATINGISLAND_GRANDREEF = new Transition(BiomeHandler.FLOATINGISLAND, BiomeHandler.GRANDREEF, new Vector3(-710, -84, -1075), new Vector3(87, 180, 0), "A");
        static Transition FLOATINGISLAND_SPARSEREEF = new Transition(BiomeHandler.FLOATINGISLAND, BiomeHandler.SPARSEREEF, new Vector3(-774, -80, -814), new Vector3(60, 350, 0), "B");

        static Transition JELLYSHROOMCAVE_GRASSYPLATEAUS = new Transition(BiomeHandler.JELLYSHROOMCAVE, BiomeHandler.GRASSYPLATEAUS, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(131, -95, -378), new SerialisableVector3(342, 100, 0))
        }, new String[] { "A" });
        static Transition GRASSYPLATEAUS_JELLYSHROOMCAVE = new Transition(BiomeHandler.GRASSYPLATEAUS, BiomeHandler.JELLYSHROOMCAVE, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(127, -187, -375), new SerialisableVector3(78, 282, 0))
        }, new String[] { "J" });

        static Transition BULBZONE_LOSTRIVER = new Transition(BiomeHandler.BULBZONE, BiomeHandler.LOSTRIVER, new Vector3(947, -550, 932), new Vector3(18, 272, 0), "D");
        static Transition BLOODKELP_LOSTRIVER = new Transition(BiomeHandler.BLOODKELP, BiomeHandler.LOSTRIVER, new Vector3(-771, -601, 965), new Vector3(2, 202, 0), "E");
        static Transition BLOODKELPTRENCH_LOSTRIVER = new Transition(BiomeHandler.BLOODKELPTRENCH, BiomeHandler.LOSTRIVER, new Vector3(-1097, -653, -496), new Vector3(32, 177, 0), "E");
        static Transition GRANDREEF_LOSTRIVER = new Transition(BiomeHandler.GRANDREEF, BiomeHandler.LOSTRIVER, new Vector3(-724, -656, -746), new Vector3(11, 325, 0), "F");

        static Transition LOSTRIVER_BULBZONE = new Transition(BiomeHandler.LOSTRIVER, BiomeHandler.BULBZONE, new Vector3(1029, -517, 927), new Vector3(317, 98, 0), "A");
        static Transition LOSTRIVER_BLOODKELP = new Transition(BiomeHandler.LOSTRIVER, BiomeHandler.BLOODKELP, new Vector3(-677, -545, 1170), new Vector3(328, 44, 0), "B");
        static Transition LOSTRIVER_BLOODKELPTRENCH = new Transition(BiomeHandler.LOSTRIVER, BiomeHandler.BLOODKELPTRENCH, new Vector3(-1150, -585, -429), new Vector3(353, 317, 0), "C");
        static Transition LOSTRIVER_GRANDREEF = new Transition(BiomeHandler.LOSTRIVER, BiomeHandler.GRANDREEF, new Vector3(-741, -613, -797), new Vector3(325, 175, 0), "D");

        static Transition INACTIVELAVAZONE_LOSTRIVER = new Transition(BiomeHandler.INACTIVELAVAZONE, BiomeHandler.LOSTRIVER, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(310, -878, 740), new SerialisableVector3(348, 23, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(-1303, -896, 438), new SerialisableVector3(0, 45, 0))
        }, new String[] { "A", "B" });
        static Transition INACTIVELAVAZONE_LAVALAKES = new Transition(BiomeHandler.INACTIVELAVAZONE, BiomeHandler.LAVALAKES, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(74, -1331, 340), new SerialisableVector3(57, 192, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(-207, -1360, 3), new SerialisableVector3(50, 60, 0))
        }, new String[] { "C", "D" });
        static Transition LOSTRIVER_INACTIVELAVAZONE = new Transition(BiomeHandler.LOSTRIVER, BiomeHandler.INACTIVELAVAZONE, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(296, -1031, 740), new SerialisableVector3(74, 205, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(-1185, -1026, 424), new SerialisableVector3(69, 83, 0))
        }, new String[] { "E", "F" });
        static Transition LAVALAKES_INACTIVELAVAZONE = new Transition(BiomeHandler.LAVALAKES, BiomeHandler.INACTIVELAVAZONE, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(58, -1227, 306), new SerialisableVector3(353, 204, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(-246, -1337, -72), new SerialisableVector3(356, 217, 0))
        }, new String[] { "A", "B" });

        static Transition MOUNTAINS_FLOATINGISLAND = new Transition(BiomeHandler.MOUNTAINS, BiomeHandler.FLOATINGISLAND, new Vector3(-662, 5, -1067), new Vector3(357, 178, 0), "F");
        static Transition FLOATINGISLAND_MOUNTAINS = new Transition(BiomeHandler.FLOATINGISLAND, BiomeHandler.MOUNTAINS, new Vector3(345, 63, 890), new Vector3(353, 349, 0), "C");

        static Transition DUNES_ALIENBASE = new Transition(BiomeHandler.DUNES, BiomeHandler.ALIENBASE, new Vector3(-1240, -397, 1096), new Vector3(6, 54, 0), "G");
        static Transition BLOODKELP_ALIENBASE = new Transition(BiomeHandler.BLOODKELP, BiomeHandler.ALIENBASE, new Vector3(-594, -552, 1480), new Vector3(21, 283, 0), "F");
        static Transition MUSHROOMFOREST_ALIENBASE = new Transition(BiomeHandler.MUSHROOMFOREST, BiomeHandler.ALIENBASE, new Vector3(-796, -228, 402), new Vector3(1, 104, 0), "J");
        static Transition BULBZONE_ALIENBASE = new Transition(BiomeHandler.BULBZONE, BiomeHandler.ALIENBASE, new Vector3(1372, -299, 741), new Vector3(360, 225, 0), "E");
        static Transition SPARSEREEF_ALIENBASE = new Transition(BiomeHandler.SPARSEREEF, BiomeHandler.ALIENBASE, new Vector3(-888, -305, -790), new Vector3(20, 174, 0), "G");
        static Transition CRAGFIELD_ALIENBASE = new Transition(BiomeHandler.CRAGFIELD, BiomeHandler.ALIENBASE, new Vector3(-80, -290, -1349), new Vector3(2, 354, 0), "E");
        static Transition MOUNTAINS_ALIENBASE = new Transition(BiomeHandler.MOUNTAINS, BiomeHandler.ALIENBASE, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(470, -110, 1233), new SerialisableVector3(357, 198, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(392, 6, 1113), new SerialisableVector3(360, 141, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(245, -1586, -310), new SerialisableVector3(8, 150, 0))
        }, new String[] { "G", "H", "I" });
        static Transition LOSTRIVER_ALIENBASE = new Transition(BiomeHandler.LOSTRIVER, BiomeHandler.ALIENBASE, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(-253, -793, 302), new SerialisableVector3(4, 183, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(-942, -621, 1034), new SerialisableVector3(357, 10, 0))
        }, new String[] { "G", "H" });
        static Transition INACTIVELAVAZONE_ALIENBASE = new Transition(BiomeHandler.INACTIVELAVAZONE, BiomeHandler.ALIENBASE, new Vector3(-36, -1205, 138), new Vector3(3, 260, 0), "E");
        static Transition LAVALAKES_ALIENBASE = new Transition(BiomeHandler.LAVALAKES, BiomeHandler.ALIENBASE, new Vector3(217, -1450, -261), new Vector3(357, 150, 0), "C");

        static Transition ALIENBASE_DUNES = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.DUNES, new Vector3(-1256, -398, 1098), new Vector3(346, 19, 0), "A");
        static Transition ALIENBASE_BLOODKELP = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.BLOODKELP, new Vector3(-562, -539, 1489), new Vector3(348, 16, 0), "B");
        static Transition ALIENBASE_MUSHROOMFOREST = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.MUSHROOMFOREST, new Vector3(-803, -224, 407), new Vector3(357, 268, 0), "C");
        static Transition ALIENBASE_BULBZONE = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.BULBZONE, new Vector3(1378, -292, 750), new Vector3(360, 25, 0), "D");
        static Transition ALIENBASE_SPARSEREEF = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.SPARSEREEF, new Vector3(-909, -298, -773), new Vector3(333, 315, 0), "E");
        static Transition ALIENBASE_CRAGFIELD = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.CRAGFIELD, new Vector3(-80, -282, -1359), new Vector3(31, 180, 0), "F");
        static Transition ALIENBASE_MOUNTAINS = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.MOUNTAINS, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(479, -116, 1270), new SerialisableVector3(53, 26, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(385, 6, 1117), new SerialisableVector3(350, 243, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(458, -165, 1363), new SerialisableVector3(348, 154, 0))
        }, new String[] { "I", "H", "G" });
        static Transition ALIENBASE_LOSTRIVER = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.LOSTRIVER, new KeyValuePair<SerialisableVector3, SerialisableVector3>[] {
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3 (-229, -791, 317), new SerialisableVector3(5, 70, 0)),
            new KeyValuePair<SerialisableVector3, SerialisableVector3>(new SerialisableVector3(-940, -614, 1009), new SerialisableVector3(355, 149, 0))
        }, new String[] { "J", "K" });
        static Transition ALIENBASE_INACTIVELAVAZONE = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.INACTIVELAVAZONE, new Vector3(-35, -1198, 151), new Vector3(359, 345, 0), "L");
        static Transition ALIENBASE_LAVALAKES = new Transition(BiomeHandler.ALIENBASE, BiomeHandler.LAVALAKES, new Vector3(211, -1444, -249), new Vector3(357, 334, 0), "M");


        public static Transition[] ALL_TRANSITIONS = new Transition[]
        {
            DUNES_BLOODKELP,
            DUNES_MUSHROOMFOREST,
            DUNES_KELPFOREST,
            DUNES_GRASSYPLATEAUS,
            DUNES_BLOODKELPTRENCH,
            DUNES_SEATREADERSPATH,
            BLOODKELP_DUNES,
            BLOODKELP_MUSHROOMFOREST,
            BLOODKELP_UNDERWATERISLANDS,
            BLOODKELP_MOUNTAINS,
            MOUNTAINS_BLOODKELP,
            MOUNTAINS_UNDERWATERISLANDS,
            MOUNTAINS_KELPFOREST,
            MOUNTAINS_MUSHROOMFOREST,
            MOUNTAINS_BULBZONE,
            UNDERWATERISLANDS_BLOODKELP,
            UNDERWATERISLANDS_MOUNTAINS,
            UNDERWATERISLANDS_KELPFOREST,
            UNDERWATERISLANDS_GRASSYPLATEAUS,
            UNDERWATERISLANDS_MUSHROOMFOREST,
            BULBZONE_MOUNTAINS,
            BULBZONE_MUSHROOMFOREST,
            BULBZONE_CRASHZONE,
            MUSHROOMFOREST_BLOODKELP,
            MUSHROOMFOREST_DUNES,
            MUSHROOMFOREST_KELPFOREST,
            MUSHROOMFOREST_GRASSYPLATEAUS,
            MUSHROOMFOREST_UNDERWATERISLANDS,
            MUSHROOMFOREST_MOUNTAINS,
            MUSHROOMFOREST_BULBZONE,
            MUSHROOMFOREST_CRASHZONE,
            MUSHROOMFOREST_SAFESHALLOWS,
            KELPFOREST_MOUNTAINS,
            KELPFOREST_MUSHROOMFOREST,
            KELPFOREST_GRASSYPLATEAUS,
            KELPFOREST_SAFESHALLOWS,
            KELPFOREST_UNDERWATERISLANDS,
            KELPFOREST_CRASHZONE,
            KELPFOREST_CRAGFIELD,
            KELPFOREST_GRANDREEF,
            KELPFOREST_SPARSEREEF,
            KELPFOREST_DUNES,
            GRASSYPLATEAUS_UNDERWATERISLANDS,
            GRASSYPLATEAUS_MUSHROOMFOREST,
            GRASSYPLATEAUS_KELPFOREST,
            GRASSYPLATEAUS_SAFESHALLOWS,
            GRASSYPLATEAUS_CRAGFIELD,
            GRASSYPLATEAUS_CRASHZONE,
            GRASSYPLATEAUS_DUNES,
            GRASSYPLATEAUS_BLOODKELPTRENCH,
            GRASSYPLATEAUS_SPARSEREEF,
            CRASHZONE_BULBZONE,
            CRASHZONE_MUSHROOMFOREST,
            CRASHZONE_SAFESHALLOWS,
            CRASHZONE_KELPFOREST,
            CRASHZONE_GRASSYPLATEAUS,
            CRASHZONE_CRAGFIELD,
            CRAGFIELD_CRASHZONE,
            CRAGFIELD_GRASSYPLATEAUS,
            CRAGFIELD_KELPFOREST,
            CRAGFIELD_GRANDREEF,
            GRANDREEF_CRAGFIELD,
            GRANDREEF_KELPFOREST,
            GRANDREEF_SPARSEREEF,
            GRANDREEF_SEATREADERSPATH,
            SPARSEREEF_GRASSYPLATEAUS,
            SPARSEREEF_KELPFOREST,
            SPARSEREEF_GRANDREEF,
            SPARSEREEF_SEATREADERSPATH,
            SPARSEREEF_BLOODKELPTRENCH,
            SEATREADERSPATH_DUNES,
            SEATREADERSPATH_BLOODKELPTRENCH,
            SEATREADERSPATH_SPARSEREEF,
            SEATREADERSPATH_GRANDREEF,
            BLOODKELPTRENCH_GRASSYPLATEAUS,
            BLOODKELPTRENCH_SPARSEREEF,
            BLOODKELPTRENCH_SEATREADERSPATH,
            BLOODKELPTRENCH_DUNES,
            SAFESHALLOWS_GRASSYPLATEAUS,
            SAFESHALLOWS_KELPFOREST,
            SAFESHALLOWS_MUSHROOMFOREST,
            SAFESHALLOWS_CRASHZONE,
            GRANDREEF_FLOATINGISLAND,
            SPARSEREEF_FLOATINGISLAND,
            FLOATINGISLAND_GRANDREEF,
            FLOATINGISLAND_SPARSEREEF,
            JELLYSHROOMCAVE_GRASSYPLATEAUS,
            GRASSYPLATEAUS_JELLYSHROOMCAVE,
            BULBZONE_LOSTRIVER,
            BLOODKELP_LOSTRIVER,
            BLOODKELPTRENCH_LOSTRIVER,
            GRANDREEF_LOSTRIVER,
            LOSTRIVER_BULBZONE,
            LOSTRIVER_BLOODKELP,
            LOSTRIVER_BLOODKELPTRENCH,
            LOSTRIVER_GRANDREEF,
            INACTIVELAVAZONE_LOSTRIVER,
            INACTIVELAVAZONE_LAVALAKES,
            LOSTRIVER_INACTIVELAVAZONE,
            LAVALAKES_INACTIVELAVAZONE,
            MOUNTAINS_FLOATINGISLAND,
            FLOATINGISLAND_MOUNTAINS,
            DUNES_ALIENBASE,
            BLOODKELP_ALIENBASE,
            MUSHROOMFOREST_ALIENBASE,
            BULBZONE_ALIENBASE,
            SPARSEREEF_ALIENBASE,
            CRAGFIELD_ALIENBASE,
            MOUNTAINS_ALIENBASE,
            LOSTRIVER_ALIENBASE,
            INACTIVELAVAZONE_ALIENBASE,
            LAVALAKES_ALIENBASE,
            ALIENBASE_DUNES,
            ALIENBASE_BLOODKELP,
            ALIENBASE_MUSHROOMFOREST,
            ALIENBASE_BULBZONE,
            ALIENBASE_SPARSEREEF,
            ALIENBASE_CRAGFIELD,
            ALIENBASE_MOUNTAINS,
            ALIENBASE_LOSTRIVER,
            ALIENBASE_INACTIVELAVAZONE,
            ALIENBASE_LAVALAKES
        };

        private static Dictionary<Transition, TeleportLocation[]> transitionMap;

        private static Biome CurrentBiome = BiomeHandler.SAFESHALLOWS;
        private static Biome PreviousBiome = CurrentBiome;

        private static TeleportLocation UndoLocation;

        private static Boolean initialised = false;

        internal static void SetCurrentBiome(Biome Biome, Boolean dontUpdatePreviousBiome = false)
        {
            if (CurrentBiome.GetName() != Biome.GetName() && !dontUpdatePreviousBiome)
            {
                PreviousBiome = CurrentBiome;
            }
            CurrentBiome = Biome;
        }

        internal static void ResetCurrentBiome()
        {
            CurrentBiome = BiomeHandler.SAFESHALLOWS;
            PreviousBiome = CurrentBiome;
        }

        internal static Biome GetPreviousBiome()
        {
            return PreviousBiome;
        }

        internal static Biome GetCurrentBiome()
        {
            return CurrentBiome;
        }

        public static Boolean IsInitialised()
        {
            return initialised;
        }

        internal static TeleportLocation GetUndoLocation()
        {
            return UndoLocation;
        }

        internal static void ClearUndoLocation()
        {
            UndoLocation = null;
        }

        internal static string ToBase64String()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, transitionMap);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        internal static void FromBase64String(String base64string)
        {
            byte[] bytes = Convert.FromBase64String(base64string);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                transitionMap = (Dictionary<Transition, TeleportLocation[]>)(new BinaryFormatter().Deserialize(ms));
                initialised = true;
            }
        }

        internal static Dictionary<Transition, TeleportLocation[]> GetTransitionsFromBiome(Biome biome)
        {
            Dictionary<Transition, TeleportLocation[]> result = new Dictionary<Transition, TeleportLocation[]>();

            foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
            {
                if (pair.Key.GetFrom().GetName() == biome.GetName())
                {
                    result.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }

        internal static Dictionary<Transition, TeleportLocation[]> GetTransitionsToBiome(Biome biome)
        {
            Dictionary<Transition, TeleportLocation[]> result = new Dictionary<Transition, TeleportLocation[]>();

            foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
            {
                if (pair.Key.GetTo().GetName() == biome.GetName())
                {
                    result.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }

        internal static void GenerateRandomTransitionMap()
        {
            // This is horrible code but it works :)
            List<TeleportLocation> allLocations = new List<TeleportLocation>();
            for (int i = 0; i < ALL_TRANSITIONS.Length; i++)
            {
                Transition transition = ALL_TRANSITIONS[i];
                for (int j = 0; j < transition.GetPlayerStates().Length; j++)
                {
                    allLocations.Add(new TeleportLocation(transition.GetTo(), transition.GetPlayerStates()[j].Key, transition.GetPlayerStates()[j].Value, transition.GetFrom(), false, transition.getMarkers()[j]));
                }
            }

            transitionMap = new Dictionary<Transition, TeleportLocation[]>();

            List<KeyValuePair<int, int>> ignoredIndices = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < ALL_TRANSITIONS.Length; i++)
            {
                Transition transition = ALL_TRANSITIONS[i];

                TeleportLocation[] locations = new TeleportLocation[transition.GetPlayerStates().Length];
                Boolean existsInMap = false;
                foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
                {
                    if (pair.Key.GetFrom().GetName() == transition.GetFrom().GetName() && pair.Key.GetTo().GetName() == transition.GetTo().GetName())
                    {
                        existsInMap = true;
                        locations = pair.Value;
                        break;
                    }
                }

                for (int j = 0; j < locations.Length; j++)
                {
                    if (ignoredIndices.Contains(new KeyValuePair<int, int>(i, j))) continue;

                    int random;
                    int attempts = 0;
                    do
                    {
                        random = Random.Range(0, allLocations.Count - 1);
                        attempts++;
                    } while (allLocations[random].GetBiome().GetName() == transition.GetFrom().GetName() && attempts < 50);

                    locations[j] = allLocations[random];
                    allLocations.RemoveAt(random);

                    // Make the transition work the other way round
                    Transition opposite = FindTransition(locations[j].GetBiome(), locations[j].GetOrigin());
                    int oppositeLocationIndex = -1;

                    for (int k = 0; k < allLocations.Count; k++)
                    {
                        if (allLocations[k].GetOrigin().GetName() == transition.GetTo().GetName() && allLocations[k].GetBiome().GetName() == transition.GetFrom().GetName())
                        {
                            oppositeLocationIndex = k;
                            break;
                        }
                    }
                    if (oppositeLocationIndex == -1)
                    {
                        //Console.WriteLine("0 " + transition.GetFrom().GetName() + " " + transition.GetTo().GetName() + " " + j);
                        continue;
                    }
                    //Console.WriteLine("1 " + oppositeLocationIndex + " " + transition.GetFrom().GetName() + " " + transition.GetTo().GetName());
                    TeleportLocation oppositeLocation = allLocations[oppositeLocationIndex];

                    int oppositeIndex = -1;
                    for (int b = 0; b < ALL_TRANSITIONS.Length; b++)
                    {
                        if (ALL_TRANSITIONS[b].GetFrom().GetName() == locations[j].GetBiome().GetName() && ALL_TRANSITIONS[b].GetTo().GetName() == locations[j].GetOrigin().GetName())
                        {
                            oppositeIndex = b;
                            break;
                        }
                    }
                    //Console.WriteLine("2 " + oppositeIndex);

                    int reverseOppositeIndex = -1;
                    for (int b = 0; b < ALL_TRANSITIONS.Length; b++)
                    {
                        if (ALL_TRANSITIONS[b].GetFrom().GetName() == locations[j].GetOrigin().GetName() && ALL_TRANSITIONS[b].GetTo().GetName() == locations[j].GetBiome().GetName())
                        {
                            reverseOppositeIndex = b;
                            break;
                        }
                    }
                    //Console.WriteLine("3 " + reverseOppositeIndex);

                    // Find opposite transition in the map
                    Boolean exists = false;
                    foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
                    {
                        if (pair.Key.GetFrom().GetName() == opposite.GetFrom().GetName() && pair.Key.GetTo().GetName() == opposite.GetTo().GetName())
                        {
                            exists = true;

                            int originalIndex = -1;
                            for (int a = 0; a < ALL_TRANSITIONS[reverseOppositeIndex].GetPlayerStates().Length; a++)
                            {
                                if (ALL_TRANSITIONS[reverseOppositeIndex].GetPlayerStates()[a].Key == locations[j].GetPosition())
                                {
                                    originalIndex = a;
                                    break;
                                }
                            }
                            //Console.WriteLine("4 " + originalIndex + " " + pair.Value.Length);

                            pair.Value[originalIndex] = oppositeLocation;
                            allLocations.RemoveAt(oppositeLocationIndex);

                            ignoredIndices.Add(new KeyValuePair<int, int>(oppositeIndex, originalIndex));
                            break;
                        }
                    }

                    if (!exists)
                    {
                        TeleportLocation[] oppositeLocations = new TeleportLocation[ALL_TRANSITIONS[oppositeIndex].GetPlayerStates().Length];
                        int originalIndex = -1;
                        for (int a = 0; a < ALL_TRANSITIONS[reverseOppositeIndex].GetPlayerStates().Length; a++)
                        {
                            if (ALL_TRANSITIONS[reverseOppositeIndex].GetPlayerStates()[a].Key == locations[j].GetPosition())
                            {
                                originalIndex = a;
                                break;
                            }
                        }
                        //Console.WriteLine("5 " + originalIndex + " " + oppositeLocations.Length);
                        oppositeLocations[originalIndex] = oppositeLocation;
                        allLocations.RemoveAt(oppositeLocationIndex);
                        ignoredIndices.Add(new KeyValuePair<int, int>(oppositeIndex, originalIndex));
                        transitionMap.Add(ALL_TRANSITIONS[oppositeIndex], oppositeLocations);
                    }

                }
                if (!existsInMap)
                {
                    transitionMap.Add(transition, locations);
                }
            }

            foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
            {
                Transition t = FindTransition(pair.Key.GetFrom(), pair.Key.GetTo());
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    pair.Value[i].SetMarker(t.getMarkers()[i]);
                }
            }

            if (CheckForSoftlock())
            {
                GenerateRandomTransitionMap();
                return;
            }

            WriteTransitionLog();
            initialised = true;
        }

        internal static Boolean CheckForSoftlock()
        {
            List<TeleportLocation> notVisited = new List<TeleportLocation>();
            List<TeleportLocation> stack = new List<TeleportLocation>();
            foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
            {
                Transition transition = pair.Key;
                for (int j = 0; j < pair.Value.Length; j++)
                {
                    if (transition.GetFrom().GetName() == BiomeHandler.SAFESHALLOWS.GetName())
                    {
                        stack.Add(pair.Value[j]);
                    }
                    notVisited.Add(pair.Value[j]);
                }
            }

            while (stack.Count > 0)
            {
                if (notVisited.Count == 0) break;

                TeleportLocation l = stack[0];
                stack.RemoveAt(0);

                for (int i = 0; i < notVisited.Count; i++)
                {
                    TeleportLocation other = notVisited[i];

                    if (l.GetBiome().GetName() == BiomeHandler.ALIENBASE.GetName())
                    {
                        notVisited.RemoveAt(i);
                        // Don't assume that the alien bases are all interconnected
                        break;
                    }

                    if (other.GetBiome().GetName() == l.GetBiome().GetName() && other.GetOrigin().GetName() == l.GetOrigin().GetName() && other.GetPosition().ToVector3() == l.GetPosition().ToVector3())
                    {
                        // Not visited yet, add all outgoing transitions
                        foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
                        {
                            Transition transition = pair.Key;
                            if (transition.GetFrom().GetName() == l.GetBiome().GetName())
                            {
                                for (int j = 0; j < pair.Value.Length; j++)
                                {
                                    // Add all neighbours
                                    stack.Add(pair.Value[j]);
                                }
                            }
                        }

                        notVisited.RemoveAt(i);
                        break;
                    }
                }
            }

            Console.WriteLine("SOFTLOCK DETECTION: " + notVisited.Count + " / " + stack.Count);

            return notVisited.Count > 0;
        }

        internal static void WriteTransitionLog()
        {
            Console.WriteLine("TRANSITION LOG");
            foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
            {
                Console.WriteLine(pair.Key.GetFrom().GetName() + " -> " + pair.Key.GetTo().GetName());
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    Console.WriteLine(pair.Value[i].GetBiome().GetName() + " (from " + pair.Value[i].GetOrigin().GetName() + ")");
                }
            }
        }

        internal static Transition FindTransition(Biome from, Biome to)
        {
            Transition transition = null;
            for (int i = 0; i < ALL_TRANSITIONS.Length; i++)
            {
                if (ALL_TRANSITIONS[i].GetFrom().GetName() == from.GetName() && ALL_TRANSITIONS[i].GetTo().GetName() == to.GetName())
                {
                    transition = ALL_TRANSITIONS[i];
                    break;
                }
            }
            return transition;
        }

        internal static TeleportLocation GetTeleportLocationOfTransition(Transition transition, int index)
        {
            foreach (KeyValuePair<Transition, TeleportLocation[]> pair in transitionMap)
            {
                if (pair.Key.GetFrom().GetName() == transition.GetFrom().GetName() && pair.Key.GetTo().GetName() == transition.GetTo().GetName())
                {
                    return pair.Value[index];
                }
            }
            return null;
        }

        internal static TeleportLocation getTeleportPositionForBiomeTransfer(Biome newBiome, Vector3 playerPosition, Biome currentBiome = null, Boolean updateUndoLocation = true)
        {
            if (currentBiome == null)
            {
                currentBiome = CurrentBiome;
            }

            Transition transition = FindTransition(currentBiome, newBiome);
            Transition reverse = FindTransition(newBiome, currentBiome);

            if (transition == null || reverse == null) return null;

            int closestLocationToPlayerIndex = -1;
            double closestDistance = Double.MaxValue;
            for (int i = 0; i < reverse.GetPlayerStates().Length; i++)
            {
                double distance = Vector3.Distance(playerPosition, reverse.GetPlayerStates()[i].Key.ToVector3());
                if (closestDistance > distance)
                {
                    closestDistance = distance;
                    closestLocationToPlayerIndex = i;
                }
            }

            TeleportLocation result = GetTeleportLocationOfTransition(transition, closestLocationToPlayerIndex);
            if (currentBiome.GetName() == CurrentBiome.GetName() && updateUndoLocation)
            {
                UndoLocation = getTeleportPositionForBiomeTransfer(result.GetOrigin(), result.GetPosition().ToVector3(), result.GetBiome());
            }
            return result;
        }

    }
}
