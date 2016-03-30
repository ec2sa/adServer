using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADServerDAL.Entities.Presentation
{
    /// <summary>
    /// Reprezentacja obiektu priorytetu
    /// </summary>
    public class PriorityItem : PresentationItem
    {
        /// <summary>
        /// Identyfikator
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kod
        /// </summary>
        public int Code { get; set; }


    }
}