using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using ShaftApp;

namespace ShaftApp
{
    public class DetailBuilder
    {
        /// <summary>
        /// экземпляр компаса 
        /// </summary>
        private KompasConnector _kompasconnector;
        private KompasObject _kompas;


        private Document3D _doc3D;
        private ksEntity _entity;
        private ksPart _part;

        private ksRectangleParam _par; 




        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="kompasConnector"></param>
        public DetailBuilder(KompasObject kompas)
        {
            this._kompas = kompas;
        }

        /// <summary>
        /// Построение вала
        /// </summary>
        /// <param name="parameters"></param>

        public void BuildDetail(Parameters parameters)
        {
         
            _doc3D = _kompas.Document3D();
            _doc3D.Create(false, true);//FALSE - види­мый режим FALSE - види­мый режим


            BuildHead(parameters.DiameterHead, parameters.LengthHead); 
            BuildLeg(parameters.DiameterLeg,parameters.LengthLeg,parameters.LengthHead);
            BuildBracing(parameters.DiameterBracing, parameters.LengthBracing, parameters.LengthLeg,parameters.LengthHead);

            BuildChamfer(parameters.DiameterHead, parameters.DiameterLeg);
        }


        /// <summary>
        /// Создание головки
        /// </summary>

        private void BuildHead(double diameter, double length)
        {


            //Эскиз головки 

            #region Константы для эскиза

            const int pTop_part = -1;            //Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int o3d_sketch = 5;           //Эскиз
            const int o3d_planeXOY = 1;            // Плостость XOY.
            #endregion


            _part = _doc3D.GetPart(pTop_part);      //Получаем интерфейс 3D-модели
            
            _entity = _part.NewEntity(o3d_sketch);      //Получаем интерфейс объекта "Эскиз"
           
            ksSketchDefinition SketchDefinition = _entity.GetDefinition();       //Получаем интерфейс параметров эскиза
            
            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);       //Получаем интерфейс объекта "плоскость XOY"
       
            SketchDefinition.SetPlane(EntityPlane);     //Устанавливаем плоскость XOY базовой для эскиза

            _entity.Create();            //Создаем эскиз
        
            ksDocument2D Document2D = SketchDefinition.BeginEdit();     //Входим в режим редактирования эскиза

            Document2D.ksCircle(0, 0, diameter / 2, 1);            //Строим окружность 

            SketchDefinition.EndEdit();               //Выходим из режима редактирования эскиза




            //Выдавливание головки 

            #region Константы для выдавливания


            const int o3d_baseExtrusion = 24;            //Выдавливание 
            const int vm_Shaded = 3;            // Полутоновое изображение модели
            const int etBlind = 0;             //Выдавливание на глубину 
            #endregion


            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);            //Получаем интерфейс объекта "операция выдавливание"

            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();             //Получаем интерфейс параметров операции "выдавливание"

            BaseExtrusionDefinition.SetSideParam(true, etBlind, length, 0, true);            //Устанавливаем параметры операции выдавливания

            BaseExtrusionDefinition.SetSketch(_entity);             //Устанавливаем эскиз операции выдавливания

            EntityExtrusion.Create();            //Создаем операцию выдавливания

            _doc3D.drawMode = vm_Shaded;            //Устанавливаем полутоновое изображение модели

            _doc3D.shadedWireframe = true;            //Включаем отображение каркаса

        }


        /// <summary>
        /// Создание ножки
        /// </summary>

        private void BuildLeg(double diameter,double length,double leng)
        {

            //Эскиз ножки

            #region Константы для эскиза

            const int pTop_part = -1;             // Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int o3d_sketch = 5;           //Эскиз
            const int o3d_planeXOY = 1;            // Плоскость XOY.
            const int o3d_planeOffset = 14;          //Выдавливание вырезанием 

            #endregion


            _part = _doc3D.GetPart(pTop_part);           //Получаем интерфейс 3D-модели 

            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);  //Получаем интерфейс объекта "плоскость XO"

            ///Смещение плоскости

            ksEntity PlaneOff = _part.NewEntity(o3d_planeOffset);  //Получаем интерфейс объекта "смещенная плоскость"
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition();   //Получаем интерфейс параметров смещенной плоскости 
            planeOffsetDefinition.direction = true;      //Направление смещения - прямое
            planeOffsetDefinition.offset = leng;   //Смещение
            planeOffsetDefinition.SetPlane(EntityPlane); //Устанавливаем базовую плоскость
            PlaneOff.Create();            //Создаем эскиз плоскости


            ksEntity Entity = _part.NewEntity(o3d_sketch);      ////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();   //Получаем интерфейс параметров эскиза
            sketchDefinition.SetPlane(PlaneOff);  //Устанавливаем смещенную плоскость базовой для эскиза 
            Entity.Create();   //Создаем эскиз 
            ksDocument2D Document2D = sketchDefinition.BeginEdit();  //Входим в режим редактирования эскиза

            Document2D.ksCircle(0, 0, diameter / 2, 1); //Строим окружность 
            sketchDefinition.EndEdit();            //Выходим из режима редактирования эскиза


            //Выдавливание ножки 

            #region Константы для выдавливания

 
            const int o3d_baseExtrusion = 24;           //Выдавливание
            const int vm_Shaded = 3;            // Полутоновое изображение модели
            const int etBlind = 0;           //Тип выдавливания. Строго на глубину
            #endregion


            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);   //Получаем интерфейс объекта "операция выдавливание"
           
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition(); //Получаем интерфейс параметров операции "выдавливание"

            BaseExtrusionDefinition.SetSideParam(true, etBlind, length, 0, true);//Устанавливаем параметры операции выдавливания

            BaseExtrusionDefinition.SetSketch(Entity);  //Устанавливаем эскиз операции выдавливания
      
            EntityExtrusion.Create();      //Создаем операцию выдавливания

       
            _doc3D.drawMode = vm_Shaded;     //Устанавливаем полутоновое изображение модели
      
            _doc3D.shadedWireframe = true;      //Включаем отображение каркаса

        }



        /// <summary>
        /// Создание крепления
        /// </summary>

        private void BuildBracing(double diameter, double length, double leng,double len)
        {


            //Эскиз ножки

            #region Константы для эскиза
        
            const int pTop_part = -1;    // Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.  
            const int o3d_sketch = 5; //Эскиз
            const int o3d_planeXOY = 1;   // Плоскость XOY.
            const int o3d_planeOffset = 14;//Смещённая плоскость
            const int ko_RectangleParam = 91;//Прямоугольник по центру

            #endregion


            _part = _doc3D.GetPart(pTop_part);    //Получаем интерфейс 3D-модели 

         
            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);   //Получаем интерфейс объекта "плоскость XO"


            ///Смещение плоскости

            ksEntity PlaneOff = _part.NewEntity(o3d_planeOffset);   //Получаем интерфейс объекта "смещенная плоскость"
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition();  //Получаем интерфейс параметров смещенной плоскости
            planeOffsetDefinition.direction = true; //Направление смещения - прямое 
            planeOffsetDefinition.offset = leng+len; //Смещение 
            planeOffsetDefinition.SetPlane(EntityPlane);//Устанавливаем базовую плоскость 
            PlaneOff.Create();  //Создаем эскиз



        

            ksEntity Entity = _part.NewEntity(o3d_sketch);    ////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();//Получаем интерфейс параметров эскиза 
            sketchDefinition.SetPlane(PlaneOff);//Устанавливаем смещенную плоскость базовой для эскиза 
            Entity.Create();  //Создаем эскиз
            ksDocument2D Document2D = sketchDefinition.BeginEdit(); //Входим в режим редактирования эскиза 

            Document2D.ksCircle(0, 0, diameter / 2, 1);       //Строим окружность 
            sketchDefinition.EndEdit();        //Выходим из режима редактирования эскиза


            //Выдавливание ножки 

            #region Константы для выдавливания

    
            const int o3d_baseExtrusion = 24;        //Выдавливание
            const int vm_Shaded = 3;     //  полутоновое изображение модели
            const int etBlind = 0;     //Выдавливание на глубину 
            #endregion

           
            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion); //Получаем интерфейс объекта "операция выдавливание"
            
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();//Получаем интерфейс параметров операции "выдавливание

            BaseExtrusionDefinition.SetSideParam(true, etBlind, length, 0, true);    //Устанавливаем параметры операции выдавливания

            BaseExtrusionDefinition.SetSketch(Entity);//Устанавливаем эскиз операции выдавливания
          
            EntityExtrusion.Create();  //Создаем операцию выдавливания

            _doc3D.drawMode = vm_Shaded;    //Устанавливаем полутоновое изображение модели
       
            _doc3D.shadedWireframe = true;     //Включаем отображение каркаса




            //Эскиз крепления 

            ///Смещение плоскости

            ksEntity PlaneOff2 = _part.NewEntity(o3d_planeOffset);     //Включаем отображение каркаса
            ksPlaneOffsetDefinition planeOffsetDefinition2 = PlaneOff2.GetDefinition();//Получаем интерфейс параметров смещенной плоскости
            planeOffsetDefinition2.direction = true;  //Направление смещения - прямое 
            planeOffsetDefinition2.offset = leng + len+length; //Смещение
            planeOffsetDefinition2.SetPlane(EntityPlane);  //Устанавливаем базовую плоскост
            PlaneOff2.Create();//Создаем эскиз

            

            ksEntity Entity2 = _part.NewEntity(o3d_sketch);////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinition2 = Entity2.GetDefinition();//Получаем интерфейс параметров эскиза 
            sketchDefinition2.SetPlane(PlaneOff2);   //Устанавливаем смещенную плоскость базовой для эскиза
            Entity2.Create();//Создаем эскиз
            ksDocument2D Document2D2 = sketchDefinition2.BeginEdit();//Входим в режим редактирования эскиз

            _par = _kompas.GetParamStruct(ko_RectangleParam);  //Получаем интерфейс параметров прямоугольника 

            _par.height = diameter / 2;
            _par.width = diameter;
            _par.x = -(diameter / 2);
            _par.y = -(diameter / 4);
            _par.ang = 0;
            _par.style = 1;
            Document2D2.ksRectangle(_par, 0);
            sketchDefinition2.EndEdit();  //Выходим из режима редактирования эскиза




            //Выдавливание крепления 

            #region Константы для выдавливания
            const int o3d_cutExtrusion = 26;      //Выдавливание
            #endregion

            ksEntity EntityCutExtrusion = _part.NewEntity(o3d_cutExtrusion);   //Получаем интерфейс объекта "операция вырезание выдавливанием"   

            ksCutExtrusionDefinition CutExtrusionDefinition = EntityCutExtrusion.GetDefinition();   //Получаем интерфейс параметров операции 

            CutExtrusionDefinition.cut = true;   //Вычитание элементов ,,,,,,,,,,,,,,,,,,,,,
             
            CutExtrusionDefinition.directionType = 0;  //Прямое направление 
              
            CutExtrusionDefinition.SetSideParam(true, etBlind, length / 2, 0, false);//Устанавливаем параметры выдавливания  
          
            CutExtrusionDefinition.SetSketch(Entity2);  //Устанавливаем экиз операции   
        
            EntityCutExtrusion.Create();    //Создаем операцию вырезания выдавливанием 

            _doc3D.drawMode = vm_Shaded; //Устанавливаем полутоновое изображение модели
            _doc3D.shadedWireframe = true;  //Включаем отображение каркаса



            //Эскиз отверстия крепения 

            #region Константы для эскиза

        
            const int o3d_planeXOZ = 2;    // Плоскость XOZ.

            #endregion


            _part = _doc3D.GetPart(pTop_part);  //Получаем интерфейс 3D-модели 

            ksEntity EntityPlane3 = _part.GetDefaultEntity(o3d_planeXOZ); //Получаем интерфейс объекта "плоскость XOZ"


            ///Смещение плоскости


            ksEntity PlaneOff3 = _part.NewEntity(o3d_planeOffset); //Получаем интерфейс объекта "смещенная плоскость
            ksPlaneOffsetDefinition planeOffsetDefinition3 = PlaneOff3.GetDefinition(); //Получаем интерфейс параметров смещенной плоскости
            planeOffsetDefinition3.direction = true;   //Направление смещения - прямое 
            planeOffsetDefinition3.offset = diameter / 2;   //Смещение 
            planeOffsetDefinition3.SetPlane(EntityPlane3);  //Устанавливаем базовую плоскость
            PlaneOff3.Create();   //Создаем смещенную плоскость 




            ksEntity Entity3 = _part.NewEntity(o3d_sketch);////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinition3 = Entity3.GetDefinition(); //Получаем интерфейс параметров эскиза
            sketchDefinition3.SetPlane(PlaneOff3); //Устанавливаем смещенную плоскость базовой для эскиза 
            Entity3.Create(); //Создаем эскиз 
            ksDocument2D Document2D3 = sketchDefinition3.BeginEdit();//Входим в режим редактирования эскиза 

           
            Document2D3.ksCircle(0, -(0.75*length+leng+len), diameter / 6, 1); //Строим окружность 
            sketchDefinition3.EndEdit();//Выходим из режима редактирования эскиза




            //Выдавливание отверстия крепления

            

            ksEntity EntityCutExtrusion2 = _part.NewEntity(o3d_cutExtrusion);  //Получаем интерфейс объекта "операция вырезание выдавливанием" 
   
            ksCutExtrusionDefinition CutExtrusionDefinition2 = EntityCutExtrusion2.GetDefinition(); //Получаем интерфейс параметров операции
            
            CutExtrusionDefinition2.cut = true;//Вычитание элементов  
           
            CutExtrusionDefinition2.directionType = 0;    //Прямое направление 
            
            CutExtrusionDefinition2.SetSideParam(true, etBlind, diameter, 0, false); //Устанавливаем параметры выдавливания   
           
            CutExtrusionDefinition2.SetSketch(Entity3); //Устанавливаем экиз операции   
           
            EntityCutExtrusion2.Create(); //Создаем операцию вырезания выдавливанием 

            _doc3D.drawMode = vm_Shaded;  //Устанавливаем полутоновое изображение модели
          
            _doc3D.shadedWireframe = true;  //Включаем отображение каркаса


        }


        /// <summary>
        ///Создание фаски 
        /// </summary>
        /// 
        private void BuildChamfer(double diameter, double diamete)

        {

            #region Константы для фаски
    
            const int o3d_face = 6;         //Поверхность
      
            const int o3d_chamfer = 33;      // Фаска
          
            double index = 1.6;             //Устанавливаем tg 45 
            #endregion

   

            ksEntity EntityChamferIn = (_part.NewEntity(o3d_chamfer));   //Получаем интерфейс объекта "фаска"

            ksChamferDefinition ChamferDefinitionIn = EntityChamferIn.GetDefinition(); //Получаем интерфейс параметров объекта 
          
            ChamferDefinitionIn.tangent = false;  //Не продолжать по касательным ребрам
     
            ChamferDefinitionIn.SetChamferParam(true, diameter - diamete, (diameter - diamete) / index);       //Устанавливаем параметры фаски 

            ksEntityCollection EntityCollectionPart = (_part.EntityCollection(o3d_face)); //Получаем массив поверхностей детали

            ksEntityCollection EntityCollectionChamferIn = (ChamferDefinitionIn.array());   //Получаем массив поверхностей, на которых будет строиться фаска

            EntityCollectionChamferIn.Clear();

            //Заполняем массив поверхностей, на которых будет строится фаска

            EntityCollectionChamferIn.Add(EntityCollectionPart.GetByIndex(1));
           

            EntityCollectionChamferIn.Add(EntityCollectionPart.GetByIndex(4));
     
            EntityChamferIn.Create();      //Создаем фаску

        }

    }
}
