using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXCitySearcher
    {
        string[] getCityValuesList(string field);
        void updateCityValuesList(string field);
        Dictionary<string, string> getCityData(string field, string value);
        void setCityData(Dictionary<string, string> values);
    }
}
