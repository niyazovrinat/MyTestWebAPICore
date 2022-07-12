using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApiDTO_Models;
using TodoApiDTO_Models.DTO;

namespace TodoApiDTO_Business.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<TodoItemDTO, TodoItem>();
                config.CreateMap<TodoItem, TodoItemDTO>();
            });

            return mappingConfig;
        }
    }
}
