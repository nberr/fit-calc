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

                    foreach(var data in insertData)
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
            else {
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

        
    }
}
