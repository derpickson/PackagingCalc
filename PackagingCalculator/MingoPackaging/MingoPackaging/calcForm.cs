using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilmCalc;

namespace MingoPackaging
{
    public partial class calcForm : Form
    {
        public calcForm()
        {
            InitializeComponent();
            txtHeight.Text = "0.5";
            txtFinseal.Text = "0.625";
            txtCrimp.Text = txtFinseal.Text;
        }

        private void txtWidth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                MessageBox.Show("Enter");
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            //accidental click, cant delete
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //exit button
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //reset button, sets back to default
            txtHeight.Text = "0.5";
            txtFinseal.Text = "0.625";
            txtCrimp.Text = txtFinseal.Text;
            txtWidth.Text = "";
            txtLength.Text = "";
            txtWeb.Text = "";
            txtRepeat.Text = "";
            txtRelm.Text = "";
            txtCLD.Text = "";
            ast1.Visible = false;
            ast2.Visible = false;
            ast3.Visible = false;
            ast4.Visible = false;
            ast6.Visible = false;
            btnExport.Visible = false;
        }

        public bool errorCheck()
        {
            //checks for empty or improper variables
            ast1.Visible = false;
            ast2.Visible = false;
            ast3.Visible = false;
            ast4.Visible = false;
            ast6.Visible = false;
            bool fillError = false;
            if (String.IsNullOrEmpty(txtWidth.Text))
            {
                fillError = true;
                ast1.Visible = true;
            }
            if (String.IsNullOrEmpty(txtHeight.Text))
            {
                fillError = true;
                ast2.Visible = true;
            }
            if (String.IsNullOrEmpty(txtFinseal.Text))
            {
                fillError = true;
                ast3.Visible = true;
            }
            if (String.IsNullOrEmpty(txtLength.Text))
            {
                fillError = true;
                ast4.Visible = true;
            }
            if (String.IsNullOrEmpty(txtCrimp.Text))
            {
                fillError = true;
                ast6.Visible = true;
            }
            return fillError;
        }

        public bool numericCheck()
        {
            //checks to make sure that the text boxes have no letters or symbols in them.
            bool numericStatus = true;
            double number;
            if ((double.TryParse(txtWidth.Text, out number)) == false)
            {
                numericStatus = false;
                ast1.Visible = true;
            }
            if ((double.TryParse(txtHeight.Text, out number)) == false)
            {
                numericStatus = false;
                ast2.Visible = true;
            }
            if ((double.TryParse(txtFinseal.Text, out number)) == false)
            {
                numericStatus = false;
                ast3.Visible = true;
            }
            if ((double.TryParse(txtLength.Text, out number)) == false)
            {
                numericStatus = false;
                ast4.Visible = true;
            }
            if ((double.TryParse(txtCrimp.Text, out number)) == false)
            {
                numericStatus = false;
                ast6.Visible = true;
            }
            if (numericStatus == false)
            {
                MessageBox.Show("Cannot parse the information, please verify that there are no letters or symbols in the textboxes.");
            }
            return numericStatus;
        }

        public string getRelm(double webActual, double repeatActual)
        {
            string[,] dataRelm = new string[,]
            {
                {"GCS0016", "4",    "2.875"},
                {"GCS0020", "4.75", "3.265625"},
                {"GCS0005", "4.75", "3.75"},
                {"GCS0021", "4.875",    "4"},
                {"GCS0007", "5.25", "5.2795"},
                {"GCS0019", "5.25", "6.25"},
                {"GCS0008", "5.375",    "6.125"},
                {"GCS0013", "5.5",  "6"},
                {"GCS0017", "5.5",  "6"},
                {"GCS0018", "5.875",    "6.125"},
                {"GCS0015", "6",    "5.375"},
                {"GCS0010", "6",    "5.5"},
                {"GCS0014", "6",    "6"},
                {"GCS0002", "6.375",    "3.625"},
                {"GCS0004", "6.375",    "5.75"},
                {"GCS0009", "6.9375",   "4.75"},
                {"GCS0001", "6.9375",   "5.5"},
                {"GCS0012", "7",    "5"},
                {"GCS0006", "7.432",    "8.25"},
                {"GCS0011", "7.4375",   "4.375"}
            };

            string toolNumber = "CUSTOM";

            int bound0 = dataRelm.GetUpperBound(0);
            int bound1 = dataRelm.GetUpperBound(1);
            // finds upper bounds for data array

            double xPlay = 0.125;
            double yPlay = 0.125;

            int[,] isMatch = new int[(bound0 + 1), bound1];
            // initializes found match array

            int bound2 = isMatch.GetUpperBound(0);
            int bound3 = isMatch.GetUpperBound(1);
            // finds upper bounds for match array

            for (int i = 0; i <= bound2; i++)
            {
                for (int j = 0; j <= bound3; j++)
                {
                    isMatch[i, j] = 0;
                }
            }
            // fills found match array

            for (int i = 0; i <= bound0; i++)
            {
                if ((double.Parse(dataRelm[i, 1]) >= (webActual - xPlay)) && (double.Parse(dataRelm[i, 1]) <= (webActual + xPlay)))
                {
                    isMatch[i, 0] = 1;
                }
                if ((double.Parse(dataRelm[i, 2]) >= (repeatActual - yPlay)) && (double.Parse(dataRelm[i, 2]) <= (repeatActual + yPlay)))
                {
                    isMatch[i, 1] = 1;
                }
            }
            // finds matches

            double difference = 5555555;
            double location = -1;
            double width;
            double repeat;
            double absval;
            
            for (int i = 0; i <= bound2; i++)
            {
                if (isMatch[i, 0] == 1 && isMatch[i, 1] == 1)
                {
                    width = double.Parse(dataRelm[i, 1]);
                    repeat = double.Parse(dataRelm[i, 2]);
                    absval = Math.Abs(width - webActual);
                    absval += Math.Abs(repeat - repeatActual);
                    if (absval < difference)
                    {
                        difference = absval;
                        location = i;
                    }
                }
            }
            if (location != -1)
            {
                toolNumber = dataRelm[(int)location, 0];
            }

            return toolNumber;
        }

        /*public string getCLD(double webActual, double repeatActual)
        {
            string[,] dataCLD = new string[,]
            {
                {"GC035",   "4.1730",   "5.7500",   "0.625",    "0.625"},
                {"GC021",   "4.6250",   "4.3750",   "0.625",    "0.625"},
                {"GC022",   "5.0000",   "4.5313",   "0.625",    "0.625"},
                {"GC008",   "5.0000",   "5.5420",   "0.625",    "0.625"},
                {"GC055",   "5.0000",   "6.0000",   "0.625",    "0.625"},
                {"GC005",   "5.2500",   "5.6670",   "0.625",    "0.625"},
                {"GC015",   "5.2500",   "6.0000",   "0.625",    "0.625"},
                {"GC073",   "5.2500",   "6.3125",   "0.625",    "0.625"},
                {"GC061",   "5.2500",   "6.6250",   "0.625",    "0.625"},
                {"GC041",   "5.3150",   "5.2500",   "0.625",    "0.625"},
                {"GC053",   "5.4720",   "6.9375",   "0.625",    "0.625"},
                {"GC013",   "5.4720",   "7.1875",   "0.625",    "0.625"},
                {"GC074",   "5.5000",   "3.7500",   "0.625",    "0.625"},
                {"GC042",   "5.5000",   "6.0000",   "0.625",    "0.625"},
                {"GC004",   "5.5000",   "6.5625",   "0.625",    "0.625"},
                {"GC076",   "5.5000",   "7.7500",   "0.625",    "0.625"},
                {"GC029",   "5.6000",   "4.0000",   "0.625",    "0.625"},
                {"GC031",   "5.6000",   "5.3750",   "0.625",    "0.625"},
                {"GC032",   "5.6500",   "7.0000",   "0.625",    "0.625"},
                {"GC043",   "5.6875",   "5.7500",   "0.625",    "0.625"},
                {"GC011",   "5.6875",   "5.9166",   "0.625",    "0.625"},
                {"GC003",   "5.6875",   "8.2500",   "0.625",    "0.625"},
                {"GC045",   "5.6875",   "4.0000",   "0.625",    "0.625"},
                {"GC016",   "5.6875",   "6.0000",   "0.625",    "0.625"},
                {"GC006",   "5.6875",   "6.1670",   "0.625",    "0.625"},
                {"GC062",   "5.6875",   "6.6250",   "0.625",    "0.625"},
                {"GC046",   "5.6875",   "7.3750",   "0.625",    "0.625"},
                {"GC009",   "5.6875",   "8.0625",   "0.625",    "0.625"},
                {"GC027",   "5.6875",   "3.5000",   "0.625",    "0.625"},
                {"GC038",   "6.0000",   "3.7813",   "0.625",    "0.625"},
                {"GC052",   "6.0000",   "4.7500",   "0.625",    "0.625"},
                {"GC017",   "6.0000",   "6.0000",   "0.625",    "0.625"},
                {"GC049",   "6.0000",   "6.2500",   "0.625",    "0.625"},
                {"GC051",   "6.0000",   "6.5625",   "0.625",    "0.625"},
                {"GC007",   "6.0000",   "6.8750",   "0.625",    "0.625"},
                {"GC070",   "6.0000",   "7.0000",   "0.625",    "0.625"},
                {"GC001",   "6.0000",   "7.6250",   "0.625",    "0.625"},
                {"GC034",   "6.0000",   "8.0625",   "0.625",    "0.625"},
                {"GC020",   "6.0000",   "8.5625",   "0.625",    "0.625"},
                {"GC044",   "6.1250",   "5.5833",   "0.625",    "0.625"},
                {"GC058",   "6.1420",   "5.9167",   "0.625",    "0.625"},
                {"GC036",   "6.2000",   "6.5625",   "0.625",    "0.625"},
                {"GC030",   "6.2500",   "4.5000",   "0.625",    "0.625"},
                {"GC012",   "6.2500",   "5.7500",   "0.625",    "0.625"},
                {"GC048",   "6.3750",   "3.8750",   "0.625",    "0.625"},
                {"GC028",   "6.5000",   "7.3125",   "0.625",    "0.625"},
                {"GC026",   "6.5630",   "6.4375",   "0.625",    "0.625"},
                {"GC014",   "6.6250",   "6.0000",   "0.625",    "0.625"},
                {"GC023",   "6.6250",   "8.2500",   "0.625",    "0.625"},
                {"GC059",   "6.7500",   "6.3125",   "0.625",    "0.625"},
                {"GC075",   "6.7500",   "7.7500",   "0.625",    "0.625"},
                {"GC054",   "7.0000",   "4.7500",   "0.625",    "0.625"},
                {"GC067",   "7.0000",   "5.2500",   "0.625",    "0.625"},
                {"GC033",   "7.0250",   "7.3750",   "0.625",    "0.625"},
                {"GC025",   "7.7500",   "6.5625",   "0.625",    "0.625"},
                {"GC039",   "8.0000",   "4.2500",   "0.625",    "0.625"},
                {"GC040",   "8.0000",   "4.5000",   "0.625",    "0.625"},
                {"GC002",   "8.0000",   "7.1250",   "0.625",    "0.625"},
                {"GC057",   "8.2500",   "4.7500",   "0.625",    "0.625"},
                {"GC068",   "9.8750",   "7.6875",   "0.625",    "0.625"},
                {"GC047",   "10.5000",  "5.7500",   "0.625",    "0.625"}
            };

            string toolNumber = "CUSTOM";

            int bound0 = dataCLD.GetUpperBound(0);
            int bound1 = dataCLD.GetUpperBound(1);
            // finds upper bounds for data array

            double xPlay = 0.125;
            double yPlay = 0.125;

            int[,] isMatch = new int[(bound0 + 1), bound1];
            // initializes found match array

            int bound2 = isMatch.GetUpperBound(0);
            int bound3 = isMatch.GetUpperBound(1);
            // finds upper bounds for match array

            for (int i = 0; i <= bound2; i++)
            {
                for (int j = 0; j <= bound3; j++)
                {
                    isMatch[i, j] = 0;
                }
            }
            // fills found match array

            for (int i = 0; i <= bound0; i++)
            {
                if ((double.Parse(dataCLD[i, 1]) >= (webActual - xPlay)) && (double.Parse(dataCLD[i, 1]) <= (webActual + xPlay)))
                {
                    isMatch[i, 0] = 1;
                }
                if ((double.Parse(dataCLD[i, 2]) >= (repeatActual - yPlay)) && (double.Parse(dataCLD[i, 2]) <= (repeatActual + yPlay)))
                {
                    isMatch[i, 1] = 1;
                }
            }
            // finds matches

            double difference = 5555555;
            double location = -1;
            double width;
            double repeat;
            double absval;

            for (int i = 0; i <= bound2; i++)
            {
                if (isMatch[i, 0] == 1 && isMatch[i, 1] == 1)
                {
                    width = double.Parse(dataCLD[i, 1]);
                    repeat = double.Parse(dataCLD[i, 2]);
                    absval = Math.Abs(width - webActual);
                    absval += Math.Abs(repeat - repeatActual);
                    if (absval < difference)
                    {
                        difference = absval;
                        location = i;
                    }
                }
            }
            if (location != -1)
            {
                toolNumber = dataCLD[(int)location, 0];
            }

            return toolNumber;
        }*/

        public string getCLDalt(double webActual, double repeatActual, double finsealActual, double crimpActual)
        {
            string[,] dataCLDalt = new string[,] {
                {"GC079",   "3.8750",   "3.0750",   "0.5000",   "0.5000"},
                {"GC035",   "4.1730",   "5.7500",   "0.6250",   "0.6250"},
                {"GC021",   "4.6250",   "4.3750",   "0.6250",   "0.6250"},
                {"GC050",   "5.0000",   "3.5000",   "0.5000",   "0.5000"},
                {"GC022",   "5.0000",   "4.5313",   "0.6250",   "0.6250"},
                {"GC008",   "5.0000",   "5.5420",   "0.6250",   "0.6250"},
                {"GC055",   "5.0000",   "6.0000",   "0.6250",   "0.6250"},
                {"GC037",   "5.0400",   "2.8250",   "0.5000",   "0.5000"},
                {"GC084",   "5.0400",   "2.9583",   "0.5000",   "0.5000"},
                {"GC087",   "5.2500",   "3.5000",   "0.6250",   "0.6250"},
                {"GC005",   "5.2500",   "5.6670",   "0.6250",   "0.6250"},
                {"GC015",   "5.2500",   "6.0000",   "0.6250",   "0.6250"},
                {"GC073",   "5.2500",   "6.3125",   "0.6250",   "0.6250"},
                {"GC061",   "5.2500",   "6.6250",   "0.6250",   "0.6250"},
                {"GC066",   "5.2500",   "6.6250",   "0.5000",   "0.6250"},
                {"GC041",   "5.3150",   "5.2500",   "0.6250",   "0.6250"},
                {"GC053",   "5.4720",   "6.9375",   "0.6250",   "0.6250"},
                {"GC013",   "5.4720",   "7.1875",   "0.6250",   "0.6250"},
                {"GC074",   "5.5000",   "3.7500",   "0.6250",   "0.6250"},
                {"GC063",   "5.5000",   "4.5625",   "0.5000",   "0.6250"},
                {"GC088",   "5.5000",   "5.7500",   "0.6250",   "0.6250"},
                {"GC042",   "5.5000",   "6.0000",   "0.6250",   "0.6250"},
                {"GC060",   "5.5000",   "6.1667",   "0.5000",   "0.6250"},
                {"GC060-7", "5.5000",   "6.1667",   "0.6250",   "0.6250"},
                {"GC060-5", "5.5000",   "6.1667",   "0.5000",   "0.6250"},
                {"GC082",   "5.5000",   "6.2500",   "0.5000",   "0.5000"},
                {"GC064",   "5.5000",   "6.3125",   "0.5000",   "0.6250"},
                {"GC004",   "5.5000",   "6.5625",   "0.6250",   "0.6250"},
                {"GC076",   "5.5000",   "7.7500",   "0.6250",   "0.6250"},
                {"GC029",   "5.6000",   "4.0000",   "0.6250",   "0.6250"},
                {"GC031",   "5.6000",   "5.3750",   "0.6250",   "0.6250"},
                {"GC032",   "5.6500",   "7.0000",   "0.6250",   "0.6250"},
                {"GC043",   "5.6875",   "5.7500",   "0.6250",   "0.6250"},
                {"GC011",   "5.6875",   "5.9166",   "0.6250",   "0.6250"},
                {"GC045",   "5.7500",   "4.0000",   "0.6250",   "0.6250"},
                {"GC016",   "5.7500",   "6.0000",   "0.6250",   "0.6250"},
                {"GC006",   "5.7500",   "6.1670",   "0.6250",   "0.6250"},
                {"GC062",   "5.7500",   "6.6250",   "0.6250",   "0.6250"},
                {"GC046",   "5.7500",   "7.3750",   "0.6250",   "0.6250"},
                {"GC009",   "5.7500",   "8.0625",   "0.6250",   "0.6250"},
                {"GC027",   "6.0000",   "3.5000",   "0.6250",   "0.6250"},
                {"GC038",   "6.0000",   "3.7813",   "0.6250",   "0.6250"},
                {"GC085",   "6.0000",   "4.0000",   "0.6250",   "0.6250"},
                {"GC017",   "6.0000",   "6.0000",   "0.6250",   "0.6250"},
                {"GC049",   "6.0000",   "6.2500",   "0.6250",   "0.6250"},
                {"GC056",   "6.0000",   "6.2500",   "0.5000",   "0.6250"},
                {"GC051",   "6.0000",   "6.5625",   "0.6250",   "0.6250"},
                {"GC007",   "6.0000",   "6.8750",   "0.6250",   "0.6250"},
                {"GC070",   "6.0000",   "7.0000",   "0.6250",   "0.6250"},
                {"GC001",   "6.0000",   "7.6250",   "0.6250",   "0.6250"},
                {"GC034",   "6.0000",   "8.0625",   "0.6250",   "0.6250"},
                {"GC044",   "6.1250",   "5.5833",   "0.6250",   "0.6250"},
                {"GC058",   "6.1420",   "5.9167",   "0.6250",   "0.6250"},
                {"GC036",   "6.2000",   "6.5625",   "0.6250",   "0.6250"},
                {"GC065",   "6.2500",   "4.1250",   "0.6250",   "0.5000"},
                {"GC030",   "6.2500",   "4.5000",   "0.6250",   "0.6250"},
                {"GC012",   "6.2500",   "5.7500",   "0.6250",   "0.6250"},
                {"GC086",   "6.2500",   "6.2500",   "0.5000",   "0.6250"},
                {"GC078",   "6.3125",   "6.0000",   "0.6250",   "0.6250"},
                {"GC072",   "6.4375",   "5.6250",   "0.6250",   "0.7500"},
                {"GC081",   "6.5000",   "4.5000",   "0.6250",   "0.6250"},
                {"GC028",   "6.5000",   "7.3125",   "0.6250",   "0.6250"},
                {"GC026",   "6.5630",   "6.4375",   "0.6250",   "0.6250"},
                {"GC014",   "6.6250",   "6.0000",   "0.6250",   "0.6250"},
                {"GC023",   "6.6250",   "8.2500",   "0.6250",   "0.6250"},
                {"GC075",   "6.7500",   "7.7500",   "0.6250",   "0.6250"},
                {"GC054",   "7.0000",   "4.7500",   "0.6250",   "0.6250"},
                {"GC067",   "7.0000",   "5.2500",   "0.6250",   "0.6250"},
                {"GC083",   "7.0000",   "5.6250",   "0.6250",   "0.6250"},
                {"GC033",   "7.0250",   "7.3750",   "0.6250",   "0.6250"},
                {"GC069",   "7.5000",   "5.5000",   "0.5000",   "0.6250"},
                {"GC025",   "7.7500",   "6.5625",   "0.6250",   "0.6250"},
                {"GC039",   "8.0000",   "4.2500",   "0.6250",   "0.6250"},
                {"GC040",   "8.0000",   "4.5000",   "0.6250",   "0.6250"},
                {"GC002",   "8.0000",   "7.1250",   "0.6250",   "0.6250"},
                {"GC057",   "8.2500",   "4.7500",   "0.6250",   "0.6250"},
                {"GC077",   "8.2500",   "5.5000",   "0.6250",   "0.6250"},
                {"GC068",   "9.8750",   "7.6875",   "0.6250",   "0.6250"},
                {"GC047",   "10.5000",  "5.7500",   "0.6250",   "0.6250"},
            };

            string toolNumber = "CUSTOM";

            int bound0 = dataCLDalt.GetUpperBound(0);
            int bound1 = dataCLDalt.GetUpperBound(1);
            // finds upper bounds for data array

            double xPlay = 0.125;
            double yPlay = 0.125;

            int[,] isMatch = new int[(bound0 + 1), bound1];
            // initializes found match array

            int bound2 = isMatch.GetUpperBound(0);
            int bound3 = isMatch.GetUpperBound(1);
            // finds upper bounds for match array

            for (int i = 0; i <= bound2; i++)
            {
                for (int j = 0; j <= bound3; j++)
                {
                    isMatch[i, j] = 0;
                }
            }
            // fills found match array

            for (int i = 0; i <= bound0; i++)
            {
                if ((double.Parse(dataCLDalt[i, 1]) >= (webActual - xPlay)) && (double.Parse(dataCLDalt[i, 1]) <= (webActual + xPlay)))
                {
                    isMatch[i, 0] = 1;
                }
                if ((double.Parse(dataCLDalt[i, 2]) >= (repeatActual - yPlay)) && (double.Parse(dataCLDalt[i, 2]) <= (repeatActual + yPlay)))
                {
                    isMatch[i, 1] = 1;
                }
                if (double.Parse(dataCLDalt[i, 3]) == finsealActual)
                {
                    isMatch[i, 2] = 1;
                }
                if (double.Parse(dataCLDalt[i, 4]) == crimpActual)
                {
                    isMatch[i, 3] = 1;
                }
            }
            // finds matches

            double difference = 5555555;
            double location = -1;
            double width;
            double repeat;
            double absval;

            for (int i = 0; i <= bound2; i++)
            {
                if (isMatch[i, 0] == 1 && isMatch[i, 1] == 1 && isMatch[i, 2] == 1 && isMatch[i, 3] == 1)
                {
                    width = double.Parse(dataCLDalt[i, 1]);
                    repeat = double.Parse(dataCLDalt[i, 2]);
                    absval = Math.Abs(width - webActual);
                    absval += Math.Abs(repeat - repeatActual);
                    if (absval < difference)
                    {
                        difference = absval;
                        location = i;
                    }
                }
            }
            if (location != -1)
            {
                toolNumber = dataCLDalt[(int)location, 0];
            }

            return toolNumber;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //calculate button
            txtRelm.Text = "";
            txtCLD.Text = "";
            bool fillError = errorCheck();
            if (fillError == true)
            {
                MessageBox.Show("Please fill out the missing information.");
            }
            else
            {
                bool isNumeric = numericCheck();
                if (isNumeric == true)
                {
                    double widthActual = (double.Parse(txtWidth.Text))*2;
                    double HeightActual = (double.Parse(txtHeight.Text)) * 2;
                    double finsealActual = (double.Parse(txtFinseal.Text)) * 2;
                    double webActual = widthActual + HeightActual + finsealActual + 0.25;

                    double lengthActual = double.Parse(txtLength.Text);
                    double crimpActual = (double.Parse(txtCrimp.Text)) * 2;
                    double repeatActual = lengthActual + HeightActual + crimpActual + 0.25;

                    txtWeb.Text = webActual.ToString();
                    txtRepeat.Text = repeatActual.ToString();

                    if ((txtFinseal.Text == "0.625") && (txtCrimp.Text == "0.625"))
                    {
                        //txtRelm.Text = getRelm(webActual, repeatActual);
                        txtCLD.Text = getCLDalt(webActual, repeatActual, (finsealActual / 2), (crimpActual / 2));
                    }
                    else
                    {
                        //txtRelm.Text = "CUSTOM";
                        txtCLD.Text = getCLDalt(webActual, repeatActual, (finsealActual / 2), (crimpActual / 2));
                    }
                    btnExport.Visible = true;
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            
            exportForm exportForm1 = new exportForm(txtCLD.Text, txtRelm.Text, txtWeb.Text, txtRepeat.Text, txtFinseal.Text, txtHeight.Text, txtCrimp.Text);
            exportForm1.Show();
            this.Hide();
        }

        private void btnMasterCase_Click(object sender, EventArgs e)
        {
            masterCase masterCase1 = new masterCase();
            masterCase1.Show();
            this.Hide();
        }
    }
}