using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShaftApp;

namespace ShaftApp
{
    public class Parameters
    {
        /// <summary>
        /// Описание полей
        /// </summary>
        private double _diameterBracing;
        private double _diameterHead;
        private double _diameterLeg;
        private double _lengthBracing;
        private double _lengthHead;
        private double _lengthLeg;
        private string _thread;     ///////////////////////////////




        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="diameterBracing"></param>
        /// <param name="diameterHead"></param>
        /// <param name="diameterLeg"></param>
        /// <param name="lengthBracing"></param>
        /// <param name="lengthHead"></param>
        /// <param name="lengthLeg"></param>
        /// 
        public Parameters(double diameterBracing,double diameterHead, double diameterLeg, double lengthBracing,  double lengthHead,    double lengthLeg, string thread)//////////////
        {
            //TODO: NaN Бесконечность 


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




            this._diameterBracing = diameterBracing;
            this._diameterHead = diameterHead;
            this._diameterLeg = diameterLeg;
            this._lengthBracing = lengthBracing;
            this._lengthHead = lengthHead;
            this._lengthLeg = lengthLeg;
            this._thread = thread; ///////////////////////////////////////////////////////

              Validate();

        }



        private void Validate()
        {
            var exeption = new List<string>();


            if (_diameterHead > 40 || _diameterHead < 4)
            {
                 exeption.Add("Диаметр головки должен быть от 4 до 40 см \n");
                //throw new ArgumentException("Диаметр головки должен быть от 4 до 40 см");
            }



            if ( _diameterLeg > 30 || _diameterLeg < 3|| _diameterLeg>_diameterHead)
            {
                 exeption.Add("Диаметр ножки должен быть меньше диаметра головки, от 3 до 30 см \n");
                //throw new ArgumentException("Диаметр ножки должен быть меньше диаметра головки , от 3 до 30 см");
            }


            if ( _diameterBracing > 20 || _diameterBracing < 2||_diameterBracing>_diameterLeg)
            {
                exeption.Add("Диаметр крепления должен быть меньше диаметра ножки, от 2 до 20 см \n");
                //throw new ArgumentException("Диаметр крепления должен быть меньше диаметра ножки, от 2 до 20 см");
            }


       

            if (_lengthHead > 20 || _lengthHead < 2)
            {
                exeption.Add("Длина головки должна быть от 2 до 20 см \n");
                //throw new ArgumentException("Длина крепления должна быть меньше длины ножки, от 2 до 20 см");
            } 


            if (_lengthLeg  > 40 || _lengthLeg  < 4)
            {
                exeption.Add("Длина ножки должна быть от 4 до 40 см \n");
                //throw new ArgumentException("Длина ножки должна быть от 4 до 40 см");
            }


            if ( _lengthBracing > 25 || _lengthBracing  < 2 || _lengthBracing < _diameterBracing)//
            {
                exeption.Add("Длина крепления должна быть больше диаметра крепления, от 2 до 25 см ");
                // throw new ArgumentException("Длина крепления должна быть меньше длины ножки, от 2 до 25 см \n");
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

        public double DiameterBracing => _diameterBracing;
        public double DiameterHead => _diameterHead;
        public double DiameterLeg => _diameterLeg;
        public double LengthBracing => _lengthBracing;
        public double LengthHead => _lengthHead;
        public double LengthLeg => _lengthLeg;
        public string Thread => _thread;///////////////////////////////////



        //public double DiameterBracing {
        //    get { return _diameterBracing; }
        //    set { _diameterBracing = value; } }
    }


}
