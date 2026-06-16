using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.SessionsViewModels
{
    public class CategorySelectViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = default!;
    }
}
