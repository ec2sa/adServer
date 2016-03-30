using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Reprezentacja typu obiektu multimedialnego
    /// </summary>
    public class MultimediaTypeItem
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Nazwa typu
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Szerokość
        /// </summary>
        public int Width { get; set; }


        /// <summary>
        /// Wysokość
        /// </summary>
        public int Height { get; set; }


        /// <summary>
        /// Opis typu wraz z nazwą (dla list)
        /// </summary>
        public string DescriptorWithName
        {
            get
            {
                return string.Format("{0} ({1}x{2})", Name, Width, Height);
            }
        }

        /// <summary>
        /// Opis typu bez nazwy (dla list)
        /// </summary>
        public string DescriptorWithoutName
        {
            get
            {
                return string.Format("{0}x{1}", Width, Height);
            }
        }
    }
}