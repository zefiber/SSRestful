using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;

namespace SSService
{


    public class Resource
    {
        private static volatile Resource instance;
        private static object syncRoot = new Object();


        public DbAccess _Database = new DbAccess();
        private Resource() 
        { 
        
        }

        public static Resource Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Resource();
                    }
                }

                return instance;
            }
        }
    }
}
