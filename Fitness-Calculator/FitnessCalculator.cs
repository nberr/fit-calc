using FileHelpers;
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
                            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            cb.SetColorFill(BaseColor.BLACK);

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
                            }
                            cb.EndText();
                        }

                    }
                }

                // close the streams and voilá the file should be changed :)
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
            cd.weight = WeightTextBox.Text;
        }

        private void HeightFtTextBox_TextChanged(object sender, EventArgs e)
        {
            cd.height_ft = HeightFtTextBox.Text;
        }

        private void HeightInTextBox_TextChanged(object sender, EventArgs e)
        {
            cd.height_in = HeightInTextBox.Text;
        }

        private void AgeTextBox_TextChanged(object sender, EventArgs e)
        {
            cd.age = AgeTextBox.Text;
        }

        private void MaleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cd.sex = "Male";
        }

        private void FemaleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cd.sex = "Female";
        }

        private void WeightLossTextBox_TextChanged(object sender, EventArgs e)
        {
            cd.weight_loss = WeightLossTextBox.Text;
        }

        private void GoalDateCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            cd.start_date = e.Start;
            cd.end_date = e.End;
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
                    switch(DAL)
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
                PAMTextBox.Text = cd.PAM.ToString();
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
                PAMTextBox.Text = cd.PAM.ToString();
            }
        }

        private void PAMCustom_CheckedChanged(object sender, EventArgs e)
        {
            PAMTextBox.ReadOnly = false;
        }


        private void PAMEstimate_CheckedChanged(object sender, EventArgs e)
        {
            PAMTextBox.ReadOnly = true;
            if (WPEComboBox.SelectedIndex > -1 && DALComboBox.SelectedIndex > -1)
            {
                cd.PAM = CalculatePAM(WPEComboBox.SelectedIndex, DALComboBox.SelectedIndex);
                PAMTextBox.Text = cd.PAM.ToString();
            }
            else
            {
                cd.PAM = 0.0;
                PAMTextBox.Text = "";
            }
        }

        private void PAMTextBox_TextChanged(object sender, EventArgs e)
        {
            if (PAMTextBox.Text != "")
            {
                try
                {
                    cd.PAM = Convert.ToDouble(PAMTextBox.Text);

                    if (cd.PAM > 3)
                    {
                        cd.PAM = 3;
                        PAMTextBox.Text = cd.PAM.ToString();
                        MessageBox.Show("Physical Activity Multiplier too high!", "Invalid Value");
                    }

                    if (cd.PAM < 1.11)
                    {
                        cd.PAM = 1.11;
                        PAMTextBox.Text = cd.PAM.ToString();
                        MessageBox.Show("Physical Activity Multiplier too low!", "Invalid Value");
                    }
                }
                catch (Exception)
                {
                    cd.PAM = 0.0;
                    PAMTextBox.Clear();

                    MessageBox.Show("Please enter a valid number", "Invalid Value");
                }

            }
        }

        private void MenuTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MenuTab.SelectedIndex == 5)
            {
                MessageBox.Show("generating results", "wait");
            }
        }

        private void DietaryPrefComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MacroRatioComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MealsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
