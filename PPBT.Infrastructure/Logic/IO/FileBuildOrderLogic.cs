﻿using Newtonsoft.Json;
using PPBT.DataTransferObject;
using PPBT.DataTransferObject.AzureDevOps;
using PPBT.DataTransferObject.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PPBT.Infrastructure.IO
{
    public class FileBuildOrderLogic
    {
        private readonly OptionsDto _optionsDto;

        public FileBuildOrderLogic(OptionsDto optionsDto)
        {
            _optionsDto = optionsDto;

            _optionsDto.LastKnownFile = _optionsDto.LastKnownFile.Contains("BuildOrder.json") ? _optionsDto.LastKnownFile : _optionsDto.LastKnownFile + "/BuildOrder.json";

        }

        /// <summary>
        /// Create or Modify the File
        /// </summary>
        /// <param name="fileSuccessOrderDtoList"></param>
        /// <returns></returns>
        public bool CreateOrModifyFile(List<BuildOrderDto> fileSuccessOrderDtoList)
        {
            bool success = false;

            try
            {
                fileSuccessOrderDtoList = fileSuccessOrderDtoList.Where(x => x.Order != 0).OrderBy(x => x.Order).ToList();

                string json = JsonConvert.SerializeObject(fileSuccessOrderDtoList, Formatting.Indented);

                File.WriteAllText(_optionsDto.LastKnownFile, json);

                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: { ex.ToString() }");

            }

            return success;
        }

        /// <summary>
        /// Open and read the file
        /// </summary>
        /// <returns></returns>
        public List<BuildOrderDto> OpenFile()
        {
            List<BuildOrderDto> buildOrderList = new List<BuildOrderDto>();

            try
            {
                var fileContent = File.ReadAllLines(_optionsDto.LastKnownFile)?.Aggregate((i, j) => i + j);

                buildOrderList = JsonConvert.DeserializeObject<List<BuildOrderDto>>(fileContent);

                buildOrderList = buildOrderList.OrderBy(x => x.Order).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: { ex.ToString() }");
            }

            return buildOrderList;
        }
    }
}
