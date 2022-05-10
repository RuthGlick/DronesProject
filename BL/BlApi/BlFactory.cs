using System;
using System.Collections.Generic;
using System.Text;
using BL;

namespace BlApi
{
    public static class BlFactory
    {
        /// <summary>
        /// get Instance of bl
        /// </summary>
        /// <returns>static IBL</returns>
        public static IBL GetBl()
        {
            try
            {
                var b = BL.BL.Instance;
                return b;
            }
            catch (BO.BLConfigException e)
            {
                throw e;
            }
            catch (BO.DiscrepanciesException e) { throw e; }
            catch (BO.UnextantException e) { throw e; }
            catch (BO.XMLFileLoadCreateException e) { throw e; }
        }
    }
}
