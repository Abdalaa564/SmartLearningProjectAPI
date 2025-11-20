
namespace SmartLearning.Application.DTOs.UnitDto
{
    public class CreateUnitDto
    {
        public int Crs_Id { get; set; }
        public string Unit_Name { get; set; } = string.Empty;
        public string Unit_Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
