using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DO;

namespace DalApi
{
    public static class DalFactory
    {
        /// <summary>
        /// create an instance of an object which realize IDal
        /// </summary>
        /// <returns>IDal object</returns>
        public static IDal GetDal()
        {
            string dalType = DalConfig.DalName;
            string dalNamespace = DalConfig.DalPackages[dalType].Item1;
            string dalClass = DalConfig.DalPackages[dalType].Item2;
            string dalAssembly = DalConfig.DalPackages[dalType].Item3;

            if (dalNamespace == null || dalClass == null || dalAssembly == null)
                throw new DalConfigException($"Package {dalType} is not found in packages list in dal-config.xml");

            Type type1 = Type.GetType($"{dalNamespace}.{dalClass}, {dalClass}");
            if (type1 == null) throw new DalConfigException($"Class {dalClass} was not found in the {dalNamespace}.dll");

            IDal dal = (IDal)type1.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
           
            if (dal == null) throw new DalConfigException($"Class {dalClass} is not a singleton or wrong propertry name for Instance");

            return dal;
        }
    }
}
