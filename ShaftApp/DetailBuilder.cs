using Kompas6API5;

namespace ShaftApp
{
    public class DetailBuilder
    {
        /// <summary>
        /// экземпляр компаса 
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Экземпляр документа 
        /// </summary>
        private Document3D _doc3D;

        /// <summary>
        /// Экземпляр  сущности
        /// </summary>
        private ksEntity _entity;

        /// <summary>
        /// Экземпляр компонента 
        /// </summary>
        private ksPart _part;
    
        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="kompas">Экземпляр компаса</param>
        public DetailBuilder(KompasObject kompas)
        {
            this._kompas = kompas;
        }

        /// <summary>
        /// Построение вала
        /// </summary>
        /// <param name="parameters">Параметры делали</param>
        public void BuildDetail(Parameters parameters)
        {
            _doc3D = _kompas.Document3D();
            _doc3D.Create(false, true);

            BuildHead(parameters.DiameterHead, parameters.LengthHead);
            BuildLeg(parameters.DiameterLeg, parameters.LengthLeg, parameters.LengthHead);
            BuildBracing(parameters.DiameterBracing, parameters.LengthBracing, 
                parameters.LengthLeg, parameters.LengthHead);
            BuildBracingCut(parameters.DiameterBracing, parameters.LengthBracing, parameters.LengthLeg, parameters.LengthHead);
            BuildBracingHole(parameters.DiameterBracing, parameters.LengthBracing, parameters.LengthLeg, parameters.LengthHead);
            //

            switch (parameters.Thread)
            {
                case "-":
                    break;
                case "Head":
                    BuildThreadHead(parameters.DiameterHead, parameters.LengthHead, parameters.DiameterLeg);                 
                    break;
                case "Leg":
                    BuildThreadLeg(parameters.DiameterLeg, parameters.LengthLeg, parameters.DiameterBracing, parameters.LengthHead);
                    break;
            }
        }

       /// <summary>
       /// Создание головки
       /// </summary>
       /// <param name="diameterHead">Диаметр головки </param>
       /// <param name="lengthHead">Длина головки </param>
        private void BuildHead(double diameterHead, double lengthHead)
        {        
            #region Константы для головки
            const int part = -1;          
            const int sketch = 5;          
            const int planeXOY = 1;
            const int baseExtrusion = 24;
            const int shaded = 3;
            const int etBlind = 0;
            #endregion

            //Эскиз головки 
            _part = _doc3D.GetPart(part);                                  
            _entity = _part.NewEntity(sketch);                                       
            ksSketchDefinition SketchDefinition = _entity.GetDefinition();      
            ksEntity EntityPlane = _part.GetDefaultEntity(planeXOY);              
            SketchDefinition.SetPlane(EntityPlane);                             
            _entity.Create();                                                           
            ksDocument2D Document2D = SketchDefinition.BeginEdit();             
            Document2D.ksCircle(0, 0, diameterHead / 2, 1);                     
            SketchDefinition.EndEdit(); 

            //Выдавливание головки 
            ksEntity EntityExtrusion = _part.NewEntity(baseExtrusion);                        
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();  
            BaseExtrusionDefinition.SetSideParam(true, etBlind, lengthHead, 0, true);             
            BaseExtrusionDefinition.SetSketch(_entity);                                           
            EntityExtrusion.Create();                                                             
            _doc3D.drawMode = shaded;                                                          
            _doc3D.shadedWireframe = true;                                                        
        }

     /// <summary>
     /// Создание ножки
     /// </summary>
     /// <param name="diameterLeg">Диаметр ножки</param>
     /// <param name="lengthLeg">Длина ножки</param>
     /// <param name="lengthHead">Длина головки </param>
        private void BuildLeg(double diameterLeg,double lengthLeg,double lengthHead)
        {
            #region Константы для ножки
            const int part = -1;          
            const int sketch = 5;          
            const int planeXOY = 1;        
            const int planeOffset = 14;
            const int baseExtrusion = 24;
            const int shaded = 3;
            const int etBlind = 0;
            #endregion

            _part = _doc3D.GetPart(part);                                        
            ksEntity EntityPlane = _part.GetDefaultEntity(planeXOY);   
            
            ///Смещение плоскости
            ksEntity PlaneOff = _part.NewEntity(planeOffset);                     
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition(); 
            planeOffsetDefinition.direction = true;                                   
            planeOffsetDefinition.offset = lengthHead;                                
            planeOffsetDefinition.SetPlane(EntityPlane);                              
            PlaneOff.Create();

            //Эскиз ножки
            ksEntity Entity = _part.NewEntity(sketch);                            
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();             
            sketchDefinition.SetPlane(PlaneOff);                                      
            Entity.Create();                                                          
            ksDocument2D Document2D = sketchDefinition.BeginEdit();                   
            Document2D.ksCircle(0, 0, diameterLeg / 2, 1);                            
            sketchDefinition.EndEdit(); 
            
            //ВЫдавливание ножки
            ksEntity EntityExtrusion = _part.NewEntity(baseExtrusion);                       
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();
            BaseExtrusionDefinition.SetSideParam(true, etBlind, lengthLeg, 0, true);            
            BaseExtrusionDefinition.SetSketch(Entity);                                          
            EntityExtrusion.Create();                                                           
            _doc3D.drawMode = shaded;                                                        
            _doc3D.shadedWireframe = true;                                                      
        }
        
    /// <summary>
    /// Создание крепления
    /// </summary>
    /// <param name="diameterBracing">Диаметр крепления</param>
    /// <param name="lengthBracing">Длина крепления</param>
    /// <param name="lengthLeg">Длина ножки</param>
    /// <param name="lengthHead">Длина головки</param>
        private void BuildBracing(double diameterBracing, double lengthBracing, double lengthLeg,double lengthHead)
        {
            #region Константы для крепления
            const int part = -1;  
            const int sketch = 5; 
            const int planeXOY = 1; 
            const int planeOffset = 14;
            const int baseExtrusion = 24;
            const int shaded = 3;
            const int etBlind = 0;
            #endregion

            _part = _doc3D.GetPart(part);                                        
            ksEntity EntityPlane = _part.GetDefaultEntity(planeXOY);
            
            ///Смещение плоскости
            ksEntity PlaneOff = _part.NewEntity(planeOffset);                     
            ksPlaneOffsetDefinition planeOffsetDefinition = PlaneOff.GetDefinition(); 
            planeOffsetDefinition.direction = true;                                   
            planeOffsetDefinition.offset = lengthLeg+lengthHead;                      
            planeOffsetDefinition.SetPlane(EntityPlane);                              
            PlaneOff.Create();

            //Эскиз крепления
            ksEntity Entity = _part.NewEntity(sketch);                            
            ksSketchDefinition sketchDefinition = Entity.GetDefinition();             
            sketchDefinition.SetPlane(PlaneOff);                                      
            Entity.Create();                                                          
            ksDocument2D Document2D = sketchDefinition.BeginEdit();                   
            Document2D.ksCircle(0, 0, diameterBracing / 2, 1);                        
            sketchDefinition.EndEdit();  
            
            //Выдавливание крепления
            ksEntity EntityExtrusion = _part.NewEntity(baseExtrusion);                        
            ksBaseExtrusionDefinition BaseExtrusionDefinition = EntityExtrusion.GetDefinition();  
            BaseExtrusionDefinition.SetSideParam(true, etBlind, lengthBracing, 0, true);          
            BaseExtrusionDefinition.SetSketch(Entity);                                            
            EntityExtrusion.Create();                                                             
            _doc3D.drawMode = shaded;                                                          
            _doc3D.shadedWireframe = true;                                                        
        }

        /// <summary>
        /// Создане выреза крепления
        /// </summary>
        /// <param name="diameterBracing">Диаметр крепления</param>
        /// <param name="lengthBracing">Длина крепления</param>
        /// <param name="lengthLeg">Длина ножки</param>
        /// <param name="lengthHead">Длина головки</param>
        private void BuildBracingCut(double diameterBracing, double lengthBracing, double lengthLeg, double lengthHead)
        {
            #region Константы для выреза крепления
            const int part = -1;
            const int sketch = 5;
            const int planeXOY = 1;
            const int planeOffset = 14;
            const int rectangleParam = 91;
            const int shaded = 3;
            const int etBlind = 0;
            const int cutExtrusion = 26;
            #endregion

            _part = _doc3D.GetPart(part);
            ksEntity EntityPlane = _part.GetDefaultEntity(planeXOY);

            //Смещение плоскости
            ksEntity PlaneOffSet = _part.NewEntity(planeOffset);
            ksPlaneOffsetDefinition planeOffDefinition = PlaneOffSet.GetDefinition();
            planeOffDefinition.direction = true;
            planeOffDefinition.offset = lengthLeg + lengthHead + lengthBracing;
            planeOffDefinition.SetPlane(EntityPlane);
            PlaneOffSet.Create();

            //Эскиз 
            ksEntity EntityRectangle = _part.NewEntity(sketch);
            ksSketchDefinition sketchDefinitionRectangle = EntityRectangle.GetDefinition();
            sketchDefinitionRectangle.SetPlane(PlaneOffSet);
            EntityRectangle.Create();
            ksDocument2D Document2 = sketchDefinitionRectangle.BeginEdit();
            ksRectangleParam _par;
            _par = _kompas.GetParamStruct(rectangleParam);
            _par.height = diameterBracing / 2;
            _par.width = diameterBracing;
            _par.x = -(diameterBracing / 2);
            _par.y = -(diameterBracing / 4);
            _par.ang = 0;
            _par.style = 1;
            Document2.ksRectangle(_par, 0);
            sketchDefinitionRectangle.EndEdit();

            //Вырезание
            ksEntity EntityCutExtrusion = _part.NewEntity(cutExtrusion);
            ksCutExtrusionDefinition CutExtrusionDefinition = EntityCutExtrusion.GetDefinition();
            CutExtrusionDefinition.cut = true;
            CutExtrusionDefinition.directionType = 0;
            CutExtrusionDefinition.SetSideParam(true, etBlind, lengthBracing / 2, 0, false);
            CutExtrusionDefinition.SetSketch(EntityRectangle);
            EntityCutExtrusion.Create();
            _doc3D.drawMode = shaded;
            _doc3D.shadedWireframe = true;
        }

        /// <summary>
        /// Создание отверстия крепления
        /// </summary>
        /// <param name="diameterBracing">Диаметр крепления</param>
        /// <param name="lengthBracing">Длина крепления</param>
        /// <param name="lengthLeg">Длина ножки</param>
        /// <param name="lengthHead">Длина головки</param>
        private void BuildBracingHole(double diameterBracing, double lengthBracing, double lengthLeg, double lengthHead)
        {
            #region Константы отверстия крепления
            const int part = -1;
            const int sketch = 5;
            const int planeXOY = 1;
            const int planeOffset = 14;
            const int shaded = 3;
            const int etBlind = 0;
            const int planeXOZ = 2;
            const int cutExtrusion = 26;
            #endregion

            ksEntity EntityPlane = _part.GetDefaultEntity(planeXOY);
            _part = _doc3D.GetPart(part);
            ksEntity EntityPlane3 = _part.GetDefaultEntity(planeXOZ);

            //Смещение плоскости
            ksEntity PlaneOffHole = _part.NewEntity(planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinitionHole = PlaneOffHole.GetDefinition();
            planeOffsetDefinitionHole.direction = true;
            planeOffsetDefinitionHole.offset = diameterBracing / 2;
            planeOffsetDefinitionHole.SetPlane(EntityPlane3);
            PlaneOffHole.Create();

            //Эскиз отверстия крепения 
            ksEntity EntityHole = _part.NewEntity(sketch);
            ksSketchDefinition sketchDefinitionHole = EntityHole.GetDefinition();
            sketchDefinitionHole.SetPlane(PlaneOffHole);
            EntityHole.Create();
            ksDocument2D DocumentHole = sketchDefinitionHole.BeginEdit();
            DocumentHole.ksCircle(0, -(0.75 * lengthBracing + lengthLeg + lengthHead), diameterBracing / 6, 1);
            sketchDefinitionHole.EndEdit();

            //Выдавливание отверстия крепления
            ksEntity EntityCutExtrusionHole = _part.NewEntity(cutExtrusion);
            ksCutExtrusionDefinition CutExtrusionDefinitionHole = EntityCutExtrusionHole.GetDefinition();
            CutExtrusionDefinitionHole.cut = true;
            CutExtrusionDefinitionHole.directionType = 0;
            CutExtrusionDefinitionHole.SetSideParam(true, etBlind, diameterBracing, 0, false);
            CutExtrusionDefinitionHole.SetSketch(EntityHole);
            EntityCutExtrusionHole.Create();
            _doc3D.drawMode = shaded;
            _doc3D.shadedWireframe = true;
        }

     /// <summary>
     /// Создание резьбы на головке
     /// </summary>
     /// <param name="diameterHead">Диамтр головки</param>
     /// <param name="lengthHead">Длина головки</param>
     /// <param name="diameterLeg">Диаметр ножки</param>
        private void BuildThreadHead(double diameterHead, double lengthHead, double diameterLeg)//,double length
        {
            #region Константы для резьбы
            const int part = -1; 
            const int cylindricspiral = 56;
            const int planeXOY = 1;
            const int planeOffset = 14;
            const int planeXOZ = 2;
            const int sketch = 5;
            const int cutEvolution = 47;
            #endregion

            //Смещение плоскости
            _part = _doc3D.GetPart(part);                                                          
            ksEntity ksEntity = _part.NewEntity(planeXOZ);           
            ksEntity entityDrawOffset = _part.NewEntity(planeOffset);
            ksPlaneOffsetDefinition planeDefinition = entityDrawOffset.GetDefinition();
            planeDefinition.offset = 0;
            planeDefinition.direction = false;
            ksEntity EntityPlaneOffset = _part.GetDefaultEntity(planeXOY);
            planeDefinition.SetPlane(EntityPlaneOffset);
            entityDrawOffset.Create();

            //Построение спирали 
            ksEntity entityCylinderic = _part.NewEntity(cylindricspiral);                       
            ksCylindricSpiralDefinition cylindricSpiral = entityCylinderic.GetDefinition();         
            cylindricSpiral.SetPlane(entityDrawOffset);                                           
            cylindricSpiral.buildDir = true;
            cylindricSpiral.buildMode = 1;                                                          
            cylindricSpiral.height = lengthHead;                                                    
            cylindricSpiral.diam = diameterHead;                                                    
            cylindricSpiral.firstAngle = 0;                                                         
            cylindricSpiral.turnDir = true;                                                         
            cylindricSpiral.step = 0.5;                                                             
            entityCylinderic.Create();  
            
            //  Эскиз треуголника
            _entity = _part.NewEntity(sketch);
            ksSketchDefinition sketchDefinition = _entity.GetDefinition();
            ksEntity Entity = _part.GetDefaultEntity(planeXOZ);
            sketchDefinition.SetPlane(ksEntity);
            _entity.Create();

            Document2D document2D = sketchDefinition.BeginEdit();
            var StartX = diameterHead / 2 -(diameterHead/100); //
            var StartY = - 0.5 / 2 + 0.01;
            document2D.ksLineSeg(StartX, 0, diameterHead-StartX,StartY, 1);
            document2D.ksLineSeg(StartX, 0, diameterHead - StartX, -StartY, 1);
            document2D.ksLineSeg(diameterHead - StartX, StartY, diameterHead - StartX, -StartY, 1);
            sketchDefinition.EndEdit();

            //Кинематическое вырезание
            ksEntity entityCutEvolution = _part.NewEntity(cutEvolution);                       
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();  
            cutEvolutionDefinition.cut = true;                                                     
            cutEvolutionDefinition.sketchShiftType = 1;                                            
            cutEvolutionDefinition.SetSketch(sketchDefinition);                                    
            ksEntityCollection EntityCollection = (cutEvolutionDefinition.PathPartArray());        
            EntityCollection.Clear();
            EntityCollection.Add(entityCylinderic);                                                
            entityCutEvolution.Create();                                                           
        }
        
        /// <summary>
        /// CСоздание резьбы на ножке
        /// </summary>
        /// <param name="diameterLeg">Диаметр ножки</param>
        /// <param name="lengthLeg">Длина ножки</param>
        /// <param name="diameterBracing">Диаметр крепления</param>
        /// <param name="lengthHead">Длина головки</param>
        private void BuildThreadLeg(double diameterLeg, double lengthLeg, double diameterBracing, double lengthHead)
        {
            #region Константы для резьбы
            const int part = -1;               
            const int planeOffset = 14;         
            const int sketch = 5;               
            const int planeXOY = 1;             
            const int cylindricspiral = 56;     
            const int cutEvolution = 47;        
            const int planeXOZ = 2;
            #endregion
            
            //Смещенеие плоскости 
            _part = _doc3D.GetPart(part);
            ksEntity entityOffset = _part.NewEntity(planeOffset);                     
            ksPlaneOffsetDefinition planeDefinition = entityOffset.GetDefinition();       
            planeDefinition.offset = lengthHead;                                          
            planeDefinition.direction = true;                                             
            ksEntity EntityPlaneOffset = _part.GetDefaultEntity(planeXOY);            
            planeDefinition.SetPlane(EntityPlaneOffset);                                  
            entityOffset.Create();  
            
            //Построение спирали
            ksEntity entityCylinderic = _part.NewEntity(cylindricspiral);                   
            ksCylindricSpiralDefinition cylindricSpiral = entityCylinderic.GetDefinition();     
            cylindricSpiral.SetPlane(entityOffset);                                             
            cylindricSpiral.buildDir = true;
            cylindricSpiral.buildMode = 1;                                                      
            cylindricSpiral.height = lengthLeg;                                                 
            cylindricSpiral.diam = diameterLeg;                                                 
            cylindricSpiral.firstAngle = 0;                                                     
            cylindricSpiral.turnDir = true;                                                     
            cylindricSpiral.step = 0.5;                                                          
            entityCylinderic.Create(); 
            
            //Эскиз треуголника     
            ksEntity Entity2 = _part.NewEntity(sketch);
            ksSketchDefinition sketchDefinition = Entity2.GetDefinition();
            ksEntity Entity = _part.GetDefaultEntity(planeXOZ);
            sketchDefinition.SetPlane(Entity);
            Entity2.Create();

            Document2D document2D = sketchDefinition.BeginEdit();
            var StartX = diameterLeg / 2 - (diameterLeg / 100); //
            var StartY = -0.5 / 2 + 0.01;
            document2D.ksLineSeg(StartX, -lengthHead, diameterLeg - StartX, -lengthHead + StartY, 1);
            document2D.ksLineSeg(StartX, -lengthHead, diameterLeg - StartX, -lengthHead - StartY, 1);
            document2D.ksLineSeg(diameterLeg - StartX, -lengthHead + StartY, diameterLeg - StartX, -lengthHead - StartY, 1);
            sketchDefinition.EndEdit();

            //Кинематическое вырезание
            ksEntity entityCutEvolution = _part.NewEntity(cutEvolution);                          
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();     
            cutEvolutionDefinition.cut = true;                                                        
            cutEvolutionDefinition.sketchShiftType = 1;                                               
            cutEvolutionDefinition.SetSketch(sketchDefinition);                                       
            ksEntityCollection EntityCollection = (cutEvolutionDefinition.PathPartArray());           
            EntityCollection.Clear();
            EntityCollection.Add(entityCylinderic);                                                   
            entityCutEvolution.Create();                                                              
        }
    }
}
