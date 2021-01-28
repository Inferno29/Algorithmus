using System;
using System.Collections.Generic;
using System.Text;

namespace pMedian
{
    
    
    public class Location
    {
        int locationIndex;
        bool isLocationAWarehouse;

        //Constructor1 for Location
        public Location( int locationIndex, bool isLocationWarehouse)
        {
            this.locationIndex = locationIndex;
            this.isLocationAWarehouse = isLocationWarehouse;
        }
        
    }



}
