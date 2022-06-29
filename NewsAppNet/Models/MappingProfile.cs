using AutoMapper;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;

namespace NewsAppNet.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewsItem, NewsItemDTO>();

            CreateMap<Comment, CommentDTO>()
                .ForMember(c => c.UserFullName,
                a => a.MapFrom(x => string.Join(' ', x.User.FirstName, x.User.LastName)));
                                    
            CreateMap<Comment, ReplyDTO>()
                .ForMember(c => c.UserFullName,
                a => a.MapFrom(x => string.Join(' ', x.User.FirstName, x.User.LastName)));

            CreateMap<ReplyAddDTO, Comment>()
                .ForMember(c => c.NewsItemId,
                a => a.MapFrom(x => x.NewsId));
            
            CreateMap<NewsFeedModel, NewsFeedDTO>();

            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(u => u.FullName,
                a => a.MapFrom(x => string.Join(' ', x.FirstName, x.LastName)));

            CreateMap<UserRegister, ApplicationUser>();

            CreateMap<User, UserDTO>()
                .ForMember(u => u.FullName, 
                a => a.MapFrom(x => string.Join(' ', x.FirstName, x.LastName)));
        }
    }
}
