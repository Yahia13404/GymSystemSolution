using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.MemberShipsViewModel
{
    public class MemberShipsViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public int PlanId { get; set; }
        public string PlanName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
