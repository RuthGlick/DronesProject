using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    class XMLTools
    {
        private static readonly string dirPath = @"../../../../DalXml\xml\";

        /// <summary>
        /// constructor
        /// </summary>
        static XMLTools()
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }


        #region SaveLoadWithXElement

        /// <summary>
        /// save list to xelement
        /// </summary>
        /// <param name="rootElem">the first Xelement value</param>
        /// <param name="filePath">the second string value</param>
        public static void SaveListToXmlElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dirPath + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        /// <summary>
        /// load list from Xelement
        /// </summary>
        /// <param name="filePath">the first string value</param>
        /// <returns>Xelement value</returns>
        public static XElement LoadListFromXmlElement(string filePath)
        {
            try
            {
                if (File.Exists(dirPath + filePath))
                {
                    return XElement.Load($@"{dirPath}{filePath}");
                }
                else
                {
                    XElement rootElem = new XElement($@"{filePath}");
                    rootElem.Save($@"{dirPath}{filePath}");
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }

        #endregion

        #region SaveLoadWithXMLSerializer

        /// <summary>
        /// save list to xmlserializer
        /// </summary>
        /// <typeparam name="T">every type</typeparam>
        /// <param name="list">the first IEnumerable<T> value </param>
        /// <param name="filePath">the second string value</param>
        public static void SaveListToXmlSerializer<T>(IEnumerable<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dirPath + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        /// <summary>
        /// load list from xmlserializer
        /// </summary>
        /// <typeparam name="T">every type</typeparam>
        /// <param name="filePath">the first string value</param>
        /// <returns>List<T> value</returns>
        public static List<T> LoadListFromXmlSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dirPath + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dirPath + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        
        #endregion        
    }
}