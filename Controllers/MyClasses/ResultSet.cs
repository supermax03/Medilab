using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediLab.Controllers.MyClasses
{

    public class ResultSet
    {
        private int code;
        private String msg;
        public ResultSet()
        {

        }
        public ResultSet(int code, String msg)
        {
            this.Code = code;
            this.Msg = msg;

        }

        public int Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }
    }

}