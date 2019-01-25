using System;
using System.Collections.Generic;

namespace ShaftApp
{
    /// <summary>
    /// Класс параметров вала
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="diameterBracing">Диаметр крепления</param>
        /// <param name="diameterHead">Диаметр головки</param>
        /// <param name="diameterLeg">Диаметр ножки</param>
        /// <param name="lengthBracing">Длина крепления</param>
        /// <param name="lengthHead">Длина головки</param>
        /// <param name="lengthLeg">Длина ножки</param>
        public Parameters(double diameterBracing,double diameterHead, double diameterLeg, 
            double lengthBracing,  double lengthHead,    double lengthLeg, string thread)
        {
            if (double.IsNaN(diameterBracing) && !double.IsInfinity(diameterBracing))
            {
                throw new ArgumentException("Некорректное значение диаметра крепления.");
            }
            if (double.IsNaN(diameterHead) && !double.IsInfinity(diameterHead))
            {
                throw new ArgumentException("Некорректное значение диаметра головки.");
            }
            if (double.IsNaN(diameterLeg) && !double.IsInfinity(diameterLeg))
            {
                throw new ArgumentException("Некорректное значение диаметра ножки.");
            }
            if (double.IsNaN(lengthBracing) && !double.IsInfinity(lengthBracing))
            {
                throw new ArgumentException("Некорректное значение длины крепления.");
            }
            if (double.IsNaN(lengthHead) && !double.IsInfinity(lengthHead))
            {
                throw new ArgumentException("Некорректное значение длина головки.");
            }
            if (double.IsNaN(lengthLeg) && !double.IsInfinity(lengthLeg))
            {
                throw new ArgumentException("Некорректное значение длины ножки");
            }

            this.DiameterBracing = diameterBracing;
            this.DiameterHead = diameterHead;
            this.DiameterLeg = diameterLeg;
            this.LengthBracing = lengthBracing;
            this.LengthHead = lengthHead;
            this.LengthLeg = lengthLeg;
            this.Thread = thread; 
            Validate();
        }

        /// <summary>
        /// Валидация параметров по диапазону значений
        /// </summary>
        private void Validate()
        {
            var exeption = new List<string>();
            if (DiameterHead > 40 || DiameterHead < 4)
            {
                 exeption.Add("Диаметр головки должен быть от 4 до 40 см \n");
            }            
            if ( DiameterLeg > 30 || DiameterLeg < 3|| DiameterLeg>DiameterHead)
            {
                 exeption.Add("Диаметр ножки должен быть меньше диаметра головки, от 3 до 30 см \n");
            }
            if ( DiameterBracing > 20 || DiameterBracing < 2||DiameterBracing>DiameterLeg)
            {
                exeption.Add("Диаметр крепления должен быть меньше диаметра ножки, от 2 до 20 см \n");
            }
            if (LengthHead > 20 || LengthHead < 2)
            {
                exeption.Add("Длина головки должна быть от 2 до 20 см \n");
            } 
            if (LengthLeg  > 40 || LengthLeg  < 4)
            {
                exeption.Add("Длина ножки должна быть от 4 до 40 см \n");
            }
            if ( LengthBracing > 25 || LengthBracing  < 2 || LengthBracing < DiameterBracing)
            {
                exeption.Add("Длина крепления должна быть больше диаметра крепления, от 2 до 25 см ");
            }
            if (exeption.Count != 0)
            {
                var error = string.Empty;
                foreach (string e in exeption)
                {
                    error += e;
                }
                throw new ArgumentException(error);
            }
        }

        /// <summary>
        /// Диаметр крепоения
        /// </summary>
        public double DiameterBracing { get; private set; }

        /// <summary>
        /// Диаметр головки 
        /// </summary>
        public double DiameterHead { get; private set; }

        /// <summary>
        /// Диамтер ножки
        /// </summary>
        public double DiameterLeg { get; private set; }

        /// <summary>
        /// Длина крепления
        /// </summary>
        public double LengthBracing { get; private set; }

        /// <summary>
        /// Длина головки
        /// </summary>
        public double LengthHead { get; private set; }

        /// <summary>
        /// Длина ножки
        /// </summary>
        public double LengthLeg { get; private set; }

        /// <summary>
        /// Расположение резьбы
        /// </summary>
        public string Thread { get; private set; }
    }
}
