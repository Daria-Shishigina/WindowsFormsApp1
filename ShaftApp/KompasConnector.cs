using System;
using Kompas6API5;

namespace ShaftApp
{
    public class KompasConnector
    {
        /// <summary>
        /// Свойства экземпляра
        /// </summary> 
        public KompasObject KompasObject { get; private set; }
  
        /// <summary>
        /// Метод подключения к компасу
        /// </summary>
        public void Connector()
        {
            var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
            KompasObject = (KompasObject)Activator.CreateInstance(type);
            KompasObject.Visible = true;
        }
    }
}
