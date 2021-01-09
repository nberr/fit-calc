using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Calculator
{
    class ClientData
    {
        public String name ="";
        public String weight = "";
        public double weight_kg;
        public String height_ft = "";
        public String height_in = "";
        public double height_cm;
        public String age = "";
        public int age_num;
        public String sex = "Prefer not to say";

        public double weight_start;
        public double weight_end;
        public double weight_diff;

        public String weight_loss = "";
        public DateTime start_date;
        public DateTime end_date;

        public double PAM = 1.11; //1.11-3
                                 // very light   1.4 1.5 1.6 1.7 - 0.1
                                 // light        1.5 1.6 1.7 1.8 - 0.1
                                 // moderate     1.6 1.7 1.8 1.9 - 0.1
                                 // intense      1.7 1.8 1.9 2.1 - 0.1 and 0.2
                                 // very intense 1.9 2.0 2.2 2.3 - 0.1 and 0.2

        public String Diet = "";
        public String Macro = "";
        public String Meals = "";

        public float proteinPercent = 30;
        public float carbPercent = 35;
        public float fatPercent = 35;

    }
}
