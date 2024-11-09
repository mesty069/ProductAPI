using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Sharing
{
    public class ProductParams
    {


        //頁數
        public int maxpagesize { get; set; } = 50;
        private int pagesize = 13;
        public int Pagesize
        {
            get => pagesize;
            set => pagesize = value > maxpagesize ? maxpagesize : value;
        }
        public int PageNumber { get; set; } = 1;

        //類別代碼
        public int? Categoryid { get; set; }
        //排序
        public string Sorting { get; set; }
        //查詢 
        private string _search;

        public string Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }

    }
}
