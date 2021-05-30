using AutoMapper;
using ConfluenceRecruitmentTest.Data.Entities;
using ConfluenceRecruitmentTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfluenceRecruitmentTest.Data
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            this.CreateMap<Stock, StockModel>()
              .ReverseMap();
        }
    }
}
