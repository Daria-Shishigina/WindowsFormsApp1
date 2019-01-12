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


       // private KompasConnector _kompasconnector;


        private KompasObject _kompas;


        private Document3D _doc3D;
        private ksEntity _entity;
        private ksPart _part;
     
 



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
            BuildLeg(parameters.DiameterLeg, parameters.LengthLeg, parameters.LengthHead);
            BuildBracing(parameters.DiameterBracing, parameters.LengthBracing, parameters.LengthLeg, parameters.LengthHead);


            switch (parameters.Thread)
            {
                case "-":

            
            //           BuildChamfer(parameters.DiameterHead, parameters.DiameterLeg);

                    break;


                case "Head":

                    BuildThread(parameters.DiameterHead, parameters.LengthHead, parameters.DiameterLeg);
                   
                    break;


                case "Leg":

                    BuildThread2(parameters.DiameterLeg, parameters.LengthLeg, parameters.DiameterBracing, parameters.LengthHead);

                    break;
            }
        }


        /// <summary>
        /// Создание головки
        /// </summary>

        private void BuildHead(double diameterHead, double lengthHead)
        {


            //Эскиз головки 

            #region Константы для эскиза

            const int pTop_part = -1;            //Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int o3d_sketch = 5;           //Эскиз
            const int o3d_planeXOY = 1;            // Плостость XOY.
            #endregion

            _part = _doc3D.GetPart(pTop_part);                                  //Получаем интерфейс 3D-модели 
            
            _entity = _part.NewEntity(o3d_sketch);                              //Получаем интерфейс объекта "Эскиз"
           
            ksSketchDefinition SketchDefinition = _entity.GetDefinition();       //Получаем интерфейс параметров эскиза
            
            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);          //Получаем интерфейс объекта "плоскость XOY"
       
            SketchDefinition.SetPlane(EntityPlane);                                //Устанавливаем плоскость XOY базовой для эскиза

            _entity.Create();                                                        //Создаем эскиз
        
            ksDocument2D Document2D = SketchDefinition.BeginEdit();                 //Входим в режим редактирования эскиза

            Document2D.ksCircle(0, 0, diameterHead / 2, 1);                              //Строим окружность 

            SketchDefinition.EndEdit();                                              //Выходим из режима редактирования эскиза




            //Выдавливание головки 

            #region Константы для выдавливания


            const int o3d_baseExtrusion = 24;            //Выдавливание 
            const int vm_Shaded = 3;            // Полутоновое изображение модели
            const int etBlind = 0;             //Выдавливание на глубину 
            #endregion


            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);                           //Получаем интерфейс объекта "операция выдавливание"

            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();    //Получаем интерфейс параметров операции "выдавливание"

            BaseExtrusionDefinition.SetSideParam(true, etBlind, lengthHead, 0, true);                   //Устанавливаем параметры операции выдавливания

            BaseExtrusionDefinition.SetSketch(_entity);                                             //Устанавливаем эскиз операции выдавливания

            EntityExtrusion.Create();                                                               //Создаем операцию выдавливания

            _doc3D.drawMode = vm_Shaded;                                                            //Устанавливаем полутоновое изображение модели

            _doc3D.shadedWireframe = true;                                                           //Включаем отображение каркаса

        }


        /// <summary>
        /// Создание ножки
        /// </summary>

        private void BuildLeg(double diameterLeg,double lengthLeg,double lengthHead)
        {

            //Эскиз ножки

            #region Константы для эскиза

            const int pTop_part = -1;             // Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int o3d_sketch = 5;           //Эскиз
            const int o3d_planeXOY = 1;            // Плоскость XOY.
            const int o3d_planeOffset = 14;          //Выдавливание вырезанием 

            #endregion


            _part = _doc3D.GetPart(pTop_part);                                           //Получаем интерфейс 3D-модели 
            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);                 //Получаем интерфейс объекта "плоскость XO"

            ///Смещение плоскости

            ksEntity PlaneOff = _part.NewEntity(o3d_planeOffset);                       //Получаем интерфейс объекта "смещенная плоскость"
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition();   //Получаем интерфейс параметров смещенной плоскости 
            planeOffsetDefinition.direction = true;                                     //Направление смещения - прямое
            planeOffsetDefinition.offset = lengthHead;                                   //Смещение
            planeOffsetDefinition.SetPlane(EntityPlane);                                //Устанавливаем базовую плоскость
            PlaneOff.Create();                                                           //Создаем эскиз плоскости


            ksEntity Entity = _part.NewEntity(o3d_sketch);                              ////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();               //Получаем интерфейс параметров эскиза
            sketchDefinition.SetPlane(PlaneOff);                                        //Устанавливаем смещенную плоскость базовой для эскиза 
            Entity.Create();                                                            //Создаем эскиз 
            ksDocument2D Document2D = sketchDefinition.BeginEdit();                     //Входим в режим редактирования эскиза

            Document2D.ksCircle(0, 0, diameterLeg / 2, 1);                                 //Строим окружность 
            sketchDefinition.EndEdit();                                                  //Выходим из режима редактирования эскиза


            //Выдавливание ножки 

            #region Константы для выдавливания

 
            const int o3d_baseExtrusion = 24;           //Выдавливание
            const int vm_Shaded = 3;            // Полутоновое изображение модели
            const int etBlind = 0;           //Тип выдавливания. Строго на глубину
            #endregion


            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);                       //Получаем интерфейс объекта "операция выдавливание"
           
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition(); //Получаем интерфейс параметров операции "выдавливание"

            BaseExtrusionDefinition.SetSideParam(true, etBlind, lengthLeg, 0, true);                 //Устанавливаем параметры операции выдавливания

            BaseExtrusionDefinition.SetSketch(Entity);                                           //Устанавливаем эскиз операции выдавливания
      
            EntityExtrusion.Create();                                                             //Создаем операцию выдавливания

            _doc3D.drawMode = vm_Shaded;                                                          //Устанавливаем полутоновое изображение модели
      
            _doc3D.shadedWireframe = true;                                                        //Включаем отображение каркаса

        }



        /// <summary>
        /// Создание крепления
        /// </summary>

        private void BuildBracing(double diameterBracing, double lengthBracing, double lengthLeg,double lengthHead)
        {
             

            //Эскиз крепления

            #region Константы для эскиза
        
            const int pTop_part = -1;    // Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.  
            const int o3d_sketch = 5; //Эскиз
            const int o3d_planeXOY = 1;   // Плоскость XOY.
            const int o3d_planeOffset = 14;//Смещённая плоскость
            const int ko_RectangleParam = 91;//Прямоугольник по центру

            #endregion


            _part = _doc3D.GetPart(pTop_part);                                           //Получаем интерфейс 3D-модели 

            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);                 //Получаем интерфейс объекта "плоскость XO"


            ///Смещение плоскости

            ksEntity PlaneOff = _part.NewEntity(o3d_planeOffset);                       //Получаем интерфейс объекта "смещенная плоскость"
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition();  //Получаем интерфейс параметров смещенной плоскости
            planeOffsetDefinition.direction = true;                                     //Направление смещения - прямое 
            planeOffsetDefinition.offset = lengthLeg+lengthHead;                                    //Смещение 
            planeOffsetDefinition.SetPlane(EntityPlane);                                //Устанавливаем базовую плоскость 
            PlaneOff.Create();                                                           //Создаем эскиз



        

            ksEntity Entity = _part.NewEntity(o3d_sketch);                              ////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();               //Получаем интерфейс параметров эскиза 
            sketchDefinition.SetPlane(PlaneOff);                                        //Устанавливаем смещенную плоскость базовой для эскиза 
            Entity.Create();                                                            //Создаем эскиз
            ksDocument2D Document2D = sketchDefinition.BeginEdit();                      //Входим в режим редактирования эскиза 

            Document2D.ksCircle(0, 0, diameterBracing / 2, 1);                                   //Строим окружность 
            sketchDefinition.EndEdit();                                                   //Выходим из режима редактирования эскиза


            //Выдавливание

            #region Константы для выдавливания

    
            const int o3d_baseExtrusion = 24;        //Выдавливание
            const int vm_Shaded = 3;     //  полутоновое изображение модели
            const int etBlind = 0;     //Выдавливание на глубину 
            #endregion

           
            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);                           //Получаем интерфейс объекта "операция выдавливание"
            
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();    //Получаем интерфейс параметров операции "выдавливание

            BaseExtrusionDefinition.SetSideParam(true, etBlind, lengthBracing, 0, true);                    //Устанавливаем параметры операции выдавливания

            BaseExtrusionDefinition.SetSketch(Entity);                                              //Устанавливаем эскиз операции выдавливания
          
            EntityExtrusion.Create();                                                                //Создаем операцию выдавливания

            _doc3D.drawMode = vm_Shaded;                                                              //Устанавливаем полутоновое изображение модели
       
            _doc3D.shadedWireframe = true;                                                            //Включаем отображение каркаса




            //Эскиз крепления 

            ///Смещение плоскости

            ksEntity PlaneOffSet = _part.NewEntity(o3d_planeOffset);                              //Включаем отображение каркаса
            ksPlaneOffsetDefinition planeOffDefinition = PlaneOffSet.GetDefinition();         //Получаем интерфейс параметров смещенной плоскости
            planeOffDefinition.direction = true;                                            //Направление смещения - прямое 
            planeOffDefinition.offset = lengthLeg + lengthHead+lengthBracing;                                  //Смещение
            planeOffDefinition.SetPlane(EntityPlane);                                       //Устанавливаем базовую плоскост
            PlaneOffSet.Create();                                                                 //Создаем эскиз

            

            ksEntity EntityRectangle = _part.NewEntity(o3d_sketch);                                      ////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinitionRectangle = EntityRectangle.GetDefinition();                         //Получаем интерфейс параметров эскиза 
            sketchDefinitionRectangle.SetPlane(PlaneOffSet);                                                  //Устанавливаем смещенную плоскость базовой для эскиза
            EntityRectangle.Create();                                                                       //Создаем эскиз
            ksDocument2D Document2 = sketchDefinitionRectangle.BeginEdit();                               //Входим в режим редактирования эскиз
            ksRectangleParam _par;
            _par = _kompas.GetParamStruct(ko_RectangleParam);                                       //Получаем интерфейс параметров прямоугольника 

            _par.height = diameterBracing / 2;
            _par.width = diameterBracing;
            _par.x = -(diameterBracing / 2);
            _par.y = -(diameterBracing / 4);
            _par.ang = 0;
            _par.style = 1;
            Document2.ksRectangle(_par, 0);
            sketchDefinitionRectangle.EndEdit();                                                             //Выходим из режима редактирования эскиза




            //Выдавливание крепления 

            #region Константы для выдавливания
            const int o3d_cutExtrusion = 26;      //Выдавливание
            #endregion

            ksEntity EntityCutExtrusion = _part.NewEntity(o3d_cutExtrusion);                            //Получаем интерфейс объекта "операция вырезание выдавливанием"   

            ksCutExtrusionDefinition CutExtrusionDefinition = EntityCutExtrusion.GetDefinition();       //Получаем интерфейс параметров операции 

            CutExtrusionDefinition.cut = true;                                                          //Вычитание элементов ,
             
            CutExtrusionDefinition.directionType = 0;                                                   //Прямое направление 
              
            CutExtrusionDefinition.SetSideParam(true, etBlind, lengthBracing / 2, 0, false);             //Устанавливаем параметры выдавливания  
          
            CutExtrusionDefinition.SetSketch(EntityRectangle);                                                   //Устанавливаем экиз операции   
        
            EntityCutExtrusion.Create();                                                                 //Создаем операцию вырезания выдавливанием 

            _doc3D.drawMode = vm_Shaded;                                                                 //Устанавливаем полутоновое изображение модели
            _doc3D.shadedWireframe = true;                                                               //Включаем отображение каркаса



            //Эскиз отверстия крепения 

            #region Константы для эскиза

        
            const int o3d_planeXOZ = 2;    // Плоскость XOZ.

            #endregion


            _part = _doc3D.GetPart(pTop_part);                                                          //Получаем интерфейс 3D-модели 

            ksEntity EntityPlane3 = _part.GetDefaultEntity(o3d_planeXOZ);                               //Получаем интерфейс объекта "плоскость XOZ"


            ///Смещение плоскости


            ksEntity PlaneOffHole = _part.NewEntity(o3d_planeOffset);                                      //Получаем интерфейс объекта "смещенная плоскость
            ksPlaneOffsetDefinition planeOffsetDefinitionHole = PlaneOffHole.GetDefinition();                 //Получаем интерфейс параметров смещенной плоскости
            planeOffsetDefinitionHole.direction = true;                                                   //Направление смещения - прямое 
            planeOffsetDefinitionHole.offset = diameterBracing / 2;                                               //Смещение 
            planeOffsetDefinitionHole.SetPlane(EntityPlane3);                                               //Устанавливаем базовую плоскость
            PlaneOffHole.Create();                                                                         //Создаем смещенную плоскость 




            ksEntity EntityHole = _part.NewEntity(o3d_sketch);                                             ////Получаем интерфейс объекта "Эскиз"
            ksSketchDefinition sketchDefinitionHole = EntityHole.GetDefinition();                              //Получаем интерфейс параметров эскиза
            sketchDefinitionHole.SetPlane(PlaneOffHole);                                                       //Устанавливаем смещенную плоскость базовой для эскиза 
            EntityHole.Create();                                                                               //Создаем эскиз 
            ksDocument2D Document2D3 = sketchDefinitionHole.BeginEdit();                                       //Входим в режим редактирования эскиза 

           
            Document2D3.ksCircle(0, -(0.75*lengthBracing+lengthLeg+lengthHead), diameterBracing / 6, 1);    //Строим окружность 
            sketchDefinitionHole.EndEdit();                                                                    //Выходим из режима редактирования эскиза




            //Выдавливание отверстия крепления

            

            ksEntity EntityCutExtrusionHole = _part.NewEntity(o3d_cutExtrusion);                               //Получаем интерфейс объекта "операция вырезание выдавливанием" 
   
            ksCutExtrusionDefinition CutExtrusionDefinitionHole = EntityCutExtrusionHole.GetDefinition();         //Получаем интерфейс параметров операции
            
            CutExtrusionDefinitionHole.cut = true;                                                             //Вычитание элементов  
           
            CutExtrusionDefinitionHole.directionType = 0;                                                      //Прямое направление 
            
            CutExtrusionDefinitionHole.SetSideParam(true, etBlind, diameterBracing, 0, false);                         //Устанавливаем параметры выдавливания   
           
            CutExtrusionDefinitionHole.SetSketch(EntityHole);                                                      //Устанавливаем экиз операции   
           
            EntityCutExtrusionHole.Create();                                                                    //Создаем операцию вырезания выдавливанием 

            _doc3D.drawMode = vm_Shaded;                                                                     //Устанавливаем полутоновое изображение модели
          
            _doc3D.shadedWireframe = true;                                                                   //Включаем отображение каркаса


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

   

            ksEntity EntityChamferIn = (_part.NewEntity(o3d_chamfer));                                           //Получаем интерфейс объекта "фаска"

            ksChamferDefinition ChamferDefinitionIn = EntityChamferIn.GetDefinition();                          //Получаем интерфейс параметров объекта 
          
            ChamferDefinitionIn.tangent = false;                                                                //Не продолжать по касательным ребрам
     
            ChamferDefinitionIn.SetChamferParam(true, diameter - diamete, (diameter - diamete) / index);       //Устанавливаем параметры фаски 

            ksEntityCollection EntityCollectionPart = (_part.EntityCollection(o3d_face));                       //Получаем массив поверхностей детали

            ksEntityCollection EntityCollectionChamferIn = (ChamferDefinitionIn.array());                       //Получаем массив поверхностей, на которых будет строиться фаска

            EntityCollectionChamferIn.Clear();

                                                                                                                  //Заполняем массив поверхностей, на которых будет строится фаска

            EntityCollectionChamferIn.Add(EntityCollectionPart.GetByIndex(1));
           

            EntityCollectionChamferIn.Add(EntityCollectionPart.GetByIndex(4));
     
            EntityChamferIn.Create();                                                                                   //Создаем фаску

        }


    /// <summary>
    /// Резьба
    /// </summary>
  
  
        private void BuildThread(double diameterHead, double lengthHead, double diameterLeg)//,double length
        {


            #region Константы для резьбы

            const int pTop_part = -1;  // Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int o3d_cylindricSpiral = 56; // Указывает на цилиндрическую спираль
            const int o3d_planeXOY = 1;
            const int o3d_planeOffset = 14;

            const int o3d_planeXOZ = 2;
            const int o3d_sketch = 5;
            const int o3d_cutEvolution = 47;

            #endregion


            _part = _doc3D.GetPart(pTop_part);                                                              //Получаем интерфейс 3D-модели 

            ksEntity ksEntity = _part.NewEntity(o3d_planeXOZ);
            
            ksEntity entityDrawOffset = _part.NewEntity(o3d_planeOffset);

            ksPlaneOffsetDefinition planeDefinition = entityDrawOffset.GetDefinition();

            planeDefinition.offset = 0;

            planeDefinition.direction = false;

            ksEntity EntityPlaneOffset = _part.GetDefaultEntity(o3d_planeXOY);

            planeDefinition.SetPlane(EntityPlaneOffset);

            entityDrawOffset.Create();
           

            //Построение спирали  

            ksEntity entityCylinderic = _part.NewEntity(o3d_cylindricSpiral);                           //Получаем интерфейс объекта "Цилиндрическая спираль"

            ksCylindricSpiralDefinition cylindricSpiral = entityCylinderic.GetDefinition();             //Получаем интерфейс параметров цилиндрической спирали

            cylindricSpiral.SetPlane(entityDrawOffset);                                                 //Получаем базовую плоскость цилиндрической спирали по смещенной плоскости


            cylindricSpiral.buildDir = true;

            cylindricSpiral.buildMode = 1;                                                             //Задаем тип построения спирали (Шаг и высота)

            cylindricSpiral.height = lengthHead;                                                            //Задаем высоту спирали 

            cylindricSpiral.diam = diameterHead;                                                            //Задаем диаметр спирали

            cylindricSpiral.firstAngle = 0;                                                              //Задаем начальный угол спирали

            cylindricSpiral.turnDir = true;                                                             //Задаем направление навивки спирали (по часовой)

            cylindricSpiral.step = 0.5;                                                                 //Инициализируем шаг резбы спирали

            entityCylinderic.Create();                                                                   //Создаем спирал

          //  Эскиз треуголника

            _entity = _part.NewEntity(o3d_sketch);
            ksSketchDefinition sketchDefinition = _entity.GetDefinition();
            ksEntity Entity = _part.GetDefaultEntity(o3d_planeXOZ);
            sketchDefinition.SetPlane(ksEntity);
            _entity.Create();



            Document2D document2D = sketchDefinition.BeginEdit();
            var StartX = diameterHead / 2 -(diameterHead/100); //
            var StartY = - 0.5 / 2 + 0.01;
         
    
            document2D.ksLineSeg(StartX, 0, diameterHead-StartX,StartY, 1);
            document2D.ksLineSeg(StartX, 0, diameterHead - StartX, -StartY, 1);
            document2D.ksLineSeg(diameterHead - StartX, StartY, diameterHead - StartX, -StartY, 1);

         

            //var StartX = diametr1 / 2; //
            //var StartY = 2 - 0.5 / 2 + 0.01;
            //var Length = 0.5 * 0.5 * (60.0 * Math.PI / 180.0);

            //document2D.ksLineSeg(StartX, 0, xStart + Length, StartY, 1);
            //document2D.ksLineSeg(StartX, 0, xStart + Length, -StartY, 1);
            //document2D.ksLineSeg(xStart + Length, StartY, xStart + Length, -StartY, 1);

            sketchDefinition.EndEdit();



            //Кинематическое вырезание

            ksEntity entityCutEvolution = _part.NewEntity(o3d_cutEvolution);                             //Получаем интерфейс операции кинематического вырезания

            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();       //Получаем интерфейс параметров операции кинематического вырезания

            cutEvolutionDefinition.cut = true;                                                           //Вычитане объектов 

            cutEvolutionDefinition.sketchShiftType = 1;                                                 //Тип движения (сохранение исходного угла направляющей)

            cutEvolutionDefinition.SetSketch(sketchDefinition);                                          //Устанавливаем эскиз сечения

            ksEntityCollection EntityCollection = (cutEvolutionDefinition.PathPartArray());             //Получаем массив объектов
            EntityCollection.Clear();

            EntityCollection.Add(entityCylinderic);                                                     //Добавляем в массив эскиз с траекторией (спираль)

            entityCutEvolution.Create();                                                                //Создаем операцию кинематического вырезания

        }

        private void BuildThread2(double diameterLeg, double lengthLeg, double diameterBracing, double lengthHead)
        {
            #region Константы для резьбы

            const int pTop_part = -1;  // Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int o3d_planeOffset = 14;  // Указывает на создание эскиза//Смщение плоскости
            const int o3d_sketch = 5;//Указывает на создание эскиза.
            const int o3d_planeXOY = 1;   // Указывает на работу в плостости XOY.
            const int o3d_cylindricSpiral = 56;// Указывает на цилиндрическую спираль
           
            const int o3d_cutEvolution = 47; // Указывает на создание кинематического вырезания
            const int o3d_planeXOZ = 2;
   
            #endregion


            _part = _doc3D.GetPart(pTop_part);//Получаем интерфейс 3D-модели 



            ksEntity entityOffset = _part.NewEntity(o3d_planeOffset);                       //Получаем интерфейс объекта "смещенная плоскость"

            ksPlaneOffsetDefinition planeDefinition = entityOffset.GetDefinition();             //Получаем интерфейс параметров смещенной плоскости

            planeDefinition.offset = lengthHead;                                                    //Второй случай  //////

            planeDefinition.direction = true;                                                   //Задаем направление смещенной плоскости

            ksEntity EntityPlaneOffset = _part.GetDefaultEntity(o3d_planeXOY);                   //Получаем интерфейс объекта "плоскость XOY"

            planeDefinition.SetPlane(EntityPlaneOffset);                                         //Получаем базовую плоскость смещенной плоскости по "XOY"

            entityOffset.Create();                                                                  //Создаем смещенную плоскость
            

            //Построение спирали


            ksEntity entityCylinderic = _part.NewEntity(o3d_cylindricSpiral);                       //Получаем интерфейс объекта "Цилиндрическая спираль"

            ksCylindricSpiralDefinition cylindricSpiral = entityCylinderic.GetDefinition();        //Получаем интерфейс параметров цилиндрической спирали

            cylindricSpiral.SetPlane(entityOffset);                                                 //Получаем базовую плоскость цилиндрической спирали по смещенной плоскости

            cylindricSpiral.buildDir = true;

            cylindricSpiral.buildMode = 1;                                                          //Задаем тип построения спирали (Шаг и высота)

            cylindricSpiral.height = lengthLeg;                                                         //Задаем высоту спирали //////

            cylindricSpiral.diam = diameterLeg;                                                        //Задаем диаметр спирали

            cylindricSpiral.firstAngle = 0;                                                          //Задаем начальный угол спирали 

            cylindricSpiral.turnDir = true;                                                         //Задаем направление навивки спирали (по часовой)

            cylindricSpiral.step = 0.5;                                                             //Инициализируем шаг резбы спирали


            entityCylinderic.Create();                                                              //Создаем спираль
            
            //////Эскиз треуголника

     

            ksEntity Entity2 = _part.NewEntity(o3d_sketch);

            ksSketchDefinition sketchDefinition = Entity2.GetDefinition();

            ksEntity Entity = _part.GetDefaultEntity(o3d_planeXOZ);

            sketchDefinition.SetPlane(Entity);//entityOffset
            Entity2.Create();



            Document2D document2D = sketchDefinition.BeginEdit();

            var StartX = diameterLeg / 2 - (diameterLeg / 100); //

            var StartY = -0.5 / 2 + 0.01;


            document2D.ksLineSeg(StartX, -lengthHead, diameterLeg - StartX, -lengthHead + StartY, 1);
            document2D.ksLineSeg(StartX, -lengthHead, diameterLeg - StartX, -lengthHead - StartY, 1);
            document2D.ksLineSeg(diameterLeg - StartX, -lengthHead + StartY, diameterLeg - StartX, -lengthHead - StartY, 1);


            sketchDefinition.EndEdit();



            //////Кинематическое вырезание

            ksEntity entityCutEvolution = _part.NewEntity(o3d_cutEvolution);                            //Получаем интерфейс операции кинематического вырезания

            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();           //Получаем интерфейс параметров операции кинематического вырезания

            cutEvolutionDefinition.cut = true;                                                              //Вычитане объектов 

            cutEvolutionDefinition.sketchShiftType = 1;                                                     //Тип движения (сохранение исходного угла направляющей)

            cutEvolutionDefinition.SetSketch(sketchDefinition);                                             //Устанавливаем эскиз сечения

            ksEntityCollection EntityCollection = (cutEvolutionDefinition.PathPartArray());                 //Получаем массив объектов
            EntityCollection.Clear();

            EntityCollection.Add(entityCylinderic);                                                         //Добавляем в массив эскиз с траекторией (спираль)

            entityCutEvolution.Create();                                                                     //Создаем операцию кинематического вырезания

        }
    }
}
