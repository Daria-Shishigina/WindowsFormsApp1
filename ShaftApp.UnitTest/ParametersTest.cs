using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using ShaftApp;

namespace ShaftApp.UnitTest
{
    [TestFixture]

    class ParametersTest
    {
        private Parameters _parameters;
 

        [SetUp]
        public void Test()
        {
            _parameters = new Parameters(10, 30, 20, 10, 10, 20);
        }

        [Test(Description = "Позитивный тест конструктора класса ")]
        public void TestParametrs_CorrectValue()
        {
            var expectedParameters = new Parameters(10, 30, 20, 10, 10, 20);
            var actual = _parameters;

            Assert.AreEqual
                (expectedParameters.DiameterBracing, actual.DiameterBracing,
                "Некорректное значение DiameterBracing");
            Assert.AreEqual
                (expectedParameters.DiameterHead, actual.DiameterHead,
                "Некорректное значение DiameterHead");
            Assert.AreEqual
                (expectedParameters.DiameterLeg, actual.DiameterLeg,
                "Некорректное значение DiameterLeg");
            Assert.AreEqual
                (expectedParameters.LengthBracing, actual.LengthBracing,
                "Некорректное значение LengthBracing");
            Assert.AreEqual
                (expectedParameters.LengthHead, actual.LengthHead,
                "Некорректное значение LengthHead");
            Assert.AreEqual
                (expectedParameters.LengthLeg, actual.LengthLeg,
             "Некорректное значение LengthLeg");
        }



        [TestCase(double.NegativeInfinity, 30, 20, 10, 10,20, 
       TestName = "Негативный тест на infinity поля DiameterBracing")]

        [TestCase(10, double.NegativeInfinity, 20, 10, 10, 20,
       TestName = "Негативный тест на infinity поля DiameterHead")]

        [TestCase(10, 30, double.NegativeInfinity, 10, 10, 20,
       TestName = "Негативный тест на infinity поля DiameterLeg")]

        [TestCase(10, 30, 20, double.NegativeInfinity, 10, 20,
       TestName = "Негативный тест на infinity поля LengthBracing")]

        [TestCase(10, 30, 20, 10, double.NegativeInfinity, 20,
       TestName = "Негативный тест на infinity поля LengthHead")]
        
        [TestCase(10, 30, 20, 10,10, double.NegativeInfinity,
       TestName = "Негативный тест на infinity поля LengthLeg")]



        [TestCase(double.NaN, 30, 20, 10, 10, 20,
       TestName = "Негативный тест на infinity поля DiameterBracing")]

        [TestCase(10, double.NaN, 20, 10, 10, 20,
       TestName = "Негативный тест на infinity поля DiameterHead")]

        [TestCase(10, 30, double.NaN, 10, 10, 20,
       TestName = "Негативный тест на infinity поля DiameterLeg")]

        [TestCase(10, 30, 20, double.NaN, 10, 20,
       TestName = "Негативный тест на infinity поля LengthBracing")]

        [TestCase(10, 30, 20, 10, double.NaN, 20,
       TestName = "Негативный тест на infinity поля LengthHead")]

        [TestCase(10, 30, 20, 10, 10, double.NaN,
       TestName = "Негативный тест на infinity поля LengthLeg")]

        //Максимальное значение


        [TestCase(30, 30, 20, 10, 10, 20,
            TestName = "Негативный тест поля DiameterBracing если > 20")]

        [TestCase(10, 50, 20, 10, 10, 20,
            TestName = "Негативный тест поля DiameterHead если > 40")]

        [TestCase(10, 30, 40, 10, 10, 20,
            TestName = "Негативный тест поля DiameterLeg>30")]

        [TestCase(10, 30, 20, 30, 10, 20, 
            TestName = "Негативный тест поля LengthBracing>25")]


        [TestCase(10, 30, 20, 10, 30, 20, 
            TestName = "Негативный тест поля LengthHead>20")]

        [TestCase(10, 30, 20, 10, 10, 50,
            TestName = "Негативный тест поля LengthLeg>40")]

        //Минимальное значение 

        [TestCase(1, 30, 20, 10, 10, 20,
            TestName = "Негативный тест поля DiameterBracing если <2")]

        [TestCase(10, 3, 20, 10, 10, 20,
            TestName = "Негативный тест поля DiameterHead если <4")]

        [TestCase(10, 30, 2, 10, 10, 20, 
            TestName = "Негативный тест поля DiameterLeg<3")]

        [TestCase(10, 30, 20, 1, 10, 20,
            TestName = "Негативный тест поля LengthBracing<2")]


        [TestCase(10, 30, 20, 10, 1, 20, 
            TestName = "Негативный тест поля LengthHead<2")]

        [TestCase(10, 30, 20, 10, 10, 3, //"LengthLeg",
            TestName = "Негативный тест поля LengthLeg<4")]

        //Соотношение 

        [TestCase(10, 15, 20, 10, 10, 20, 
           TestName = "Негативный тест поля DiameterLeg>DiameterHead")]


        [TestCase(20, 30, 15, 10, 10, 20,
           TestName = "Негативный тест поля DiameterBracing>DiameterLeg")]


        [TestCase(15, 30, 20, 10, 10, 20, 
           TestName = "Негативный тест поля LengthBracing<DianeterBtacing")]
        

        public void TestParametrs(double diameterBracing, double diameterHead, double diameterLeg,
            double lengthBracing, double lengthHead, double lengthLeg )
        {
            Assert.Throws<ArgumentException>(
                () =>
                {
                    var parameters = new Parameters
                        (diameterBracing, diameterHead, diameterLeg, 
                        lengthBracing, lengthHead,lengthLeg);
                },   "Должно возникнуть исключение");
        }

        

         


        //[Test(Description = " Негативный тест Diameter Head")]

        //public void TestDiameterHead_Less4()
        //{
        //    var wrongDiameter = 50;
        //    _parameters = new Parameters(_diameterBracing,
        //    _diameterHead, _diameterLeg, _lengthBracing, _lengthHead, _lengthLeg);

        //    Assert.Throws<ArgumentException>(
        //       () => { _parameters.DiameterHead = wrongDiameter; });
        //}





    }
}
