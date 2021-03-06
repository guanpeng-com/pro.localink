﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.Configuration.Host.Dto;

namespace DM.AbpZeroTemplate.Web.Areas.Mpa.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<ComboboxItemDto> EditionItems { get; set; }
    }
}