using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.UnitDto
{
    public class UnitResponseDto
    {
        public int Unit_Id { get; set; }
        public int Crs_Id { get; set; }
        public string Unit_Name { get; set; } = string.Empty;
        public string Unit_Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
