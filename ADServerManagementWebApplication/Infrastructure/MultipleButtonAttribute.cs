using System;
using System.Reflection;
using System.Web.Mvc;

namespace ADServerManagementWebApplication.Infrastructure
{
    /// <summary>
    /// Atrybut do obsługi akcji o tych samych nazwach
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        #region - Properties -
        /// <summary>
        /// Nazwa akcji
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Argument wywołania akcji
        /// </summary>
        public string Argument { get; set; } 
        #endregion

        #region - Overriden methods -
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        } 
        #endregion
    }
}