using SmartLearning.Application.DTOs.PaymentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Mappings
{
    public class PaymentProfile:Profile
    {
        public PaymentProfile()
        {
            // Mapping from Payment → PaymentInfoDto
            CreateMap<Payment, PaymentInfoDto>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment_Method))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            // Mapping from PaymentInfoDto → Payment
            CreateMap<PaymentInfoDto, Payment>()
                .ForMember(dest => dest.Payment_Method, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Transaction_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Payment_Date, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Gateway_Response, opt => opt.Ignore());

            CreateMap<Payment, PaymentResponseDto>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Payment_Id))
                .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Enroll_Id))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment_Method))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Transaction_Id))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.Payment_Date))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
                //.ForMember(dest => dest.GatewayResponse, opt => opt.MapFrom(src => src.Gateway_Response));

            // Mapping Payment → PaymentResult
            CreateMap<Payment, PaymentResult>()
                .ConstructUsing(src => PaymentResult.Successful("Success", src.Gateway_Response));
        }
    }
}
