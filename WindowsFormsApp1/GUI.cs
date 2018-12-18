using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Convert.ToDouble(lengthLegTextBox.Text));

                _kompasConnector.Connector();
                _kompas = _kompasConnector.KompasObject;
                buildButton.Enabled = false;


                DetailBuilder detailBuilder = new DetailBuilder(_kompas);
                detailBuilder.BuildDetail(parameters);

            }

            catch (ArgumentException exeption)

            {
               MessageBox.Show(exeption.Message);
            }

        }


        private void Validate()
        {


            //////////////////////////////////////////////ввод букв ограничение цветом+ вывод ошибки 


        }

    }
}
