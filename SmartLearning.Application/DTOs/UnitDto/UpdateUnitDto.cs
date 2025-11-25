
namespace SmartLearning.Application.DTOs.UnitDto
{
    public class UpdateUnitDto
    {
        public string Unit_Name { get; set; } = string.Empty;
        public string Unit_Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }

        public string? ImageUrl { get; set; }
    }
}
