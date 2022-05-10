using System.ComponentModel;

namespace PL
{
    static class ExtensionMethods
    {
        /// <summary>
        /// Alerts for change
        /// </summary>
        /// <typeparam name="T">every Type</typeparam>
        /// <param name="dependency">the first INotifyPropertyChanged value</param>
        /// <param name="handler">the second PropertyChangedEventHandler value</param>
        /// <param name="property">the third string value</param>
        /// <param name="field">the forth out T value</param>
        /// <param name="value">the fifth T value</param>
        internal static void setAndNotify<T>(this INotifyPropertyChanged dependency, PropertyChangedEventHandler handler, string property, out T field, T value)
        {
            field = value;
            handler?.Invoke(dependency, new PropertyChangedEventArgs(property));
        }
    }
}
