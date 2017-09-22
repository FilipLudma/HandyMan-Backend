using WebAPI.Models;
using System;
using System.Linq;

namespace WebAPI.Schema.Context
{
    public static class DbInitializer
    {
        public static void Initialize(HandyManContext context)
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EmployeeDBContext>());    
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any orders and order category.
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }

            var subCategories1 = new OrderSubCategory[]
            {
                new OrderSubCategory{ SubCategoryName="Drobné opravy v domácnosti"},
                new OrderSubCategory{ SubCategoryName="Oprava a výmena vodovodných batérií"},
                new OrderSubCategory{ SubCategoryName="Oprava  výmena odtokov"},
                new OrderSubCategory{ SubCategoryName="Zapojenie spotrebičov"},
                new OrderSubCategory{ SubCategoryName="Výmena spotrebičov"},
                new OrderSubCategory{ SubCategoryName="Oprava a výmena zásuviek"},
                new OrderSubCategory{ SubCategoryName="Oprava a výmena vypínačov"},
                new OrderSubCategory{ SubCategoryName="Nastavovanie dverí"},
                new OrderSubCategory{ SubCategoryName="Zavesenie obrazov, televízie"},
                new OrderSubCategory{ SubCategoryName="Vyvŕtanie dier"},
                new OrderSubCategory{ SubCategoryName="Parketovanie"},
                new OrderSubCategory{ SubCategoryName="Nastavovanie okien"}          
            };

            var subCategories2 = new OrderSubCategory[]
            {
                new OrderSubCategory{ SubCategoryName="Drobné opravy nábytku"},
                new OrderSubCategory{ SubCategoryName="Skladanie nového nábytku"},
                new OrderSubCategory{ SubCategoryName="Premiestňovanie nábytku"},
                new OrderSubCategory{ SubCategoryName="Brúsenie drevených častí nábytku"},
                new OrderSubCategory{ SubCategoryName="Lakovanie nábytku"},
                new OrderSubCategory{ SubCategoryName="Zavesenie nábytku"},
                new OrderSubCategory{ SubCategoryName="Montáž garniží"},
                new OrderSubCategory{ SubCategoryName="Nalepenie kuchynskej zásteny"},
                new OrderSubCategory{ SubCategoryName="Výmena kľučiek"},
                new OrderSubCategory{ SubCategoryName="Výmena pracovnej dosky v kuchyni"} 
            };

            var subCategories3 = new OrderSubCategory[]
            {
                new OrderSubCategory{ SubCategoryName="Strihanie živých plotov"},
                new OrderSubCategory{ SubCategoryName="Kosenie"},
                new OrderSubCategory{ SubCategoryName="Iné záhradné práce"}
            };

            var subCategories4 = new OrderSubCategory[]
            {
                new OrderSubCategory{ SubCategoryName="Prekáblovanie internetu"},
                new OrderSubCategory{ SubCategoryName="Nastavenie routrov"},
                new OrderSubCategory{ SubCategoryName="Nastavenie tlačiarní"},
                new OrderSubCategory{ SubCategoryName="Nastavenie audio techniky"},
                new OrderSubCategory{ SubCategoryName="Prepájanie bezdrôtových zariadení"},
                new OrderSubCategory{ SubCategoryName="Inteligentna domácnost"},
                new OrderSubCategory{ SubCategoryName="Štrukturovanie kabeláže"}
            };

            var subCategories5 = new OrderSubCategory[]
            {
                new OrderSubCategory{ SubCategoryName="Balenie"},
                new OrderSubCategory{ SubCategoryName="Vybaľovanie"},
                new OrderSubCategory{ SubCategoryName="Nakladanie a vykladanie auta"}
            };

            var subCategories6 = new OrderSubCategory[]
            {
                new OrderSubCategory{ SubCategoryName="Umývanie okien"},
                new OrderSubCategory{ SubCategoryName="Utieranie prachu"},
                new OrderSubCategory{ SubCategoryName="Čistenie podláh"},
                new OrderSubCategory{ SubCategoryName="Čistenie sociálnych zariadení"},
                new OrderSubCategory{ SubCategoryName="Tepovanie"},
                new OrderSubCategory{ SubCategoryName="Vypratanie odpadu"},
            };

            var subCategories7 = new OrderSubCategory[]
            {
                new OrderSubCategory{ SubCategoryName="Doručovanie obsahu zásielok do 1m3"},
                new OrderSubCategory{ SubCategoryName="Nakupovanie"},
                new OrderSubCategory{ SubCategoryName="Vyzdvihnutie tovaru a vecí z čistiarne"},
                new OrderSubCategory{ SubCategoryName="Odvoz auta"},
                new OrderSubCategory{ SubCategoryName="Time management"},
                new OrderSubCategory{ SubCategoryName="Pomoc pri organizovaní drobných akcií"},
                new OrderSubCategory{ SubCategoryName="Vystátie rád (na koncert)"}
            };

            var categories = new OrderCategory[]
            {
                new OrderCategory { CategoryName="Opraváreň", CategoryType = 1, OrderSubCategories = subCategories1 },
                new OrderCategory { CategoryName="Nábytkáreň", CategoryType = 2, OrderSubCategories = subCategories2 },
                new OrderCategory { CategoryName="Záhradkáreň", CategoryType = 3, OrderSubCategories = subCategories3 },
                new OrderCategory { CategoryName="Bezdrotáreň", CategoryType = 4, OrderSubCategories = subCategories4 },
                new OrderCategory { CategoryName="Sťahováreň", CategoryType = 5, OrderSubCategories = subCategories5 },
                new OrderCategory { CategoryName="Čistiareň", CategoryType = 6, OrderSubCategories = subCategories6 },
                new OrderCategory { CategoryName="Pomôckáreň", CategoryType = 7, OrderSubCategories = subCategories7 }
            };

            foreach (OrderCategory category in categories)
            {
                context.OrderCategory.Add(category);
            }

            context.SaveChanges();
        }
    }
}