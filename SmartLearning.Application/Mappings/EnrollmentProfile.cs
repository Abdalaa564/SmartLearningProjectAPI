using SmartLearning.Application.DTOs.EnrollmentDto;
using SmartLearning.Application.DTOs.PaymentDto;


namespace SmartLearning.Application.Mappings
{
    public class EnrollmentProfile:Profile
    {
        public EnrollmentProfile()
        {
            // ------------------ EnrollmentRequestDto → Enrollment ------------------
            CreateMap<EnrollmentRequestDto, Enrollment>()
             .ForMember(dest => dest.Enroll_Date, opt => opt.MapFrom(src => DateTime.UtcNow))
             .ForMember(dest => dest.Paid_Amount, opt => opt.MapFrom(src => src.Payment.Amount));

            CreateMap<PaymentInfoDto, Payment>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Payment_Method, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Payment_Date, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Completed"))
                .ForMember(dest => dest.Gateway_Response, opt => opt.Ignore())
                .ForMember(dest => dest.Transaction_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Enroll_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Payment_Id, opt => opt.Ignore()); // مهم جداً

            CreateMap<Enrollment, EnrollmentResponseDto>()
                .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Enroll_Id))
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.Enroll_Date))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Paid_Amount))
                .ForMember(dest => dest.TransactionId, opt => opt.Ignore()) // مهم جداً
                .ForMember(dest => dest.Success, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore());


            CreateMap<Enrollment, EnrollmentDetailsDto>()
             .ForMember(dest => dest.EnrollId, opt => opt.MapFrom(src => src.Enroll_Id))
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.StudentId))
             .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.User.UserName))
             .ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src => src.Student.User.Email))
             .ForMember(dest => dest.StudentPhone, opt => opt.MapFrom(src => src.Student.User.PhoneNumber))
             .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Crs_Id))
             .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Crs_Name))
             .ForMember(dest => dest.CoursePrice, opt => opt.MapFrom(src => src.Course.Price))
             .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(
              src => src.Payments.OrderByDescending(p => p.Payment_Date).FirstOrDefault().Transaction_Id))
             .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(
             src => src.Payments.OrderByDescending(p => p.Payment_Date).FirstOrDefault().Status))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(
             src => src.Payments.OrderByDescending(p => p.Payment_Date).FirstOrDefault().Payment_Date));

            CreateMap<Payment, EnrollmentStatusDto>()
             .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Enroll_Id))
             .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Transaction_Id))
             .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Enrollment.Crs_Id))
             .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Status))
             .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
             .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.Payment_Date));



        }
    }
}
