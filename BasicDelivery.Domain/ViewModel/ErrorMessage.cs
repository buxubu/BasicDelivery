using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Domain.ViewModel
{
    public class ErrorMessage<T>
    {
        public T? Entity { get; set; }
        public int Code { get; set; }
        public string? Message { get; set; }
    }


}
