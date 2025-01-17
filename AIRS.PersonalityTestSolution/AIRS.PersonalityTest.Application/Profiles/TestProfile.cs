using AIRS.PersonalityTest.Application.DTOs.AwnserDtos;
using AIRS.PersonalityTest.Application.DTOs.QuestionDTOs;
using AIRS.PersonalityTest.Application.DTOs.TestDto;
using AIRS.PersonalityTest.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Application.Profiles;
public class TestProfile : Profile
{
    public TestProfile()
    {
        // Mapping from TestDTO to Test
        CreateMap<TestDTO, Test>()
            .ForMember(dest => dest.TestId, opt => opt.Ignore())
            .ForMember(dest => dest.TestCode, opt => opt.Ignore());
        CreateMap<Test, TestDTO>();

        // Question mapping
        CreateMap<QuestionCreateDTO, Question>()
            .ForMember(dest => dest.QuestionId, opt => opt.Ignore())
            .ForMember(dest => dest.TestId, opt => opt.Ignore());
        CreateMap<Question, QuestionCreateDTO>();

        // Answer mapping
        CreateMap<AnswerDTO, Answer>()
            .ForMember(dest => dest.AnswerId, opt => opt.Ignore())
            .ForMember(dest => dest.QuestionId, opt => opt.Ignore());
        CreateMap<Answer, AnswerDTO>();
    }
}

