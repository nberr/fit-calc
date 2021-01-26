using FileHelpers;
using iText.Kernel.Pdf.Canvas;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitness_Calculator
{
    public partial class FitnessCalculator : Form
    {
        private ClientData cd = new ClientData();

        const String default_filename = "template.pdf";
        const String default_insertname = "insert.txt";
        const String default_outputname = "output.pdf";

        private String filename = default_filename;
        private String insertname = default_insertname;
        private String outputFilename = default_outputname;

        public FitnessCalculator()
        {
            InitializeComponent();

            filename = default_filename;
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void GenerateReport_Click(object sender, EventArgs e)
        {
            string insertFile = "C:\\Program Files (x86)\\BWC\\Fitness-Calculator\\" + insertname;

            var engine = new FileHelperEngine(typeof(InsertData));
            var insertData = (InsertData[])engine.ReadFile(insertFile);

            if (insertData.Any())
            {
                //variables
                string templateFile = "C:\\Program Files (x86)\\BWC\\Fitness-Calculator\\" + filename;
                string outputFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + outputFilename;

                // open the reader
                PdfReader reader = new PdfReader(templateFile);
                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
                Document document = new Document(size);

                // open the writer
                FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // the pdf content
                PdfContentByte cb = writer.DirectContent;

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    document.NewPage();
                    // create the new page and add it to the pdf
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page, 0, 0);

                    foreach (var data in insertData)
                    {
                        if (data.Page == i)
                        {
                            // select the font properties
                            BaseFont bf;

                            try
                            {
                                string fontPath = "C:\\Program Files (x86)\\BWC\\Fitness-Calculator\\Fonts\\" + data.Font;
                                bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find font: " + data.Font + "\nMake sure font is in the program folder. Setting font to Helvetica.", "Font Missing");
                                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                cb.SetColorFill(BaseColor.BLACK);
                            }

                            try
                            {
                                cb.SetColorFill(new BaseColor(data.Color_r, data.Color_g, data.Color_b));
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not set color. Setting color to Black", "Invalid Color");
                                cb.SetColorFill(BaseColor.BLACK);
                            }


                            cb.SetFontAndSize(bf, data.Size);

                            // write the text in the pdf content
                            cb.BeginText();
                            switch (data.Keyword)
                            {
                                case "name":
                                    cb.ShowTextAligned(0, cd.name + "'s", data.X, data.Y, 0);

                                    break;
                                case "age":
                                    cb.ShowTextAligned(1, cd.age, data.X, data.Y, 0);
                                    break;
                                case "sex":
                                    cb.ShowTextAligned(1, cd.sex, data.X, data.Y, 0);
                                    break;
                                case "weight":
                                    cb.ShowTextAligned(1, cd.weight, data.X, data.Y, 0);
                                    break;
                                case "height":
                                    cb.ShowTextAligned(1, cd.height_ft + "'" + cd.height_in + "\"", data.X, data.Y, 0);
                                    break;
                                case "coach":
                                    cb.ShowTextAligned(1, CoachNameTextBox.Text, data.X, data.Y, 0);
                                    break;
                                case "header":
                                    cb.ShowTextAligned(2, data.Text + cd.name, data.X, data.Y, 0);
                                    break;
                            }
                            cb.EndText();
                        }
                    }

                    if (i == 3)
                    {
                        // draw rectangle for activity level
                        // TODO: adjust position
                        // adjust the size and color depending on activity level
                        double rectFill = (float)(cd.PAM - 1.11) / 1.89; // set this value based on the activity level number PAM?
                        
                       

                        if (cd.PAM >= 2)
                        {

                            //cb.SetColorFill(BaseColor.BLACK);
                            //cb.ShowTextAligned(0, "Highly Active", 167, 505, 0);

                            cb.SetColorFill(new BaseColor(245, 130, 32));
                        }
                        else if (cd.PAM >= 1.7)
                        {

                            //cb.SetColorFill(BaseColor.BLACK);
                            //cb.ShowTextAligned(0, "Moderately Active", 167, 505, 0);
                            cb.SetColorFill(new BaseColor(254, 202, 10));
                            
                        }
                        else
                        {

                            //cb.SetColorFill(BaseColor.BLACK);
                            //cb.ShowTextAligned(0, "Lightly Active", 230, 505, 0);

                            cb.SetColorFill(new BaseColor(34, 177, 76));
                        }

                        rectFill += 0.15;
                        if (rectFill > 1.0)
                        {
                            rectFill = 1.0;
                        }

                        //cb.SetColorFill(BaseColor.CYAN); // set the color based on activity level as well
                        cb.Rectangle(167, 498, rectFill * 398, 28);
                        cb.FillStroke();

                        if (cd.PAM >= 2)
                        {

                            cb.SetColorFill(BaseColor.BLACK);
                            cb.ShowTextAligned(0, "Highly Active", 180, 508, 0);

                           
                        }
                        else if (cd.PAM >= 1.7)
                        {

                            cb.SetColorFill(BaseColor.BLACK);
                            cb.ShowTextAligned(0, "Moderately Active", 180, 508, 0);

                            
                        }
                        else
                        {

                            cb.SetColorFill(BaseColor.BLACK);
                            cb.ShowTextAligned(0, "Lightly Active", 180, 508, 0);

                            
                        }

                        // draw pi-graph for food
                        int centerX = 370;
                        int centerY = 233;
                        int radius = 72;
                        float startAng = 0;
                        float fillPercent1 = cd.proteinPercent;
                        float fillPercent2 = cd.carbPercent;
                        float fillPercent3 = cd.fatPercent;
                        float circleThickness = 10;
                        float arcThickness = 20;

                        // protein
                        cb.SetColorStroke(new BaseColor(0, 186, 227));
                        cb.SetLineWidth(arcThickness);
                        cb.Arc(centerX - radius + circleThickness / 2, centerY - radius + circleThickness / 2,
                                centerX + radius - circleThickness / 2, centerY + radius - circleThickness / 2, startAng, -fillPercent1 / 100 * 360.0);
                        cb.Stroke();

                        startAng += (float)(-fillPercent1 / 100 * 360.0);

                        // carbs
                        cb.SetColorStroke(new BaseColor(245, 130, 32));
                        cb.SetLineWidth(arcThickness);
                        cb.Arc(centerX - radius + circleThickness / 2, centerY - radius + circleThickness / 2,
                                centerX + radius - circleThickness / 2, centerY + radius - circleThickness / 2, startAng, -fillPercent2 / 100 * 360.0);
                        cb.Stroke();

                        startAng += (float)(-fillPercent2 / 100 * 360.0);

                        // fat
                        cb.SetColorStroke(new BaseColor(254, 202, 10));
                        cb.SetLineWidth(arcThickness);
                        cb.Arc(centerX - radius + circleThickness / 2, centerY - radius + circleThickness / 2,
                                centerX + radius - circleThickness / 2, centerY + radius - circleThickness / 2, startAng, -fillPercent3 / 100 * 360.0);
                        cb.Stroke();


                    }
                }

                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();

                MessageBox.Show("Document has been generated", "Process Complete");
            }
        }

        private void FilenameTextBox_TextChanged(object sender, EventArgs e)
        {
            filename = FilenameTextBox.Text;
        }

        private void InsertTextBox_TextChanged(object sender, EventArgs e)
        {
            insertname = InsertTextBox.Text;
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                outputFilename = default_outputname;
            }
            else
            {
                cd.name = NameTextBox.Text;
                outputFilename = cd.name + ".pdf";
            }
        }

        private void WeightTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cd.weight = WeightTextBox.Text;
                cd.weight_kg = double.Parse(cd.weight) * 0.453592;
            }
            catch(Exception)
            {

            }
           
        }

        private void HeightFtTextBox_TextChanged(object sender, EventArgs e)
        {
            cd.height_ft = HeightFtTextBox.Text;
        }

        private void HeightInTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cd.height_in = HeightInTextBox.Text;
                cd.height_cm = (double.Parse(cd.height_in) + (double.Parse(cd.height_ft) * 12)) * 2.54;
            }
            catch(Exception)
            {

            }
            
        }

        private void AgeTextBox_TextChanged(object sender, EventArgs e)
        {
            cd.age = AgeTextBox.Text;
            cd.age_num = int.Parse(cd.age);
        }

        private void MaleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cd.sex = "Male";
        }

        private void FemaleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cd.sex = "Female";
        }

        private Tuple<double, double> GenerateWeeks(double fromFactor, double toFactor)
        {
            double to, from;

            double average = (double)(cd.weight_end + cd.weight_start) / 2;
            double diff = cd.weight_diff;
            
            to = Math.Round(diff / (fromFactor * average));
            from = Math.Round(diff / (toFactor * average));

            return new Tuple<double, double>(to, from);
        }

        private void WeightLossTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cd.weight_loss = WeightLossTextBox.Text;

                cd.weight_start = double.Parse(cd.weight);
                cd.weight_end = double.Parse(cd.weight_loss);
                cd.weight_diff = cd.weight_start - cd.weight_end;

                Tuple<double, double> comfortable = GenerateWeeks(0.0025, 0.0049);
                Tuple<double, double> reasonable = GenerateWeeks(0.005, 0.01);
                Tuple<double, double> extreme = GenerateWeeks(0.0101, 0.015);

                if (comfortable.Item1 >= 0 && comfortable.Item2 >= 0)
                {
                    TimeFrameComfortableLabel.Text = "Comfortable: " + comfortable.Item2.ToString() + " to " + comfortable.Item1.ToString() + " weeks";
                }
                else
                {
                    TimeFrameComfortableLabel.Text = "";
                }

                if (reasonable.Item1 >= 0 && reasonable.Item2 >= 0)
                {
                    TimeFrameReasonableLabel.Text = "Reasonable: " + reasonable.Item2.ToString() + " to " + reasonable.Item1.ToString() + " weeks";
                }
                else
                {
                    TimeFrameReasonableLabel.Text = "";
                }
                
                if (extreme.Item1 >= 0 && extreme.Item2 >= 0)
                {
                    TimeFrameExtremeLabel.Text = "Extreme: " + extreme.Item2.ToString() + " to " + extreme.Item1.ToString() + " weeks";
                }
                else
                {
                    TimeFrameExtremeLabel.Text = "";
                }
                
            }
            catch(Exception)
            {
                TimeFrameComfortableLabel.Text = "";
                TimeFrameReasonableLabel.Text = "";
                TimeFrameExtremeLabel.Text = "";
            }
            
        }

        private void GoalDateCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            cd.start_date = e.Start;
            cd.end_date = e.End;

            Console.WriteLine(e.Start);
            Console.WriteLine(e.End);

        }

        private double CalculatePAM(int WPE, int DAL)
        {
            // very light   1.4 1.5 1.6 1.7 - 0.1
            // light        1.5 1.6 1.7 1.8 - 0.1
            // moderate     1.6 1.7 1.8 1.9 - 0.1
            // intense      1.7 1.8 1.9 2.1 - 0.1 and 0.2
            // very intense 1.9 2.0 2.2 2.3 - 0.1 and 0.2
            double PAM = 0.0;
            double inc = 0.1;
            switch (WPE)
            {
                case 0:
                    PAM = 1.3 + ((DAL + 1) * inc);
                    break;
                case 1:
                    PAM = 1.4 + ((DAL + 1) * inc);
                    break;
                case 2:
                    PAM = 1.5 + ((DAL + 1) * inc);
                    break;
                case 3:
                    switch (DAL)
                    {
                        case 0:
                            PAM = 1.7;
                            break;
                        case 1:
                            PAM = 1.8;
                            break;
                        case 2:
                            PAM = 1.9;
                            break;
                        case 3:
                            PAM = 2.1;
                            break;
                    }
                    break;
                case 4:
                    switch (DAL)
                    {
                        case 0:
                            PAM = 1.9;
                            break;
                        case 1:
                            PAM = 2.0;
                            break;
                        case 2:
                            PAM = 2.2;
                            break;
                        case 3:
                            PAM = 2.3;
                            break;
                    }
                    break;
            }

            return PAM;
        }

        private void WPEComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // very light - Almost no purposeful exercise.
            // light - 1-3 hours of gentle to moderate exercise.
            // moderate - 3-4 hours of moderate exercise.
            // intense 4-6 hours of moderate to strenuous exercise.
            // very intense - 7+ hours of strenuous exercise.

            switch (WPEComboBox.SelectedIndex)
            {
                case 0:
                    WPELabel.Text = "Almost no purposeful exercise.";
                    break;
                case 1:
                    WPELabel.Text = "1-3 hours of gentle to moderate exercise.";
                    break;
                case 2:
                    WPELabel.Text = "3-4 hours of moderate exercise.";
                    break;
                case 3:
                    WPELabel.Text = "4-6 hours of moderate to strenuous exercise.";
                    break;
                case 4:
                    WPELabel.Text = "7+ hours of strenuous exercise.";
                    break;
                default:
                    WPELabel.Text = "";
                    break;
            }

            if (WPEComboBox.SelectedIndex > -1 && DALComboBox.SelectedIndex > -1)
            {
                cd.PAM = CalculatePAM(WPEComboBox.SelectedIndex, DALComboBox.SelectedIndex);

                PAMUpDown.Value = (decimal)cd.PAM;
            }

        }

        private void DALComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // very light - Sitting most of the day (example: desk job).
            // light - A mix of sitting, standing, and light activity (example: teacher).
            // moderate - Continuous gentle to moderate activity (example: restaurant server).
            // heavy - Strenuous activity throughout the day (example: construction work).

            switch (DALComboBox.SelectedIndex)
            {
                case 0:
                    DALLabel.Text = "Sitting most of the day (example: desk job).";
                    break;
                case 1:
                    DALLabel.Text = "A mix of sitting, standing, and light activity (example: teacher).";
                    break;
                case 2:
                    DALLabel.Text = "Continuous gentle to moderate activity (example: restaurant server).";
                    break;
                case 3:
                    DALLabel.Text = "Strenuous activity throughout the day (example: construction work).";
                    break;
                default:
                    DALLabel.Text = "";
                    break;

            }

            if (WPEComboBox.SelectedIndex > -1 && DALComboBox.SelectedIndex > -1)
            {
                cd.PAM = CalculatePAM(WPEComboBox.SelectedIndex, DALComboBox.SelectedIndex);

                PAMUpDown.Value = (decimal)cd.PAM;
            }
        }

        private void PAMCustom_CheckedChanged(object sender, EventArgs e)
        {

            PAMUpDown.Enabled = true;
        }


        private void PAMEstimate_CheckedChanged(object sender, EventArgs e)
        {

            PAMUpDown.Enabled = false;
            if (WPEComboBox.SelectedIndex > -1 && DALComboBox.SelectedIndex > -1)
            {
                cd.PAM = CalculatePAM(WPEComboBox.SelectedIndex, DALComboBox.SelectedIndex);

                PAMUpDown.Value = (decimal)cd.PAM;
            }
            else
            {
                cd.PAM = 1.11;
                PAMUpDown.Value = (decimal)cd.PAM;
            }
        }



        private void PAMUpDown_ValueChanged(object sender, EventArgs e)
        {
            cd.PAM = (double)PAMUpDown.Value;

        }

        private double GetMaintenanceCalories()
        {
            
            // convert values from string to numbers
            try
            {
                cd.weight_kg = double.Parse(cd.weight) * 0.453592;
                cd.height_cm = (double.Parse(cd.height_in) + (double.Parse(cd.height_ft) * 12)) * 2.54;
                cd.age_num = int.Parse(cd.age);


                if (cd.sex == "Male")
                {
                    cd.RMR = ((10 * cd.weight_kg) + (6.25 * cd.height_cm) - (5 * cd.age_num) + 5);
                    Console.WriteLine(cd.RMR);
                    //cd.BMR = cd.RMR * cd.PAM;
                }
                else if (cd.sex == "Female")
                {
                    cd.RMR = ((10 * cd.weight_kg) + (6.25 * cd.height_cm) - (5 * cd.age_num) - 161);
                    //cd.BMR =  cd.RMR * cd.PAM;
                }
                else
                {
                    cd.BMR = -1;
                }

                if (cd.client_level == 1)
                {
                    cd.TDEE = cd.RMR * cd.PAM;
                }
                else if (cd.client_level == 2)
                {
                    cd.FFM = cd.weight_kg * ((100 - cd.BFP) / 100);
                    cd.BMR = 370 + (21.6 * cd.FFM);
                    cd.TDEE = cd.BMR * cd.PAM;
                }
                else
                {
                    Console.WriteLine("Something went wrong");
                }

            }
            catch(Exception)
            {
                Console.WriteLine("Unable to convert numbers for calories");
            }

            return cd.TDEE;
        }

        private void MenuTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MenuTab.SelectedIndex == 2)
            {
                if (cd.client_level == 1)
                {

                }
                else if (cd.client_level == 2)
                {
                    cd.FFM = cd.weight_kg * ((100 - cd.BFP) / 100);
                    cd.BMR = 370 + (21.6 * cd.FFM);
                }
            }
            if (MenuTab.SelectedIndex == 5)
            {
                // set all the labels
                ResultAgeLabel.Text = cd.age.ToString();
                ResultCurrentWeightLabel.Text = cd.weight_start.ToString();
                ResultTargetWeightLabel.Text = cd.weight_end.ToString();
                int days = (cd.end_date - cd.start_date).Days;
                ResultNumberDaysLabel.Text = (cd.end_date - cd.start_date).Days.ToString();
                ResultEatingStyleLabel.Text = cd.Diet;

                ResultMacroProteinLabel.Text = cd.proteinPercent.ToString() + " % protein";
                ResultMacroCarbsLabel.Text = cd.carbPercent.ToString() + " % carbs";
                ResultMacroFatLabel.Text = cd.fatPercent.ToString() + " % fat";

                ResultMaintenanceCLabel.Text = (Math.Round(GetMaintenanceCalories() / 5.0) * 5).ToString();
                ResultGoalCLabel.Text = (Math.Round((GetMaintenanceCalories() - 200 - (500 * (cd.weight_diff / (days / 7)))) / 5.0) * 5).ToString();

                Console.WriteLine(cd.Macro);

                Console.WriteLine(cd.Macro);
                if (cd.Macro == "hand")
                {
                    CustomCalLabel.Visible = true;
                    ResultCustomCLabel.Visible = true;

                    PalmProteinLabel.Text = HPProteinTextBox.Text + " palm(s) of protein";
                    FistVeggieLabel.Text = HPVeggieTextBox.Text + " fist(s) of veggies";
                    HandfulCarbLabel.Text = HPCarbTextBox.Text + " cupped handful(s) of carbs";
                    ThumbFatsLabel.Text = HPFatTextBox.Text + " thumb-sized portion(s) of healthy fat";

                    ProteinPerMealLabel.Text = double.Parse(HPProteinTextBox.Text) / double.Parse(cd.Meals) + " palms per meal";
                    VeggiesPerMealLabel.Text = double.Parse(HPVeggieTextBox.Text) / double.Parse(cd.Meals) + " fists per meal";
                    CarbsPerMealLabel.Text = double.Parse(HPCarbTextBox.Text) / double.Parse(cd.Meals) + " handfuls per meal";
                    FatsPerMealLabel.Text = double.Parse(HPFatTextBox.Text) / double.Parse(cd.Meals) + " thumbs per meal";

                    double palm = double.Parse(HPProteinTextBox.Text) * 140;
                    double fist = double.Parse(HPVeggieTextBox.Text) * 25;
                    double hand = double.Parse(HPCarbTextBox.Text) * 120;
                    double thumb = double.Parse(HPFatTextBox.Text) * 110;

                    ResultCustomCLabel.Text = (palm + fist + hand + thumb).ToString();

                    double protein_macro = (double.Parse(HPProteinTextBox.Text) * 30) + (double.Parse(HPVeggieTextBox.Text) * 1.5) + (double.Parse(HPCarbTextBox.Text) * 3) + (double.Parse(HPFatTextBox.Text) * 2);
                    double carbs_macro = (double.Parse(HPVeggieTextBox.Text) * 5) + (double.Parse(HPCarbTextBox.Text) * 25);
                    double fats_macro = (double.Parse(HPProteinTextBox.Text) * 1.5) + (double.Parse(HPFatTextBox.Text) * 11);

                    double macro_total = protein_macro + carbs_macro + fats_macro;

                    cd.proteinPercent = (float)Math.Round((protein_macro / macro_total) * 100);
                    cd.carbPercent = (float)Math.Round((carbs_macro / macro_total) * 100);
                    cd.fatPercent = (float)Math.Round((fats_macro / macro_total) * 100);

                    ResultMacroProteinLabel.Text = cd.proteinPercent.ToString() + " % protein";
                    ResultMacroCarbsLabel.Text = cd.carbPercent.ToString() + " % carbs";
                    ResultMacroFatLabel.Text = cd.fatPercent.ToString() + " % fat";

                }
                else if (cd.Macro == "macro totals")
                {
                    CustomCalLabel.Visible = true;
                    ResultCustomCLabel.Visible = true;

                    // protein = 30g -> 1 unit and 140 cals
                    // veggies = n/a
                    // carbs = 50g -> 1 unit and 120 cals
                    // fats = 11g -> 1 unit 110 cals

                    double protein_macro = double.Parse(MacroProteinTextBox.Text) * 4;
                    double carbs_macro = double.Parse(MacroCarbsTextBox.Text) * 4;
                    double fats_macro = double.Parse(MacroFatTextBox.Text) * 9;

                    double macro_total = protein_macro + carbs_macro + fats_macro;

                    cd.proteinPercent = (float)Math.Round((protein_macro / macro_total) * 100);
                    cd.carbPercent = (float)Math.Round((carbs_macro / macro_total) * 100);
                    cd.fatPercent = (float)Math.Round((fats_macro / macro_total) * 100);

                    ResultMacroProteinLabel.Text = cd.proteinPercent.ToString() + " % protein";
                    ResultMacroCarbsLabel.Text = cd.carbPercent.ToString() + " % carbs";
                    ResultMacroFatLabel.Text = cd.fatPercent.ToString() + " % fat";

                    // calculate hands
                    double cals = macro_total;

                    double protein_amount = cals * (cd.proteinPercent / 100);
                    double the_rest = cals - protein_amount;

                    double veggies_amount = the_rest * .10;

                    double carbs_amount = ((cd.carbPercent - 5) / 100) * the_rest;
                    double fat_amount = ((cd.fatPercent - 5) / 100) * the_rest;
                    
                    protein_amount = (protein_amount / 4) / 30;
                    carbs_amount = (carbs_amount / 4) / 25;
                    fat_amount = ((fat_amount / 9) / 11) + 1;
                    veggies_amount = veggies_amount / 25;

                    PalmProteinLabel.Text = Math.Round(protein_amount) + " palm(s) of protein";
                    FistVeggieLabel.Text = Math.Round(veggies_amount) + " fist(s) of veggies";
                    HandfulCarbLabel.Text = Math.Round(carbs_amount) + " cupped handful(s) of carbs";
                    ThumbFatsLabel.Text = Math.Round(fat_amount) + " thumb-sized portion(s) of healthy fat";

                    ProteinPerMealLabel.Text = Math.Round(protein_amount) / double.Parse(cd.Meals) + " palms per meal";
                    VeggiesPerMealLabel.Text = Math.Round(veggies_amount) / double.Parse(cd.Meals) + " fists per meal";
                    CarbsPerMealLabel.Text = Math.Round(carbs_amount) / double.Parse(cd.Meals) + " handfuls per meal";
                    FatsPerMealLabel.Text = Math.Round(fat_amount) / double.Parse(cd.Meals) + " thumbs per meal";

                    ResultCustomCLabel.Text = macro_total.ToString();
                }
                else if (cd.Macro == "custom %")
                {
                    CustomCalLabel.Visible = true;
                    ResultCustomCLabel.Visible = true;
                }
                else if (cd.Macro == "custom grams")
                {
                    CustomCalLabel.Visible = true;
                    ResultCustomCLabel.Visible = true;
                }
                else
                {
                    CustomCalLabel.Visible = false;
                    ResultCustomCLabel.Visible = false;

                    // calculate protein first
                    double cals = double.Parse(ResultGoalCLabel.Text);

                    double protein_amount = cals * (cd.proteinPercent / 100);
                    double the_rest = cals - protein_amount;

                    double veggies_amount = the_rest * .10;

                    double carbs_amount = ((cd.carbPercent - 5) / 100) * the_rest;
                    double fat_amount = ((cd.fatPercent - 5) / 100) * the_rest;



                    protein_amount = (protein_amount / 4) / 30;
                    carbs_amount = (carbs_amount / 4) / 25;
                    fat_amount = ((fat_amount / 9) / 11) + 1;
                    veggies_amount = veggies_amount / 25;

                    PalmProteinLabel.Text = Math.Round(protein_amount) + " palm(s) of protein";
                    FistVeggieLabel.Text = Math.Round(veggies_amount) + " fist(s) of veggies";
                    HandfulCarbLabel.Text = Math.Round(carbs_amount) + " cupped handful(s) of carbs";
                    ThumbFatsLabel.Text = Math.Round(fat_amount) + " thumb-sized portion(s) of healthy fat";

                    ProteinPerMealLabel.Text = Math.Round(protein_amount) / double.Parse(cd.Meals) + " palms per meal";
                    VeggiesPerMealLabel.Text = Math.Round(veggies_amount) / double.Parse(cd.Meals) + " fists per meal";
                    CarbsPerMealLabel.Text = Math.Round(carbs_amount) / double.Parse(cd.Meals) + " handfuls per meal";
                    FatsPerMealLabel.Text = Math.Round(fat_amount) / double.Parse(cd.Meals) + " thumbs per meal";
                }

            }
        }

        // index 1 - anything, medditteranean, paleo,
        private Dictionary<int, Dictionary<string, string>> MacroDisplay = new Dictionary<int, Dictionary<string, string>>();
        private bool initialized = false;
        private void InitDictionary()
        {
            if (!initialized)
            {
                Dictionary<string, string> AnythingOptions = new Dictionary<string, string>();
                AnythingOptions.Add("balanced", "~30% protein, ~35% carbs, ~35% fats");
                AnythingOptions.Add("low-fat", "~30% protein, ~50% carbs, ~20% fats");
                AnythingOptions.Add("low-carb", "~30% protein, ~20% carbs, ~50% fats");


                Dictionary<string, string> MeditterraneanOptions = new Dictionary<string, string>();
                MeditterraneanOptions.Add("balanced", "~30% protein, ~35% carbs, ~35% fats");
                MeditterraneanOptions.Add("low-carb", "~30% protein, ~20% carbs, ~50% fats");

                Dictionary<string, string> PaleoOptions = new Dictionary<string, string>();
                PaleoOptions.Add("balanced", "~30% protein, ~35% carbs, ~35% fats");
                PaleoOptions.Add("low-carb", "~30% protein, ~20% carbs, ~50% fats");

                Dictionary<string, string> VegetarianOptions = new Dictionary<string, string>();
                VegetarianOptions.Add("balanced", "~30% protein, ~35% carbs, ~35% fats");
                VegetarianOptions.Add("low-fat", "~30% protein, ~50% carbs, ~20% fats");
                VegetarianOptions.Add("low-carb", "~30% protein, ~20% carbs, ~50% fats");

                Dictionary<string, string> KetoOptions = new Dictionary<string, string>();
                KetoOptions.Add("very low-carb", "20% protein, 10% carbs, 70% fats");

                Dictionary<string, string> VeganOptions = new Dictionary<string, string>();
                VeganOptions.Add("balanced", "~30% protein, ~35% carbs, ~35% fats");
                VeganOptions.Add("low-fat", "~30% protein, ~50% carbs, ~20% fats");
                VeganOptions.Add("low-carb", "~30% protein, ~20% carbs, ~50% fats");
                VeganOptions.Add("very low-fat", "20% protein, 70% carbs, 10% fats");


                MacroDisplay.Add(0, AnythingOptions);
                MacroDisplay.Add(1, MeditterraneanOptions);
                MacroDisplay.Add(2, PaleoOptions);
                MacroDisplay.Add(3, VegetarianOptions);
                MacroDisplay.Add(4, KetoOptions);
                MacroDisplay.Add(5, VeganOptions);

                initialized = true;
            }
        }

        private void DietaryPrefComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // anything       - balanced, low-fat, low-carb, custom %, custom grams, macro totals, hand
            // meditterranean - balanced,        , low-carb, custom %, custom grams, macro totals, hand
            // paleo          - balanced,        , low-carb, custom %, custom grams, macro totals, hand
            // vegetarian     - balanced, low-fat, low-carb, custom %, custom grams, macro totals, hand
            // keto           -               very low-carb, custom %, custom grams, macro totals, hand
            // fully plant    - balanced, low-fat, low-carb, very low-fat, custom %, custom grams, macro totals, hand
            InitDictionary();
            

            Dictionary<string, string> source = new Dictionary<string, string>();
            //MacroRatioComboBox.DataSource 
            cd.Diet = DietaryPrefComboBox.Text;
            switch (DietaryPrefComboBox.SelectedIndex)
            {
                case 0:
                    DietPrefLabel.Text = "No major preferences or restrictions. Will eat practically anything.";
                    source.Add("balanced", "Balanced");
                    source.Add("low-fat", "Low-fat");
                    source.Add("low-carb", "Low-carb");

                    break;
                case 1:
                    DietPrefLabel.Text = "Features plant foods, healthy fats, and moderate amounts of lean protein.";
                    source.Add("balanced", "Balanced");
                    source.Add("low-carb", "Low-carb");

                    break;
                case 2:
                    DietPrefLabel.Text = "Emphasizes meats, vegetables, and healthy fats.";
                    source.Add("balanced", "Balanced");
                    source.Add("low-carb", "Low-carb");

                    break;
                case 3:
                    DietPrefLabel.Text = "A plant-based diet, plus small amounts of eggs and dairy.";
                    source.Add("balanced", "Balanced");
                    source.Add("low-fat", "Low-fat");
                    source.Add("low-carb", "Low-carb");
                    break;
                case 4:
                    DietPrefLabel.Text = "A high-fat, very-low carbohydrate diet.";
                    source.Add("very low-carb", "Very low-carb");
                    break;
                case 5:
                    DietPrefLabel.Text = "All plant-based foods. No animal products of any kind.";
                    source.Add("balanced", "Balanced");
                    source.Add("low-fat", "Low-fat");
                    source.Add("low-carb", "Low-carb");
                    source.Add("very low-fat", "Very low-fat");
                    break;
            }

            source.Add("custom %", "Customize Macro Percentages");
            source.Add("custom grams", "Customize Macro Grams");
            source.Add("macro totals", "Enter Desired Macro Totals");
            source.Add("hand", "Enter Desired Hand Portions");

            MacroRatioComboBox.DataSource = new BindingSource(source, null);
            MacroRatioComboBox.DisplayMember = "Value";
            MacroRatioComboBox.ValueMember = "Key";
            MacroRatioComboBox.Enabled = true;
            MacroRatioComboBox.SelectedIndex = -1;
        }

        private bool panels_init = false;

        public object TimeFrameReasonable { get; private set; }

        private void InitPanels()
        {
            if (!panels_init)
            {
                int x = 81, y = 105, width = 306, height = 192;
                MacroPercentagePanel.SetBounds(x, y, width, height);
                MacroGramsPanel.SetBounds(x, y, width, height);
                MacroTotalsPanel.SetBounds(x, y, width, height);
                HandPanel.SetBounds(x, y, width, height);
            }

            panels_init = true;
        }

        private void MacroRatioComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<string, string> values = MacroDisplay[DietaryPrefComboBox.SelectedIndex];
            InitPanels();
            try
            {
                MacroRatLabel.Text = values[MacroRatioComboBox.SelectedValue.ToString()];
            }
            catch(Exception)
            {
                MacroRatLabel.Text = "";
            }

            try
            {
                HandPanel.Visible = false;
                MacroPercentagePanel.Visible = false;
                MacroGramsPanel.Visible = false;
                MacroTotalsPanel.Visible = false;
                switch (MacroRatioComboBox.SelectedValue.ToString())
                {
                    case "custom %":
                        MacroPercentagePanel.Visible = true;
                        break;
                    case "custom grams":
                        MacroGramsPanel.Visible = true;
                        break;
                    case "macro totals":
                        MacroTotalsPanel.Visible = true;
                        break;
                    case "hand":
                        HandPanel.Visible = true;
                        break;
                    case null:
                        break;
                    default:
                        // disable all the controls
                        HandPanel.Visible = false;
                        MacroPercentagePanel.Visible = false;
                        MacroGramsPanel.Visible = false;
                        MacroTotalsPanel.Visible = false;

                        break;

                }
                cd.Macro = MacroRatioComboBox.SelectedValue.ToString();
            }
            catch(Exception)
            {

            }

            try
            {
                switch (MacroRatioComboBox.SelectedValue.ToString())
                {
                    case "balanced":
                        cd.proteinPercent = 35;
                        cd.carbPercent = 35;
                        cd.fatPercent = 35;
                        break;
                    case "low-fat":
                        cd.proteinPercent = 30;
                        cd.carbPercent = 50;
                        cd.fatPercent = 20;
                        break;
                    case "low-carb":
                        cd.proteinPercent = 30;
                        cd.carbPercent = 20;
                        cd.fatPercent = 50;
                        break;
                    case "very low-fat":
                        cd.proteinPercent = 20;
                        cd.carbPercent = 70;
                        cd.fatPercent = 10;
                        break;
                    case "very low-carb":
                        cd.proteinPercent = 20;
                        cd.carbPercent = 10;
                        cd.fatPercent = 70;
                        break;
                    case "custom %":

                        break;
                    case "custom grams":

                        break;
                    case "macro totals":

                        break;
                    case "hand":

                        break;
                }
                cd.Macro = MacroRatioComboBox.SelectedValue.ToString();
                Console.WriteLine(cd.Macro);
            }
            catch(Exception) {

            }
            


        }

        private void MealsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd.Meals = MealsComboBox.Text;
        }

        private void HPVeggieTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void HPProteinTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void HPCarbTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void HPFatTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CarbRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FatRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void GramsProteinTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void GramsCarbsTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void GramsFatTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CoachNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProteinTrackBar_Scroll(object sender, EventArgs e)
        {

        }

        private void DatePickerStart_ValueChanged(object sender, EventArgs e)
        {
            cd.start_date = DatePickerStart.Value.Date;
        }

        private void DatePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            cd.end_date = DatePickerEnd.Value.Date;
        }

        private void Level1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cd.client_level = 1;

            BodyFatGroup.Visible = false;
            RMRGroup.Visible = false;
        }

        private void Level23RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cd.client_level = 2;

            BodyFatGroup.Visible = true;
            RMRGroup.Visible = true;
        }

        private void BodyFatEstimateRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            BodyFatTextBox.Enabled = false;
        }

        private void BodyFatCustomRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            BodyFatTextBox.Enabled = true;
        }

        private void RMREstimateRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RMRTextBox.Enabled = false;
        }

        private void RMRCustomRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RMRTextBox.Enabled = true;
        }

        private void BodyFatTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void RMRTextBox_TextChanged(object sender, EventArgs e)
        {

        }
   
        private double GetBMI()
        {
            return cd.weight_kg / Math.Pow(cd.height_cm/100, 2);
        }

        private void BodyFatCalculateButton_Click(object sender, EventArgs e)
        {
            if (cd.sex == "Male")
            {
                cd.BFP = (0.14 * cd.age_num + 37.310000000000002 * Math.Log(GetBMI()) - 103.94);
            }
            else
            {
                cd.BFP = (0.14 * cd.age_num + 39.960000000000001 * Math.Log(GetBMI()) - 102.01000000000001);
            }

            cd.BFP = (cd.BFP < 0.0) ? 0.0 : cd.BFP;
            cd.BFP = (cd.BFP > 60.0) ? 60.0 : cd.BFP;

            cd.BFP = Math.Round(cd.BFP, 1);
            
            if (BodyFatEstimateRadioButton.Checked)
            {
                BodyFatTextBox.Text = cd.BFP.ToString();
            }
            else
            {

            }
        }

        private void RMRCalculateButton_Click(object sender, EventArgs e)
        {
            if (cd.sex == "Male")
            {
                cd.RMR = (9.99 * cd.weight_kg + 625.0 * cd.height_cm / 100.0 - 4.92 * cd.age_num + 5.0);
            }
            else
            {
                cd.RMR = (9.99 * cd.weight_kg + 625.0 * cd.height_cm / 100.0 - 4.92 * cd.age_num - 161.0);
            }

            cd.RMR = Math.Round(cd.RMR);

            if (RMREstimateRadioButton.Checked)
            {
                RMRTextBox.Text = cd.RMR.ToString();
            }
            else
            {

            }
        }
    }
}
