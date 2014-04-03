using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Targets;
using System.Text;
using System.IO;

/* Distillerie façon Vivre , Turanar , elle se comporte d'un element principale et de quelques gadget roleplay 
 * 1 tonneau qui fait le distilleur * j'ai un tiledata pour un vrai mais on ne touche pas au client pour le moment*
 * 1 couvercle purement RP
 * 2 target 1 pour remplir 'ingrédient , un autre pour prelever le produit finni
 * la chose amusante je l'ai fait avec des boléen , il m'a fallu 4 bool la ou 1 int aurais suffit avec la methode C++
 * on peut remplir n'importe quel basebeverage a partir du tonneau 
 * la skill cooking determine le seuil de hazard de la reussite , un elder ne rate presque jamais
 * on peut etiquetter ses boissons , un autre keg sera copier pour des spiritueux.
*/


namespace Server.Items
{
    public class FermentationBarrel : Item
    {
        public int m_Quantity;
        public int m_QuantityMax;
        public int m_IdIngredient;
        public int m_FromBonusSkill;
        private int m_StadeFermentation;
        public bool m_FermentationEnCours = false;
        public bool m_FermentationSuccess = true;
        public bool m_FermentationDone = false;
        public bool m_FermentationTimerRun = false;

        [CommandProperty(AccessLevel.GameMaster)]
        public int quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int quantitymax
        {
            get { return m_QuantityMax; }
            set { m_QuantityMax = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int idingredient
        {
            get { return m_IdIngredient; }
            set { m_IdIngredient = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int FromBonusSkill
        {
            get { return m_FromBonusSkill; }
            set { m_FromBonusSkill = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int stadeFermentation
        {
            get { return m_StadeFermentation; }
            set { m_StadeFermentation = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool fermentationencours
        {
            get { return m_FermentationEnCours; }
            set { m_FermentationEnCours = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool fermentationsuccess
        {
            get { return m_FermentationSuccess; }
            set { m_FermentationSuccess = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool fermentationdone
        {
            get { return m_FermentationDone; }
            set { m_FermentationDone = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool FermentationtimerRun
        {
            get { return m_FermentationTimerRun; }
            set { m_FermentationTimerRun = value; InvalidateProperties(); }
        }



        [Constructable]
        public FermentationBarrel()
            : base(0xE77)
        {

            Weight = 15.0;
            quantity = 0;
            quantitymax = 50;
            Name = "Barrille de Distillation";
        }

        public FermentationBarrel(Serial serial)
            : base(serial)
        {
        }

        public virtual void CheckQuantity()
        {
            InvalidateProperties();
            if (quantity > 1)
            {
                Movable = true;//Movable = false;
            }
            else
            {
                Movable = true;
            }
            Weight = quantity + 15;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(GetQuantityDescription());
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            LabelTo(from, GetQuantityDescription());
        }

        public virtual string GetQuantityDescription()
        {
            if (quantity <= 0)
                return "C'est vide";
            else if (quantity <= 25)
                return "c'est à moitier Vide.";
            else if (quantity <= 40)
                return "c'est presque plein.";
            else
                return "c'est plein.";
        }

        public override void OnDoubleClick(Mobile from)/*me gave ce truc*/
        {
            m_FromBonusSkill = (10 + ((int)(from.Skills[SkillName.Cooking].Value)));
            CheckQuantity();
            if ((fermentationencours == false) && (fermentationdone == false))
            {
                if ((quantity == 0) && (fermentationencours == false))
                {
                    from.SendMessage(0x96D, "Quel fruit voulez-vous utiliser ?"); // on lui dit qu'il doit choisir un ingredient
                    from.Target = new InternalInTarget(from, this); /* on appelle le target pour remplir de fruit*/
                }
                else if ((quantity > 0) && (fermentationencours == false))
                {
                    from.SendMessage(0x96D, "Voulez-vous ajoutez un ingrédient de même type ou fermer le tonneau ?"); // on lui dit qu'il doit choisir un ingredient
                    from.Target = new InternalInTarget(from, this); /* on appelle le target pour remplir de fruit ou refermer*/
                }
            }
            else
            {
                if ((m_FermentationTimerRun == false) && (fermentationdone == false))// faut tester si le timer existe pas 
                {
                    new timerDistillerie(this).Start();
                    m_FermentationTimerRun = true;
                }
                else if ((quantity > 0) && (fermentationencours == false) && (fermentationdone == true))
                {
                    from.SendMessage(0x96D, "Quel recipient vide voulez-vous remplir ?");
                    from.Target = new InternalOutTarget(from, this);/* on appelle le target pour remplir les bouteilles*/
                }
                else
                {
                    from.PlaySound(0X021);
                    from.SendMessage(0x96D, "c'est pas en le secouant que ça va allez plus vite ^^");
                }
            }


        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((int)m_Quantity);
            writer.Write((int)m_QuantityMax);
            writer.Write((int)m_IdIngredient);
            writer.Write((int)m_FromBonusSkill);
            writer.Write((bool)m_FermentationEnCours);
            writer.Write((bool)m_FermentationSuccess);
            writer.Write((bool)m_FermentationDone);
            writer.Write((bool)m_FermentationTimerRun);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);


            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_Quantity = reader.ReadInt();
                        m_QuantityMax = reader.ReadInt();
                        m_IdIngredient = reader.ReadInt();
                        m_FromBonusSkill = reader.ReadInt();
                        m_FermentationEnCours = reader.ReadBool();
                        m_FermentationSuccess = reader.ReadBool();
                        m_FermentationDone = reader.ReadBool();
                        m_FermentationTimerRun = reader.ReadBool();
                        break;
                    }
            }
        }

        /*-------------le target pour ajouter des ingrédients -----------------------------------*/

        private class InternalInTarget : Target
        {
            Mobile from;
            FermentationBarrel barrel;
            int Quantity, QuantityMax, IdIngredient, idFruit;
            bool FermentationEnCours;

            /*this is my problem's how to give the items properties to the target ?*/
            /*reponse faut tous refaire ou presque ,c'est simple il parait....    ?*/

            public InternalInTarget(Mobile from, FermentationBarrel t)
                : base(10, false, TargetFlags.None)
            {
                this.from = from;
                this.barrel = t;
                Quantity = t.quantity;
                QuantityMax = t.quantitymax;
                IdIngredient = t.idingredient;
                FermentationEnCours = t.fermentationencours;

            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (!(o is Item))
                {
                    from.SendMessage("ciblez un objet !");
                    return;
                }

                int idFruit = ((Item)o).Amount; /*entrez la donnee d'un target apres l'avoir verifier ((Item)o.ce que je veux)  ID du tiledata pour une pomme item.id , peach:0x9D2 , raisin: 0x9D1*/
                ArrayList packitems = new ArrayList(from.Backpack.Items);

                if (packitems.Contains(o))
                {
                    if (Quantity <= QuantityMax)
                    {
                        if ((o is Apple) && ((IdIngredient == 0) || (IdIngredient == 1)))
                        {
                            barrel.idingredient = 1;
                            from.Backpack.ConsumeTotal(typeof(Apple), idFruit);
                            barrel.quantity += idFruit;
                        }
                        else if ((o is Grapes) && ((IdIngredient == 0) || (IdIngredient == 2)))
                        {
                            barrel.idingredient = 2;
                            from.Backpack.ConsumeTotal(typeof(Grapes), idFruit);
                            barrel.quantity += idFruit;
                        }
                        else if ((o is Peach) && ((IdIngredient == 0) || (IdIngredient == 3)))
                        {
                            barrel.idingredient = 1;
                            from.Backpack.ConsumeTotal(typeof(Peach), idFruit);
                            barrel.quantity += idFruit;
                        }
                        else if (o is BarrelLid)
                        {
                            from.SendMessage(0x96D, "vous refermez le tonneau !");
                            barrel.ItemID = 0x0FAE; /*et de 2 */
                            barrel.fermentationencours = true;
                            from.Backpack.ConsumeTotal(typeof(BarrelLid), 1);
                            from.SendMessage(0x96D, "une fois fermer un simple dclick sur le tonneau fera demarrer la fermentation!");
                        }
                        else
                        {

                            from.SendMessage(0x96D, "il n'y a pas de recettes disponibles pour cet ingrédients");
                            return;
                        }
                    }
                    else
                    {
                        from.SendMessage(0x96D, "Le tonneau est plein a ras bord ");
                        barrel.quantity = barrel.quantitymax;
                        return;
                    }
                }
                else
                    from.SendMessage(0x96D, "L'ingredient doit être dans votre inventaire!");
                return;
            }
        }

        /* ------------------le target pour remplir avec le produit finni--------------------------- */

        private class InternalOutTarget : Target
        {
            Mobile from;
            FermentationBarrel barrel;
            int Quantity, QuantityMax, IdIngredient, Fruit;
            bool FermentationEnCours, FermentationSuccess, FermentationDone;

            public InternalOutTarget(Mobile from, FermentationBarrel t)
                : base(5, false, TargetFlags.None)
            {
                this.from = from;
                this.barrel = t;
                Quantity = t.quantity;
                QuantityMax = t.quantitymax;
                IdIngredient = t.idingredient;
                FermentationEnCours = t.fermentationencours;
                FermentationSuccess = t.fermentationsuccess;
                FermentationDone = t.fermentationdone;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                ArrayList packitems = new ArrayList(from.Backpack.Items);
                if (packitems.Contains(o))
                    if (o is BaseBeverage)
                    {
                        BaseBeverage p = (BaseBeverage)o;

                        if ((barrel.quantity >= p.Quantity) && (p.Quantity == 0))
                        {
                            if ((barrel.idingredient == 3) && (barrel.fermentationsuccess == true))
                            {
                                p.Content = BeverageType.Liquor;
                                p.Quantity = p.MaxQuantity;
                                barrel.quantity = barrel.quantity - p.MaxQuantity;
                                from.SendMessage(0x96D, "Vous remplissez le recipient avec de la liqueur.");
                                barrel.Name = "Barille: " + barrel.quantity.ToString() + "/50 Litres de jus de peche.";
                                from.PlaySound(0X240);

                            }
                            else if ((barrel.idingredient == 3) && (barrel.fermentationsuccess == false))
                            {
                                p.Content = BeverageType.JusPeche;
                                p.Quantity = p.MaxQuantity;
                                barrel.quantity = barrel.quantity - p.MaxQuantity;
                                from.SendMessage(0x96D, "Vous remplissez le recipient avec du jus de peche.");
                                barrel.Name = "Barille: " + barrel.quantity.ToString() + "/50 Litres de jus de peche.";
                                from.PlaySound(0X240);

                            }
                            else if ((barrel.idingredient == 2) && (barrel.fermentationsuccess == true))
                            {
                                p.Content = BeverageType.Wine;
                                p.Quantity = p.MaxQuantity;
                                barrel.quantity = barrel.quantity - p.MaxQuantity;
                                from.SendMessage(0x96D, "Vous remplissez le recipient avec du vin.");
                                from.PlaySound(0X240);
                                barrel.Name = "Barille: " + barrel.quantity.ToString() + "/50 Litres de vin.";
                            }
                            else if ((barrel.idingredient == 2) && (barrel.fermentationsuccess == false))
                            {
                                p.Content = BeverageType.JusRaisin;
                                p.Quantity = p.MaxQuantity;
                                barrel.quantity = barrel.quantity - p.MaxQuantity;
                                from.SendMessage(0x96D, "Vous remplissez le recipient avec de jus de raisin.");
                                from.PlaySound(0X240);
                                barrel.Name = "barille: " + barrel.quantity.ToString() + "/50 Litres de jus de raisin.";
                            }
                            else if ((barrel.idingredient == 1) && (barrel.fermentationsuccess == true))
                            {
                                p.Content = BeverageType.Cider;
                                p.Quantity = p.MaxQuantity;
                                barrel.quantity = barrel.quantity - p.MaxQuantity;
                                from.SendMessage(0x96D, "Vous remplissez le recipient avec du cidre.");
                                barrel.Name = "Barille: " + barrel.quantity.ToString() + "/50 Litres de cidre.";
                                from.PlaySound(0X240);
                            }
                            else if ((barrel.idingredient == 1) && (barrel.fermentationsuccess == false))
                            {
                                p.Content = BeverageType.JusPomme;
                                p.Quantity = p.MaxQuantity;
                                barrel.quantity = barrel.quantity - p.MaxQuantity;
                                from.SendMessage(0x96D, "Vous remplissez le recipient avec du jus de pomme.");
                                barrel.Name = "Barille: " + barrel.quantity.ToString() + "/50 Litres de jus de pomme.";
                                from.PlaySound(0X240);
                            }
                            else
                            {
                                from.SendMessage(0x84B, "il y a un problemes de Quantité");

                            }

                        }
                        else
                        {
                            from.SendMessage(0x84B, "Utilisez un récipient vide de préférence !");

                        }

                        if (barrel.quantity <= 0)
                        {
                            barrel.quantity = 0;
                            barrel.idingredient = 0;
                            barrel.Name = "Barrille de Distillation : (Vide)";
                            barrel.fermentationencours = false;
                            barrel.fermentationdone = false;
                        }

                    }
                    else
                        from.SendMessage(0x96D, "L'objet doit être dans votre inventaire!");
            }
        }
        /*---------------------------------------------------------------------------------------------------------*/
    }
}