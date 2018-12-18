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


        public void BuildDetail(Parameters parameters)
        {
         
            _doc3D = _kompas.Document3D();
            _doc3D.Create(false, true);///////////

            BuildHead(parameters.DiameterHead, parameters.LengthHead);

            BuildLeg(parameters.DiameterLeg,parameters.LengthLeg,parameters.LengthHead);


            BuildChamfer(parameters.DiameterHead, parameters.DiameterLeg);//////////////////////////////////////////////////////////////////////////////////////////////////

            BuildBracing(parameters.DiameterBracing, parameters.LengthBracing, parameters.LengthLeg,parameters.LengthHead);

        }


        /// <summary>
        /// Создание головки
        /// </summary>
        /// <param name="diameterBracing"></param>
        /// <param name="diameterHead"></param>
        /// <param name="diameterLeg"></param>
        private void BuildHead(double diameter, double length)
        {

            #region Константы для эскиза
            // Тип компо­нента Get Param. Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int pTop_part = -1;

            //Тип объекта NewEntity. Указывает на создание эскиза.
            const int o3d_sketch = 5;

            // Тип объекта GetDefaultEntity. Указывает на работу в плостости XOY.
            const int o3d_planeXOY = 1;
            #endregion


            _part = _doc3D.GetPart(pTop_part);
            //Получаем интерфейс объекта "Эскиз"
            _entity = _part.NewEntity(o3d_sketch);
            //Получаем интерфейс параметров эскиза
            ksSketchDefinition SketchDefinition = _entity.GetDefinition();
            //Получаем интерфейс объекта "плоскость XOY"
            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);
            //Устанавливаем плоскость XOY базовой для эскиза
            SketchDefinition.SetPlane(EntityPlane);
            //Создаем эскиз
            _entity.Create();
            //Входим в режим редактирования эскиза
            ksDocument2D Document2D = SketchDefinition.BeginEdit();
            //Строим окружность 
            Document2D.ksCircle(0, 0, diameter / 2, 1);
            //Выходим из режима редактирования эскиза
            SketchDefinition.EndEdit();



            #region Константы для выдавливания

            //Тип объекта NewEntity. Указывает на создание операции выдавливания.
            const int o3d_baseExtrusion = 24;
            // Тип обекта DrawMode. Устанавливает полутоновое изображение модели
            const int vm_Shaded = 3;
            //Тип выдавливания. Строго на глубину
            const int etBlind = 0;
            #endregion

            //Получаем интерфейс объекта "операция выдавливание"
            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);
            //Получаем интерфейс параметров операции "выдавливание"
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();
            //Устанавливаем параметры операции выдавливания

            BaseExtrusionDefinition.SetSideParam(true, etBlind, length, 0, true);

            //Устанавливаем эскиз операции выдавливания
            BaseExtrusionDefinition.SetSketch(_entity);
            //Создаем операцию выдавливания
            EntityExtrusion.Create();

            //Устанавливаем полутоновое изображение модели
            _doc3D.drawMode = vm_Shaded;
            //Включаем отображение каркаса
            _doc3D.shadedWireframe = true;

        }


        /// <summary>
        /// Создание ножки
        /// </summary>

        private void BuildLeg(double diameter,double length,double leng)
        {
            #region Константы для эскиза
            // Тип компо­нента Get Param. Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int pTop_part = -1;

            //Тип объекта NewEntity. Указывает на создание эскиза.
            const int o3d_sketch = 5;

            // Тип объекта GetDefaultEntity. Указывает на работу в плостости XOY.
            const int o3d_planeXOY = 1;

            const int o3d_planeOffset = 14;



            #endregion


            _part = _doc3D.GetPart(pTop_part);

            //Получаем интерфейс объекта "плоскость XOZ"
            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);


            ///Смещение плоскости

            ksEntity PlaneOff = _part.NewEntity(o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition();
            planeOffsetDefinition.direction = true;
            planeOffsetDefinition.offset = leng;


            planeOffsetDefinition.SetPlane(EntityPlane);
            //Создаем эскиз
            PlaneOff.Create();



            ////Получаем интерфейс объекта "Эскиз"
            //_entity = _part.NewEntity(o3d_sketch);

            ksEntity Entity = _part.NewEntity(o3d_sketch);
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();
            sketchDefinition.SetPlane(PlaneOff);
            Entity.Create();
            ksDocument2D Document2D = sketchDefinition.BeginEdit();

            //Строим окружность 
            Document2D.ksCircle(0, 0, diameter / 2, 1);

            //Выходим из режима редактирования эскиза
            sketchDefinition.EndEdit();



         

           // const int o3d_cutExtrusion = 26;



            #region Константы для выдавливания

            //Тип объекта NewEntity. Указывает на создание операции выдавливания.
            const int o3d_baseExtrusion = 24;
            // Тип обекта DrawMode. Устанавливает полутоновое изображение модели
            const int vm_Shaded = 3;
            //Тип выдавливания. Строго на глубину
            const int etBlind = 0;
            #endregion

            //Получаем интерфейс объекта "операция выдавливание"
            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);
            //Получаем интерфейс параметров операции "выдавливание"
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();
            //Устанавливаем параметры операции выдавливания


            BaseExtrusionDefinition.SetSideParam(true, etBlind, length, 0, true);

            //Устанавливаем эскиз операции выдавливания
            BaseExtrusionDefinition.SetSketch(Entity);
            //Создаем операцию выдавливания
            EntityExtrusion.Create();

            //Устанавливаем полутоновое изображение модели
            _doc3D.drawMode = vm_Shaded;
            //Включаем отображение каркаса
            _doc3D.shadedWireframe = true;

        }



        /// <summary>
        /// Операция "Фаска" для всех граней
        /// </summary>
        /// 
        private void BuildChamfer(double diameter, double diamete)
     
        {

            #region Константы для фаски
            // Тип получения массива объектов. Выбираются поверхности.
            const int o3d_face = 6;
            // Тип объекта NewEntity. Указывает на операцию "Фаска"
            const int o3d_chamfer = 33;
            //Устанавливаем tg 45         
            //double index = 45;
            double index = 1.6;     //1.6
            #endregion

            //Получаем интерфейс объекта "фаска"

            ksEntity EntityChamferIn = (_part.NewEntity(o3d_chamfer));


            //Получаем интерфейс параметров объекта 

            ksChamferDefinition ChamferDefinitionIn = EntityChamferIn.GetDefinition();

            //Не продолжать по касательным ребрам
            ChamferDefinitionIn.tangent = false;


            //Устанавливаем параметры фаски 
            ChamferDefinitionIn.SetChamferParam(true, diameter - diamete, (diameter - diamete) / index);
        

            //Получаем массив поверхностей детали


            ksEntityCollection EntityCollectionPart = (_part.EntityCollection(o3d_face));

            //Получаем массив поверхностей, на которых будет строиться фаска

            ksEntityCollection EntityCollectionChamferIn = (ChamferDefinitionIn.array());

             EntityCollectionChamferIn.Clear();

            //Заполняем массив поверхностей, на которых будет строится фаска (Внутреняя поверхность)

             EntityCollectionChamferIn.Add(EntityCollectionPart.GetByIndex(1));

            EntityCollectionChamferIn.Add(EntityCollectionPart.GetByIndex(4));

            //Создаем фаску

            EntityChamferIn.Create();

        }




        /// <summary>
        /// Создание крепления
        /// </summary>

        private void BuildBracing(double diameter, double length, double leng,double len)
        {
            #region Константы для эскиза
            // Тип компо­нента Get Param. Главный компонент, в составе которо­го находится новый или редактируе­мый компонент.
            const int pTop_part = -1;

            //Тип объекта NewEntity. Указывает на создание эскиза.
            const int o3d_sketch = 5;

            // Тип объекта GetDefaultEntity. Указывает на работу в плостости XOY.
            const int o3d_planeXOY = 1;

            const int o3d_planeOffset = 14;
            const int ko_RectangleParam = 91;



            #endregion


            _part = _doc3D.GetPart(pTop_part);

            //Получаем интерфейс объекта "плоскость XOZ"
            ksEntity EntityPlane = _part.GetDefaultEntity(o3d_planeXOY);


            ///Смещение плоскости

            ksEntity PlaneOff = _part.NewEntity(o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition();
            planeOffsetDefinition.direction = true;
            planeOffsetDefinition.offset = leng+len;


            planeOffsetDefinition.SetPlane(EntityPlane);
            //Создаем эскиз
            PlaneOff.Create();



            ////Получаем интерфейс объекта "Эскиз"
            //_entity = _part.NewEntity(o3d_sketch);

            ksEntity Entity = _part.NewEntity(o3d_sketch);
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();
            sketchDefinition.SetPlane(PlaneOff);
            Entity.Create();
            ksDocument2D Document2D = sketchDefinition.BeginEdit();


            //Строим окружность 
            Document2D.ksCircle(0, 0, diameter / 2, 1);


            //Выходим из режима редактирования эскиза
            sketchDefinition.EndEdit();



            #region Константы для выдавливания

            //Тип объекта NewEntity. Указывает на создание операции выдавливания.
            const int o3d_baseExtrusion = 24;
            // Тип обекта DrawMode. Устанавливает полутоновое изображение модели
            const int vm_Shaded = 3;
            //Тип выдавливания. Строго на глубину
            const int etBlind = 0;
            #endregion

            //Получаем интерфейс объекта "операция выдавливание"
            ksEntity EntityExtrusion = _part.NewEntity(o3d_baseExtrusion);
            //Получаем интерфейс параметров операции "выдавливание"
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();
            //Устанавливаем параметры операции выдавливания


            BaseExtrusionDefinition.SetSideParam(true, etBlind, length, 0, true);

            //Устанавливаем эскиз операции выдавливания
            BaseExtrusionDefinition.SetSketch(Entity);
            //Создаем операцию выдавливания
            EntityExtrusion.Create();

            //Устанавливаем полутоновое изображение модели
            _doc3D.drawMode = vm_Shaded;
            //Включаем отображение каркаса
            _doc3D.shadedWireframe = true;






            ///Смещение плоскости

            ksEntity PlaneOff2 = _part.NewEntity(o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinition2 = PlaneOff2.GetDefinition();
            planeOffsetDefinition2.direction = true;
            planeOffsetDefinition2.offset = leng + len+length;


            planeOffsetDefinition2.SetPlane(EntityPlane);
            //Создаем эскиз
            PlaneOff2.Create();

            ////Получаем интерфейс объекта "Эскиз"

            ksEntity Entity2 = _part.NewEntity(o3d_sketch);
            ksSketchDefinition sketchDefinition2 = Entity2.GetDefinition();
            sketchDefinition2.SetPlane(PlaneOff2);
            Entity2.Create();
            ksDocument2D Document2D2 = sketchDefinition2.BeginEdit();




            _par = _kompas.GetParamStruct(ko_RectangleParam);

            _par.height = diameter / 2;
            _par.width = diameter;
            _par.x = -(diameter / 2);
            _par.y = -(diameter / 4);
            _par.ang = 0;
            _par.style = 1;

            Document2D2.ksRectangle(_par, 0);

            //Выходим из режима редактирования эскиза
            sketchDefinition2.EndEdit();


            #region Константы для выдавливания

            //Тип объекта NewEntity. Указывает на создание операции выдавливания.
            const int o3d_cutExtrusion = 26;
            #endregion

            //Получаем интерфейс объекта "операция вырезание выдавливанием"   


            ksEntity EntityCutExtrusion = _part.NewEntity(o3d_cutExtrusion);
            //Получаем интерфейс параметров операции 

            ksCutExtrusionDefinition CutExtrusionDefinition = EntityCutExtrusion.GetDefinition();

            //Вычитание элементов   

            CutExtrusionDefinition.cut = true;
            //Прямое направление    
            CutExtrusionDefinition.directionType = 0;
            //Устанавливаем параметры выдавливания    
            CutExtrusionDefinition.SetSideParam(true, etBlind, length / 2, 0, false);
            //Устанавливаем экиз операции   
            CutExtrusionDefinition.SetSketch(Entity2);
            //Создаем операцию вырезания выдавливанием 
            EntityCutExtrusion.Create();


            //Устанавливаем полутоновое изображение модели
            _doc3D.drawMode = vm_Shaded;
            //Включаем отображение каркаса
            _doc3D.shadedWireframe = true;


















            #region Константы для эскиза
          

            // Тип объекта GetDefaultEntity. Указывает на работу в плостости XOZ.
            const int o3d_planeXOZ = 2;

            


            #endregion


            _part = _doc3D.GetPart(pTop_part);

            //Получаем интерфейс объекта "плоскость XOZ"
            ksEntity EntityPlane3 = _part.GetDefaultEntity(o3d_planeXOZ);


            ///Смещение плоскости

            ksEntity PlaneOff3 = _part.NewEntity(o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinition3 = PlaneOff3.GetDefinition();
            planeOffsetDefinition3.direction = true;
            planeOffsetDefinition3.offset = diameter / 2;
            planeOffsetDefinition3.SetPlane(EntityPlane3);
            //Создаем эскиз
            PlaneOff3.Create();



            ////Получаем интерфейс объекта "Эскиз"

            ksEntity Entity3 = _part.NewEntity(o3d_sketch);
            ksSketchDefinition sketchDefinition3 = Entity3.GetDefinition();
            sketchDefinition3.SetPlane(PlaneOff3);
            Entity3.Create();
            ksDocument2D Document2D3 = sketchDefinition3.BeginEdit();

            //Строим окружность 
            Document2D3.ksCircle(0, -(0.75*length+leng+len), diameter / 6, 1);

            //Выходим из режима редактирования эскиза
            sketchDefinition3.EndEdit();




            //Выдавливание отверстия 

            //Получаем интерфейс объекта "операция вырезание выдавливанием"   

            ksEntity EntityCutExtrusion2 = _part.NewEntity(o3d_cutExtrusion);
            //Получаем интерфейс параметров операции 

            ksCutExtrusionDefinition CutExtrusionDefinition2 = EntityCutExtrusion2.GetDefinition();

            //Вычитание элементов   

            CutExtrusionDefinition2.cut = true;
            //Прямое направление    
            CutExtrusionDefinition2.directionType = 0;
            //Устанавливаем параметры выдавливания    
            CutExtrusionDefinition2.SetSideParam(true, etBlind, diameter, 0, false);
            //Устанавливаем экиз операции   
            CutExtrusionDefinition2.SetSketch(Entity3);
            //Создаем операцию вырезания выдавливанием 
            EntityCutExtrusion2.Create();


            //Устанавливаем полутоновое изображение модели
            _doc3D.drawMode = vm_Shaded;
            //Включаем отображение каркаса
            _doc3D.shadedWireframe = true;



        }

    }
}
