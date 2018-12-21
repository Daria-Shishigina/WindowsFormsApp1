using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Kompas6API5;
using ShaftApp;

namespace ShaftAppForm
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();

            threadComboBox.Items.Add("-");
            threadComboBox.Items.Add("Head");
            threadComboBox.Items.Add("Leg");

            threadComboBox.SelectedIndex = 0;
        }

        private DetailBuilder _detailBuilder;
     
        private KompasConnector _kompasConnector = new KompasConnector();
        private Parameters _parameters;
        private KompasObject _kompas;

        private void buildButton_Click(object sender, EventArgs e)
        {
            try
            {

                Parameters parameters = new Parameters (Convert.ToDouble(diamBracingTextBox.Text),
                    Convert.ToDouble(diamHeadTextBox.Text),
                    Convert.ToDouble(diamLegTextBox.Text),
                    Convert.ToDouble(lengthBracingTextBox.Text),
                    Convert.ToDouble(lengthHeadTextBox.Text),
                    Convert.ToDouble(lengthLegTextBox.Text),
                    Convert.ToString(threadComboBox.Text));


                _kompasConnector.Connector();
                _kompas = _kompasConnector.KompasObject;
                buildButton.Enabled = true;
              //  buildButton.Enabled = false;

                DetailBuilder detailBuilder = new DetailBuilder(_kompas);

                

                detailBuilder.BuildDetail(parameters);

            }

            catch (ArgumentException exeption)

            {
               MessageBox.Show(exeption.Message);
            }

        }


      
        private void Validate(TextBox text)
        {
            /////ввод букв ограничение

            double d = 0;
            if (!double.TryParse(text.Text, out d))
            {
                errorProvider.SetError(text, "Некорректно введено значение!");

            }
            else
            {
                errorProvider.SetError(text, "");
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            

            Validate(((TextBox)sender));
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var num = e.KeyChar;

            if (!Char.IsDigit(e.KeyChar) && !((e.KeyChar == ',')) && (num != 8))
            {
                e.Handled = true;
            }
        }



        //private void threadComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch(threadComboBox.SelectedIndex)
        //    {
        //        case 0:
        //            {
        //                break;
        //            }
        //        case 1:
        //            {






        //                break;
        //            }
        //        case 2:
        //            {




        //                break;
        //            }
                
        //    }
        //}
    }
}
