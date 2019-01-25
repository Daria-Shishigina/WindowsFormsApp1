using System;
using System.Windows.Forms;
using Kompas6API5;
using ShaftApp;

namespace ShaftAppForm
{
    public partial class GUI : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public GUI()
        {
            InitializeComponent();
            threadComboBox.Items.Add("-");
            threadComboBox.Items.Add("Head");
            threadComboBox.Items.Add("Leg");
            threadComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Подключение компаса 
        /// </summary>
        private KompasConnector _kompasConnector = new KompasConnector();

        /// <summary>
        /// Экземпляр компаса
        /// </summary>
        private KompasObject _kompas;
        
        /// <summary>
        /// Кнопка построения
        /// </summary>
        private void BuildButton_Click(object sender, EventArgs e)
        {
            try
            {
                Parameters parameters = new Parameters(Convert.ToDouble(diamBracingTextBox.Text),
                    Convert.ToDouble(diamHeadTextBox.Text),
                    Convert.ToDouble(diamLegTextBox.Text),
                    Convert.ToDouble(lengthBracingTextBox.Text),
                    Convert.ToDouble(lengthHeadTextBox.Text),
                    Convert.ToDouble(lengthLegTextBox.Text),
                    Convert.ToString(threadComboBox.Text));
                if (_kompas != null)
                {
                    DetailBuilder detailBuilder = new DetailBuilder(_kompas);
                    detailBuilder.BuildDetail(parameters);
                }
                if (_kompas == null)
                {
                    _kompasConnector.Connector();
                    _kompas = _kompasConnector.KompasObject;
                    BuildButton.Enabled = true;
                    DetailBuilder detailBuilder = new DetailBuilder(_kompas);
                    detailBuilder.BuildDetail(parameters);
                }              
            }      
            catch (ArgumentException exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        /// <summary>
        /// Валидация вводимых данных
        /// </summary>
        private void Validate(TextBox text)
        {
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

        /// <summary>
        /// Проверка изменения в текстовом поле
        /// </summary>
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Validate(((TextBox)sender));
        }
         
        /// <summary>
        /// Запрет на ввод букв 
        /// </summary>
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var num = e.KeyChar;
            if (!Char.IsDigit(e.KeyChar) && !((e.KeyChar == ',')) && (num != 8))
            {
                e.Handled = true;
            }
        }
    }
}
