using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDB.Model
{
    public partial class Service
    {
        public Service(int id, string name, decimal price)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.LastUpdateDateTime = DateTime.Now;
            this.CreatedDateTime = DateTime.Now;
        }

        public Service()
        {
            this.Id = 0;
            this.Name = string.Empty;
            this.Price = 0.0m;
            this.LastUpdateDateTime = DateTime.Now;
            this.CreatedDateTime = DateTime.Now;
        }
    }
}
