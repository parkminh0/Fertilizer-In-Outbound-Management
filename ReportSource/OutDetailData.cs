using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanIlCNS
{
    public class OutDetailData
    {
        private string _no;

        public string no
        {
            get { return _no; }
            set { _no = value; }
        }

        private string _productname;

        public string productname
        {
            get { return _productname; }
            set { _productname = value; }
        }

        private double? _outqty;

        public double? outqty
        {
            get { return _outqty; }
            set { _outqty = value; }
        }

        private double? _outprice;

        public double? outprice
        {
            get { return _outprice; }
            set { _outprice = value; }
        }

        private double? _outunitprice;

        public double? outunitprice
        {
            get { return _outunitprice; }
            set { _outunitprice = value; }
        }

        private string _outnote;

        public string outnote
        {
            get { return _outnote; }
            set { _outnote = value; }
        }
    }
}
