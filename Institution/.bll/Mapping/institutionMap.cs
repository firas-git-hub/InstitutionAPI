using InstitutionAPI.bll.Dtos;
using InstitutionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InstitutionAPI.bll.Mapping
{
    public class institutionMap
    {
        public institutionDTO mapToDTO(institution inst)
        {
            return new institutionDTO
            {
                CodeName = inst.Code + "_" + inst.Name,
                Id = inst.Id
            };
        }

        public institution mapToInstitution(institutionDTO instDTO)
        {
            string[] data = new string[2];
            data = instDTO.CodeName.Split('_');
            return new institution
            {
                Id = instDTO.Id,
                Code = data[1],
                Name = data[2]
            };
        }
    }
}
