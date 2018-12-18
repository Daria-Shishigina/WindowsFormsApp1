using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using ShaftApp;

namespace ShaftApp
{
    public class KompasConnector
    {
        /// <summary>
        /// Экземпляр компаса 
        /// </summary>
        private KompasObject _kompasObject;



        /// <summary>
        /// Свойства 
        /// </summary>
        public KompasObject KompasObject
        {
            get
            {
                return _kompasObject;
            }
            set
            {
                _kompasObject = value;
            }
        }

        public void Connector()
        {
            var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
            _kompasObject = (KompasObject)Activator.CreateInstance(type);
            _kompasObject.Visible = true;    
        }
          

        //CreateDocument3d 
        //Document 3D свойства


    }
}
