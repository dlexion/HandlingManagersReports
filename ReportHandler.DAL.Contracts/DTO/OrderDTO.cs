using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportHandler.DAL.Contracts.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Cost { get; set; }

        public virtual ManagerDTO Manager { get; set; }
        public virtual CustomerDTO Customer { get; set; }
        public virtual ItemDTO Item { get; set; }
    }
}
