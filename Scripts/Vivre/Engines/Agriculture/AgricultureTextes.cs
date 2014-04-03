using System;

namespace Server.Items.Crops
{
    public class AgriTxt
    {
        // Misc
        public static String Seed = "Graine";
        public static String Seedling = "Pousse";
        public static String Root = "Racine";

        // Crops
        public static String CannotWorkMounted = "Vous ne pouvez planter en étant sur votre monture.";
        public static String CannotGrowHere = "Cette graine ne germera pas ici.";
        public static String AlreadyCrop = "Il y a deja une graine de plantée ici.";
        public static String TooMuchCrops = "Il y a trop de pousses ici.";
        public static String CropPlanted = "Vous plantez la graine.";
        public static String PickCrop = "Vous arrachez la pousse.";
        public static String TooYoungCrop = "La pousse est trop jeune pour être arrachée.";
        public static String WitherCrop = "La plante se flétrit.";
        public static String DunnoHowTo = "Vous ne savez pas comment cultiver cela.";
        public static String NoCrop = "Il n y a rien à cultiver ici.";
        public static String ZeroPicked = "Vous n'obtenez pas de récolte.";
        public static String YouPick = "Vous récoltez"; // x plante(s)
        public static String TooFar = "Vous êtes trop loin pour cultiver.";

        // Roots
        public static String PullRoot = "Vous tirez sur la plante par la racine.";
        public static String HardPull = "La plante est dure à arracher.";
        public static String NoRoot = "Vous ne trouvez pas de racines utilisables.";
        public static String YouCut = "Vous coupez";
        
        // Tree
        public static String LogsAndFruits = "Vous mettez quelques bûches et des fruits dans votre sac.";
    }
}