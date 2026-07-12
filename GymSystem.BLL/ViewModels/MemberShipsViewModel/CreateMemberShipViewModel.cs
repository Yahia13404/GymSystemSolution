using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.MemberShipsViewModel
{
    public class CreateMemberShipViewModel
    {
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
