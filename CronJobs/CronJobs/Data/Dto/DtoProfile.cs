using AutoMapper;
using CronJobs.Data.Entity;
using CronJobs.Data.Request;

namespace CronJobs.Data.Dto
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            //Add as many of these lines as you need to map your objects
            CreateMap<CronJobAddRequest, CronJob>();
           
            //CreateMap<RecordListViewModel, CallRecordExportModel>()
            //    .ForMember(dest => dest.AnswerState, opt => opt.MapFrom(src => ((AnswerStateEnum)src.AnswerState).ToString()))
            //    .ForMember(dest => dest.CallType, opt => opt.MapFrom(src => ((CallTypeEnum)src.CallType).ToString()))
            //    //.ForMember(dest => dest.nodes, opt => opt.MapFrom(src => src.Nodes))
            //    ;

        }
    }}
